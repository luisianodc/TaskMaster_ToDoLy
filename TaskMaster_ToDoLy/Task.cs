namespace TaskMaster_ToDoLy;


/// Represents one item in the TaskMaster ToDoLy task list.
/// The class stores the task title, project, due date, and completion status used by the UI.
/// Instances are created when sample data is loaded, when the user adds a task, or when JSON is deserialized.
/// It is used throughout the application whenever tasks are displayed, edited, sorted, saved, or loaded.
public class Task
{
    
    //Gets or sets the title of the task.
    public string Title { get; set; } = "";

    
    //Gets or sets the due date of the task.
    public DateTime DueDate { get; set; }

    
    //Gets or sets whether the task has been completed.
    public bool IsDone { get; set; }

    
    //Gets or sets the project that the task belongs to.
    public string Project { get; set; } = "";

    /// Initializes an empty task for property-based population.
    /// This constructor is used by JSON deserialization when the saved task list is opened,
    /// and it is called once for each task reconstructed from the save file.
    public Task()
    {
    }

    /// Initializes a task with its title, project, due date, and completion status.
    /// This constructor is used when the application creates sample tasks or when a user adds
    /// a new task, so it runs once for each task created through those application workflows.
    /// <param name="title">The task title.</param>
    /// <param name="project">The project name.</param>
    /// <param name="dueDate">The task due date.</param>
    /// <param name="isDone">Whether the task is completed.</param>
    public Task(string title, string project, DateTime dueDate, bool isDone = false)
    {
        Title = title;
        Project = project;
        DueDate = dueDate;
        IsDone = isDone;
    }
}