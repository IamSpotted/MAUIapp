namespace ITSF_App.Data;

// A static class to store constants used throughout the application.
// It includes the database filename and path for the SQLite database.
public static class Constants
{
	// The filename for the SQLite database.
	// This file will be used to store application data locally.
	public const string DatabaseFilename = "AppSQLite.db3";

	// A computed property that returns the full path to the SQLite database file.
	// The path is constructed using the app's data directory, making it suitable for the platform.
	// The DatabaseFilename is combined with the app's data directory to form the complete path.
	public static string DatabasePath =>
		$"Data Source={Path.Combine(FileSystem.AppDataDirectory, DatabaseFilename)}";
}
