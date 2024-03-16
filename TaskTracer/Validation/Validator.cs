using System.Globalization;
using System.Reflection;
using TaskTracer.Models;
using TaskTracer.Storage;

namespace TaskTracer.Validation;

public class Validator : IValidator
{
    /// <summary>
    /// Validates user-provided parameters. Not null, valid date format, and valid enum type.
    /// </summary>
    /// <typeparam name="T">The type of the model that parameters are validated against.</typeparam>
    /// <param name="parameters">A dictionary containing the parameters to validate, where the key is the parameter name and the value is the parameter's value.</param>
    /// <param name="requiresAllProperties">Indicates whether all properties of the model <typeparamref name="T"/> need to be present in the parameters for the validation to pass. Useful for operations like adding a new record, where completeness is necessary.</param>
    /// <param name="requiresId">Indicates whether the validation process should verify the presence of an 'ID' parameter. This is typically used for operations like editing or deleting, where identifying the target record is essential.</param>
    /// <returns>A <see cref="ValidationResult"/> object that contains the outcome of the validation process, including whether the validation succeeded and any messages related to validation failures.</returns>
    public ValidationResult ValidateParameters<T>(Dictionary<string, string> parameters, bool requiresAllProperties = true, bool requiresId = false)
    {
        ValidationResult result = new ValidationResult();

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var parametersLowerCase = parameters.ToDictionary(k => k.Key.ToLower(), v => v.Value);

        foreach (var property in properties)
        {
            var name = property.Name.ToLower();
            
            if (!parametersLowerCase.ContainsKey(name))
            {
                if ((requiresId && name.Equals("id")) || (!name.Equals("id") && requiresAllProperties))
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
        return Enum.TryParse(enumType, value, ignoreCase: true, out var enumValue);
    }

    private bool ValidateDateTime(string value)
    {
        var format = "MM-dd-yyyy"; 
        var culture = System.Globalization.CultureInfo.InvariantCulture;
        return DateTime.TryParseExact(value, format, culture, DateTimeStyles.None, out var dateTimeValue);
    }

    // Unique ID for the task
    // Task start date before the task due date
    // The project for that task exist
    // Task start date before 
    // Task due date before project due date 
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
                continue;
            }
            
            var project = projects.First();

            if (project.DueDate > task.DueDate)
            {
                result.AddError($"Task {task.ID} in Project {project.ID} has a due date after the project's due date.");
            } 
        }
        return result;
    }

    // Make sure there is a path parameter passed
    // Validate the ability te read from that path 
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
            result.AddError("Error importing the file");
        }

        return result;
    }
}