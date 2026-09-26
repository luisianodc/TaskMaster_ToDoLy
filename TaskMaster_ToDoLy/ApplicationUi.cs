namespace TaskMaster_ToDoLy;

/// Handles the console-based menu and user interactions for the ToDoLy app.
/// Displays tasks, accepts input for adding and editing entries, and confirms save/exit choices.
/// Validates user actions and keeps the workflow guided for task management operations.
/// Serves as the main UI layer between the user and the task data model.
/// Invalid menu choices, task numbers, required fields, and dates are detected before changes are made.
/// The application responds with a clear error message and asks the user to try the input again.
/// Users can enter 0 during supported task-entry workflows to cancel without changing the task list.
/// Confirmation prompts are used before destructive actions, such as deleting tasks or exiting without saving.
/// This validation keeps invalid input from terminating the console session or corrupting task data.
public static class ApplicationUi
{
    /// Starts the main program loop for the TaskMaster ToDoLy console app.
    /// It is used immediately after the saved data is loaded in the application entry point,
    /// and it runs continuously throughout the session while the user chooses menu actions like
    /// viewing tasks, adding entries, editing details, saving, or exiting the app.
    /// Starts the application's main console loop after task data has been loaded.
    /// It is called once by Program and continues running until the user exits,
    /// dispatching each menu selection to the appropriate task-management method.
    public static void Start()
    {
        DisplayHeader();
        FileHandler.Open();
        while (true)
        {
            DisplayMenu();
            var option = Console.ReadLine();
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
                    WriteLineInColor(ConsoleColor.Yellow, "Exiting...");
                    return;
                case "5":
                    if (ExitWithoutSaving()) return;
                    break;
                case "6":
                    //var unitTesting = new UnitTesting();
                    UnitTesting.Run();
                    break;
                default:
                    DisplayInvalidOption();
                    break;
            }
        }
    }

    /// Displays the main menu and its available task-management commands.
    /// Start calls this method once at the beginning of every menu-loop iteration,
    /// so it normally runs repeatedly throughout the application session.
    private static void DisplayMenu()
    {
        Console.WriteLine("");
        Console.WriteLine("(1) Show Task List");
        Console.WriteLine("(2) Add New Task");
        Console.WriteLine("(3) Edit Task (update, mark as done, remove)");
        Console.WriteLine("(4) Save and Exit");
        Console.WriteLine("(5) Exit without Saving");
        Console.WriteLine("(6) Unit Testing");
        Console.WriteLine("ENTER A NUMBER FROM 1 TO 5");
        Console.Write("===> : ");
    }
    
    /// Displays tasks sorted by date or project and allows the user to return to the main menu.
    /// Start calls this method whenever the user selects the task-list option, so it may run
    /// multiple times during a session and once per completed list-view action.
    private static void ShowTaskList()
    {
        while (true)
        {
            Console.WriteLine("");
            WriteLineInColor(ConsoleColor.Yellow, "Show task list");
            Console.WriteLine("(1) Sort by date");
            Console.WriteLine("(2) Sort by project");
            Console.WriteLine("(0) Back to Main Menu");
            Console.WriteLine();
            Console.Write("===> : ");

            var choice = Console.ReadLine();

            if (choice == "0") return;
            if (choice == "1" || choice == "2")
            {
                IEnumerable<Task> tasks;

                if (choice == "2")
                    tasks = TaskList.Tasks.OrderBy(task => task.Project, StringComparer.OrdinalIgnoreCase).ThenBy(task => task.DueDate);
                else
                    tasks = TaskList.Tasks.OrderBy(task => task.DueDate);

                Console.WriteLine();
                PrintTasks(tasks);
                Pause();
                return;
            }

            DisplayInvalidOption();
        }
    }

    /// Prints the supplied tasks as a numbered, formatted console table.
    /// It is used by ShowTaskList and EditTask whenever tasks must be displayed,
    /// and it runs once for each list-view or edit-screen display.
    /// <param name="tasks">The tasks to display.</param>
    private static void PrintTasks(IEnumerable<Task> tasks)
    {
        Console.WriteLine("");
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

        var number = 1;

        foreach (var task in tasks)
        {
            var project =
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

        if (number == 1) Console.WriteLine("No tasks found.");
    }
    
    /// Shortens a value so titles and project names fit within the task table columns.
    /// PrintTasks calls this helper for each displayed title and project, so it may run
    /// several times during every task-list or edit-screen display.
    /// <param name="value">The text to shorten.</param>
    /// <param name="max">The maximum permitted length.</param>
    /// <returns>The original value or a shortened value with an ellipsis.</returns>
    private static string Shorten(string value, int max)
    {
        if (value.Length <= max) return value;
        return value[..(max - 1)] + "…";
    }

    /// Collects input and creates a new task in the current task list.
    /// Start calls this method whenever the user selects Add New Task, so it may run
    /// multiple times during a session and once for each add attempt.
    private static void AddTask()
    {
        Console.WriteLine("");
        WriteLineInColor(ConsoleColor.Yellow, "Add New Task");
        Console.WriteLine(
            "Enter 0 at any step to cancel and return to the main menu.");
        Console.WriteLine();

        var title = ReadRequiredOrCancel("Title: ");

        if (title is null) return;

        var project = ReadOptionalOrCancel("Project (optional): ");

        if (project is null) return;

        var dueDate = ReadDateOrCancel("Due date (yyyy-MM-dd): ");

        if (dueDate is null) return;

        var newTask = new Task(
            title,
            project,
            dueDate.Value);

        TaskList.Tasks.Add(newTask);

        WriteLineInColor(ConsoleColor.Green, "Task added successfully");
        Pause();
    }
    
    /// Displays the task-selection and action menu for updating, completing, or removing a task.
    /// Start calls this method whenever the user selects Edit Task, so it may run multiple
    /// times during a session and once for each edit attempt.
    private static void EditTask()
    {
        Console.WriteLine("");
        WriteLineInColor(ConsoleColor.Yellow, "Edit task");
        Console.WriteLine("Edit Task");
        Console.WriteLine("Enter 0 to return to the main menu.");
        Console.WriteLine();

        PrintTasks(TaskList.Tasks.OrderBy(task => task.DueDate));

        if (TaskList.Tasks.Count == 0)
        {
            Pause();
            return;
        }

        var index = ReadTaskIndexOrCancel();

        if (index is null) return;

        var selectedTask = TaskList.Tasks[index.Value];

        Console.WriteLine();
        Console.WriteLine($"Selected: {selectedTask.Title}");
        Console.WriteLine("(1) Update task");
        Console.WriteLine("(2) Mark as done / undone");
        Console.WriteLine("(3) Remove task");
        Console.WriteLine("(0) Cancel and return to Main Menu");
        Console.WriteLine();
        Console.Write("===> : ");

        var choice = Console.ReadLine();

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

    /// Toggles the selected task between completed and incomplete states.
    /// EditTask calls this method when the user chooses the status action, so it runs
    /// once for each status-change request.
    /// <param name="task">The task whose completion status should change.</param>
    private static void ToggleTaskStatus(Task task)
    {
        task.IsDone = !task.IsDone;

        var message = task.IsDone
            ? "Task marked as done."
            : "Task marked as not done.";
        WriteLineInColor(ConsoleColor.Green, message);
        Pause();
    }
    
    /// Reads replacement values and updates a selected task's title, project, and due date.
    /// EditTask calls this method when the user chooses Update Task, so it runs once
    /// for each update attempt and may be cancelled before all fields are changed.
    /// <param name="task">The task to update.</param>
    private static void UpdateTask(Task task)
    {
        Console.WriteLine("");
        WriteLineInColor(ConsoleColor.Yellow, "Update task");
        Console.WriteLine("Press Enter to keep the current value.");
        Console.WriteLine("Enter 0 at any step to cancel.");
        Console.WriteLine();

        Console.Write($"Title [{task.Title}]: ");
        var title = Console.ReadLine();

        if (title == "0") return;

        if (!string.IsNullOrWhiteSpace(title)) task.Title = title.Trim();

        Console.Write($"Project [{task.Project}]: ");
        var project = Console.ReadLine();

        if (project == "0") return;

        if (!string.IsNullOrWhiteSpace(project)) task.Project = project.Trim();

        while (true)
        {
            Console.Write($"Due date [{task.DueDate:yyyy-MM-dd}]: ");
            var input = Console.ReadLine();

            if (input == "0") return;

            if (string.IsNullOrWhiteSpace(input)) break;

            if (DateTime.TryParse(input, out var date))
            {
                task.DueDate = date.Date;
                break;
            }

            WriteLineInColor(ConsoleColor.Red, "Invalid date. Try again, or enter 0 to cancel.");
        }

        WriteLineInColor(ConsoleColor.Green, "Task updated.");
        Pause();
    }
    
    /// Confirms and removes a selected task from the current task list.
    /// EditTask calls this method when the user chooses Remove Task, so it runs once
    /// for each removal attempt.
    /// <param name="task">The task to remove if the user confirms.</param>
    private static void RemoveTask(Task task)
    {
        WriteLineInColor(ConsoleColor.Yellow, "WARNING: You are about to delete a task.");
        WriteLineInColor(ConsoleColor.Blue, "Are you sure you want to delete this task (y/n): ");
        var answer = Console.ReadLine();

        if (answer?.Equals("y", StringComparison.OrdinalIgnoreCase) == true)
        {
            TaskList.Tasks.Remove(task);
            WriteLineInColor(ConsoleColor.Green, "Task removed.");
            
        }
        
        Pause();
    }
    
    /// Reads and validates a one-based task number entered in the edit workflow.
    /// EditTask calls this method once per edit attempt; it may repeat input prompts
    /// until a valid task number is entered or the user cancels with 0.
    /// <returns>The zero-based task index, or null when the operation is cancelled.</returns>
    private static int? ReadTaskIndexOrCancel()
    {
        while (true)
        {
            Console.Write("Enter task number (0 to cancel): ");
            var input = Console.ReadLine();

            if (input == "0") return null;

            if (int.TryParse(input, out var number))
                if (number >= 1 && number <= TaskList.Tasks.Count)
                    return number - 1;

            WriteLineInColor(ConsoleColor.Red, "Invalid task number. Please try again.");
        }
    }

    /// Reads a non-empty required value during task creation.
    /// AddTask calls this method once for each add attempt, and it repeats the prompt
    /// until valid text is entered or the user cancels with 0.
    /// <param name="prompt">The prompt shown to the user.</param>
    /// <returns>The entered value, or null when the operation is cancelled.</returns>
    private static string? ReadRequiredOrCancel(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var value = Console.ReadLine()?.Trim() ?? "";

            if (value == "0") return null;

            if (!string.IsNullOrWhiteSpace(value)) return value;
            WriteLineInColor(ConsoleColor.Red, "This field is required. Enter 0 to cancel.");
        }
    }
    
    /// Reads an optional value during task creation while supporting cancellation.
    /// AddTask calls this method once per add attempt for the project field.
    /// <param name="prompt">The prompt shown to the user.</param>
    /// <returns>The entered value, including an empty value, or null when cancelled.</returns>
    private static string? ReadOptionalOrCancel(string prompt)
    {
        Console.Write(prompt);
        var value = Console.ReadLine()?.Trim() ?? "";

        if (value == "0") return null;
        return value;
    }
    
    /// Reads and validates a due date during task creation.
    /// AddTask calls this method once per add attempt, and it repeats the prompt
    /// until a valid date is entered or the user cancels with 0.
    /// <param name="prompt">The prompt shown to the user.</param>
    /// <returns>The date without a time component, or null when cancelled.</returns>
    private static DateTime? ReadDateOrCancel(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            var input = Console.ReadLine();

            if (input == "0") return null;

            if (DateTime.TryParse(input, out var date)) return date.Date;
            WriteLineInColor(ConsoleColor.Red, "Invalid date. Please use yyyy-MM-dd, or enter 0 to cancel.");
        }
    }

    /// Asks the user to confirm leaving the application without saving changes.
    /// Start calls this method whenever the user selects Exit without Saving,
    /// so it runs once for each such exit attempt.
    /// <returns>True when the user confirms the exit; otherwise, false.</returns>
    private static bool ExitWithoutSaving()
    {
        WriteLineInColor(ConsoleColor.Yellow, "WARNING: Any changes made since the last save will be lost.");
        WriteLineInColor(ConsoleColor.Blue, "Are you sure you want to exit without saving? (y/n): ");

        var answer = Console.ReadLine();

        if (answer?.Equals("y", StringComparison.OrdinalIgnoreCase) == true)
        {
            WriteLineInColor(ConsoleColor.Yellow, "GOODBYE!");
            return true;
        }

        return false;
    }
    
    /// Displays an error and pauses after an invalid menu or action selection.
    /// It is called whenever user input does not match an available option,
    /// so it may run any number of times during a session.
    private static void DisplayInvalidOption()
    {
        WriteLineInColor(ConsoleColor.Red, "Invalid option. Please try again.");
        Pause();
    }
    
    /// Pauses the console until the user presses Enter so feedback can be read.
    /// It is called after completed actions and validation errors, so it may run
    /// multiple times during each application session.
    private static void Pause()
    {
        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    /// Writes a line in the requested console color and then restores the default color.
    /// It is used throughout the UI for status, warning, error, and informational messages,
    /// so it may run many times during every application session.
    /// <param name="color">The color to use while writing the message.</param>
    /// <param name="text">The message to write.</param>
    public static void WriteLineInColor(ConsoleColor color, string text)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    /// Writes text in the requested console color without adding a new line.
    /// It is used by the header and colored prompts, so it may run several times
    /// during startup and whenever those UI elements are rendered.
    /// <param name="color">The color to use while writing the text.</param>
    /// <param name="text">The text to write.</param>
    public static void WriteInColor(ConsoleColor color, string text)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }
    
    /// Displays the application's branded banner at startup before the main menu is shown.
    /// Start calls this method once at the beginning of each application session,
    /// and it can be reused later if the UI gains a screen-refresh workflow.
    private static void DisplayHeader()
    {
        Console.WriteLine("***************************************************");
        Console.Write("*********  ");
        WriteInColor(ConsoleColor.Magenta, "TASKMASTER");
        Console.Write(" *********** ");
        WriteInColor(ConsoleColor.DarkRed, "TODOLY");
        Console.Write(" **********");
        Console.WriteLine("");
        //Console.WriteLine("*********  TASKMASTER *********** TODOLY **********");
        Console.WriteLine("***************************************************");
    }
}