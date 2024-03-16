using TaskTracer.Commands;
using TaskTracer.Commands.CommandFactory;
using TaskTracer.Storage;
using TaskTracer.UserInput;

namespace TaskTracer.Application;

public class Application(IUserInput userInput, ICommandFactory commandFactory, StorageRepository storage)
{
    public void Run()
    {
        if (!storage.Load()) return;
        
        userInput.ShowMenu();
        ExecuteCommandsLoop();
    }

    private void ExecuteCommandsLoop()
    {
        bool isRunning = true;
        while (isRunning)
        {
            try
            {
                isRunning = ProcessCommand();
            }
            catch (Exception ex)
            {
                userInput.ShowError($"Error executing command: {ex.Message}\n");
            }
        }
    }

    private bool ProcessCommand()
    {
        var line = userInput.ReadCommand();
        var parsedCommand = userInput.ParseCommand(line);
        
        if (parsedCommand.Command.Equals("stop", StringComparison.OrdinalIgnoreCase))
        {
            return false;
        }
        
        ExecuteParsedCommand(parsedCommand);
        return true;
    }

    private void ExecuteParsedCommand(ParsedCommand parsedCommand)
    {
        var command = commandFactory.CreateCommand(parsedCommand.Command, parsedCommand.Parameters);
        command.Execute(parsedCommand.Parameters);
    }
}