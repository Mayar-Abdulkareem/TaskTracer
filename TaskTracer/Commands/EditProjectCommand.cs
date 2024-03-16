using TaskTracer.Application;
using TaskTracer.Models;
using TaskTracer.Storage;
using TaskTracer.UserInput;
using TaskTracer.Validation;

namespace TaskTracer.Commands;

public class EditProjectCommand(IValidator validator, ModelFactory factory, IUserInput userInput, StorageRepository storage) : ICommand
{
    public void Execute(Dictionary<string, string> parameters)
    {
        var result = validator.ValidateParameters<Project>(parameters, false, true);
        if (result.IsValid)
        {
            parameters.TryGetValue("id", out string id);
            var projectWithUpdate = storage.FindProjectById(id.Trim());
            if (projectWithUpdate != null)
            {
                parameters.Remove("ID");
                var projectAfterUpdate = factory.CreateProjectWithUpdate(parameters, projectWithUpdate);
                storage.EditProject(id, projectAfterUpdate);
                userInput.ShowSuccessMessage($"Project with ID {id} updated successfully.\n");
            }
            else
            {
                userInput.ShowError("Project ID not found.\n");
            }
        }
        else
        {
            userInput.ShowError(result.ToString());
        }
    }
}