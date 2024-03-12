using TaskTracer.DataAccessor;
using TaskTracer.Enums;
using TaskTracer.Models;
using TaskTracer.UserInput;
using TaskTracer.Validation;

namespace TaskTracer.Storage;

public class StorageRepository
{
    private Storage<Project> projects;
    private Storage<ToDoTask> tasks;
    private IUserInput _userInput;
    private IDataStorageAccessor _dataStorageAccessor;
    private IValidator _validator;
    
    public StorageRepository(IDataStorageAccessor dataStorageAccessor, IUserInput userInput, IValidator validator)
    {
        projects = new Storage<Project>(dataStorageAccessor, userInput);
        tasks = new Storage<ToDoTask>(dataStorageAccessor, userInput);
        _userInput = userInput;
        _dataStorageAccessor = dataStorageAccessor;
        _validator = validator;
    }
    
    public bool Load()
    {
        return 
            projects.Load("/Users/ftsmobileteam/Desktop/Backend/RiderProject/TaskTracer/TaskTracer/Files/Projects.csv")
            && tasks.Load("/Users/ftsmobileteam/Desktop/Backend/RiderProject/TaskTracer/TaskTracer/Files/ToDoTasks.csv");
    }

    public void ImportTasks(string filePath)
    {
        var importedTasks = new Storage<ToDoTask>(_dataStorageAccessor, _userInput);
        if (importedTasks.Load(filePath))
        {
            var result = _validator.ValidateTasks(projects, importedTasks, tasks);
            if (result.IsValid)
            {
                importedTasks.Save(FileType.ToDoTasks, true);
                tasks.AppendStorage(importedTasks);
                _userInput.ShowSuccessMessage("File imported successfully.\n");
            }
            else
            {
                _userInput.ShowError(result.ToString());
            }
        }
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

        _userInput.ShowSuccessMessage($"Tasks due on {date.ToShortDateString()}:");
        foreach (var task in dueTasks)
        {
            _userInput.ShowSuccessMessage(task.ToString()); 
        }
    }
    
    public void DisplayOverdueTasks()
    {
        var overdueTasks = tasks.GetOverdueTasks(task => 
            task.DueDate < DateTime.Today && task.Status != ToDoTaskStatus.Completed);

        foreach (var task in overdueTasks)
        {
            _userInput.ShowSuccessMessage(task.ToString()); 
        }
    }

    public void SaveProjects()
    {
        projects.Save(FileType.Projects);
        _userInput.ShowSuccessMessage("Projects saved successfully.\n");
    }
    
    public void SaveTasks()
    {
        tasks.Save(FileType.ToDoTasks);
        _userInput.ShowSuccessMessage("Tasks saved successfully.\n");
    }
}