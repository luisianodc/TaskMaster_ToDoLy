using System.Diagnostics;
using System.Text;

namespace TaskMaster_ToDoLy;

// Runs manual unit tests for the main ToDoLy application operations
// and for the application's main supporting classes.
public static class UnitTesting
{
    private static int passedTests;
    private static int failedTests;

    private static readonly string TestFileName =
        Path.Combine(
            Environment.CurrentDirectory,
            "ToDoList.json");


    // Starts all unit tests and creates the HTML test report.
    public static void Run()
    {
        passedTests = 0;
        failedTests = 0;

        var html = new StringBuilder();

        CreateHtmlHeader(html);

        Console.WriteLine();

        ApplicationUi.WriteLineInColor(
            ConsoleColor.Cyan,
            "==========================================");

        ApplicationUi.WriteLineInColor(
            ConsoleColor.Cyan,
            "        TASKMASTER UNIT TESTING");

        ApplicationUi.WriteLineInColor(
            ConsoleColor.Cyan,
            "==========================================");

        Console.WriteLine();


        // Tests the main application operations.
        TestAddTask(html);
        TestEditUpdate(html);
        TestEditChangeStatus(html);
        TestEditRemove(html);
        TestShowTaskList(html);
        TestExitWithSaving(html);
        TestExitWithoutSaving(html);


        // Tests the individual application classes.
        RunApplicationUiTesting(html);
        RunFileHandlerTesting(html);
        RunTaskListTesting(html);
        RunTaskTesting(html);


        AddSummary(html);

        html.AppendLine("</body>");
        html.AppendLine("</html>");


        var reportPath = Path.Combine(
            Environment.CurrentDirectory,
            "UnitTestResults.html");

        File.WriteAllText(
            reportPath,
            html.ToString());


        Console.WriteLine();

        ApplicationUi.WriteLineInColor(
            ConsoleColor.Cyan,
            "==========================================");

        ApplicationUi.WriteLineInColor(
            ConsoleColor.Green,
            $"Tests passed: {passedTests}");

        ApplicationUi.WriteLineInColor(
            ConsoleColor.Red,
            $"Tests failed: {failedTests}");

        ApplicationUi.WriteLineInColor(
            ConsoleColor.Cyan,
            "==========================================");

        Console.WriteLine();


        ApplicationUi.WriteLineInColor(
            ConsoleColor.Yellow,
            "HTML test report created:");

        Console.WriteLine(reportPath);

        Console.WriteLine();

        Console.Write(
            "Do you want to open the HTML report? (y/n): ");

        var answer = Console.ReadLine();

        if (answer?.Equals(
                "y",
                StringComparison.OrdinalIgnoreCase) == true)
        {
            try
            {
                Process.Start(
                    new ProcessStartInfo
                    {
                        FileName = reportPath,
                        UseShellExecute = true
                    });
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Could not open the HTML report: {ex.Message}");
            }
        }

        Console.WriteLine();

        Console.WriteLine(
            "Press Enter to return to the main menu...");

        Console.ReadLine();
    }


    // ============================================================
    // EXISTING APPLICATION OPERATION TESTS
    // ============================================================


    // Tests adding a task with valid and invalid input.
    private static void TestAddTask(StringBuilder html)
    {
        // VALID TEST

        PrepareTestData();

        var validInput =
            "2\n" +
            "Unit Test Task\n" +
            "Testing\n" +
            "2030-01-01\n" +
            "\n" +
            "4\n";

        var validOutput =
            RunApplication(validInput);

        var validPassed =
            TaskList.Tasks.Any(task =>
                task.Title == "Unit Test Task" &&
                task.Project == "Testing" &&
                task.DueDate == new DateTime(2030, 1, 1));

        AddResult(
            html,
            "Add Task",
            "Valid input",
            validInput,
            validOutput,
            validPassed);


        // INVALID TEST

        PrepareTestData();

        var invalidInput =
            "2\n" +
            "\n" +
            "0\n" +
            "4\n";

        var invalidOutput =
            RunApplication(invalidInput);

        var invalidPassed =
            !TaskList.Tasks.Any(task =>
                task.Title == "Unit Test Task");

        AddResult(
            html,
            "Add Task",
            "Invalid input",
            invalidInput,
            invalidOutput,
            invalidPassed);
    }


