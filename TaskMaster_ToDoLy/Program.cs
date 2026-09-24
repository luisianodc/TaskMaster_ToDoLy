using TaskMaster_ToDoLy;

/// Serves as the application's executable entry point.
/// It coordinates the initial loading of saved task data and the launch of the console user interface.
/// The class is used by the .NET runtime whenever the TaskMaster ToDoLy application starts.
/// Its entry method runs once per application session and remains active through the UI workflow.
internal static class Program
{
    /// Loads saved task data and starts the main console application loop.
    /// The .NET runtime calls this method once when the program launches; FileHandler.Open prepares
    /// the in-memory task list before ApplicationUi.Start handles all user interaction.
    private static void Main()
    {
        FileHandler.Open();
        ApplicationUi.Start();
    }
}

/**
 Below is the text or prompt used in the github copilot
 to generate documentation comments:
 add few rows of documentation comment describing what 
 this class does and where and how often in the application 
 this class is used. generate documentation comments for 
 all the functions and methods in this class, where th will 
 be used in the application and how often
*/