using TaskTracer.DataAccessor;
using TaskTracer.Models;

namespace TaskTracer.Storage;

public class StorageFactory
{
    private Storage<Project> projects;
    private Storage<ToDoTask> tasks;
    
    public StorageFactory(IDataStorageAccessor dataStorageAccessor)
    {
        projects = new Storage<Project>(dataStorageAccessor);
        tasks = new Storage<ToDoTask>(dataStorageAccessor);
    }
    
    public void Load()
    {
        projects.Load(FileType.Projects);
        tasks.Load(FileType.ToDoTasks);
    }

    public void AddProject(Project project)
    {
        string key = Guid.NewGuid().ToString();
        project.ID = key;
        projects.Add(key, project);
    }

    public void AddTask(ToDoTask task)
    {
        string key = Guid.NewGuid().ToString();
        task.ID = key;
        tasks.Add(key, task);
    }

    public override string ToString()
    {
        return "Projects:\n" + projects.ToString() + "Tasks:\n" + tasks.ToString();
    }
}