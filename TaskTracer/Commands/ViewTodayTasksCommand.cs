using TaskTracer.Storage;

namespace TaskTracer.Commands;

public class ViewTodayTasksCommand(StorageRepository storage) : ICommand
{
    public void Execute(Dictionary<string, string> parameters)
    {
        var now = DateTime.Now;
        storage.ViewTasksDueOnDate(now);
    }
}