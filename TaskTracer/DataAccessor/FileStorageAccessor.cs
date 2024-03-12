using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace TaskTracer.DataAccessor;

public class FileStorageAccessor : IDataStorageAccessor
{
    private readonly string _fileDirectoryPath = "/Users/ftsmobileteam/Desktop/Backend/RiderProject/TaskTracer/TaskTracer/Files";

    public Dictionary<string, T> LoadData<T>(string filePath)
    {
        //var filePath = Path.Combine(_fileDirectoryPath, fileType.ToString() + ".csv");

        var config = new CsvConfiguration(CultureInfo.InvariantCulture);
        Dictionary<string, T> dictionary;
        
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);
        csv.Context.RegisterClassMap<ToDoTaskMap>();
        var records = csv.GetRecords<T>().ToList();


        dictionary = records.ToDictionary(record =>
        {
            var idProperty = typeof(T).GetProperty("ID");
            if (idProperty == null)
            {
                throw new InvalidOperationException("The ID property was not found on the type.");
            }

            var idValue = idProperty.GetValue(record) as string;
            if (string.IsNullOrEmpty(idValue))
            {
                throw new InvalidOperationException("The ID property cannot be null or empty.");
            }

            return idValue;
        });
        return dictionary;
    }

    public void WriteData<T>(FileType fileType, Dictionary<string, T> data, bool append = false)
    {
        var filePath = Path.Combine(_fileDirectoryPath, fileType.ToString() + ".csv");
    
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
        };

        using var writer = new StreamWriter(filePath, append: append);
        using var csv = new CsvWriter(writer, config);
        
        if (!append || new FileInfo(filePath).Length == 0)
        {
            csv.WriteHeader<T>();
            csv.NextRecord();
        }
    
        foreach (var item in data.Values)
        {
            csv.WriteRecord(item);
            csv.NextRecord();
        }
    }
}