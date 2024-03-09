using System.Text;
using TaskTracer.DataAccessor;

namespace TaskTracer.Storage;

public class Storage<T>(IDataStorageAccessor dataStorageAccessor)
{
    public Dictionary<string, T> storage = new Dictionary<string, T>();

    private IDataStorageAccessor _dataStorageAccessor = dataStorageAccessor;

    public void Load(FileType fileType)
    {
        storage = _dataStorageAccessor.LoadData<T>(fileType);
    }

    public void Add(string key, T item)
    {
        storage.Add(key, item);
    }
    
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Storage Contents:");
        foreach (var item in storage)
        {
            sb.AppendLine($"({item.Key}, {item.Value})");
        }
        return sb.ToString();
    }
}