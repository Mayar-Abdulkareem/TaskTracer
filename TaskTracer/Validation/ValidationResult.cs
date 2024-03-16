using System.Text;

namespace TaskTracer.Validation;

public class ValidationResult
{
    public bool IsValid { get; set; } = true;
    private List<string> Errors = new List<string>();
    
    public void AddError(string error)
    {
        IsValid = false;
        Errors.Add(error);
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        foreach (var error in Errors)
        {
            stringBuilder.Append(error + "\n");
        }
        return stringBuilder.ToString();
    }
}