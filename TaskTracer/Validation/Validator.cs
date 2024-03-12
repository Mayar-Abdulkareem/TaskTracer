using System.Reflection;
using TaskTracer.Models;
using TaskTracer.Storage;

namespace TaskTracer.Validation;

public class Validator : IValidator
{
    public ValidationResult ValidateParameters<T>(Dictionary<string, string> parameters, bool containsAllProperty = true, bool validateId = false)
    {
        ValidationResult result = new ValidationResult();

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var parametersLowerCase = parameters.ToDictionary(k => k.Key.ToLower(), v => v.Value);

        foreach (var property in properties)
        {
            var name = property.Name.ToLower();
            
            if (!parametersLowerCase.ContainsKey(name))
            {
                if ((validateId && name.Equals("id")) || (!name.Equals("id") && containsAllProperty))
                {
                    result.AddError($"Missing {property.Name} property.");
                }
                continue;
            }

            var parameterValue = parametersLowerCase[name];
            if (property.PropertyType == typeof(string) && !ValidateString(parameterValue))
            {
                result.AddError($"{property.Name} can't be null or empty.");
            }
            else if (property.PropertyType.IsEnum && !ValidateEnum(property.PropertyType, parameterValue))
            {
                result.AddError($"Invalid value for {property.Name}.");
            }
            else if (property.PropertyType == typeof(DateTime) && !ValidateDateTime(parameterValue))
            {
                result.AddError($"Invalid value for {property.Name}.");
            }
        }
        return result;
    }
    
    private bool ValidateString(string value)
    {
        return !string.IsNullOrWhiteSpace(value);
    }

    private bool ValidateEnum(Type enumType, string value)
    {
        object enumValue;
        return Enum.TryParse(enumType, value, ignoreCase: true, out enumValue);
    }

    private bool ValidateDateTime(string value)
    {
        DateTime dateTimeValue;
        return DateTime.TryParse(value, out dateTimeValue);
    }

    public ValidationResult ValidateTasks(Storage<Project> projectsStorage, Storage<ToDoTask> tasksStorage, Storage<ToDoTask> previousTasks)
    {
        ValidationResult result = new ValidationResult();

        var tasks = tasksStorage.GetItems();

        foreach (var task in tasks)
        {
            if (previousTasks.ContainsID(task.ID))
            {
                result.AddError($"{task.ID} already exist");
            }
            
            if (task.StartDate > task.DueDate)
            {
                result.AddError($"Task {task.ID}'s start date is not before its due date.");
            }

            var projects = projectsStorage.GetItems(project => project.ID == task.PID);
            
            if (projects.Count() == 0)
            {
                result.AddError($"Project {task.PID} doesn't exist.");
            }
            else
            {
                var project = projects.First();

                if (project.DueDate > task.StartDate)
                {
                    result.AddError($"Task {task.ID} in Project {project.ID} has a start date after the project's due date.");
                } 
            }
        }
        return result;
    }

    public ValidationResult ValidatePathParameter(Dictionary<string, string> parameters)
    {
        ValidationResult result = new ValidationResult();

        if (!parameters.ContainsKey("path"))
        {
            result.AddError("Missing path parameter.");
            return result;
        }

        parameters.TryGetValue("path", out string path);
        
        try
        {
            using (var fs = File.OpenRead(path)) { }
        }
        catch (Exception ex)
        {
            result.AddError(ex.Message);
        }

        return result;
    }
}