    // Tests updating an existing task with valid and invalid input.
    private static void TestEditUpdate(StringBuilder html)
    {
        // VALID TEST

        PrepareTestData();

        var validInput =
            "3\n" +
            "1\n" +
            "1\n" +
            "Updated Task\n" +
            "Updated Project\n" +
            "2030-02-02\n" +
            "\n" +
            "4\n";

        var validOutput =
            RunApplication(validInput);

        var validPassed =
            TaskList.Tasks.Count > 0 &&
            TaskList.Tasks[0].Title == "Updated Task" &&
            TaskList.Tasks[0].Project == "Updated Project" &&
            TaskList.Tasks[0].DueDate ==
            new DateTime(2030, 2, 2);

        AddResult(
            html,
            "Edit Task - Update",
            "Valid input",
            validInput,
            validOutput,
            validPassed);


        // INVALID TEST

        PrepareTestData();

        var originalTitle =
            TaskList.Tasks[0].Title;

        var invalidInput =
            "3\n" +
            "99\n" +
            "0\n" +
            "4\n";

        var invalidOutput =
            RunApplication(invalidInput);

        var invalidPassed =
            TaskList.Tasks.Count > 0 &&
            TaskList.Tasks[0].Title == originalTitle;

        AddResult(
            html,
            "Edit Task - Update",
            "Invalid input",
            invalidInput,
            invalidOutput,
            invalidPassed);
    }


    // Tests changing a task's completion status with valid and invalid input.
    private static void TestEditChangeStatus(StringBuilder html)
    {
        // VALID TEST

        PrepareTestData();

        var originalStatus =
            TaskList.Tasks[0].IsDone;

        var validInput =
            "3\n" +
            "1\n" +
            "2\n" +
            "\n" +
            "4\n";

        var validOutput =
            RunApplication(validInput);

        var validPassed =
            TaskList.Tasks.Count > 0 &&
            TaskList.Tasks[0].IsDone != originalStatus;

        AddResult(
            html,
            "Edit Task - Change Status",
            "Valid input",
            validInput,
            validOutput,
            validPassed);


        // INVALID TEST

        PrepareTestData();

        originalStatus =
            TaskList.Tasks[0].IsDone;

        var invalidInput =
            "3\n" +
            "99\n" +
            "0\n" +
            "4\n";

        var invalidOutput =
            RunApplication(invalidInput);

        var invalidPassed =
            TaskList.Tasks.Count > 0 &&
            TaskList.Tasks[0].IsDone == originalStatus;

        AddResult(
            html,
            "Edit Task - Change Status",
            "Invalid input",
            invalidInput,
            invalidOutput,
            invalidPassed);
    }


    // Tests removing a task with valid and invalid input.
    private static void TestEditRemove(StringBuilder html)
    {
        // VALID TEST

        PrepareTestData();

        var originalCount =
            TaskList.Tasks.Count;

        var validInput =
            "3\n" +
            "1\n" +
            "3\n" +
            "y\n" +
            "\n" +
            "4\n";

        var validOutput =
            RunApplication(validInput);

        var validPassed =
            TaskList.Tasks.Count ==
            originalCount - 1;

        AddResult(
            html,
            "Edit Task - Remove",
            "Valid input",
            validInput,
            validOutput,
            validPassed);


        // INVALID TEST

        PrepareTestData();

        originalCount =
            TaskList.Tasks.Count;

        var invalidInput =
            "3\n" +
            "99\n" +
            "0\n" +
            "4\n";

        var invalidOutput =
            RunApplication(invalidInput);

        var invalidPassed =
            TaskList.Tasks.Count ==
            originalCount;

        AddResult(
            html,
            "Edit Task - Remove",
            "Invalid input",
            invalidInput,
            invalidOutput,
            invalidPassed);
    }


