using System.Text.Json.Serialization;
using ITSF_App.Models;

// This class is used to define a custom JSON serialization context for the application.
// The JsonContext class is marked with the JsonSerializable attribute to indicate which types 
// should be serialized using this context. By using this, you can control how the types are 
// serialized and deserialized, improving performance and flexibility.

[JsonSerializable(typeof(Project))]  // Register the Project class for JSON serialization.
[JsonSerializable(typeof(ProjectTask))]  // Register the ProjectTask class for JSON serialization.
[JsonSerializable(typeof(ProjectsJson))]  // Register the ProjectsJson class for JSON serialization.
[JsonSerializable(typeof(Category))]  // Register the Category class for JSON serialization.
[JsonSerializable(typeof(Tag))]  // Register the Tag class for JSON serialization.
public partial class JsonContext : JsonSerializerContext
{
    // The JsonContext class extends JsonSerializerContext, providing a centralized 
    // place for serializing and deserializing the specified types (Project, ProjectTask, 
    // ProjectsJson, Category, and Tag).
}
