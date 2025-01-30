using ITSF_App.Models;

namespace ITSF_App.Pages
{
    // This is the code-behind for the ProjectDetailPage in your MAUI app.
    public partial class ProjectDetailPage : ContentPage
    {
        // Constructor that initializes the page and sets up the binding context with the model.
        public ProjectDetailPage(ProjectDetailPageModel model)
        {
            InitializeComponent();
            BindingContext = model;  // Bind the view model to the page's UI elements.
        }
    }

    // A custom DataTemplateSelector class to handle the selection of templates for the tags.
    public class ChipDataTemplateSelector : DataTemplateSelector
    {
        // The required data templates for selected and normal tags.
        public required DataTemplate SelectedTagTemplate { get; set; }
        public required DataTemplate NormalTagTemplate { get; set; }

        // Overriding the OnSelectTemplate method to choose the correct template for a tag.
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            // Check if the tag is selected and return the appropriate template.
            return (item as Tag)?.IsSelected ?? false ? SelectedTagTemplate : NormalTagTemplate;
        }
    }
}