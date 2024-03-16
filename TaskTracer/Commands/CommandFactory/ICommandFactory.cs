namespace TaskTracer.Commands.CommandFactory;

public interface ICommandFactory
{
    ICommand CreateCommand(string commandName, Dictionary<string, string> parameters);
}