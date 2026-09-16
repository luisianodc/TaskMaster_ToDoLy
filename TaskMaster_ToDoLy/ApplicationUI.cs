
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
            DisplayHeader();
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
    private static void DisplayHeader()
    {
        Console.Clear();
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

    
    // Displays the task list and allows the user to select a sort order.
    private static void ShowTaskList()
    {
        while (true)
        {
            Console.Clear();

            DisplayHeader();

            Console.WriteLine("Show Task List");
            Console.WriteLine("(1) Sort by date");
            Console.WriteLine("(2) Sort by project");
            Console.WriteLine("(0) Back to Main Menu");
            Console.WriteLine();

            Console.Write("> ");

            string? choice = Console.ReadLine();

            if (choice == "0")
            {
                return;
            }

            if (choice == "1" || choice == "2")
            {
                IEnumerable<Task> tasks;

                if (choice == "2")
                {
                    tasks = TaskList.Tasks
                        .OrderBy(
                            task => task.Project,
                            StringComparer.OrdinalIgnoreCase)
                        .ThenBy(task => task.DueDate);
                }
                else
                {
                    tasks = TaskList.Tasks
                        .OrderBy(task => task.DueDate);
                }

                Console.WriteLine();
                PrintTasks(tasks);
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
        Console.Clear();
        DisplayHeader();
        Console.WriteLine(
            "--------------------------------------------------------------------------------");

        Console.WriteLine(
            "{0,-4} {1,-25} {2,-16} {3,-14} {4,-10}",
            "#",
            "Title",
            "Due Date",
            "Project",
            "Status");

        Console.WriteLine(
            "--------------------------------------------------------------------------------");

        int number = 1;

        foreach (Task task in tasks)
        {
            string project =
                string.IsNullOrWhiteSpace(task.Project)
                    ? "-"
                    : task.Project;

            Console.WriteLine(
                "{0,-4} {1,-25} {2,-16} {3,-14} {4,-10}",
                number,
                Shorten(task.Title, 24),
                task.DueDate.ToString("yyyy-MM-dd"),
                Shorten(project, 13),
                task.IsDone ? "DONE" : "TODO");

            number++;
        }

        if (number == 1)
        {
            Console.WriteLine("No tasks found.");
        }
    }
    
    
    //Shortens text so it fits inside the task table.
    //<param name="value">The text to shorten.</param>
    //<param name="max">The maximum length.</param>
    //<returns>The original or shortened text.</returns>
    private static string Shorten(string value, int max)
    {
        if (value.Length <= max)
        {
            return value;
        }

        return value[..(max - 1)] + "…";
    }


    
//Allows the user to create a new task.
    //Entering 0 at any input step cancels the operation.
    private static void AddTask()
    {
        Console.Clear();

        DisplayHeader();

        Console.WriteLine("Add New Task");
        Console.WriteLine(
            "Enter 0 at any step to cancel and return to the main menu.");
        Console.WriteLine();

        string? title = ReadRequiredOrCancel("Title: ");

        if (title is null)
        {
            return;
        }

        string? project = ReadOptionalOrCancel("Project (optional): ");

        if (project is null)
        {
            return;
        }

        DateTime? dueDate = ReadDateOrCancel("Due date (yyyy-MM-dd): ");

        if (dueDate is null)
        {
            return;
        }

        Task newTask = new Task(
            title,
            project,
            dueDate.Value);

        TaskList.Tasks.Add(newTask);

        WriteInColor(ConsoleColor.Green, "Task added successfully");

        Pause();
    }

    
    //Allows the user to update, complete, or remove a task.
    
    private static void EditTask()
    {
        Console.Clear();

        DisplayHeader();

        Console.WriteLine("Edit Task");
        Console.WriteLine("Enter 0 to return to the main menu.");
        Console.WriteLine();

        PrintTasks(TaskList.Tasks.OrderBy(task => task.DueDate));

        if (TaskList.Tasks.Count == 0)
        {
            Pause();
            return;
        }

        int? index = ReadTaskIndexOrCancel();

        if (index is null)
        {
            return;
        }

        Task selectedTask = TaskList.Tasks[index.Value];

        Console.WriteLine();
        Console.WriteLine($"Selected: {selectedTask.Title}");
        Console.WriteLine("(1) Update task");
        Console.WriteLine("(2) Mark as done / undone");
        Console.WriteLine("(3) Remove task");
        Console.WriteLine("(0) Cancel and return to Main Menu");
        Console.WriteLine();

        Console.Write("> ");

        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                UpdateTask(selectedTask);
                break;

            case "2":
                ToggleTaskStatus(selectedTask);
                break;

            case "3":
                RemoveTask(selectedTask);
                break;

            case "0":
                return;

            default:
                DisplayInvalidOption();
                break;
        }
    }

    //Changes a task between completed and not completed.
    //<param name="task">The task whose status should change.</param>
    private static void ToggleTaskStatus(Task task)
    {
        task.IsDone = !task.IsDone;

        string message = task.IsDone
            ? "Task marked as done."
            : "Task marked as not done.";
        WriteInColor(ConsoleColor.Green, message);
        Pause();
    }
    
    

    //Updates the title, project and due date of a task.
    //<param name="task">The task to update.</param>
    private static void UpdateTask(Task task)
    {
        Console.Clear();

        DisplayHeader();

        Console.WriteLine("Update Task");
        Console.WriteLine("Press Enter to keep the current value.");
        Console.WriteLine("Enter 0 at any step to cancel.");
        Console.WriteLine();

        Console.Write($"Title [{task.Title}]: ");
        string? title = Console.ReadLine();

        if (title == "0")
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(title))
        {
            task.Title = title.Trim();
        }

        Console.Write($"Project [{task.Project}]: ");
        string? project = Console.ReadLine();

        if (project == "0")
        {
            return;
        }

        if (!string.IsNullOrWhiteSpace(project))
        {
            task.Project = project.Trim();
        }

        while (true)
        {
            Console.Write($"Due date [{task.DueDate:yyyy-MM-dd}]: ");
            string? input = Console.ReadLine();

            if (input == "0")
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                break;
            }

            if (DateTime.TryParse(input, out DateTime date))
            {
                task.DueDate = date.Date;
                break;
            }

            WriteInColor(ConsoleColor.Red, "Invalid date. Try again, or enter 0 to cancel.");
        }

        WriteInColor(ConsoleColor.Green, "Task updated.");

        Pause();
    }



    
    //Removes the selected task from the task list.
    //<param name="task">The task to remove.</param>
    private static void RemoveTask(Task task)
    {
        TaskList.Tasks.Remove(task);

        WriteInColor(ConsoleColor.Green, "Task removed.");

        Pause();
    }
    
    
    //Reads and validates a task number.
    //Entering 0 cancels the operation.
    //<returns>
    //The zero-based task index, or null when cancelled.
    //</returns>
    private static int? ReadTaskIndexOrCancel()
    {
        while (true)
        {
            Console.Write("Enter task number (0 to cancel): ");
            string? input = Console.ReadLine();

            if (input == "0")
            {
                return null;
            }

            if (int.TryParse(input, out int number))
            {
                if (number >= 1 && number <= TaskList.Tasks.Count)
                {
                    return number - 1;
                }
            }

            WriteInColor(ConsoleColor.Red, "Invalid task number. Please try again.");
        }
    }

    
    //Reads a required value from the user.
    //Entering 0 cancels the operation.
    
    //<param name="prompt">The prompt shown to the user.</param>
    //<returns>The entered value, or null when cancelled.</returns>
    private static string? ReadRequiredOrCancel(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);

            string value = Console.ReadLine()?.Trim() ?? "";

            if (value == "0")
            {
                return null;
            }

            if (!string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            WriteInColor(ConsoleColor.Red, "This field is required. Enter 0 to cancel.");
        }
    }

    
    //Reads an optional value from the user.
    //Entering 0 cancels the operation.
    
    //<param name="prompt">The prompt shown to the user.</param>
    //<returns>The entered value, or null when cancelled.</returns>
    private static string? ReadOptionalOrCancel(string prompt)
    {
        Console.Write(prompt);

        string value = Console.ReadLine()?.Trim() ?? "";

        if (value == "0")
        {
            return null;
        }

        return value;
    }

    
    //Reads and validates a date from the user.
    //Entering 0 cancels the operation.
    
    //<param name="prompt">The prompt shown to the user.</param>
    //<returns>The parsed date, or null when cancelled.</returns>
    private static DateTime? ReadDateOrCancel(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);

            string? input = Console.ReadLine();

            if (input == "0")
            {
                return null;
            }

            if (DateTime.TryParse(input, out DateTime date))
            {
                return date.Date;
            }

            WriteInColor(ConsoleColor.Red, "Invalid date. Please use yyyy-MM-dd, or enter 0 to cancel.");
        }
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
