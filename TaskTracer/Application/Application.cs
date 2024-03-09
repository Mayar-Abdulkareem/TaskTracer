using System.Reflection;
using TaskTracer.DataAccessor;
using TaskTracer.Enums;
using TaskTracer.Models;
using TaskTracer.Storage;
using TaskTracer.UserInput;

namespace TaskTracer.Application;

public class Application
{
    private readonly IDataStorageAccessor _dataStorageAccessor;
    private StorageFactory storage;
    private IUserInput _userInput;
    
    public Application(IDataStorageAccessor dataStorageAccessor, IUserInput userInput)
    {
        _dataStorageAccessor = dataStorageAccessor;
        _userInput = userInput;
        storage = new StorageFactory(_dataStorageAccessor);
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
                    var project = CreateProject(parameters);
                    storage.AddProject(project);
                    Console.WriteLine(storage.ToString());
                    break;
                case "add-task":
                    break;
                case "stop":
                    isValid = false;
                    break;
            }
        }
    }

    Project CreateProject(Dictionary<string, string> parameters)
    {
        Project project = new Project();
        
        foreach (var param in parameters)
        {
            var propertyInfo = typeof(Project).GetProperty(param.Key);
            if (propertyInfo != null)
            {
                if (propertyInfo.PropertyType == typeof(DateTime))
                {
                    if (DateTime.TryParse(param.Value, out DateTime dateTimeValue))
                    {
                        propertyInfo.SetValue(project, dateTimeValue);
                    }
                    else
                    {
                        Console.WriteLine($"Invalid format for property {param.Key}");
                    }
                }
                else if (propertyInfo.PropertyType == typeof(string))
                {
                    propertyInfo.SetValue(project, param.Value);
                }
            }
        }

        storage.AddProject(project); 

        return project;
    }
    //
    // ToDoTask CreateTask(Dictionary<string, string> parameters)
    // {
    //     ToDoTask task = new ToDoTask();
    //
    //     foreach (var param in parameters)
    //     {
    //         var propertyInfo = typeof(ToDoTask).GetProperty(param.Key);
    //         if (propertyInfo != null)
    //         {
    //             if (param.Key == "Status")
    //             {
    //                 switch (param.Value)
    //                 {
    //                     case MyTaskStatus.NotStarted:
    //                         propertyInfo.SetValue(task, "NotStarted");
    //                         break;
    //                     case MyTaskStatus.InProgress:
    //                         propertyInfo.SetValue(task, "InProgress");
    //                         break;
    //                     
    //                 }
    //             }
    //             if (propertyInfo.PropertyType == typeof(DateTime))
    //             {
    //                 if (DateTime.TryParse(param.Value, out DateTime dateTimeValue))
    //                 {
    //                     propertyInfo.SetValue(task, dateTimeValue);
    //                 }
    //                 else
    //                 {
    //                     Console.WriteLine($"Invalid format for property {param.Key}");
    //                 }
    //             }
    //             else if (propertyInfo.PropertyType == typeof(string))
    //             {
    //                 propertyInfo.SetValue(task, param.Value);
    //             }
    //         }
    //     }
    //
    //     return task;
    // }
}