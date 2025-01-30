using System.Text.Json.Serialization;
using CommunityToolkit.Maui.Core.Extensions;

namespace ITSF_App.Models;

// Represents a tag that can be applied to projects or tasks.
// Each tag has a title, color, and selection state.
public class Tag
{
	// Unique identifier for the tag.
	public int ID { get; set; }

	// The title or name of the tag.
	public string Title { get; set; } = string.Empty;

	// The color associated with the tag in hex format.
	public string Color { get; set; } = "#FF0000"; // Default red color.

	// Property that provides a Brush representation of the tag's color.
	// This is used for visual display in the UI.
	[JsonIgnore]
	public Brush ColorBrush
	{
		get
		{
			// Converts the hex color code into a SolidColorBrush for UI use.
			return new SolidColorBrush(Microsoft.Maui.Graphics.Color.FromArgb(Color));
		}
	}

	// Property to get the actual display color from the hex code.
	// It returns a Color object used for UI rendering.
	[JsonIgnore]
	public Color DisplayColor
	{
		get
		{
			// Converts the hex string into a Color object for rendering.
			return Microsoft.Maui.Graphics.Color.FromArgb(Color);
		}
	}

	// Property that returns a darker version of the tag color.
	// Useful for providing a hover or pressed state in the UI.
	[JsonIgnore]
	public Color DisplayDarkColor
	{
		get
		{
			// Darkens the color by decreasing its brightness.
			return DisplayColor.WithBlackKey(0.8); // Adjusts color brightness.
		}
	}

	// Property that returns a lighter version of the tag color.
	// Can be used for states like focus or when a tag is selected.
	[JsonIgnore]
	public Color DisplayLightColor
	{
		get
		{
			// Lightens the color by increasing its brightness.
			return DisplayColor.WithBlackKey(0.2); // Adjusts color brightness.
		}
	}

	// Flag to indicate whether the tag is selected by the user.
	[JsonIgnore]
	public bool IsSelected { get; set; }
}