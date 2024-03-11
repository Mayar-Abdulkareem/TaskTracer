namespace TaskTracer.Validation;

public interface IValidator
{
    public ValidationResult Validate(Dictionary<string, string> parameters);
}