namespace ITSF_App.Services;

/// <summary>
/// Modal Error Handler.
/// A service for handling and displaying error alerts in the UI.
/// </summary>
public class ModalErrorHandler : IErrorHandler
{
    // Semaphore to ensure thread safety when displaying the alert
	SemaphoreSlim _semaphore = new(1, 1);

    /// <summary>
    /// Handle error in UI by displaying an alert.
    /// This method is the entry point for error handling.
    /// </summary>
    /// <param name="ex">The exception to handle and display.</param>
	public void HandleError(Exception ex)
	{
        // Asynchronously display the error alert
		DisplayAlert(ex).FireAndForgetSafeAsync();
	}

    // Asynchronously displays the error alert in the UI
	async Task DisplayAlert(Exception ex)
	{
		try
		{
            // Wait until it's safe to show the alert (semaphore ensures no concurrent access)
			await _semaphore.WaitAsync();

            // Display an alert with the error message
			if (Shell.Current is Shell shell)
				await shell.DisplayAlert("Error", ex.Message, "OK");
		}
		finally
		{
            // Release the semaphore once the alert is displayed
			_semaphore.Release();
		}
	}
}