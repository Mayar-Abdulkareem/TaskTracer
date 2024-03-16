using TaskTracer.UserInput;

namespace TaskTracer.Commands;

public class ShowMenuCommand(IUserInput userInput) : ICommand
{
    public void Execute(Dictionary<string, string> parameters)
    {
        userInput.ShowMenu();
    }
}