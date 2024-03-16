namespace TaskTracer.Commands;

public interface ICommand
{
    public void Execute(Dictionary<string, string> parameters);
}