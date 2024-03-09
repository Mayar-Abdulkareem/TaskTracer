using TaskTracer.ParsesCommand;

namespace TaskTracer.UserInput;

public interface IUserInput
{
    public void ShowMenu();

    public string ReadCommand();
    ParsedCommand ParseCommand(string input); 
}