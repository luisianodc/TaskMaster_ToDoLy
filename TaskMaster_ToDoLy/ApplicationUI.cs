
namespace TaskMaster;

//Provides the text-based user interface for the ToDoLy application.

public static class ApplicationUI
{
    //Starts the main menu and handles user selections.
    public static void Start()
    {
        while (true)
        {
            Console.Clear();
            Console.Clear();
            ShowTopText();
            DisplayMenu();
            string? option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ShowTaskList();
                    break;

                case "2":
                    AddTask();
                    break;

                case "3":
                    EditTask();
                    break;

                case "4":
                    FileHandler.Save();
                    return;

                case "5":
                    if (ExitWithoutSaving())
                    {
                        return;
                    }

                    break;

                default:
                    DisplayInvalidOption();
                    break;
            }
        }
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
        Console.WriteLine("(4) Save and Exit");
        Console.WriteLine("(5) Exit without Saving");
        Console.WriteLine("SELECT A NUMBER FROM 1 TO 5");
        Console.Write("---: ");

    }

    
    //Displays the task list and allows the user to select a sort order.
    private static void ShowTaskList()
    {
        while (true)
        {
            Console.Clear();

            ShowTopText();

            Console.WriteLine("Show Task List");
            Console.WriteLine("(1) Sort by date");
            Console.WriteLine("(2) Sort by project");
            Console.WriteLine("(0) Back to Main Menu");
            Console.WriteLine();

            Console.Write("> ");

            string? option = Console.ReadLine();

            if (option  == "0")
            {
                return;
            }

            if (option == "1" || option == "2")
            {
                
                if (option == "2")
                {
                    Console.Clear();
                    ShowTopText();
                    Console.WriteLine("Sorting by project");
                }
                else
                {
                    Console.Clear();
                    ShowTopText();
                    Console.WriteLine("Sorting by date");
                }

                Console.WriteLine();
                Pause();
                return;
            }

            DisplayInvalidOption();
        }
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
    

    // Asks user to confirm exit without saving.
    // <returns>
    // True if user confirms exit else false.
    // </returns>
    private static bool ExitWithoutSaving()
    {
        Console.WriteLine();

        WriteInColor(ConsoleColor.Yellow, "WARNING: Any changes made since the last save will be lost.");

        Console.Write(
            "Are you sure you want to quit without saving? (y/n): ");

        string? answer = Console.ReadLine();

        if (answer?.Equals(
                "y",
                StringComparison.OrdinalIgnoreCase) == true)
        {
            Console.WriteLine("Goodbye!");
            return true;
        }

        return false;
    }
    
    

    // Displays invalid menu option.
    private static void DisplayInvalidOption()
    {
        WriteInColor(ConsoleColor.Red, "Invalid option. Please try again.");
        Pause();


    }

    public static void WriteInColor(ConsoleColor color, string  text)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
}