    // Tests displaying the task list with valid and invalid input.
    private static void TestShowTaskList(StringBuilder html)
    {
        // VALID TEST

        PrepareTestData();

        var validInput =
            "1\n" +
            "1\n" +
            "\n" +
            "4\n";

        var validOutput =
            RunApplication(validInput);

        var validPassed =
            validOutput.Contains("Do dishes") ||
            validOutput.Contains("Take out trash") ||
            validOutput.Contains("Read a book");

        AddResult(
            html,
            "Show Task List",
            "Valid input",
            validInput,
            validOutput,
            validPassed);


        // INVALID TEST

        PrepareTestData();

        var invalidInput =
            "1\n" +
            "99\n" +
            "\n" +
            "0\n" +
            "4\n";

        var invalidOutput =
            RunApplication(invalidInput);

        var invalidPassed =
            invalidOutput.Contains(
                "Invalid option");

        AddResult(
            html,
            "Show Task List",
            "Invalid input",
            invalidInput,
            invalidOutput,
            invalidPassed);
    }


    // Tests saving and exiting with valid and invalid menu input.
    private static void TestExitWithSaving(StringBuilder html)
    {
        // VALID TEST

        PrepareTestData();

        var validInput =
            "4\n";

        var validOutput =
            RunApplication(validInput);

        var validPassed =
            File.Exists(TestFileName);

        AddResult(
            html,
            "Exit With Saving",
            "Valid input",
            validInput,
            validOutput,
            validPassed);


        // INVALID TEST

        PrepareTestData();

        var invalidInput =
            "99\n" +
            "\n" +
            "4\n";

        var invalidOutput =
            RunApplication(invalidInput);

        var invalidPassed =
            invalidOutput.Contains("Invalid option") &&
            File.Exists(TestFileName);

        AddResult(
            html,
            "Exit With Saving",
            "Invalid input",
            invalidInput,
            invalidOutput,
            invalidPassed);
    }


    // Tests exiting without saving with valid and invalid confirmation.
    private static void TestExitWithoutSaving(StringBuilder html)
    {
        // VALID TEST

        PrepareTestData();

        var validInput =
            "5\n" +
            "y\n";

        var validOutput =
            RunApplication(validInput);

        var validPassed =
            validOutput.Contains("GOODBYE!");

        AddResult(
            html,
            "Exit Without Saving",
            "Valid input",
            validInput,
            validOutput,
            validPassed);


        // INVALID CONFIRMATION TEST

        PrepareTestData();

        var invalidInput =
            "5\n" +
            "x\n" +
            "4\n";

        var invalidOutput =
            RunApplication(invalidInput);

        var invalidPassed =
            invalidOutput.Contains(
                "Exiting...");

        AddResult(
            html,
            "Exit Without Saving",
            "Invalid confirmation",
            invalidInput,
            invalidOutput,
            invalidPassed);
    }


    // ============================================================
    // NEW APPLICATIONUI TESTING
    // ============================================================


    // Tests functionality that belongs to ApplicationUi.cs.
    private static void RunApplicationUiTesting(
        StringBuilder html)
    {
        // TEST 1 - Main menu is displayed.

        PrepareTestData();

        var menuInput =
            "4\n";

        var menuOutput =
            RunApplication(menuInput);

        var menuPassed =
            menuOutput.Contains("Show Task List") &&
            menuOutput.Contains("Add New Task") &&
            menuOutput.Contains("Edit Task") &&
            menuOutput.Contains("Save and Exit");

        AddResult(
            html,
            "ApplicationUi.cs",
            "Main menu is displayed",
            menuInput,
            menuOutput,
            menuPassed);


        // TEST 2 - Invalid main menu option is handled.

        PrepareTestData();

        var invalidInput =
            "99\n" +
            "\n" +
            "4\n";

        var invalidOutput =
            RunApplication(invalidInput);

        var invalidPassed =
            invalidOutput.Contains(
                "Invalid option");

        AddResult(
            html,
            "ApplicationUi.cs",
            "Invalid menu option is handled",
            invalidInput,
            invalidOutput,
            invalidPassed);


        // TEST 3 - Add task through the UI.

        PrepareTestData();

        var addInput =
            "2\n" +
            "Application UI Test\n" +
            "UI Testing\n" +
            "2030-05-05\n" +
            "\n" +
            "4\n";

        var addOutput =
            RunApplication(addInput);

        var addPassed =
            TaskList.Tasks.Any(task =>
                task.Title == "Application UI Test" &&
                task.Project == "UI Testing" &&
                task.DueDate ==
                new DateTime(2030, 5, 5));

        AddResult(
            html,
            "ApplicationUi.cs",
            "Add task through user interface",
            addInput,
            addOutput,
            addPassed);
    }


