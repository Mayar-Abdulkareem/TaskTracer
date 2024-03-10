using TaskTracer.DataAccessor;
using TaskTracer.Models;
using TaskTracer.UserInput;

namespace TaskTracer.Storage;

public class StorageRepository
{
    private Storage<Project> projects;
    private Storage<ToDoTask> tasks;
    private IUserInput _userInput;
    
    public StorageRepository(IDataStorageAccessor dataStorageAccessor, IUserInput userInput)
    {
        projects = new Storage<Project>(dataStorageAccessor);
        tasks = new Storage<ToDoTask>(dataStorageAccessor);
        _userInput = userInput;
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
    
    public Project? FindProjectById(string id)
    {
        return projects.FindById(id);
    }
    
    public ToDoTask? FindTaskById(string id)
    {
        return tasks.FindById(id);
    }

    public void EditProject(string key, Project project)
    {
        projects.Edit(key, project);
    }
    
    public void EditTask(string key, ToDoTask task)
    {
        tasks.Edit(key, task);
    }
    
    public void DisplayProjects()
    {
        _userInput.DisplayStorage(projects);
    }
    
    public void DisplayTasks()
    {
        _userInput.DisplayStorage(tasks);
    }
}