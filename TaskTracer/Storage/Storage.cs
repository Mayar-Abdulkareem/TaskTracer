using System.Text;
using TaskTracer.DataAccessor;
using TaskTracer.Models;

namespace TaskTracer.Storage;

public class Storage<T>(IDataStorageAccessor dataStorageAccessor) where T : class, ITraceable
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

    public bool Edit(string key, T item)
    {
        if (storage.ContainsKey(key))
        {
            storage[key] = item;
            return true;
        }

        return false;
    }

    public bool Delete(string key)
    {
        if (storage.ContainsKey(key))
        {
            storage.Remove(key);
            return true;
        }
        return false;
    }

    public T? FindById(string id)
    {
        if (storage.ContainsKey(id))
        {
            return storage[id];
        }

        return null;
    }
    
    public IEnumerable<T> GetItems(Func<T, bool> filter, Func<T, object> sorter = null)
    {
        var query = storage.Values.AsEnumerable();
        if (filter != null)
        {
            query = query.Where(filter);
        }
        if (sorter != null)
        {
            query = query.OrderBy(sorter);
        }

        return query.ToList();
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