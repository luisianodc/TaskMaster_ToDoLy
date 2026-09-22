using System.Text.Json;

namespace TaskMaster_ToDoLy;

//Handles saving and loading the task list as a JSON file.
public static class FileHandler
{
    //Gets the location of the JSON save file.
    private static readonly string FileName = Path.Combine(Environment.CurrentDirectory, "ToDoList.json");

    //Gets the JSON serialization settings.
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    //Opens the saved task list.
    //If no file exists, the application starts with an empty list.
    public static void Open()
    {
        if (File.Exists(FileName))
            Load();
        else
            //TaskList.Tasks = new();
            CreateFirstTimeList();
    }
    
    //Saves the current task list to the JSON file.
    public static void Save()
    {
        try
        {
            var json = JsonSerializer.Serialize(TaskList.Tasks, Options);
            File.WriteAllText(FileName, json);
            ApplicationUi.WriteLineInColor(ConsoleColor.Green, "Your To-Do list has been saved.");
        }
        catch (Exception ex)
        {
            ApplicationUi.WriteLineInColor(ConsoleColor.Red, $"Failed to save To-Do list: {ex.Message}");
        }
    }

    //Loads tasks from the JSON save file.
    private static void Load()
    {
        try
        {
            var json = File.ReadAllText(FileName);
            TaskList.Tasks = JsonSerializer.Deserialize<List<Task>>(json, Options) ?? new List<Task>();
            ApplicationUi.WriteLineInColor(ConsoleColor.Green, "Opened your saved list.");
        }
        catch (Exception ex)
        {
            TaskList.Tasks = new List<Task>();
            ApplicationUi.WriteLineInColor(ConsoleColor.Red, $"Failed to open saved To-Do list: {ex.Message}");
        }
    }
    
    private static void CreateFirstTimeList()
    {
        TaskList.Tasks = new List<Task>
        {
            new("Do dishes", "Chores", new DateTime(2026, 2, 20)),
            new("Take out trash", "Chores", new DateTime(2026, 2, 22), true),
            new("Read a book", "Personal", new DateTime(2026, 3, 25)),
            new("Mini Project", "C# .NET", new DateTime(2026, 2, 16), true),
            new("Individual Project", "C# .NET", new DateTime(2026, 2, 22), true),
            new("HTML & CSS", "C# .NET", new DateTime(2026, 2, 25))
        };
        ApplicationUi.WriteLineInColor(ConsoleColor.Yellow, "No saved list found. Created a sample list of tasks.");
    }
}