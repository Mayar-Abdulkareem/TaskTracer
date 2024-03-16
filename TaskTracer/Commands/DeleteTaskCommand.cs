using TaskTracer.Application;
using TaskTracer.Models;
using TaskTracer.Storage;
using TaskTracer.UserInput;
using TaskTracer.Validation;

namespace TaskTracer.Commands;

public class DeleteTaskCommand(IValidator validator, ModelFactory factory, IUserInput userInput, StorageRepository storage) : ICommand
{
    public void Execute(Dictionary<string, string> parameters)
    {
        var result = validator.ValidateParameters<ToDoTask>(parameters, false, true);
        if (result.IsValid)
        {
            parameters.TryGetValue("id", out string id);
            var success = storage.DeleteTask(id.Trim());
            if (success)
            {
                userInput.ShowSuccessMessage($"Task with ID {id} deleted successfully.\n");
            }
            else
            {
                userInput.ShowError($"Task with ID {id} wasn't found.\n");
            }
        }
        else
        {
            userInput.ShowError(result.ToString());
        }
    }
}