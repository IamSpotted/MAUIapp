using System.Text.Json.Serialization;

namespace ITSF_App.Models;

// Represents a task within a project.
// Each task has a title, completion status, and a link to the project it belongs to.
public class ProjectTask
{
	// Unique identifier for the task.
	public int ID { get; set; }

	// Title or name of the task.
	public string Title { get; set; } = string.Empty;

	// Indicates whether the task is completed or not.
	public bool IsCompleted { get; set; }

	// The ID of the project that this task is associated with.
	// This is used for linking the task to a specific project.
	[JsonIgnore]
	public int ProjectID { get; set; }
}
