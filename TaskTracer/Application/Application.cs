using TaskTracer.DataAccessor;
using TaskTracer.Storage;
using TaskTracer.UserInput;

namespace TaskTracer.Application;

public class Application
{
    private readonly IDataStorageAccessor _dataStorageAccessor;
    private readonly StorageRepository storage;
    private readonly IUserInput _userInput;
    private readonly TraceableFactory factory;

    public Application(IDataStorageAccessor dataStorageAccessor, IUserInput userInput)
    {
        _dataStorageAccessor = dataStorageAccessor;
        _userInput = userInput;
        factory = new TraceableFactory();
        storage = new StorageRepository(_dataStorageAccessor, _userInput);
    }

    public void Run()
    {
        storage.Load();
        _userInput.ShowMenu();
        RunCommand();
    }

    private void RunCommand()
    {
        bool isValid = true;
        while (isValid)
        {
            string line = _userInput.ReadCommand();
            var parsedCommand = _userInput.ParseCommand(line);
            var command = parsedCommand.Command;
            var parameters = parsedCommand.Parameters;

            switch (command.ToLower())
            {
                case "show-menu":
                    ShowMenu();
                    break;
                case "add-project":
                    AddProject(parameters);
                    break;
                case "add-task":
                    AddTask(parameters);
                    break;
                case "display-projects":
                    DisplayProjects();
                    break;
                case "display-tasks":
                    DisplayTasks();
                    break;
                case "edit-project":
                    EditProject(parameters);
                    break;
                case "edit-task":
                    EditTask(parameters);
                    break;
                case "delete-project":
                    DeleteProject(parameters);
                    break;
                case "delete-task":
                    DeleteTask(parameters);
                    break;
                case "view-today-tasks":
                    ViewTodayTasks();
                    break;
                case "view-tasks":
                    ViewTasksByDate(parameters);
                    break;
                case "view-overdue-tasks":
                    ViewOverdueTasks();
                    break;
                case "stop":
                    isValid = false;
                    break;
                default:
                    _userInput.ShowError("Unknown command.");
                    break;
            }
        }
    }

    private void ViewTodayTasks()
    {
        DateTime now = DateTime.Now;
        storage.ViewTasksDueOnDate(now);
    }

    private void ViewOverdueTasks() => storage.DisplayOverdueTasks();
    
    private void ViewTasksByDate(Dictionary<string, string> parameters)
    {
        var dateArg = parameters.FirstOrDefault();
        if (DateTime.TryParse(dateArg.Value, out DateTime specificDate))
        {
            storage.ViewTasksDueOnDate(specificDate);
        }
        else
        {
            _userInput.ShowError("Invalid date format. Use 'today' or 'date=YYYY-MM-DD'.");
        }
    }
    private void ShowMenu() => _userInput.ShowMenu();

    private void AddProject(Dictionary<string, string> parameters)
    {
        var project = factory.CreateProject(parameters);
        storage.AddProject(project);
        _userInput.ShowSuccessMessage($"Project '{project.Title}' added successfully.");
    }

    private void AddTask(Dictionary<string, string> parameters)
    {
        var task = factory.CreateTask(parameters);
        storage.AddTask(task);
        _userInput.ShowSuccessMessage($"Task '{task.Title}' added successfully.");
    }

    private void DisplayProjects() => storage.DisplayProjects();

    private void DisplayTasks() => storage.DisplayTasks();

    private void EditProject(Dictionary<string, string> parameters)
    {
        if (parameters.TryGetValue("ID".Trim().ToLower(), out string id) && !string.IsNullOrWhiteSpace(id))
        {
            var projectWithUpdate = storage.FindProjectById(id.Trim());
            if (projectWithUpdate != null)
            {
                parameters.Remove("ID");
                var projectAfterUpdate = factory.CreateProjectWithUpdate(parameters, projectWithUpdate);
                storage.EditProject(id, projectAfterUpdate);
                _userInput.ShowSuccessMessage($"Project with ID {id} updated successfully.");
            }
            else
            {
                _userInput.ShowError("Project ID not found.");
            }
        }
        else
        {
            _userInput.ShowError("Project ID is required for editing.");
        }
    }

    private void EditTask(Dictionary<string, string> parameters)
    {
        if (parameters.TryGetValue("ID".Trim().ToLower(), out string id) && !string.IsNullOrWhiteSpace(id))
        {
            var taskWithUpdate = storage.FindTaskById(id.Trim());
            if (taskWithUpdate != null)
            {
                parameters.Remove("ID");
                var updatedTask = factory.CreateTaskWithUpdate(parameters, taskWithUpdate);
                storage.EditTask(id, updatedTask);
                _userInput.ShowSuccessMessage($"Task with ID {id} updated successfully.");
            }
            else
            {
                _userInput.ShowError("Task ID not found.");
            }
        }
        else
        {
            _userInput.ShowError("Task ID is required for editing.");
        }
    }

    private void DeleteProject(Dictionary<string, string> parameters)
    {
        if (parameters.TryGetValue("ID".Trim().ToLower(), out string id) && !string.IsNullOrWhiteSpace(id))
        {
            var success = storage.DeleteProject(id.Trim());
            if (success)
            {
                _userInput.ShowSuccessMessage($"Project with ID {id} deleted successfully.");
            }
            else
            {
                _userInput.ShowError($"Project with ID {id} wasn't found.");
            }
        }
        else
        {
            _userInput.ShowError("Project ID is required for deletion.");
        }
    }

    private void DeleteTask(Dictionary<string, string> parameters)
    {
        if (parameters.TryGetValue("ID".Trim().ToLower(), out string id) && !string.IsNullOrWhiteSpace(id))
        {
            var success = storage.DeleteTask(id.Trim());
            if (success)
            {
                _userInput.ShowSuccessMessage($"Task with ID {id} deleted successfully.");
            }
            else
            {
                _userInput.ShowError($"Task with ID {id} wasn't found.");
            }
        }
        else
        {
            _userInput.ShowError("Task ID is required for deletion.");
        }
    }
}