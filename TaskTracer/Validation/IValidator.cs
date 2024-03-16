using TaskTracer.Models;
using TaskTracer.Storage;

namespace TaskTracer.Validation;

public interface IValidator
{
    public ValidationResult ValidateParameters<T>(Dictionary<string, string> parameters,
        bool containsAllProperty = true, bool validateId = false);

    public ValidationResult ValidateTasks(Storage<Project> projectsStorage, Storage<ToDoTask> tasksStorage,
        Storage<ToDoTask> previousTasks);
    
    public ValidationResult ValidatePathParameter(Dictionary<string, string> parameters);
}