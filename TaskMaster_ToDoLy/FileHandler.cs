using System.Text.Json;

namespace TaskMaster;


//Handles saving and loading the task list as a JSON file.
public static class FileHandler
{
    //Gets the location of the JSON save file.
    private static readonly string FileName =
        Path.Combine(
            Environment.CurrentDirectory,
            "ToDoList.json");
    //Gets the JSON serialization settings.
    private static readonly JsonSerializerOptions Options = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };
    
    //Saves the current task list to the JSON file.
    public static void Save()
    {
        try
        {
            string json =
                JsonSerializer.Serialize(
                    TaskList.Tasks,
                    Options);

            File.WriteAllText(FileName, json);

            ApplicationUI.WriteInColor(ConsoleColor.Green, "Your To-Do list has been saved.");
        }
        catch (Exception ex)
        {
            ApplicationUI.WriteInColor(ConsoleColor.Red, $"Failed to save To-Do list: {ex.Message}");
        }
    }

    
    //Opens the saved task list.
    //If no file exists, the application starts with an empty list.
    
    public static void Open()
    {
        if (File.Exists(FileName))
        {
            ApplicationUI.WriteInColor(ConsoleColor.Yellow, "Loading...");
            Load();
        }
        else
        {
            //TaskList.Tasks = new();
            ApplicationUI.WriteInColor(ConsoleColor.Yellow, "Creating From Sample Data......");
            CreateFirstTimeList();
        }
    }

    
    //Loads tasks from the JSON save file.
    
    private static void Load()
    {
        try
        {
            string json = File.ReadAllText(FileName);

            TaskList.Tasks =
                JsonSerializer.Deserialize<List<Task>>(
                    json,
                    Options) ?? new();

            ApplicationUI.WriteInColor(ConsoleColor.Green, "Opened your saved list.");
        }
        catch (Exception ex)
        {
            TaskList.Tasks = new();

            ApplicationUI.WriteInColor(ConsoleColor.Red, $"Failed to open saved To-Do list: {ex.Message}");
        }
    }
    
    
    private static void CreateFirstTimeList()
    {
        TaskList.Tasks = new()
        {
            new("Do dishes", "Chores", new DateTime(2026, 2, 20)),
            new("Take out trash", "Chores", new DateTime(2026, 2, 22), true),
            new("Read a book", "Personal", new DateTime(2026, 3, 25)),
            new("Mini Project", "C# .NET", new DateTime(2026, 2, 16), true),
            new("Individual Project", "C# .NET", new DateTime(2026, 2, 22), true),
            new("HTML & CSS", "C# .NET", new DateTime(2026, 2, 25))
        };

        ApplicationUI.WriteInColor(ConsoleColor.Yellow, "No saved list found. Created a sample list of tasks.");
    }
}
