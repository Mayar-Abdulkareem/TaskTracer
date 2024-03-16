using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using TaskTracer.Models;

namespace TaskTracer.DataAccessor;

public class ToDoTaskMap : ClassMap<ToDoTask>
{
    public ToDoTaskMap() 
    {
        AutoMap(CultureInfo.InvariantCulture);

        Map(m => m.Status).TypeConverter<ToDoTaskStatusConverter>();
        Map(m => m.Priority).TypeConverter<ToDoTaskPriorityConverter>();
    }
}