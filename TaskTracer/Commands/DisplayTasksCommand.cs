using TaskTracer.Storage;

namespace TaskTracer.Commands;

public class DisplayTasksCommand(StorageRepository storage) : ICommand
{
    public void Execute(Dictionary<string, string> parameters)
    {
        storage.DisplayProjects();
    }
}