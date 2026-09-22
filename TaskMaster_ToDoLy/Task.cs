namespace TaskMaster_ToDoLy;


//Represents a task in the ToDoLy application.
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

    
    //Initializes an empty task.
    public Task()
    {
    }

    
    //Initializes a task with the specified values.
    //<param name="title">The task title.</param>
    //<param name="project">The project name.</param>
    //<param name="dueDate">The task due date.</param>
    //<param name="isDone">Whether the task is completed.</param>
    public Task(string title, string project, DateTime dueDate, bool isDone = false)
    {
        Title = title;
        Project = project;
        DueDate = dueDate;
        IsDone = isDone;
    }
}