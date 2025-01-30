using CommunityToolkit.Mvvm.ComponentModel; // Provides ObservableObject for automatic property change notifications.
using CommunityToolkit.Mvvm.Input; // Provides RelayCommand for easy command binding in the UI.
using ITSF_App.Models; // Namespace for domain models like Project, ProjectTask, Tag, etc.

namespace ITSF_App.PageModels;

// ProjectDetailPageModel is the ViewModel responsible for managing the project detail page.
// It implements IQueryAttributable to handle query parameters and IProjectTaskPageModel to handle project tasks.
public partial class ProjectDetailPageModel : ObservableObject, IQueryAttributable, IProjectTaskPageModel
{
	// Fields to hold project data and repositories for interacting with the data store.
	private Project? _project;
	private readonly ProjectRepository _projectRepository;
	private readonly TaskRepository _taskRepository;
	private readonly CategoryRepository _categoryRepository;
	private readonly TagRepository _tagRepository;
	private readonly ModalErrorHandler _errorHandler;

	// Observable properties for binding to UI elements.
	[ObservableProperty]
	private string _name = string.Empty; // The name of the project.

	[ObservableProperty]
	private string _description = string.Empty; // Description of the project.

	[ObservableProperty]
	private List<ProjectTask> _tasks = []; // List of tasks related to the project.

	[ObservableProperty]
	private List<Category> _categories = []; // Categories associated with the project.

	[ObservableProperty]
	private Category? _category; // Selected category for the project.

	[ObservableProperty]
	private int _categoryIndex = -1; // Index of the selected category.

	[ObservableProperty]
	private List<Tag> _allTags = []; // List of all tags available for the project.

	[ObservableProperty]
	private string _icon = FluentUI.ribbon_24_regular; // Icon for the project, default is a ribbon icon.

	[ObservableProperty]
	bool _isBusy; // Indicates whether the page is busy (e.g., loading data).

	[ObservableProperty]
	private List<string> _icons = // List of available icons for the project.
	[
		FluentUI.ribbon_24_regular,
		FluentUI.ribbon_star_24_regular,
		FluentUI.trophy_24_regular,
		FluentUI.badge_24_regular,
		FluentUI.book_24_regular,
		FluentUI.people_24_regular,
		FluentUI.bot_24_regular
	];

	// Property to check if the project has completed tasks.
	public bool HasCompletedTasks
		=> _project?.Tasks.Any(t => t.IsCompleted) ?? false;

	// Constructor to inject repositories and error handler for managing data and errors.
	public ProjectDetailPageModel(ProjectRepository projectRepository, TaskRepository taskRepository, CategoryRepository categoryRepository, TagRepository tagRepository, ModalErrorHandler errorHandler)
	{
		_projectRepository = projectRepository;
		_taskRepository = taskRepository;
		_categoryRepository = categoryRepository;
		_tagRepository = tagRepository;
		_errorHandler = errorHandler;

		// Initialize tasks list as empty.
		Tasks = [];
	}

