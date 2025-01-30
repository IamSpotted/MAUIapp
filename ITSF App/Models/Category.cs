using System.Text.Json.Serialization; // Namespace for JSON serialization and deserialization features.

namespace ITSF_App.Models;

// Category represents a task category with a title and color.
public class Category
{
	// Unique identifier for the category.
	public int ID { get; set; }

	// Title of the category (e.g., "Work", "Personal").
	public string Title { get; set; } = string.Empty;

	// Color associated with the category, in hexadecimal format (default is red).
	public string Color { get; set; } = "#FF0000";

	// Property that provides a Brush object for the category color.
	// This property is ignored during JSON serialization.
	[JsonIgnore]
	public Brush ColorBrush
	{
		// Converts the Color property (hex) into a SolidColorBrush for UI rendering.
		get
		{
			return new SolidColorBrush(Microsoft.Maui.Graphics.Color.FromArgb(Color));
		}
	}

	// Custom string representation for the Category instance.
	// It returns the category's title when converted to a string.
	public override string ToString() => $"{Title}";
}