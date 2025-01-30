using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;

namespace ITSF_App;

public static class MauiProgram
{
    /// <summary>
    /// The main method for setting up and configuring the MauiApp.
    /// </summary>
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>() // Use the main App class for the application
			.UseMauiCommunityToolkit() // Integrate the Community Toolkit for Maui
			.ConfigureSyncfusionToolkit() // Configure Syncfusion's Maui toolkit (for additional UI controls)
			.ConfigureMauiHandlers(handlers =>
			{
				// Configure custom handlers for Maui components if needed
			})
			.ConfigureFonts(fonts =>
			{
				// Register custom fonts used throughout the app
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("SegoeUI-Semibold.ttf", "SegoeSemibold");
				fonts.AddFont("FluentSystemIcons-Regular.ttf", FluentUI.FontFamily); // Register Fluent UI icons
			});

#if DEBUG
		// Enable logging for debugging purposes when in debug mode
		builder.Logging.AddDebug();
		builder.Services.AddLogging(configure => configure.AddDebug());
#endif

        // Register singleton services for dependency injection
		builder.Services.AddSingleton<ProjectRepository>();
		builder.Services.AddSingleton<TaskRepository>();
		builder.Services.AddSingleton<CategoryRepository>();
		builder.Services.AddSingleton<TagRepository>();
		builder.Services.AddSingleton<SeedDataService>();
		builder.Services.AddSingleton<ModalErrorHandler>();
		builder.Services.AddSingleton<MainPageModel>();
		builder.Services.AddSingleton<ProjectListPageModel>();
		builder.Services.AddSingleton<ManageMetaPageModel>();

        // Register pages with Shell routing
		builder.Services.AddTransientWithShellRoute<ProjectDetailPage, ProjectDetailPageModel>("project");
		builder.Services.AddTransientWithShellRoute<TaskDetailPage, TaskDetailPageModel>("task");
		
		// Build and return the configured MauiApp
		return builder.Build();
	}
}
