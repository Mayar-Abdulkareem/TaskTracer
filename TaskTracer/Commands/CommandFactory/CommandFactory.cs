namespace TaskTracer.Commands.CommandFactory;

public class CommandFactory(IServiceProvider serviceProvider) : ICommandFactory
{
    public ICommand CreateCommand(string commandName, Dictionary<string, string> parameters)
    {
        var className = ConvertToClassName(commandName);
        var commandType = Type.GetType($"TaskTracer.Commands.{className}", throwOnError: false);

        if (commandType == null)
        {
            throw new ArgumentException($"Command '{commandName}' not recognized.");
        }

        if (!typeof(ICommand).IsAssignableFrom(commandType))
        {
            throw new InvalidOperationException($"{className} does not implement ICommand interface.");
        }

        var command = (ICommand)serviceProvider.GetService(commandType);

        if (command == null)
        {
            throw new InvalidOperationException($"Unable to resolve dependencies for the command '{className}'.");
        }

        return command;
    }
    
    private string ConvertToClassName(string commandName)
    {
        var parts = commandName.Split('-');

        var className = string.Concat(parts.Select(part => char.ToUpperInvariant(part[0]) + part.Substring(1)));

        className += "Command";

        return className; 
    }
}