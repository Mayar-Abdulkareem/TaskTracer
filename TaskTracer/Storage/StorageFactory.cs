using TaskTracer.DataAccessor;
using TaskTracer.Models;
using Task = System.Threading.Tasks.Task;

namespace TaskTracer.Storage;

public class StorageFactory(IDataStorageAccessor dataStorageAccessor)
{
    private Storage<Project> projects = new Storage<Project>(dataStorageAccessor);
    private Storage<ToDoTask> tasks = new Storage<ToDoTask>(dataStorageAccessor);

    public void Load()
    {
        projects.Load(FileType.Projects);
        tasks.Load(FileType.ToDoTasks);
    }
}