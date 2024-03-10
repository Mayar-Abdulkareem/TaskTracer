using System.Reflection;
using TaskTracer.DataAccessor;
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
                    _userInput.ShowSuccessMessage($"Project with {project.ToString()} added successfully");
                    break;
                case "add-task":
                    var task = CreateTask(parameters);
                    storage.AddTask(task);
                    _userInput.ShowSuccessMessage($"Task with {task.ToString()} added successfully");
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
        SetPropertyValues(project, parameters);
        storage.AddProject(project); 
        return project;
    }
    
    ToDoTask CreateTask(Dictionary<string, string> parameters)
    {
        ToDoTask task = new ToDoTask();
        SetPropertyValues(task, parameters);
        return task;
    }
    
    private void SetPropertyValues<T>(T targetObject, Dictionary<string, string> parameters)
    {
        foreach (var param in parameters)
        {
            var propertyInfo = typeof(T).GetProperty(param.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo != null)
            {
                try
                {
                    SetPropertyValue(targetObject, propertyInfo, param.Value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error setting property {param.Key}: {ex.Message}");
                }
            }
        }
    }

    private void SetPropertyValue<T>(T targetObject, PropertyInfo propertyInfo, string value)
    {
        if (propertyInfo.PropertyType == typeof(DateTime))
        {
            SetDateTimeProperty(targetObject, propertyInfo, value);
        }
        else if (propertyInfo.PropertyType.IsEnum)
        {
            SetEnumProperty(targetObject, propertyInfo, value);
        }
        else if (propertyInfo.PropertyType == typeof(string))
        {
            propertyInfo.SetValue(targetObject, value);
        }
    }

    private void SetDateTimeProperty<T>(T targetObject, PropertyInfo propertyInfo, string value)
    {
        if (DateTime.TryParse(value, out DateTime dateTimeValue))
        {
            propertyInfo.SetValue(targetObject, dateTimeValue);
        }
        else
        {
            Console.WriteLine($"Invalid format for property {propertyInfo.Name}");
        }
    }

    private void SetEnumProperty<T>(T targetObject, PropertyInfo propertyInfo, string value)
    {
        try
        {
            var enumValue = Enum.Parse(propertyInfo.PropertyType, value, ignoreCase: true);
            propertyInfo.SetValue(targetObject, enumValue);
        }
        catch
        {
            Console.WriteLine($"Invalid enum value for property {propertyInfo.Name}");
        }
    }
}