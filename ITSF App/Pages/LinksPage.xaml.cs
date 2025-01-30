namespace ITSF_App.Pages; // Declaring the namespace for the ITSF_App Pages

public partial class ProjectListPage : ContentPage
{
    // Constructor for the ProjectListPage that accepts a ProjectListPageModel as a parameter
	public ProjectListPage(ProjectListPageModel model)
	{
        // Sets the BindingContext of this page to the provided model (ViewModel)
		BindingContext = model;

        // Initializes the page's components (loads the XAML file, sets up UI elements)
		InitializeComponent();
	}
}