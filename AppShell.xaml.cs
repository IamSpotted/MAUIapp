// Importing necessary libraries from CommunityToolkit and Maui
using CommunityToolkit.Maui.Alerts; // Provides Snackbar and Toast functionalities
using CommunityToolkit.Maui.Core; // Core functionalities for Maui Toolkit
using Font = Microsoft.Maui.Font; // Aliasing Microsoft.Maui.Font for easy use

namespace ITSF_App; // Defines the namespace for the app

// Defining the partial class AppShell that inherits from Shell
public partial class AppShell : Shell
{
    // Constructor for the AppShell class
    public AppShell()
    {
        // Initializes the components (typically linked to XAML elements)
        InitializeComponent(); 

        // Retrieves the current theme of the application (Light or Dark)
        var currentTheme = Application.Current!.UserAppTheme;

        // Sets the selected index of the segmented control based on the current theme
        // If Light theme is active, select the first item (index 0), otherwise select the second item (index 1)
        ThemeSegmentedControl.SelectedIndex = currentTheme == AppTheme.Light ? 0 : 1;
    }

    // Static method to display a Snackbar with a given message
    public static async Task DisplaySnackbarAsync(string message)
    {
        // Creates a cancellation token to allow for cancelling the Snackbar display if needed
        CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        // Defining the options for the Snackbar (appearance and behavior)
        var snackbarOptions = new SnackbarOptions
        {
            BackgroundColor = Color.FromArgb("#FF3300"), // Sets a red background color for the Snackbar
            TextColor = Colors.White, // Sets the text color to white
            ActionButtonTextColor = Colors.Yellow, // Sets the action button's text color to yellow
            CornerRadius = new CornerRadius(0), // Sets the corner radius (0 means square corners)
            Font = Font.SystemFontOfSize(18), // Sets the font size for the text in the Snackbar
            ActionButtonFont = Font.SystemFontOfSize(14) // Sets the font size for the action button text
        };

        // Creating the Snackbar with the specified message and options
        var snackbar = Snackbar.Make(message, visualOptions: snackbarOptions);

        // Displaying the Snackbar asynchronously and waiting for it to finish
        await snackbar.Show(cancellationTokenSource.Token);
    }

    // Static method to display a Toast with a given message
    public static async Task DisplayToastAsync(string message)
    {
        // Toast is currently not supported in MCT (Mobile/Tablet) for Windows
        // This check ensures that no errors occur when running on Windows
        if (OperatingSystem.IsWindows())
            return;

        // Creating a Toast with the specified message and setting the text size to 18
        var toast = Toast.Make(message, textSize: 18);

        // Creating a CancellationTokenSource with a timeout of 5 seconds
        var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

        // Displaying the Toast asynchronously and waiting for it to finish
        await toast.Show(cts.Token);
    }

    // Event handler for when the selection in the SegmentedControl changes
    private void SfSegmentedControl_SelectionChanged(object sender, Syncfusion.Maui.Toolkit.SegmentedControl.SelectionChangedEventArgs e)
    {
        // Changing the application's theme based on the selected index in the SegmentedControl
        // If the first segment (index 0) is selected, set the theme to Light, otherwise Dark
        Application.Current!.UserAppTheme = e.NewIndex == 0 ? AppTheme.Light : AppTheme.Dark;
    }
}