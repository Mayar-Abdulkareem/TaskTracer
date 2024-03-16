using TaskTracer.Storage;
using TaskTracer.UserInput;

namespace TaskTracer.Commands;

public class ViewTasksCommand(IUserInput userInput, StorageRepository storage) : ICommand
{
    public void Execute(Dictionary<string, string> parameters)
    {
        var format = "MM-dd-yyyy"; 
        var culture = System.Globalization.CultureInfo.InvariantCulture;
        
        var dateArg = parameters.FirstOrDefault();
        if (DateTime.TryParseExact(dateArg.Value, format, culture, System.Globalization.DateTimeStyles.None,
                out DateTime specificDate))
        {
            storage.ViewTasksDueOnDate(specificDate);
        }
        else
        {
            userInput.ShowError("Invalid date format. Use 'today' or 'date=mm-dd-yyyy'.\n");
        }
    }
}