    // ============================================================
    // NEW FILEHANDLER TESTING
    // ============================================================


    // Tests functionality that belongs to FileHandler.cs.
    private static void RunFileHandlerTesting(
        StringBuilder html)
    {
        // TEST 1 - Save creates the JSON file.

        PrepareTestData();

        var saveInput =
            "Direct FileHandler.Save()";

        string saveOutput;

        try
        {
            FileHandler.Save();

            saveOutput =
                $"File exists: {File.Exists(TestFileName)}" +
                Environment.NewLine +
                $"File path: {TestFileName}";
        }
        catch (Exception ex)
        {
            saveOutput =
                $"EXCEPTION: {ex.Message}";
        }

        var savePassed =
            File.Exists(TestFileName);

        AddResult(
            html,
            "FileHandler.cs",
            "Save creates JSON file",
            saveInput,
            saveOutput,
            savePassed);


        // TEST 2 - Open loads saved tasks.

        PrepareTestData();

        TaskList.Tasks = new List<Task>
        {
            new Task(
                "File Test Task",
                "File Testing",
                new DateTime(2030, 6, 6))
        };

        FileHandler.Save();

        TaskList.Tasks = new List<Task>();

        var openInput =
            "Direct FileHandler.Open()" +
            Environment.NewLine +
            "Expected task: File Test Task";

        string openOutput;

        try
        {
            FileHandler.Open();

            openOutput =
                $"Tasks loaded: {TaskList.Tasks.Count}" +
                Environment.NewLine;

            foreach (var task in TaskList.Tasks)
            {
                openOutput +=
                    $"Title: {task.Title}" +
                    Environment.NewLine +
                    $"Project: {task.Project}" +
                    Environment.NewLine +
                    $"Due Date: {task.DueDate:yyyy-MM-dd}" +
                    Environment.NewLine;
            }
        }
        catch (Exception ex)
        {
            openOutput =
                $"EXCEPTION: {ex.Message}";
        }

        var openPassed =
            TaskList.Tasks.Count == 1 &&
            TaskList.Tasks[0].Title ==
            "File Test Task" &&
            TaskList.Tasks[0].Project ==
            "File Testing";

        AddResult(
            html,
            "FileHandler.cs",
            "Open loads saved tasks",
            openInput,
            openOutput,
            openPassed);


        // TEST 3 - Open handles a missing file.

        if (File.Exists(TestFileName))
        {
            try
            {
                File.Delete(TestFileName);
            }
            catch
            {
                // Continue with the test.
            }
        }

        var missingFileInput =
            "Delete ToDoList.json" +
            Environment.NewLine +
            "Call FileHandler.Open()";

        string missingFileOutput;

        try
        {
            FileHandler.Open();

            missingFileOutput =
                $"File exists: {File.Exists(TestFileName)}" +
                Environment.NewLine +
                $"Tasks created: {TaskList.Tasks.Count}";
        }
        catch (Exception ex)
        {
            missingFileOutput =
                $"EXCEPTION: {ex.Message}";
        }

        var missingFilePassed =
            TaskList.Tasks.Count > 0;

        AddResult(
            html,
            "FileHandler.cs",
            "Open handles missing save file",
            missingFileInput,
            missingFileOutput,
            missingFilePassed);
    }


    // ============================================================
    // NEW TASKLIST TESTING
    // ============================================================


