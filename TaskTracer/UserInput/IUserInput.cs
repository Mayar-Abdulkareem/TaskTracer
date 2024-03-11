using TaskTracer.Models;
using TaskTracer.ParsesCommand;
using TaskTracer.Storage;

namespace TaskTracer.UserInput;

public interface IUserInput
{
    public void ShowMenu();
    public void DisplayStorage<T>(Storage<T> storage) where T : class;
    public string ReadCommand();
    ParsedCommand ParseCommand(string input);
    public void ShowSuccessMessage(string message);
    public void ShowError(string message);
}