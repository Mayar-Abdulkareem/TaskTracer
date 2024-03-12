using TaskTracer.DataAccessor;
using TaskTracer.Models;
using TaskTracer.Storage;
using TaskTracer.UserInput;
using TaskTracer.Validation;

namespace TaskTracer.Application;

public class Application
{
    private readonly IDataStorageAccessor _dataStorageAccessor;
    private readonly StorageRepository storage;
    private readonly IUserInput _userInput;
    private readonly TraceableFactory factory;
    private readonly IValidator _validator;
    public Application(IDataStorageAccessor dataStorageAccessor, IUserInput userInput, IValidator validator)
    {
        _dataStorageAccessor = dataStorageAccessor;
        _userInput = userInput;
        _validator = validator;
        factory = new TraceableFactory();
        storage = new StorageRepository(_dataStorageAccessor, _userInput, validator);
    }

    public void Run()
    {
        if (storage.Load())
        {
            _userInput.ShowMenu();
            RunCommand();
        }
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
                case "import-tasks":
                    ImportTasks(parameters);
                    break;
                case "save-projects":
                    SaveProjects();
                    break;
                case "save-tasks":
                    SaveTasks();
                    break;
                case "stop":
                    isValid = false;
                    break;
                default:
                    _userInput.ShowError("Unknown command.\n");
                    break;
            }
        }
    }

    private void SaveProjects() => storage.SaveProjects();

    private void SaveTasks() => storage.SaveTasks();

    private void ImportTasks(Dictionary<string, string> parameters)
    {
        var parametersLowerCase = parameters.ToDictionary(k => k.Key.ToLower(), v => v.Value);
        var result = _validator.ValidatePathParameter(parametersLowerCase);
        parameters.TryGetValue("path", out string path);
        if (result.IsValid)
        {
            storage.ImportTasks(path);
        }
        else
        {
            _userInput.ShowError(result.ToString());
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
        var result = _validator.ValidateParameters<Project>(parameters);
        if (result.IsValid)
        {
            var project = factory.CreateProject(parameters);
            storage.AddProject(project);
            _userInput.ShowSuccessMessage($"Project '{project.Title}' added successfully.\n");
        }
        else 
        {
            _userInput.ShowError(result.ToString());
        }
    }

    private void AddTask(Dictionary<string, string> parameters)
    {
        var result = _validator.ValidateParameters<ToDoTask>(parameters);
        if (result.IsValid)
        {
            var task = factory.CreateTask(parameters);
            storage.AddTask(task);
            _userInput.ShowSuccessMessage($"Task '{task.Title}' added successfully.\n");
        }
        else 
        {
            _userInput.ShowError(result.ToString());
        }
    }

    private void DisplayProjects() => storage.DisplayProjects();

    private void DisplayTasks() => storage.DisplayTasks();

    private void EditProject(Dictionary<string, string> parameters)
    {
        var result = _validator.ValidateParameters<Project>(parameters, false, true);
        if (result.IsValid)
        {
            parameters.TryGetValue("id", out string id);
            var projectWithUpdate = storage.FindProjectById(id.Trim());
            if (projectWithUpdate != null)
            {
                parameters.Remove("ID");
                var projectAfterUpdate = factory.CreateProjectWithUpdate(parameters, projectWithUpdate);
                storage.EditProject(id, projectAfterUpdate);
                _userInput.ShowSuccessMessage($"Project with ID {id} updated successfully.\n");
            }
            else
            {
                _userInput.ShowError("Project ID not found.\n");
            }
        }
        else
        {
            _userInput.ShowError(result.ToString());
        }
    }

    private void EditTask(Dictionary<string, string> parameters)
    {
        var result = _validator.ValidateParameters<ToDoTask>(parameters, false, true);
        if (result.IsValid)
        {
            parameters.TryGetValue("id", out string id);
            var taskWithUpdate = storage.FindTaskById(id.Trim());
            if (taskWithUpdate != null)
            {
                parameters.Remove("ID");
                var updatedTask = factory.CreateTaskWithUpdate(parameters, taskWithUpdate);
                storage.EditTask(id, updatedTask);
                _userInput.ShowSuccessMessage($"Task with ID {id} updated successfully.\n");
            }
            else
            {
                _userInput.ShowError("Task ID not found.\n");
            }
        }
        else
        {
            _userInput.ShowError(result.ToString());
        }
    }

    private void DeleteProject(Dictionary<string, string> parameters)
    {
        var result = _validator.ValidateParameters<Project>(parameters, false, true);
        if (result.IsValid)
        {
            parameters.TryGetValue("id", out string id);
            var success = storage.DeleteProject(id.Trim());
            if (success)
            {
                _userInput.ShowSuccessMessage($"Project with ID {id} deleted successfully.\n");
            }
            else
            {
                _userInput.ShowError($"Project with ID {id} wasn't found.\n");
            }
        }
        else
        {
            _userInput.ShowError(result.ToString());
        }
    }

    private void DeleteTask(Dictionary<string, string> parameters)
    {
        var result = _validator.ValidateParameters<ToDoTask>(parameters, false, true);
        if (result.IsValid)
        {
            parameters.TryGetValue("id", out string id);
            var success = storage.DeleteTask(id.Trim());
            if (success)
            {
                _userInput.ShowSuccessMessage($"Task with ID {id} deleted successfully.\n");
            }
            else
            {
                _userInput.ShowError($"Task with ID {id} wasn't found.\n");
            }
        }
        else
        {
            _userInput.ShowError(result.ToString());
        }
    }
}