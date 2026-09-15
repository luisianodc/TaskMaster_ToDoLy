
namespace TaskMaster;

//Provides the text-based user interface for the ToDoLy application.

public static class ApplicationUI
{
    //Starts the main menu and handles user selections.
    public static void Start()
    {
            Console.Clear();
            ShowTopText();
            DisplayMenu();
    }

    
    //Displays the application header and task statistics.
    private static void ShowTopText()
    {
        Console.WriteLine("***************************************************");
        Console.WriteLine("*********  TASKMASTER *********** TODOLY **********");
        Console.WriteLine("***************************************************");
    }

    
    //Displays the main application menu.
    private static void DisplayMenu()
    {
        Console.WriteLine("(1) Show Task List");
        Console.WriteLine("(2) Add New Task");
        Console.WriteLine("(3) Edit Task (update, mark as done, remove)");
        Console.WriteLine("(4) Save and Quit");
        Console.WriteLine("(5) Quit without Saving");
        Console.WriteLine("SELECT A NUMBER FROM 1 TO 5");
        Console.Write("---: ");
        Pause();
    }

    
    //Displays the task list and allows the user to select a sort order.
    private static void ShowTaskList()
    {
    }

    
    //Prints the supplied tasks in a formatted table.
    //<param name="tasks">The tasks to display.</param>
    private static void PrintTasks(IEnumerable<Task> tasks)
    {
    }

    
    //Allows the user to create a new task.
    //Entering 0 at any input step cancels the operation.
    private static void AddTask()
    {
    }

    
    //Allows the user to update, complete, or remove a task.
    
    private static void EditTask()
    {
    }

    
    //Updates the title, project and due date of a task.
    //<param name="task">The task to update.</param>
    private static void UpdateTask(Task task)
    {
    }


    
    //Removes the selected task from the task list.
    //<param name="task">The task to remove.</param>
    private static void RemoveTask(Task task)
    {
    }
    
    //Pauses the application until the user presses Enter.
    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}
