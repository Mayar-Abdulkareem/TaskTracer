using TaskTracer.Storage;

namespace TaskTracer.Commands;

public class ViewOverdueTasksCommand(StorageRepository storage) : ICommand
{
    public void Execute(Dictionary<string, string> parameters)
    {
        storage.DisplayOverdueTasks();
    }
}