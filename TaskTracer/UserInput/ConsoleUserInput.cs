using TaskTracer.ParsesCommand;

namespace TaskTracer.UserInput;

public class ConsoleUserInput : IUserInput
{
    public void ShowMenu()
    {
        Console.WriteLine("""
            Task Tracer Commands:
            show-menu
            add-project Title=value, DueDate=value
            add-task Title=value, Description=value, StartDate=yyyy-mm-dd, EstimatedTime=value, DueDate=yyyy-mm-dd,Priority= <High, Medium or low>,Status=<Not Started, In Progress or Completed>
            ******************************
            """);
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
                        var key = keyValue[0].Trim();
                        var value = keyValue[1].Trim();
                        parsedCommand.Parameters[key] = value; 
                    }
                }
            }
        }

        return parsedCommand;
    }
}