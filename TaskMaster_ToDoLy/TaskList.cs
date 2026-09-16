namespace TaskMaster;


// Stores the application's current collection of tasks.
public static class TaskList
{
    // Gets or sets all current tasks.
    public static List<Task> Tasks { get; set; } = new();
}