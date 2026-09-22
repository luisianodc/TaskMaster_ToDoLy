# TaskMaster_ToDoLy

A C# console application for managing a simple To-Do list, implemented with **.NET 10**.

Repository: https://github.com/luisianodc/TaskMaster_ToDoLy

Author: **Luisiano Denovan Calill**

## 1. Project overview

TaskMaster_ToDoLy is a console-based ToDoLy application. The project demonstrates:

- C# object-oriented programming
- .NET 10 console application development
- A menu-driven command-line user interface
- Task creation, editing, completion and removal
- Sorting tasks by due date or project
- JSON persistence using `System.Text.Json`
- Basic validation and cancellation/back behaviour
- Git/GitHub based source control

The project file targets:

```xml
<TargetFramework>net10.0</TargetFramework>
```

The application is a console program rather than a web application, desktop GUI or database-backed service.

## 2. Main features

- Add a new task
- View tasks
- Sort tasks by date
- Sort tasks by project
- Edit title, project and due date
- Mark a task as done/undone
- Remove a task
- Save the current list to JSON
- Load the saved list when the application starts
- Exit without saving, with a confirmation prompt
- Cancel/back behaviour during task operations
- Console feedback using colours




### Task data

Each task contains:

| Property | Type | Purpose |
|---|---|---|
| `Title` | `string` | Task name |
| `DueDate` | `DateTime` | Due date |
| `IsDone` | `bool` | Completion status |
| `Project` | `string` | Project/category |

## 3. Project structure

```text
TaskMaster_ToDoLy-main/
├── TaskMaster_ToDoLy.sln
├── TaskMaster_ToDoLy/
│   ├── ApplicationUI.cs
│   ├── FileHandler.cs
│   ├── Program.cs
│   ├── Task.cs
│   ├── TaskList.cs
│   └── TaskMaster_ToDoLy.csproj
└── README.md
```


### Responsibilities of the main classes

**`Program.cs`**

Application entry point. It opens the saved data and starts the UI:

```csharp
FileHandler.Open();
ApplicationUI.Start();
```

**`Task.cs`**

Represents one task and contains the task properties plus constructors.

**`TaskList.cs`**

Stores the application's current collection:

```csharp
public static List<Task> Tasks { get; set; } = new();
```

**`FileHandler.cs`**

Responsible for persistence. It serializes/deserializes the task list as JSON using `System.Text.Json`.

The file is named:

```text
ToDoList.json
```

and is stored under the application's current working directory.

**`ApplicationUI.cs`**

Contains the console menu, task listing, input validation, add/edit/remove functionality, sorting, status changes and save/exit interaction.

## 4. Requirements

Install:

1. **.NET 10 SDK**
2. An IDE/editor such as:
   - Visual Studio 2026
   - JetBrains Rider
   - Visual Studio Code or another editor with C# support

The .NET SDK is required for building and running the application from the terminal.

Official .NET downloads:
https://dotnet.microsoft.com/en-us/download/dotnet/10.0

The project currently targets `net10.0`. If the required .NET 10 SDK is not installed, the project will not build.

## 5. Run in Visual Studio

### Option A — Open the solution

1. Download or clone the repository.
2. Extract the ZIP if you downloaded a ZIP archive.
3. Open:

```text
TaskMaster_ToDoLy.sln
```

4. Let Visual Studio restore the project.
5. Make sure `TaskMaster_ToDoLy` is the startup project.
6. Build the solution:
   - **Build → Build Solution**
7. Start the application:
   - **Debug → Start Without Debugging**
   - or press `Ctrl + F5`

Because this is a console application, the program runs in a terminal/console window.

### If Visual Studio reports that .NET 10 is missing

Install a current .NET 10 SDK and restart Visual Studio.

## 6. Run in JetBrains Rider

1. Clone or extract the project.
2. Open the folder or solution:

```text
TaskMaster_ToDoLy.sln
```

3. Allow Rider to restore NuGet/project dependencies.
4. Open the `TaskMaster_ToDoLy` project.
5. Select the project/run configuration.
6. Run with the green **Run** button or use the configured keyboard shortcut.

The application runs as a console application in Rider's terminal/run window.

## 7. Run from a command terminal

Open PowerShell, Windows Terminal, Command Prompt, Bash or another terminal.

Go to the project directory containing the `.csproj` file:

```powershell
cd path\to\TaskMaster_ToDoLy-main\TaskMaster_ToDoLy
```

Check the installed SDK:

```powershell
dotnet --version
```

You should have a .NET 10 SDK installed.

Restore:

```powershell
dotnet restore
```

Build:

```powershell
dotnet build
```

Run:

```powershell
dotnet run
```

You can also run the solution from the directory containing the `.sln` file:

```powershell
dotnet run --project .\TaskMaster_ToDoLy\TaskMaster_ToDoLy.csproj
```

## 8. Application workflow

At startup:

