using TaskTracer.Storage;

namespace TaskTracer.Commands;

public class SaveTasksCommand(StorageRepository storage) : ICommand
{
    public void Execute(Dictionary<string, string> parameters)
    {
        storage.SaveTasks();
    }
}