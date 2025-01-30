using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ITSF_App.Models;

namespace ITSF_App.PageModels;

// MainPageModel serves as the ViewModel for the main page, managing state, data retrieval, and commands.
public partial class MainPageModel : ObservableObject, IProjectTaskPageModel
{
    private bool _isNavigatedTo; // Tracks if the user has navigated to this page
    private bool _dataLoaded; // Ensures data is only loaded once
    private readonly ProjectRepository _projectRepository;
    private readonly TaskRepository _taskRepository;
    private readonly CategoryRepository _categoryRepository;
    private readonly ModalErrorHandler _errorHandler;
    private readonly SeedDataService _seedDataService;
    private Timer _timer; // Timer to update the current time every second

    // Observable properties that notify UI when values change

    [ObservableProperty]
    private List<CategoryChartData> _todoCategoryData = []; // Stores chart data for task categories

    [ObservableProperty]
    private List<Brush> _todoCategoryColors = []; // Stores color representations for each category in the chart

    [ObservableProperty]
    private List<ProjectTask> _tasks = []; // Stores the list of tasks

    [ObservableProperty]
    private List<Project> _projects = []; // Stores the list of projects

    [ObservableProperty]
    bool _isBusy; // Indicates if a data operation is in progress

    [ObservableProperty]
    bool _isRefreshing; // Indicates if the UI is refreshing data

    [ObservableProperty]
    private string _today = DateTime.Now.ToString("dddd, MMM d, hh:mm:ss"); // Stores the current date and time

    // Property to check if there are any completed tasks
    public bool HasCompletedTasks
        => Tasks?.Any(t => t.IsCompleted) ?? false;

    // Constructor initializes dependencies and starts a timer for live clock updates
    public MainPageModel(SeedDataService seedDataService, ProjectRepository projectRepository,
        TaskRepository taskRepository, CategoryRepository categoryRepository, ModalErrorHandler errorHandler)
    {
        _projectRepository = projectRepository;
        _taskRepository = taskRepository;
        _categoryRepository = categoryRepository;
        _errorHandler = errorHandler;
        _seedDataService = seedDataService;
        
        // Start the timer to update time every second
        _timer = new Timer(UpdateTime, null, 0, 1000); // 1000ms = 1 second
    }

    // Updates the "Today" property every second to reflect the current time
    private void UpdateTime(object? state)
    {
        Today = DateTime.Now.ToString("dddd, MMM d, HH:mm:ss");
    }

    // Loads data for tasks, projects, and categories asynchronously
    private async Task LoadData()
    {
        try
        {
            IsBusy = true; // Show loading state

            Projects = await _projectRepository.ListAsync(); // Fetch projects from the repository

            var chartData = new List<CategoryChartData>();
            var chartColors = new List<Brush>();

            var categories = await _categoryRepository.ListAsync(); // Fetch categories
            foreach (var category in categories)
            {
                chartColors.Add(category.ColorBrush); // Store category colors

                var ps = Projects.Where(p => p.CategoryID == category.ID).ToList();
                int tasksCount = ps.SelectMany(p => p.Tasks).Count(); // Count tasks under this category

                chartData.Add(new(category.Title, tasksCount)); // Add to chart data
            }

            TodoCategoryData = chartData;
            TodoCategoryColors = chartColors;

            Tasks = await _taskRepository.ListAsync(); // Fetch tasks
        }
        finally
        {
            IsBusy = false; // Hide loading state
            OnPropertyChanged(nameof(HasCompletedTasks)); // Notify UI if completed tasks exist
        }
    }

    // Ensures initial seed data is loaded only once
    private async Task InitData(SeedDataService seedDataService)
    {
        bool isSeeded = Preferences.Default.ContainsKey("is_seeded");

        if (!isSeeded)
        {
            await seedDataService.LoadSeedDataAsync(); // Load initial data
        }

        Preferences.Default.Set("is_seeded", true);
        await Refresh(); // Refresh the UI with the latest data
    }

    // Command to refresh data from repositories
    [RelayCommand]
    private async Task Refresh()
    {
        try
        {
            IsRefreshing = true; // Show refresh state
            await LoadData();
        }
        catch (Exception e)
        {
            _errorHandler.HandleError(e); // Handle any errors
        }
        finally
        {
            IsRefreshing = false; // Hide refresh state
        }
    }

    // Command called when navigating to the page
    [RelayCommand]
    private void NavigatedTo() =>
        _isNavigatedTo = true;

    // Command called when navigating away from the page
    [RelayCommand]
    private void NavigatedFrom() =>
        _isNavigatedTo = false;

    // Called when the page appears
    [RelayCommand]
    private async Task Appearing()
    {
        if (!_dataLoaded) // If data isn't loaded yet, initialize it
        {
            await InitData(_seedDataService);
            _dataLoaded = true;
            await Refresh();
        }
        else if (!_isNavigatedTo) // If navigated back, refresh data
        {
            await Refresh();
        }
    }

    // Marks a task as completed and saves it
    [RelayCommand]
    private Task TaskCompleted(ProjectTask task)
    {
        OnPropertyChanged(nameof(HasCompletedTasks)); // Notify UI of completed tasks
        return _taskRepository.SaveItemAsync(task); // Save task state
    }

    // Command to navigate to the "Add Task" page
    [RelayCommand]
    private Task AddTask()
        => Shell.Current.GoToAsync($"task");

    // Command to navigate to a specific project page
    [RelayCommand]
    private Task NavigateToProject(Project project)
        => Shell.Current.GoToAsync($"project?id={project.ID}");

    // Command to navigate to a specific task page
    [RelayCommand]
    private Task NavigateToTask(ProjectTask task)
        => Shell.Current.GoToAsync($"task?id={task.ID}");

    // Deletes all completed tasks from storage and UI
    [RelayCommand]
    private async Task CleanTasks()
    {
        var completedTasks = Tasks.Where(t => t.IsCompleted).ToList(); // Get completed tasks
        foreach (var task in completedTasks)
        {
            await _taskRepository.DeleteItemAsync(task); // Delete from database
            Tasks.Remove(task); // Remove from UI
        }

        OnPropertyChanged(nameof(HasCompletedTasks)); // Notify UI
        Tasks = new(Tasks); // Refresh task list
        await AppShell.DisplayToastAsync("All cleaned up!"); // Show confirmation message
    }
}