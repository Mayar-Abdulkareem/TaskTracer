namespace TaskTracer.Commands;
using TaskTracer.Application;
using TaskTracer.Models;
using TaskTracer.Storage;
using TaskTracer.UserInput;
using TaskTracer.Validation;

public class AddTaskCommand(IValidator validator, ModelFactory factory, IUserInput userInput, StorageRepository storage) : ICommand
{
    public void Execute(Dictionary<string, string> parameters)
    {
        var result = validator.ValidateParameters<ToDoTask>(parameters);
        if (result.IsValid)
        {
            var task = factory.CreateTask(parameters);
            storage.AddTask(task);
            userInput.ShowSuccessMessage($"Task '{task.Title}' added successfully.\n");
        }
        else 
        {
            userInput.ShowError(result.ToString());
        }
    }
}