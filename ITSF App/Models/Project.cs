using System.Text.Json.Serialization;

namespace ITSF_App.Models;

// Represents a project with various properties and related data.
public class Project
{
	// Unique identifier for the project.
	public int ID { get; set; }

	// The name of the project (e.g., "Website Development").
	public string Name { get; set; } = string.Empty;

	// A description of the project to provide additional details.
	public string Description { get; set; } = string.Empty;

	// The icon associated with the project (e.g., an image URL or file name).
	public string Icon { get; set; } = string.Empty;

	// The ID of the category the project belongs to.
	// This is ignored during JSON serialization (to avoid circular references).
	[JsonIgnore]
	public int CategoryID { get; set; }

	// The category that this project belongs to, can be null if not assigned.
	public Category? Category { get; set; }

	// List of tasks associated with this project.
	public List<ProjectTask> Tasks { get; set; } = [];

	// List of tags associated with this project.
	public List<Tag> Tags { get; set; } = [];

	// Provides a string representation of the project, returning its name.
	public override string ToString() => $"{Name}";
}

// A wrapper class for a collection of projects, used for JSON serialization.
public class ProjectsJson
{
	// A list of projects contained in the wrapper.
	public List<Project> Projects { get; set; } = [];
}