    // Tests functionality that belongs to TaskList.cs.
    private static void RunTaskListTesting(
        StringBuilder html)
    {
        // TEST 1 - Task list starts with an empty collection.

        TaskList.Tasks = new List<Task>();

        var emptyInput =
            "TaskList.Tasks = new empty List<Task>()";

        var emptyOutput =
            $"Task count: {TaskList.Tasks.Count}";

        var emptyPassed =
            TaskList.Tasks.Count == 0;

        AddResult(
            html,
            "TaskList.cs",
            "Empty task list",
            emptyInput,
            emptyOutput,
            emptyPassed);


        // TEST 2 - Add a task to the collection.

        TaskList.Tasks = new List<Task>();

        var addInput =
            "Create Task:" +
            Environment.NewLine +
            "Title: TaskList Test" +
            Environment.NewLine +
            "Project: Testing" +
            Environment.NewLine +
            "Due Date: 2030-07-07";

        string addOutput;

        try
        {
            var task = new Task(
                "TaskList Test",
                "Testing",
                new DateTime(2030, 7, 7));

            TaskList.Tasks.Add(task);

            addOutput =
                $"Task count: {TaskList.Tasks.Count}" +
                Environment.NewLine +
                $"First task title: {TaskList.Tasks[0].Title}" +
                Environment.NewLine +
                $"First task project: {TaskList.Tasks[0].Project}" +
                Environment.NewLine +
                $"First task due date: " +
                $"{TaskList.Tasks[0].DueDate:yyyy-MM-dd}";
        }
        catch (Exception ex)
        {
            addOutput =
                $"EXCEPTION: {ex.Message}";
        }

        var addPassed =
            TaskList.Tasks.Count == 1 &&
            TaskList.Tasks[0].Title ==
            "TaskList Test";

        AddResult(
            html,
            "TaskList.cs",
            "Add task to collection",
            addInput,
            addOutput,
            addPassed);


        // TEST 3 - Remove a task from the collection.

        TaskList.Tasks = new List<Task>();

        var taskToRemove = new Task(
            "Remove Test",
            "Testing",
            new DateTime(2030, 8, 8));

        TaskList.Tasks.Add(taskToRemove);

        var removeInput =
            "TaskList contains one task." +
            Environment.NewLine +
            "Remove the task.";

        string removeOutput;

        try
        {
            var beforeCount =
                TaskList.Tasks.Count;

            TaskList.Tasks.Remove(taskToRemove);

            removeOutput =
                $"Tasks before remove: {beforeCount}" +
                Environment.NewLine +
                $"Tasks after remove: {TaskList.Tasks.Count}";
        }
        catch (Exception ex)
        {
            removeOutput =
                $"EXCEPTION: {ex.Message}";
        }

        var removePassed =
            TaskList.Tasks.Count == 0;

        AddResult(
            html,
            "TaskList.cs",
            "Remove task from collection",
            removeInput,
            removeOutput,
            removePassed);


        // TEST 4 - Multiple tasks can be stored.

        TaskList.Tasks = new List<Task>();

        var multipleInput =
            "Add three Task objects to TaskList.Tasks.";

        string multipleOutput;

        try
        {
            TaskList.Tasks.Add(
                new Task(
                    "Task One",
                    "Project One",
                    new DateTime(2030, 1, 1)));

            TaskList.Tasks.Add(
                new Task(
                    "Task Two",
                    "Project Two",
                    new DateTime(2030, 2, 2)));

            TaskList.Tasks.Add(
                new Task(
                    "Task Three",
                    "Project Three",
                    new DateTime(2030, 3, 3)));

            multipleOutput =
                $"Task count: {TaskList.Tasks.Count}" +
                Environment.NewLine;

            foreach (var task in TaskList.Tasks)
            {
                multipleOutput +=
                    $"{task.Title} - {task.Project}" +
                    Environment.NewLine;
            }
        }
        catch (Exception ex)
        {
            multipleOutput =
                $"EXCEPTION: {ex.Message}";
        }

        var multiplePassed =
            TaskList.Tasks.Count == 3;

        AddResult(
            html,
            "TaskList.cs",
            "Store multiple tasks",
            multipleInput,
            multipleOutput,
            multiplePassed);
    }


    // ============================================================
    // NEW TASK TESTING
    // ============================================================


