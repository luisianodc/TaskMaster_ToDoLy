using System.Text.Json;

namespace TaskMaster_ToDoLy;


/// Manages persistence of the application's task list in a local JSON file.
/// The class loads saved tasks at startup and writes current tasks when the user saves.
/// It also creates a sample list when no save file exists and reports persistence results through the console UI.
/// Because it is static, its shared operations are available throughout the application's lifetime.
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

    /// Opens the task list from the local JSON save file when one exists.
    /// Program calls this method once during application startup before ApplicationUi.Start begins,
    /// and it either loads saved tasks or creates the initial sample list when no file is found.
    public static void Open()
    {
        if (File.Exists(FileName))
            Load();
        else
            //TaskList.Tasks = new();
            CreateFirstTimeList();
    }
    
    /// Serializes the current task list and writes it to the local JSON save file.
    /// ApplicationUi calls this method when the user selects Save and Exit, so it normally runs
    /// once per saved exit and may run again in future workflows that expose manual saving.
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

    /// Reads and deserializes tasks from the existing JSON save file.
    /// Open calls this method once during startup when the save file exists, replacing the
    /// in-memory TaskList collection with the loaded values or an empty list on failure.
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
    /// Creates the initial sample tasks when no saved JSON file is available.
    /// Open calls this method once during startup on a first run or whenever the save file
    /// is absent, giving the user an example list to work with.
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