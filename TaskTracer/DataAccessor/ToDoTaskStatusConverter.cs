using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using TaskTracer.Enums;

namespace TaskTracer.DataAccessor;

public class ToDoTaskStatusConverter : DefaultTypeConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (Enum.TryParse<ToDoTaskStatus>(text, true, out var status)) 
        {
            return status;
        }
        
        throw new ArgumentException($"Unknown task status: {text}");
    }
}