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