using TaskTracer.DataAccessor;
using TaskTracer.Enums;
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

    public bool DeleteProject(string key)
    {
        return projects.Delete(key);
    }

    public bool DeleteTask(string key)
    {
        return tasks.Delete(key);
    }
    
    public void DisplayProjects()
    {
        _userInput.DisplayStorage(projects);
    }
    
    public void DisplayTasks()
    {
        _userInput.DisplayStorage(tasks);
    }
    
    public void ViewTasksDueOnDate(DateTime date)
    {
        var dueTasks = tasks.GetItems(
            task => task.DueDate.Date == date.Date && task.Status != ToDoTaskStatus.Completed,
            task => task.Priority
        );

        Console.WriteLine($"Tasks due on {date.ToShortDateString()}:");
        foreach (var task in dueTasks)
        {
            Console.WriteLine(task.ToString()); 
        }
    }
}