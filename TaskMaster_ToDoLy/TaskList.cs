namespace TaskMaster_ToDoLy;


/// Stores the application's current collection of Task objects in memory.
/// The class provides shared access to tasks used by the console interface and file persistence layer.
/// ApplicationUi reads and updates this collection while users view, add, edit, complete, or remove tasks.
/// Because it is static, the same collection is used throughout the entire application session.
public static class TaskList
{
    // Gets or sets all current tasks.
    public static List<Task> Tasks { get; set; } = new();
}