	// Method to handle query attributes (e.g., ID or refresh flag passed from the navigation).
	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		// If the query contains an "id", load the project data for the specified ID.
		if (query.ContainsKey("id"))
		{
			int id = Convert.ToInt32(query["id"]);
			LoadData(id).FireAndForgetSafeAsync(_errorHandler);
		}
		// If the query contains a "refresh", refresh the current project data.
		else if (query.ContainsKey("refresh"))
		{
			RefreshData().FireAndForgetSafeAsync(_errorHandler);
		}
		// If no relevant query attributes, just load categories and tags and initialize an empty project.
		else
		{
			Task.WhenAll(LoadCategories(), LoadTags()).FireAndForgetSafeAsync(_errorHandler);
			_project = new();
			_project.Tags = [];
			_project.Tasks = [];
			Tasks = _project.Tasks;
		}
	}

	// Asynchronous method to load categories from the category repository.
	private async Task LoadCategories() =>
		Categories = await _categoryRepository.ListAsync();

	// Asynchronous method to load tags from the tag repository.
	private async Task LoadTags() =>
		AllTags = await _tagRepository.ListAsync();

	// Method to refresh the project tasks from the task repository.
	// If the project is new, it doesn't attempt to refresh the tasks.
	private async Task RefreshData()
	{
		// If the project is null or new, just reset the tasks list.
		if (_project.IsNullOrNew())
		{
			if (_project is not null)
				Tasks = new(_project.Tasks);

			return;
		}

		// Otherwise, fetch the tasks for the project from the repository.
		Tasks = await _taskRepository.ListAsync(_project.ID);
		_project.Tasks = Tasks;
	}

	// Asynchronous method to load data for a project by ID.
	private async Task LoadData(int id)
	{
		try
		{
			IsBusy = true;

			// Fetch the project by ID from the repository.
			_project = await _projectRepository.GetAsync(id);

			// If the project is not found or is new, handle the error and return.
			if (_project.IsNullOrNew())
			{
				_errorHandler.HandleError(new Exception($"Project with id {id} could not be found."));
				return;
			}

			// Populate observable properties with the project data.
			Name = _project.Name;
			Description = _project.Description;
			Tasks = _project.Tasks;

			Icon = _project.Icon;

			// Load categories and set the selected category for the project.
			Categories = await _categoryRepository.ListAsync();
			Category = Categories?.FirstOrDefault(c => c.ID == _project.CategoryID);
			CategoryIndex = Categories?.FindIndex(c => c.ID == _project.CategoryID) ?? -1;

			// Load all tags and mark the ones already assigned to the project as selected.
			var allTags = await _tagRepository.ListAsync();
			foreach (var tag in allTags)
			{
				tag.IsSelected = _project.Tags.Any(t => t.ID == tag.ID);
			}
			AllTags = new(allTags);
		}
		catch (Exception e)
		{
			_errorHandler.HandleError(e);
		}
		finally
		{
			IsBusy = false;
			// Notify that the completed tasks property has changed.
			OnPropertyChanged(nameof(HasCompletedTasks));
		}
	}

	// Command to mark a task as completed and save it.
	[RelayCommand]
	private async Task TaskCompleted(ProjectTask task)
	{
		await _taskRepository.SaveItemAsync(task);
		// Notify that the completed tasks property has changed.
		OnPropertyChanged(nameof(HasCompletedTasks));
	}

	// Command to save the project data.
	[RelayCommand]
	private async Task Save()
	{
		// Handle the case where the project is null.
		if (_project is null)
		{
			_errorHandler.HandleError(
				new Exception("Project is null. Cannot Save."));
			return;
		}

		// Update the project properties with the values from the UI.
		_project.Name = Name;
		_project.Description = Description;
		_project.CategoryID = Category?.ID ?? 0;
		_project.Icon = Icon ?? FluentUI.ribbon_24_regular;
		// Save the project to the repository.
		await _projectRepository.SaveItemAsync(_project);

		// If the project is new, save any selected tags.
		if (_project.IsNullOrNew())
		{
			foreach (var tag in AllTags)
			{
				if (tag.IsSelected)
				{
					await _tagRepository.SaveItemAsync(tag, _project.ID);
				}
			}
		}

		// Save any new tasks associated with the project.
		foreach (var task in _project.Tasks)
		{
			if (task.ID == 0)
			{
				task.ProjectID = _project.ID;
				await _taskRepository.SaveItemAsync(task);
			}
		}

		// Navigate back to the previous page and show a success message.
		await Shell.Current.GoToAsync("..");
		await AppShell.DisplayToastAsync("Project saved");
	}

	// Command to add a new task to the project.
	[RelayCommand]
	private async Task AddTask()
	{
		// If the project is null, show an error.
		if (_project is null)
		{
			_errorHandler.HandleError(
				new Exception("Project is null. Cannot navigate to task."));
			return;
		}

		// Navigate to the task detail page, passing the project so it can be associated with the task.
		await Shell.Current.GoToAsync($"task",
			new ShellNavigationQueryParameters(){
				{TaskDetailPageModel.ProjectQueryKey, _project}
			});
	}

	// Command to delete the current project.
	[RelayCommand]
	private async Task Delete()
	{
		// If the project is new or null, just navigate back.
		if (_project.IsNullOrNew())
		{
			await Shell.Current.GoToAsync("..");
			return;
		}

		// Otherwise, delete the project from the repository.
		await _projectRepository.DeleteItemAsync(_project);
		await Shell.Current.GoToAsync("..");
		await AppShell.DisplayToastAsync("Project deleted");
	}

	// Command to navigate to a specific task's detail page.
	[RelayCommand]
	private Task NavigateToTask(ProjectTask task) =>
		Shell.Current.GoToAsync($"task?id={task.ID}");

	// Command to toggle a tag's selection state.
	[RelayCommand]
	private async Task ToggleTag(Tag tag)
	{
		// Toggle the tag's selection state.
		tag.IsSelected = !tag.IsSelected;

		// If the project is not new, update the tag association in the repository.
		if (!_project.IsNullOrNew())
		{
			if (tag.IsSelected)
			{
				await _tagRepository.SaveItemAsync(tag, _project.ID);
			}
			else
			{
				await _tagRepository.DeleteItemAsync(tag, _project.ID);
			}
		}

		// Refresh the tags list to reflect any changes.
		AllTags = new(AllTags);
	}

	// Command to clean up completed tasks (delete them).
	[RelayCommand]
	private async Task CleanTasks()
	{
		// Find all completed tasks.
		var completedTasks = Tasks.Where(t => t.IsCompleted).ToArray();
		foreach (var task in completedTasks)
		{
			// Delete completed tasks from the repository and remove them from the list.
			await _taskRepository.DeleteItemAsync(task);
			Tasks.Remove(task);
		}

		// Refresh the tasks list.
		Tasks = new(Tasks);
		// Notify that the completed tasks property has changed.
		OnPropertyChanged(nameof(HasCompletedTasks));
		// Show a success message.
		await AppShell.DisplayToastAsync("All cleaned up!");
	}
}