#nullable disable // Disables nullable reference type checking for the entire file.

using CommunityToolkit.Mvvm.ComponentModel; // Provides ObservableObject for property change notifications.
using CommunityToolkit.Mvvm.Input; // Provides RelayCommand for simple command binding in the UI.
using ITSF_App.Data; // Namespace for data-related classes like ProjectRepository.
using ITSF_App.Models; // Namespace for domain models like Project.
using ITSF_App.Services; // Namespace for services (like navigation and error handling).

namespace ITSF_App.PageModels;

// ProjectListPageModel is the ViewModel responsible for managing the project list page.
public partial class ProjectListPageModel : ObservableObject
{
	// Repository for fetching project data.
	private readonly ProjectRepository _projectRepository;

	// Observable property for binding to a list of projects in the UI.
	[ObservableProperty]
	private List<Project> _projects = []; // Default to an empty list of projects.

	// Constructor to inject the ProjectRepository for fetching project data.
	public ProjectListPageModel(ProjectRepository projectRepository)
	{
		_projectRepository = projectRepository; // Initialize the repository.
	}

	// Command that is executed when the page appears. It loads the list of projects from the repository.
	[RelayCommand]
	private async Task Appearing()
	{
		// Fetch the list of projects from the repository and update the observable property.
		Projects = await _projectRepository.ListAsync();
	}

	// Command to navigate to the project detail page for a specific project when selected.
	[RelayCommand]
	Task NavigateToProject(Project project)
		=> Shell.Current.GoToAsync($"project?id={project.ID}"); // Navigate to project page and pass the project ID.

	// Command to navigate to the page where a new project can be added.
	[RelayCommand]
	async Task AddProject()
	{
		// Navigate to the project creation page.
		await Shell.Current.GoToAsync($"project");
	}
}