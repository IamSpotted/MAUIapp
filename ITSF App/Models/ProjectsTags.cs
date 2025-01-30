namespace ITSF_App.Models;

// Represents the relationship between a project and a tag.
// This class is used to track which tags are associated with which projects.
public class ProjectsTags
{
	// Unique identifier for the relationship entry.
	public int ID { get; set; }

	// The ID of the project that the tag is associated with.
	public int ProjectID { get; set; }

	// The ID of the tag associated with the project.
	public int TagID { get; set; }
}
