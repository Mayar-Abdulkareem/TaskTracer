using TaskTracer.Storage;

namespace TaskTracer.Commands;

public class SaveProjectsCommand(StorageRepository storage) : ICommand
{
    public void Execute(Dictionary<string, string> parameters)
    {
        storage.SaveProjects();
    }
}