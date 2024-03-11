using TaskTracer.Models;
using TaskTracer.ParsesCommand;
using TaskTracer.Storage;

namespace TaskTracer.UserInput;

public class ConsoleUserInput : IUserInput
{
    public void ShowMenu()
    {
        Console.WriteLine("""
            Task Tracer Commands:
            ******************************
            show-menu
            stop
            add-project Title=value, DueDate=value
            add-task Title=value, Description=value, StartDate=yyyy-mm-dd, EstimatedTime=value, DueDate=yyyy-mm-dd,Priority= <High, Medium or low>,Status=<Not Started, In Progress or Completed>
            display-projects
            display-tasks
            edit-project ID=value, parameter1=value, parameter2=value, ... etc
            edit-task ID=value, parameter1=value, parameter2=value, ... etc
            delete-project ID=value
            delete-task ID=value
            view-today-tasks
            view-tasks date=value
            ******************************
            """);
    }

    public void DisplayStorage<T>(Storage<T> storage) where T : class, ITraceable
    {
        Console.WriteLine(storage.ToString());
    }

    public string ReadCommand()
    {
        return Console.ReadLine();
    }

    public ParsedCommand ParseCommand(string input)
    {
        var parsedCommand = new ParsedCommand();
        var parts = input.Split(new char[] { ' ' }, 2);

        if (parts.Length > 0)
        {
            parsedCommand.Command = parts[0].Trim().ToLower();

            if (parts.Length > 1)
            {
                var paramsPart = parts[1].Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var param in paramsPart)
                {
                    var keyValue = param.Split(new char[] { '=' }, 2);
                    if (keyValue.Length == 2)
                    {
                        var key = keyValue[0].Trim().ToLower();
                        var value = keyValue[1].Trim();
                        parsedCommand.Parameters[key] = value; 
                    }
                }
            }
        }

        return parsedCommand;
    }
    
    public void ShowSuccessMessage(string message)
    {
        Console.WriteLine(message);
        Console.WriteLine();
    }

    public void ShowError(string message)
    {
        Console.WriteLine(message);
        Console.WriteLine();
    }
}