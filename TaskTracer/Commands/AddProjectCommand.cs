using TaskTracer.Application;
using TaskTracer.Models;
using TaskTracer.Storage;
using TaskTracer.UserInput;
using TaskTracer.Validation;

namespace TaskTracer.Commands;

public class AddProjectCommand(IValidator validator, ModelFactory factory, IUserInput userInput, StorageRepository storage) : ICommand
{
        
    public void Execute(Dictionary<string, string> parameters)
    {
        var result = validator.ValidateParameters<Project>(parameters);
        if (result.IsValid)
        {
            var project = factory.CreateProject(parameters);
            storage.AddProject(project);
            userInput.ShowSuccessMessage($"Project '{project.Title}' added successfully.\n");
        }
        else 
        {
            userInput.ShowError(result.ToString());
        }
    }
}