namespace TaskTracer.Commands;

public class ParsedCommand()
{
    public string Command { get; set; }
    public Dictionary<string, string> Parameters { get; set; } = new();
}
