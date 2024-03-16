using System.Reflection;
using TaskTracer.Models;
using TaskTracer.UserInput;

namespace TaskTracer.Application;

public class ModelFactory(IUserInput userInput)
{
    public Project CreateProject(Dictionary<string, string> parameters)
    {
        Project project = new Project();
        SetPropertyValues(project, parameters);
        return project;
    }
    
    public ToDoTask CreateTask(Dictionary<string, string> parameters)
    {
        ToDoTask task = new ToDoTask();
        SetPropertyValues(task, parameters);
        return task;
    }

    public Project CreateProjectWithUpdate(Dictionary<string, string> parameters, Project project)
    {
        SetPropertyValues(project, parameters);
        return project;
    }
    
    public ToDoTask CreateTaskWithUpdate(Dictionary<string, string> parameters, ToDoTask task)
    {
        SetPropertyValues(task, parameters);
        return task;
    }
    
    private void SetPropertyValues<T>(T targetObject, Dictionary<string, string> parameters)
    {
        foreach (var param in parameters)
        {
            var propertyInfo = typeof(T).GetProperty(param.Key, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (propertyInfo == null) continue;
            try
            {
                SetPropertyValue(targetObject, propertyInfo, param.Value);
            }
            catch (Exception ex)
            {
                userInput.ShowError($"Error setting property {param.Key}: {ex.Message}");
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
        var format = "MM-dd-yyyy"; 
        var culture = System.Globalization.CultureInfo.InvariantCulture;

        if (DateTime.TryParseExact(value, format, culture, System.Globalization.DateTimeStyles.None, out DateTime dateTimeValue))
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