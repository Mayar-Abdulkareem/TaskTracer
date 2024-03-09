namespace TaskTracer.ParsesCommand;

public class ParsedCommand
{
    public string Command { get; set; }
    public Dictionary<string, string> Parameters { get; set; }

    public ParsedCommand()
    {
        Parameters = new Dictionary<string, string>();
    }
}
