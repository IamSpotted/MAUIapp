using System.Windows.Input;
using ITSF_App.Models;

namespace ITSF_App.Pages.Controls;

public partial class TaskView
{
    // Constructor that initializes the TaskView component
    public TaskView()
    {
        InitializeComponent();
    }

    // Bindable property for handling task completion commands
    public static readonly BindableProperty TaskCompletedCommandProperty = BindableProperty.Create(
        nameof(TaskCompletedCommand),  // Property name
        typeof(ICommand),              // Property type
        typeof(TaskView),              // Declaring type
        null);                         // Default value

    // Command executed when a task is marked as completed or uncompleted
    public ICommand TaskCompletedCommand
    {
        get => (ICommand)GetValue(TaskCompletedCommandProperty);
        set => SetValue(TaskCompletedCommandProperty, value);
    }

    // Event handler for when the checkbox checked state changes
    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var checkbox = (CheckBox)sender;
        
        // Ensure the binding context is of type ProjectTask
        if (checkbox.BindingContext is not ProjectTask task)
            return;
        
        // Prevent unnecessary updates if the value hasn't changed
        if (task.IsCompleted == e.Value)
            return;

        // Update the task completion status
        task.IsCompleted = e.Value;

        // Execute the bound command if it exists
        TaskCompletedCommand?.Execute(task);
    }
}