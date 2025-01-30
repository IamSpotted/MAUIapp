namespace ITSF_App.Pages
{
    // The ManageMetaPage is a ContentPage that represents the UI for managing categories and tags.
    // It is initialized with a ViewModel (ManageMetaPageModel) for data binding and interaction.
    public partial class ManageMetaPage : ContentPage
    {
        // Constructor for the ManageMetaPage
        // The ManageMetaPageModel is passed as a parameter and used to set the BindingContext of the page.
        public ManageMetaPage(ManageMetaPageModel model)
        {
            // Initialize the components of the page (XAML layout).
            InitializeComponent();

            // Set the BindingContext for this page, linking it to the provided ViewModel.
            // This allows the page to bind to the ViewModel's properties and commands.
            BindingContext = model;
        }
    }
}