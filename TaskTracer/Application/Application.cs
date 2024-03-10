using System.Reflection;
using TaskTracer.DataAccessor;
using TaskTracer.Models;
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
        storage = new StorageRepository(_dataStorageAccessor);
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
                case "stop":
                    isValid = false;
                    break;
            }
        }
    }
}