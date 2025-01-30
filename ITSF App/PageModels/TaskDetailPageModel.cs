using CommunityToolkit.Mvvm.ComponentModel; // Provides ObservableObject for property change notifications.
using CommunityToolkit.Mvvm.Input; // Provides RelayCommand for simple command binding.
using ITSF_App.Data; // Namespace for data access layers like ProjectRepository and TaskRepository.
using ITSF_App.Models; // Namespace for domain models like Project, ProjectTask.
using ITSF_App.Services; // Namespace for services, including error handling.

namespace ITSF_App.PageModels;

// TaskDetailPageModel is the ViewModel for the page that shows details of a task.
public partial class TaskDetailPageModel : ObservableObject, IQueryAttributable
{
	// Constant key to reference project data in the query string.
	public const string ProjectQueryKey = "project";

	// Private fields for task details and state tracking.
	private ProjectTask? _task; // The current task being edited/viewed.
	private bool _canDelete; // Flag to determine if the task can be deleted.
	private readonly ProjectRepository _projectRepository; // Repository for project data.
	private readonly TaskRepository _taskRepository; // Repository for task data.
	private readonly ModalErrorHandler _errorHandler; // Handler for error display and logging.

	// Observable properties bound to the UI to reflect task details and state.
	[ObservableProperty]
	private string _title = string.Empty; // Title of the task.

	[ObservableProperty]
	private bool _isCompleted; // Whether the task is marked as completed.

	[ObservableProperty]
	private List<Project> _projects = []; // List of projects for task assignment.

	[ObservableProperty]
	private Project? _project; // The project associated with this task.

	[ObservableProperty]
	private int _selectedProjectIndex = -1; // Index of the selected project.

	[ObservableProperty]
	private bool _isExistingProject; // Whether the selected project is an existing one.

	// Constructor to inject necessary repositories and error handler.
	public TaskDetailPageModel(ProjectRepository projectRepository, TaskRepository taskRepository, ModalErrorHandler errorHandler)
	{
		_projectRepository = projectRepository;
		_taskRepository = taskRepository;
		_errorHandler = errorHandler;
	}

	// This method is called when the page is loaded with query parameters (like task ID).
	public void ApplyQueryAttributes(IDictionary<string, object> query)
	{
		// Start loading task data asynchronously based on query parameters.
		LoadTaskAsync(query).FireAndForgetSafeAsync(_errorHandler);
	}

	// Async method to load task data, including project assignment and task details.
	private async Task LoadTaskAsync(IDictionary<string, object> query)
	{
		// Check if the query contains project information.
		if (query.TryGetValue(ProjectQueryKey, out var project))
			Project = (Project)project;

		int taskId = 0;

		// If task ID is provided, load the task from the repository.
		if (query.ContainsKey("id"))
		{
			taskId = Convert.ToInt32(query["id"]);
			_task = await _taskRepository.GetAsync(taskId);

			if (_task is null)
			{
				// Handle error if task with the given ID is not found.
				_errorHandler.HandleError(new Exception($"Task Id {taskId} isn't valid."));
				return;
			}

			// Load the associated project for the task.
			Project = await _projectRepository.GetAsync(_task.ProjectID);
		}
		else
		{
			// If no task ID is provided, create a new task.
			_task = new ProjectTask();
		}

		// If the project is new, don't load the project dropdown.
		if (Project?.ID == 0)
        {
            IsExistingProject = false;
		}
		else
        {
            // Load the list of projects for task assignment.
            Projects = await _projectRepository.ListAsync();
            IsExistingProject = true;
		}

		// Set the selected project index based on the current task's project ID.
		if (Project is not null)
			SelectedProjectIndex = Projects.FindIndex(p => p.ID == Project.ID);
		else if (_task?.ProjectID > 0)
			SelectedProjectIndex = Projects.FindIndex(p => p.ID == _task.ProjectID);

		// If task ID is valid, populate task details (title, completion status).
		if (taskId > 0)
		{
			if (_task is null)
			{
				_errorHandler.HandleError(new Exception($"Task with id {taskId} could not be found."));
				return;
			}

			Title = _task.Title;
			IsCompleted = _task.IsCompleted;
			CanDelete = true;
		}
		else
		{
			// Initialize a new task if no valid task is found.
			_task = new ProjectTask()
			{
				ProjectID = Project?.ID ?? 0
			};
		}
	}

	// Property to determine if the task can be deleted. Updates the CanExecute state of the Delete command.
	public bool CanDelete
	{
		get => _canDelete;
		set
		{
			_canDelete = value;
			DeleteCommand.NotifyCanExecuteChanged(); // Notify the UI to update the delete button state.
		}
	}

	// Command to save the task details (title, completion status, and project assignment).
	[RelayCommand]
	private async Task Save()
	{
		// Ensure that the task is not null before saving.
		if (_task is null)
		{
			_errorHandler.HandleError(
				new Exception("Task or project is null. The task could not be saved."));
			return;
		}

		_task.Title = Title; // Set task title from the UI.

		// Ensure the task is assigned to a valid project.
		int projectId = Project?.ID ?? 0;
		if (Projects.Count > SelectedProjectIndex && SelectedProjectIndex >= 0)
			_task.ProjectID = projectId = Projects[SelectedProjectIndex].ID;

		_task.IsCompleted = IsCompleted; // Set completion status.

		// If this task is newly assigned to a project, add it to the project's task list.
		if (Project?.ID == projectId && !Project.Tasks.Contains(_task))
			Project.Tasks.Add(_task);

		// Save the task to the repository if it has a valid project ID.
		if (_task.ProjectID > 0)
			_taskRepository.SaveItemAsync(_task).FireAndForgetSafeAsync(_errorHandler);

		// Navigate back to the previous page and refresh data.
		await Shell.Current.GoToAsync("..?refresh=true");

		// Show a toast notification indicating the task was saved successfully.
		if (_task.ID > 0)
			await AppShell.DisplayToastAsync("Task saved");
	}

	// Command to delete the current task. Can only execute if CanDelete is true.
	[RelayCommand(CanExecute = nameof(CanDelete))]
	private async Task Delete()
	{
		// Ensure both task and project exist before attempting to delete.
		if (_task is null || Project is null)
		{
			_errorHandler.HandleError(
				new Exception("Task is null. The task could not be deleted."));
			return;
		}

		// Remove the task from the project's task list.
		if (Project.Tasks.Contains(_task))
			Project.Tasks.Remove(_task);

		// Delete the task from the repository.
		if (_task.ID > 0)
			await _taskRepository.DeleteItemAsync(_task);

		// Navigate back to the previous page and refresh data.
		await Shell.Current.GoToAsync("..?refresh=true");

		// Show a toast notification indicating the task was deleted.
		await AppShell.DisplayToastAsync("Task deleted");
	}
}