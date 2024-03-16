using TaskTracer.Storage;
using TaskTracer.UserInput;
using TaskTracer.Validation;

namespace TaskTracer.Commands;

public class ImportTasksCommand(IValidator validator, IUserInput userInput, StorageRepository storage) : ICommand
{
    public void Execute(Dictionary<string, string> parameters)
    {
        var parametersLowerCase = parameters.ToDictionary(k => k.Key.ToLower(), v => v.Value);
        var result = validator.ValidatePathParameter(parametersLowerCase);
        parameters.TryGetValue("path", out string path);
        if (result.IsValid)
        {
            storage.ImportTasks(path);
        }
        else
        {
            userInput.ShowError(result.ToString());
        }
    }
}