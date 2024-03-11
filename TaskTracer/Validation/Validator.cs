using System.Reflection;
using TaskTracer.Models;

namespace TaskTracer.Validation;

public class Validator 
{
    public ValidationResult Validate<T>(Dictionary<string, string> parameters)
    {
        ValidationResult result = new ValidationResult();

        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var parametersLowerCase = parameters.ToDictionary(k => k.Key.ToLower(), v => v.Value);

        foreach (var property in properties)
        {
            var name = property.Name.ToLower();
            if (!parametersLowerCase.ContainsKey(name))
            {
                if (!name.Equals("id"))
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
}