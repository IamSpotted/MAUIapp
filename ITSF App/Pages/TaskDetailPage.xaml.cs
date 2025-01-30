namespace ITSF_App.Pages
{
    public partial class TaskDetailPage : ContentPage
    {
        // Constructor to initialize the page and bind the view model
        public TaskDetailPage(TaskDetailPageModel model)
        {
            InitializeComponent();
            BindingContext = model; // Setting the view model as the binding context
        }
    }
}