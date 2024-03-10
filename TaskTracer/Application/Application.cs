using TaskTracer.DataAccessor;
using TaskTracer.Storage;
using TaskTracer.UserInput;

namespace TaskTracer.Application;

public class Application
{
    private readonly IDataStorageAccessor _dataStorageAccessor;
    private StorageRepository storage;
    private IUserInput _userInput;
    private TraceableFactory factory = new TraceableFactory();
    
    public Application(IDataStorageAccessor dataStorageAccessor, IUserInput userInput)
    {
        _dataStorageAccessor = dataStorageAccessor;
        _userInput = userInput;
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
        
            switch (command)
            {
                case "show-menu":
                    _userInput.ShowMenu();
                    break;
                case "add-project":
                    var project = factory.CreateProject(parameters);
                    storage.AddProject(project);
                    _userInput.ShowSuccessMessage($"Project with {project.ToString()} added successfully");
                    break;
                case "add-task":
                    var task = factory.CreateTask(parameters);
                    storage.AddTask(task);
                    _userInput.ShowSuccessMessage($"Task with {task.ToString()} added successfully");
                    break;
                case "display-projects":
                    storage.DisplayProjects();
                    break;
                case "display-tasks":
                    storage.DisplayTasks();
                    break;
                case "edit-project":
                    string id = parameters["ID".Trim().ToLower()];
                    var projectWithUpdate = storage.FindProjectById(id);
                    if (projectWithUpdate != null)
                    {
                        parameters.Remove("ID");
                        var projectAfterUpdate = factory.CreateProjectWithUpdate(parameters, projectWithUpdate);
                        storage.EditProject(id, projectWithUpdate);
                        _userInput.ShowSuccessMessage($"Project with {projectWithUpdate.ToString()} updated successfully");
                    }
                    else
                    {
                        _userInput.ShowError("ID not found");
                    }
                    break;
                case "edit-task":
                    string key = parameters["ID".Trim().ToLower()];
                    var taskWithUpdate = storage.FindTaskById(key);
                    if (taskWithUpdate != null)
                    {
                        parameters.Remove("ID");
                        taskWithUpdate = factory.CreateTaskWithUpdate(parameters, taskWithUpdate);
                        storage.EditTask(key, taskWithUpdate);
                        _userInput.ShowSuccessMessage($"Task with {taskWithUpdate.ToString()} updated successfully");
                    }
                    else
                    {
                        _userInput.ShowError("ID not found");
                    }
                    break;
                case "stop":
                    isValid = false;
                    break;
            }
        }
    }
}