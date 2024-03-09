namespace TaskTracer.UserInput;

public class ConsoleUserInput : IUserInput
{
    public void ShowMenu()
    {
        Console.WriteLine("""
            Task Tracer Commands:
            ******************************
            add-project project name:value, due date:value
            """);
    }
}