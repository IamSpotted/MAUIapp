using CommunityToolkit.Mvvm.Input;
using ITSF_App.Models;

namespace ITSF_App.PageModels;

// Interface for managing project task-related interactions in the page model
public interface IProjectTaskPageModel
{
    // Command for navigating to a specific task asynchronously
    IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }

    // Property indicating whether the page model is busy with a task
    bool IsBusy { get; }
}