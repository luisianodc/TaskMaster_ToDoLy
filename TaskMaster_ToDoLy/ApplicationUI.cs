
namespace TaskMaster;

//Provides the text-based user interface for the ToDoLy application.

public static class ConsoleUI
{
    
    //Starts the main menu and handles user selections.
    public static void Run()
    {
        //while (true)
        //{
            Console.Clear();

            ShowTopText();
            DisplayMenu();

        //}
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
        Console.ReadLine();
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

    
    //Changes a task between completed and not completed.
    
    //<param name="task">The task whose status should change.</param>
    private static void ToggleTaskStatus(Task task)
    {

    }

    
    //Removes the selected task from the task list.
    
    //<param name="task">The task to remove.</param>
    private static void RemoveTask(Task task)
    {

    }

    
    //Reads and validates a task number.
    //Entering 0 cancels the operation.
    
   
    //The zero-based task index, or null when cancelled.
   
    private static int? ReadTaskIndexOrCancel()
    {
        return 0;
    }

    
    //Reads a required value from the user.
    //Entering 0 cancels the operation.
    
    //<param name="prompt">The prompt shown to the user.</param>
 
    private static string? ReadRequiredOrCancel(string prompt)
    {
        return "0";
    }

    
    //Reads an optional value from the user.
    //Entering 0 cancels the operation.
    
    //<param name="prompt">The prompt shown to the user.</param>

    private static string? ReadOptionalOrCancel(string prompt)
    {
        return "0";
    }

    
    //Reads and validates a date from the user.
    //Entering 0 cancels the operation.
    
    //<param name="prompt">The prompt shown to the user.</param>

    private static DateTime? ReadDateOrCancel(string prompt)
    {
        return new DateTime();
    }

    
    //Asks the user to confirm quitting without saving.
    
   
    //True if the user confirms quitting; otherwise false.
   
    private static bool QuitWithoutSaving()
    {
        return true;
    }

    
    //Displays a message when the user enters an invalid menu option.
    
    private static void ShowInvalidOption()
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
