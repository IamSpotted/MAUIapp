namespace ITSF_App.Models;

// CategoryChartData represents the data for displaying a chart related to categories.
public class CategoryChartData
{
	// Title of the category for display (e.g., "Work", "Personal").
	public string Title { get; set; } = string.Empty;

	// The count associated with the category (e.g., number of tasks in this category).
	public int Count { get; set; }

	// Constructor to initialize the CategoryChartData with a title and count.
	public CategoryChartData(string title, int count)
	{
		Title = title;  // Sets the category title.
		Count = count;  // Sets the count for the category.
	}
}