    // Tests functionality that belongs to Task.cs.
    private static void RunTaskTesting(
        StringBuilder html)
    {
        // TEST 1 - Create a task with valid data.

        var validInput =
            "Title: Task Class Test" +
            Environment.NewLine +
            "Project: C# Testing" +
            Environment.NewLine +
            "Due Date: 2030-09-09" +
            Environment.NewLine +
            "IsDone: false";

        string validOutput;

        Task? validTask = null;

        try
        {
            validTask = new Task(
                "Task Class Test",
                "C# Testing",
                new DateTime(2030, 9, 9));

            validOutput =
                $"Title: {validTask.Title}" +
                Environment.NewLine +
                $"Project: {validTask.Project}" +
                Environment.NewLine +
                $"Due Date: {validTask.DueDate:yyyy-MM-dd}" +
                Environment.NewLine +
                $"IsDone: {validTask.IsDone}";
        }
        catch (Exception ex)
        {
            validOutput =
                $"EXCEPTION: {ex.Message}";
        }

        var validPassed =
            validTask != null &&
            validTask.Title == "Task Class Test" &&
            validTask.Project == "C# Testing" &&
            validTask.DueDate ==
            new DateTime(2030, 9, 9) &&
            validTask.IsDone == false;

        AddResult(
            html,
            "Task.cs",
            "Create task with valid data",
            validInput,
            validOutput,
            validPassed);


        // TEST 2 - Create a completed task.

        var completedInput =
            "Title: Completed Task" +
            Environment.NewLine +
            "Project: Testing" +
            Environment.NewLine +
            "Due Date: 2030-10-10" +
            Environment.NewLine +
            "IsDone: true";

        string completedOutput;

        Task? completedTask = null;

        try
        {
            completedTask = new Task(
                "Completed Task",
                "Testing",
                new DateTime(2030, 10, 10),
                true);

            completedOutput =
                $"Title: {completedTask.Title}" +
                Environment.NewLine +
                $"Project: {completedTask.Project}" +
                Environment.NewLine +
                $"Due Date: {completedTask.DueDate:yyyy-MM-dd}" +
                Environment.NewLine +
                $"IsDone: {completedTask.IsDone}";
        }
        catch (Exception ex)
        {
            completedOutput =
                $"EXCEPTION: {ex.Message}";
        }

        var completedPassed =
            completedTask != null &&
            completedTask.Title == "Completed Task" &&
            completedTask.IsDone;

        AddResult(
            html,
            "Task.cs",
            "Create completed task",
            completedInput,
            completedOutput,
            completedPassed);


        // TEST 3 - Change task properties.

        var propertyInput =
            "Create task." +
            Environment.NewLine +
            "Change Title, Project, DueDate and IsDone.";

        string propertyOutput;

        Task? propertyTask = null;

        try
        {
            propertyTask = new Task(
                "Original Title",
                "Original Project",
                new DateTime(2030, 11, 11));

            propertyTask.Title =
                "Changed Title";

            propertyTask.Project =
                "Changed Project";

            propertyTask.DueDate =
                new DateTime(2030, 12, 12);

            propertyTask.IsDone = true;

            propertyOutput =
                $"Title: {propertyTask.Title}" +
                Environment.NewLine +
                $"Project: {propertyTask.Project}" +
                Environment.NewLine +
                $"Due Date: {propertyTask.DueDate:yyyy-MM-dd}" +
                Environment.NewLine +
                $"IsDone: {propertyTask.IsDone}";
        }
        catch (Exception ex)
        {
            propertyOutput =
                $"EXCEPTION: {ex.Message}";
        }

        var propertyPassed =
            propertyTask != null &&
            propertyTask.Title == "Changed Title" &&
            propertyTask.Project == "Changed Project" &&
            propertyTask.DueDate ==
            new DateTime(2030, 12, 12) &&
            propertyTask.IsDone;

        AddResult(
            html,
            "Task.cs",
            "Change task properties",
            propertyInput,
            propertyOutput,
            propertyPassed);


        // TEST 4 - Empty Task constructor.

        var emptyInput =
            "new Task()";

        string emptyOutput;

        Task? emptyTask = null;

        try
        {
            emptyTask = new Task();

            emptyOutput =
                $"Title: '{emptyTask.Title}'" +
                Environment.NewLine +
                $"Project: '{emptyTask.Project}'" +
                Environment.NewLine +
                $"Due Date: {emptyTask.DueDate:yyyy-MM-dd}" +
                Environment.NewLine +
                $"IsDone: {emptyTask.IsDone}";
        }
        catch (Exception ex)
        {
            emptyOutput =
                $"EXCEPTION: {ex.Message}";
        }

        var emptyPassed =
            emptyTask != null &&
            emptyTask.Title == "" &&
            emptyTask.Project == "" &&
            emptyTask.IsDone == false;

        AddResult(
            html,
            "Task.cs",
            "Create empty task",
            emptyInput,
            emptyOutput,
            emptyPassed);
    }


