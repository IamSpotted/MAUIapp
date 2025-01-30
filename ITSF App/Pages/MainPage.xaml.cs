using ITSF_App.Models; // Importing the Models namespace, which likely contains the data models used in the page
using ITSF_App.PageModels; // Importing the PageModels namespace, which contains the ViewModel for this page

namespace ITSF_App.Pages; // Declaring the namespace for the pages of the ITSF_App

public partial class MainPage : ContentPage
{
    // Constructor for MainPage class that accepts a MainPageModel as a parameter
    public MainPage(MainPageModel model)
    {
        InitializeComponent(); // Initializes the components (loads XAML file, sets up UI elements)

        // Sets the BindingContext of this page to the provided model (ViewModel)
        BindingContext = model;
    }
}