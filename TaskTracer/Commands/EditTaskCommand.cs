using TaskTracer.Application;
using TaskTracer.Models;
using TaskTracer.Storage;
using TaskTracer.UserInput;
using TaskTracer.Validation;

namespace TaskTracer.Commands;

public class EditTaskCommand(IValidator validator, ModelFactory factory, IUserInput userInput, StorageRepository storage) : ICommand

{
    public void Execute(Dictionary<string, string> parameters)
    {
        var result = validator.ValidateParameters<ToDoTask>(parameters, false, true);
        if (result.IsValid)
        {
            parameters.TryGetValue("id", out string id);
            var taskWithUpdate = storage.FindTaskById(id.Trim());
            if (taskWithUpdate != null)
            {
                parameters.Remove("ID");
                var updatedTask = factory.CreateTaskWithUpdate(parameters, taskWithUpdate);
                storage.EditTask(id, updatedTask);
                userInput.ShowSuccessMessage($"Task with ID {id} updated successfully.\n");
            }
            else
            {
                userInput.ShowError("Task ID not found.\n");
            }
        }
        else
        {
            userInput.ShowError(result.ToString());
        }
    }
}