    // ============================================================
    // TEST DATA AND APPLICATION RUNNER
    // ============================================================


    // Prepares a controlled task list before every test.
    private static void PrepareTestData()
    {
        TaskList.Tasks = new List<Task>
        {
            new Task(
                "Do dishes",
                "Chores",
                new DateTime(2026, 2, 20)),

            new Task(
                "Take out trash",
                "Chores",
                new DateTime(2026, 2, 22),
                true),

            new Task(
                "Read a book",
                "Personal",
                new DateTime(2026, 3, 25))
        };


        // Remove the save file so FileHandler.Open()
        // creates the normal first-time list when the
        // application starts.
        if (File.Exists(TestFileName))
        {
            try
            {
                File.Delete(TestFileName);
            }
            catch
            {
                // The test can continue if the old file
                // cannot be deleted.
            }
        }
    }


    // Runs the real application using simulated console input.
    private static string RunApplication(string input)
    {
        var originalInput =
            Console.In;

        var originalOutput =
            Console.Out;

        using var reader =
            new StringReader(input);

        using var writer =
            new StringWriter();

        try
        {
            Console.SetIn(reader);
            Console.SetOut(writer);

            ApplicationUi.Start();

            return writer.ToString();
        }
        catch (Exception ex)
        {
            return writer.ToString() +
                   Environment.NewLine +
                   "EXCEPTION: " +
                   ex.Message;
        }
        finally
        {
            Console.SetIn(originalInput);
            Console.SetOut(originalOutput);
        }
    }


    // ============================================================
    // TEST RESULT
    // ============================================================


    // Adds one test result to the console and HTML report.
    private static void AddResult(
        StringBuilder html,
        string operation,
        string testName,
        string inputData,
        string outputData,
        bool passed)
    {
        if (passed)
        {
            passedTests++;
        }
        else
        {
            failedTests++;
        }

        var status =
            passed ? "PASS" : "FAIL";

        var cssClass =
            passed ? "pass" : "fail";


        // Console output.

        Console.WriteLine();

        Console.WriteLine(
            "--------------------------------------------------");

        Console.WriteLine(
            $"Operation : {operation}");

        Console.WriteLine(
            $"Test      : {testName}");

        Console.WriteLine(
            $"Result    : {status}");

        Console.WriteLine();

        Console.WriteLine(
            "TESTING INPUT DATA:");

        Console.WriteLine(inputData);

        Console.WriteLine(
            "TESTING OUTPUT DATA:");

        Console.WriteLine(outputData);

        Console.WriteLine(
            "--------------------------------------------------");


        // HTML output.

        html.AppendLine(
            $"<section class=\"test-card {cssClass}\">");

        html.AppendLine(
            $"<div class=\"status\">{status}</div>");

        html.AppendLine(
            $"<h2>{HtmlEncode(operation)}</h2>");

        html.AppendLine(
            $"<h3>{HtmlEncode(testName)}</h3>");

        html.AppendLine(
            "<div class=\"data-container\">");


        // Input.

        html.AppendLine(
            "<div class=\"data-box input-box\">");

        html.AppendLine(
            "<div class=\"data-title\">" +
            "TESTING INPUT DATA" +
            "</div>");

        html.AppendLine(
            $"<pre>{HtmlEncode(inputData)}</pre>");

        html.AppendLine(
            "</div>");


        // Output.

        html.AppendLine(
            "<div class=\"data-box output-box\">");

        html.AppendLine(
            "<div class=\"data-title\">" +
            "TESTING OUTPUT DATA" +
            "</div>");

        html.AppendLine(
            $"<pre>{HtmlEncode(outputData)}</pre>");

        html.AppendLine(
            "</div>");

        html.AppendLine(
            "</div>");

        html.AppendLine(
            "</section>");
    }


    // ============================================================
    // HTML REPORT
    // ============================================================