```text
Program
  │
  ├── FileHandler.Open()
  │      ├── ToDoList.json exists → Load()
  │      └── No file → initialise task list
  │
  └── ApplicationUI.Start()
         │
         ├── Show task list
         ├── Add task
         ├── Edit task
         │    ├── Update
         │    ├── Mark done/undone
         │    └── Remove
         ├── Save and exit
         └── Exit without saving
```


## Screenshot Main Menu
<img width="310" height="172" alt="amainmenu" src="https://github.com/user-attachments/assets/5efed177-1901-4b1a-9316-d03226ffaa2b" />

## Screenshot Show tasks Menu
<img width="315" height="150" alt="ashowmenu" src="https://github.com/user-attachments/assets/c21e26ed-2333-44ec-8bbc-79aefc7f5508" />

## Screenshot Show tasks sample data Menu
<img width="446" height="248" alt="ashowmenusampledata" src="https://github.com/user-attachments/assets/af2a8963-e3d3-44fe-92a3-107263d83df9" />


## 9. File handling and JSON persistence

`FileHandler` uses `System.Text.Json`.

Saving:

```csharp
string json =
    JsonSerializer.Serialize(
        TaskList.Tasks,
        Options);

File.WriteAllText(FileName, json);
```

Loading:

```csharp
string json = File.ReadAllText(FileName);

TaskList.Tasks =
    JsonSerializer.Deserialize<List<Task>>(
        json,
        Options) ?? new();
```

The JSON is indented for readability and property name matching is configured to be case-insensitive.

### Important behaviour

The supplied ZIP version contains a `CreateFirstTimeList()` method with sample tasks when `ToDoList.json` does not exist.

The public GitHub README currently describes first launch as an empty list. Therefore, the ZIP source and the GitHub README are not completely aligned on this point. The README you are downloading here documents the supplied source code behaviour.

## 10. Suggested demo

For a presentation/demo, show this sequence:

1. Start the application.
2. Show the main menu.
3. Select **Show Task List**.
4. Demonstrate sorting by date.
5. Demonstrate sorting by project.
6. Add a new task.
7. Edit the task.
8. Mark it done.
9. Remove a task.
10. Save and exit.
11. Run the program again and show that saved data is loaded from `ToDoList.json`.

## 11. Development approach

A practical development flow for this project is:

1. Understand the requirements.
2. Break the application into responsibilities.
3. Design the task model.
4. Design the menu and user flow.
5. Implement the in-memory task list.
6. Add CRUD-style task operations.
7. Add sorting.
8. Add JSON persistence.
9. Add validation and cancellation paths.
10. Test normal and error scenarios.
11. Refactor repeated logic and improve readability.
12. Document the project and prepare the demo.

## 12. Problem-solving approach

Typical problems in a console application like this are:

- Invalid menu input
- Invalid task numbers
- Invalid dates
- Keeping user input simple while still validating it
- Avoiding data loss when exiting
- Serializing/deserializing a list of objects
- Keeping UI code and persistence code understandable

The project addresses these with helper methods such as:

- `ReadRequiredOrCancel`
- `ReadOptionalOrCancel`
- `ReadDateOrCancel`
- `ReadTaskIndexOrCancel`
- `ExitWithoutSaving`

## 13. Useful development tips

- Keep each class responsible for one main area.
- Validate user input at the boundary.
- Keep persistence code separate from UI code.
- Use meaningful names for classes, methods and properties.
- Test both the successful path and invalid input.
- Save often while developing.
- Use Git commits to create recoverable milestones.
- Keep README instructions aligned with the actual source code.
- Before a presentation, rehearse the demo from a clean working directory.

## 14. AI-assisted development

If AI tools are used during development, a good workflow is:

1. Use AI to explain unfamiliar C#/.NET concepts.
2. Ask for alternative implementation approaches.
3. Use AI to help identify likely causes of compiler/runtime errors.
4. Ask for documentation and README improvements.
5. Review every generated suggestion.
6. Test the resulting code yourself.
7. Do not treat generated code as automatically correct.

AI should support understanding and problem solving rather than replace testing, debugging and developer judgement.

## 15. Useful links

- Project repository: https://github.com/luisianodc/TaskMaster_ToDoLy
- .NET 10 downloads: https://dotnet.microsoft.com/en-us/download/dotnet/10.0
- .NET documentation: https://learn.microsoft.com/dotnet/
- C# documentation: https://learn.microsoft.com/dotnet/csharp/
- System.Text.Json: https://learn.microsoft.com/dotnet/standard/serialization/system-text-json/overview
- Git documentation: https://git-scm.com/doc
- JetBrains Rider: https://www.jetbrains.com/rider/
- Visual Studio: https://visualstudio.microsoft.com/

## 16. Author

**Luisiano Denovan Calill**

---

## Licence

No explicit licence is currently specified in the supplied project files. Add a licence file if the project is intended for redistribution or open-source use.




