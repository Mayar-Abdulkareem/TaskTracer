using TaskTracer.ParsesCommand;

namespace TaskTracer.UserInput;

public interface IUserInput
{
    public void ShowMenu();

    public string ReadCommand();
    ParsedCommand ParseCommand(string input);
    public void ShowSuccessMessage(string message);
    public void ShowError(string message);
}