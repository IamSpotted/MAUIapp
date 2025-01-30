using System.Text.Json;
using ITSF_App.Models;
using Microsoft.Extensions.Logging;

namespace ITSF_App.Data;

public class SeedDataService
{
    // Repositories for interacting with the database
	private readonly ProjectRepository _projectRepository;
	private readonly TaskRepository _taskRepository;
	private readonly TagRepository _tagRepository;
	private readonly CategoryRepository _categoryRepository;

    // Path to the seed data JSON file
	private readonly string _seedDataFilePath = "SeedData.json";

    // Logger for logging errors and info
	private readonly ILogger<SeedDataService> _logger;

    // Constructor to initialize the repositories and logger
	public SeedDataService(ProjectRepository projectRepository, TaskRepository taskRepository, TagRepository tagRepository, CategoryRepository categoryRepository, ILogger<SeedDataService> logger)
	{
		_projectRepository = projectRepository;
		_taskRepository = taskRepository;
		_tagRepository = tagRepository;
		_categoryRepository = categoryRepository;
		_logger = logger;
	}

    // Main method to load seed data asynchronously
	public async Task LoadSeedDataAsync()
	{
        // Clears any existing data before seeding
		ClearTables();

        // Open the seed data JSON file as a stream
		await using Stream templateStream = await FileSystem.OpenAppPackageFileAsync(_seedDataFilePath);

        // Declare variable to hold the deserialized seed data
		ProjectsJson? payload = null;

        // Try to deserialize the seed data file into the ProjectsJson object
		try
		{
			payload = JsonSerializer.Deserialize(templateStream, JsonContext.Default.ProjectsJson);
		}
		catch (Exception e)
		{
            // Log error if there is a problem deserializing
			_logger.LogError(e, "Error deserializing seed data");
		}

        // Process the payload if it is successfully deserialized
		try
		{
			if (payload is not null)
			{
                // Loop through each project in the seed data
				foreach (var project in payload.Projects)
				{
                    // Skip if the project is null
					if (project is null)
					{
						continue;
					}

                    // Save the category if it exists and set the category ID for the project
					if (project.Category is not null)
					{
						await _categoryRepository.SaveItemAsync(project.Category);
						project.CategoryID = project.Category.ID;
					}

                    // Save the project to the repository
					await _projectRepository.SaveItemAsync(project);

                    // If the project has tasks, save them as well
					if (project?.Tasks is not null)
					{
						foreach (var task in project.Tasks)
						{
							task.ProjectID = project.ID;
							await _taskRepository.SaveItemAsync(task);
						}
					}

                    // If the project has tags, save them as well
					if (project?.Tags is not null)
					{
						foreach (var tag in project.Tags)
						{
							await _tagRepository.SaveItemAsync(tag, project.ID);
						}
					}
				}
			}
		}
		catch (Exception e)
		{
            // Log error if there is a problem saving seed data
			_logger.LogError(e, "Error saving seed data");
			throw;
		}
	}

    // Clears the existing data from the tables
	private async void ClearTables()
	{
		try
		{
            // Drops all tables asynchronously
			await Task.WhenAll(
				_projectRepository.DropTableAsync(),
				_taskRepository.DropTableAsync(),
				_tagRepository.DropTableAsync(),
				_categoryRepository.DropTableAsync());
		}
		catch (Exception e)
		{
            // Log any exception that occurs during table clearing
			Console.WriteLine(e);
		}
	}
}