using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace TaskTracer.DataAccessor;

public class FileStorageAccessor(string fileDirectoryPath) : IDataStorageAccessor
{
    private readonly string _fileDirectoryPath = fileDirectoryPath;

    public Dictionary<string, T> LoadData<T>(FileType fileType)
    {
        var filePath = Path.Combine(_fileDirectoryPath, fileType.ToString() + ".csv");

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
        var records = csv.GetRecords<T>().ToList();

        var dictionary = records.ToDictionary(record =>
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
}