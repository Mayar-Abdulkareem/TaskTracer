using TaskTracer.Commands;
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
            add-project Title=value, DueDate=value
            add-task Title=value, Description=value, StartDate=mm-dd-yyyy, EstimatedTime=value, DueDate=mm-dd-yyyy,Priority= <High, Medium or low>,Status=<Not Started, In Progress or Completed>
            edit-project ID=value, parameter1=value, parameter2=value, ... etc
            edit-task ID=value, parameter1=value, parameter2=value, ... etc
            delete-project ID=value
            delete-task ID=value
            view-today-tasks
            view-tasks date=value
            view-overdue-tasks
            save-projects
            save-tasks
            import-tasks path=value
            display-projects
            display-tasks
            stop
            ******************************
            """);
    }
    
    public void DisplayStorage<T>(Storage<T> storage) where T : class => Console.WriteLine(storage.ToString());

    public string ReadCommand()
    {
        return Console.ReadLine() ;
    }

    public ParsedCommand ParseCommand(string input)
    {
        var parts = input.Split(new char[] { ' ' }, 2);
        var parsedCommand = new ParsedCommand();

        if (parts.Length > 0)
        {
            parsedCommand.Command = parts[0].Trim().ToLower();

            if (parts.Length > 1)
            {
                parsedCommand.Parameters = ParseParameters(parts[1]);
            }
        }

        return parsedCommand;
    }

    private Dictionary<string, string> ParseParameters(string paramsPart)
    {
        var parameters = new Dictionary<string, string>();
        var paramsArray = paramsPart.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var param in paramsArray)
        {
            var keyValue = param.Split(new char[] { '=' }, 2);
            if (keyValue.Length == 2)
            {
                var key = keyValue[0].Trim().ToLower();
                var value = keyValue[1].Trim();
                parameters[key] = value;
            }
        }

        return parameters;
    }
    
    public void ShowSuccessMessage(string message) => Console.WriteLine(message);
    
    public void ShowError(string message) => Console.WriteLine(message);
}