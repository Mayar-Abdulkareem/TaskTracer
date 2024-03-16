using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using TaskTracer.Enums;

namespace TaskTracer.DataAccessor;

public class ToDoTaskPriorityConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (Enum.TryParse<PriorityLevel>(text, true, out var priority)) 
        {
            return priority;
        }
        
        throw new ArgumentException($"Unknown task priority: {text}");
    }
}