    // Creates the HTML document header and visual styling.
    private static void CreateHtmlHeader(
        StringBuilder html)
    {
        html.AppendLine(
            "<!DOCTYPE html>");

        html.AppendLine(
            "<html>");

        html.AppendLine(
            "<head>");

        html.AppendLine(
            "<meta charset=\"UTF-8\">");

        html.AppendLine(
            "<meta name=\"viewport\" " +
            "content=\"width=device-width, " +
            "initial-scale=1.0\">");

        html.AppendLine(
            "<title>TaskMaster Unit Test Results</title>");

        html.AppendLine(
            "<style>");

        html.AppendLine(@"
            * {
                box-sizing: border-box;
            }

            body {
                margin: 0;
                padding: 30px;
                font-family: Arial, sans-serif;
                background: linear-gradient(
                    135deg,
                    #141e30,
                    #243b55
                );
                color: white;
            }

            .container {
                max-width: 1200px;
                margin: auto;
            }

            h1 {
                text-align: center;
                margin-bottom: 40px;
                font-size: 42px;
            }

            .test-card {
                background: white;
                color: #222;
                margin-bottom: 25px;
                padding: 25px;
                border-radius: 15px;
                box-shadow:
                    0 10px 25px rgba(0,0,0,0.25);
                transition:
                    transform 0.25s ease,
                    box-shadow 0.25s ease;
            }

            .test-card:hover {
                transform: translateY(-5px);
                box-shadow:
                    0 15px 35px rgba(0,0,0,0.35);
            }

            .pass {
                border-left: 10px solid #2ecc71;
            }

            .fail {
                border-left: 10px solid #e74c3c;
            }

            .status {
                display: inline-block;
                padding: 8px 16px;
                border-radius: 20px;
                font-weight: bold;
                color: white;
                margin-bottom: 10px;
            }

            .pass .status {
                background: #2ecc71;
            }

            .fail .status {
                background: #e74c3c;
            }

            h2 {
                margin: 5px 0;
            }

            h3 {
                color: #666;
            }

            .data-container {
                display: grid;
                grid-template-columns:
                    1fr 1fr;
                gap: 20px;
            }

            .data-box {
                border-radius: 10px;
                overflow: hidden;
            }

            .input-box {
                background: #eef6ff;
            }

            .output-box {
                background: #f4f4f4;
            }

            .data-title {
                padding: 12px;
                font-weight: bold;
                background:
                    rgba(0,0,0,0.08);
            }

            pre {
                margin: 0;
                padding: 15px;
                white-space: pre-wrap;
                word-wrap: break-word;
                font-family: Consolas, monospace;
                max-height: 350px;
                overflow-y: auto;
            }

            .summary {
                margin-top: 40px;
                padding: 30px;
                background: white;
                color: #222;
                border-radius: 15px;
                text-align: center;
                box-shadow:
                    0 10px 25px rgba(0,0,0,0.25);
            }

            .summary-number {
                font-size: 40px;
                font-weight: bold;
            }

            @media (max-width: 800px) {
                .data-container {
                    grid-template-columns: 1fr;
                }

                body {
                    padding: 15px;
                }

                h1 {
                    font-size: 30px;
                }
            }
        ");

        html.AppendLine(
            "</style>");

        html.AppendLine(
            "</head>");

        html.AppendLine(
            "<body>");

        html.AppendLine(
            "<div class=\"container\">");

        html.AppendLine(
            "<h1>" +
            "TaskMaster ToDoLy - Unit Test Results" +
            "</h1>");
    }


    // Adds the final test summary to the HTML report.
    private static void AddSummary(
        StringBuilder html)
    {
        var total =
            passedTests + failedTests;

        html.AppendLine(
            "<div class=\"summary\">");

        html.AppendLine(
            "<h2>Test Summary</h2>");

        html.AppendLine(
            $"<div class=\"summary-number\">{total}</div>");

        html.AppendLine(
            $"<p>Tests executed: {total}</p>");

        html.AppendLine(
            $"<p>Tests passed: {passedTests}</p>");

        html.AppendLine(
            $"<p>Tests failed: {failedTests}</p>");

        html.AppendLine(
            "</div>");

        html.AppendLine(
            "</div>");
    }


    // Safely encodes text before placing it inside the HTML document.
    private static string HtmlEncode(
        string value)
    {
        return value
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;")
            .Replace("\"", "&quot;")
            .Replace("'", "&#39;");
    }
}