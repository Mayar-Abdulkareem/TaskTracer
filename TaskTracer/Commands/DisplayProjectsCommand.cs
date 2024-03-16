using TaskTracer.Storage;

namespace TaskTracer.Commands;

public class DisplayProjectsCommand(StorageRepository storage) : ICommand
{
    public void Execute(Dictionary<string, string> parameters)
    {
        storage.DisplayProjects();
    }
}