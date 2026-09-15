# TaskMaster - ToDoLy

C# .NET 8 console application based on the supplied ToDoLy project brief.

## Requirements implemented

- Task model: title, due date, status and project
- Display tasks sorted by date or project
- Add, edit, mark done/undone and remove tasks
- Text-based command-line interface
- Save/load task list as JSON
- First launch starts with an empty task list so the user can create their own tasks

## Visual Studio

Open `TaskMaster.csproj` in Visual Studio 2022 with the .NET 8 SDK installed.
Run with F5 or Ctrl+F5.

When selecting Save and Quit, the application writes `ToDoList.json` to the current directory.
