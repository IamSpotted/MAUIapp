using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel; // Provides ObservableObject base class for automatic property change notifications.
using CommunityToolkit.Mvvm.Input; // Provides RelayCommand to easily bind commands to UI elements.
using ITSF_App.Data; // Namespace containing data-related repositories.
using ITSF_App.Models; // Namespace for domain models (Category, Tag, etc.).
using ITSF_App.Services; // Namespace for services like SeedDataService.

namespace ITSF_App.PageModels;

// ManageMetaPageModel is the ViewModel for managing categories and tags.
public partial class ManageMetaPageModel : ObservableObject
{
	// Repositories for categories and tags to interact with data storage.
	private readonly CategoryRepository _categoryRepository;
	private readonly TagRepository _tagRepository;
    // Service for loading seed data, typically used for populating initial data.
	private readonly SeedDataService _seedDataService;

	// Observable properties to hold the collections of categories and tags.
	// The ObservableCollection is used to allow automatic UI updates when items are added/removed.
	[ObservableProperty]
	private ObservableCollection<Category> _categories = []; // Initialize as empty collection

	[ObservableProperty]
	private ObservableCollection<Tag> _tags = []; // Initialize as empty collection

	// Constructor to inject the repositories and seed data service.
	public ManageMetaPageModel(CategoryRepository categoryRepository, TagRepository tagRepository, SeedDataService seedDataService)
	{
		_categoryRepository = categoryRepository;
		_tagRepository = tagRepository;
        _seedDataService = seedDataService;
    }

	// Asynchronous method to load categories and tags from the repositories.
	// It fetches the list of categories and tags and updates the observable collections.
	private async Task LoadData()
	{
		// Retrieve categories from the repository and populate the _categories collection.
		var categoriesList = await _categoryRepository.ListAsync();
		Categories = new ObservableCollection<Category>(categoriesList);

		// Retrieve tags from the repository and populate the _tags collection.
		var tagsList = await _tagRepository.ListAsync();
		Tags = new ObservableCollection<Tag>(tagsList);
	}

	// Command to load the data when the page appears.
	// This command is triggered when the page is displayed.
	[RelayCommand]
	private Task Appearing()
		=> LoadData(); // Calls LoadData to populate categories and tags.

	// Command to save the categories to the repository.
	// Iterates over the categories and saves each one.
	[RelayCommand]
	private async Task SaveCategories()
	{
		// Loop through each category and save it.
		foreach (var category in Categories)
		{
			await _categoryRepository.SaveItemAsync(category);
		}

		// Show a success message once categories are saved.
		await AppShell.DisplayToastAsync("Categories saved");
	}

	// Command to delete a category.
	// Removes the category from the observable collection and deletes it from the repository.
	[RelayCommand]
	private async Task DeleteCategory(Category category)
	{
		// Remove category from the observable collection, which automatically updates the UI.
		Categories.Remove(category);
		// Delete the category from the repository.
		await _categoryRepository.DeleteItemAsync(category);
		// Display a toast notification confirming the deletion.
		await AppShell.DisplayToastAsync("Category deleted");
	}

	// Command to add a new category.
	// Creates a new category object, adds it to the collection, and saves it to the repository.
	[RelayCommand]
	private async Task AddCategory()
	{
		var category = new Category(); // Create a new, empty category.
		Categories.Add(category); // Add the new category to the collection.
		// Save the new category to the repository.
		await _categoryRepository.SaveItemAsync(category);
		// Display a toast notification confirming the addition.
		await AppShell.DisplayToastAsync("Category added");
	}

	// Command to save tags to the repository.
	// Similar to SaveCategories, it iterates over the tags and saves each one.
	[RelayCommand]
	private async Task SaveTags()
	{
		// Loop through each tag and save it.
		foreach (var tag in Tags)
		{
			await _tagRepository.SaveItemAsync(tag);
		}

		// Show a success message once tags are saved.
		await AppShell.DisplayToastAsync("Tags saved");
	}

	// Command to delete a tag.
	// Removes the tag from the observable collection and deletes it from the repository.
	[RelayCommand]
	private async Task DeleteTag(Tag tag)
	{
		// Remove the tag from the collection, updating the UI.
		Tags.Remove(tag);
		// Delete the tag from the repository.
		await _tagRepository.DeleteItemAsync(tag);
		// Show a toast notification confirming the deletion.
		await AppShell.DisplayToastAsync("Tag deleted");
	}

	// Command to add a new tag.
	// Creates a new tag object, adds it to the collection, and saves it to the repository.
	[RelayCommand]
	private async Task AddTag()
	{
		var tag = new Tag(); // Create a new, empty tag.
		Tags.Add(tag); // Add the new tag to the collection.
		// Save the new tag to the repository.
		await _tagRepository.SaveItemAsync(tag);
		// Display a toast notification confirming the addition.
		await AppShell.DisplayToastAsync("Tag added");
	}

	// Command to reset the data and re-seed it using seed data service.
	// This will clear the stored "is_seeded" preference and reload the seed data.
	[RelayCommand]
	private async Task Reset()
	{
		// Remove the "is_seeded" flag from preferences, indicating the need to reload data.
		Preferences.Default.Remove("is_seeded");
        // Load seed data from the service.
        await _seedDataService.LoadSeedDataAsync();
        // Set the "is_seeded" flag to true in preferences to indicate data has been re-seeded.
        Preferences.Default.Set("is_seeded", true);
        // Navigate to the main page after the reset is complete.
        await Shell.Current.GoToAsync("//main");
	}
}