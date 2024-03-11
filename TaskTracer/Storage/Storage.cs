using System.Text;
using TaskTracer.DataAccessor;

namespace TaskTracer.Storage;

public class Storage<T>(IDataStorageAccessor dataStorageAccessor) where T : class
{
    private Dictionary<string, T> _storage = new Dictionary<string, T>();

    private IDataStorageAccessor _dataStorageAccessor = dataStorageAccessor;

    public void Load(FileType fileType)
    {
        _storage = _dataStorageAccessor.LoadData<T>(fileType);
    }

    public void Add(string key, T item)
    {
        _storage.Add(key, item);
    }

    public bool Edit(string key, T item)
    {
        if (_storage.ContainsKey(key))
        {
            _storage[key] = item;
            return true;
        }

        return false;
    }

    public bool Delete(string key)
    {
        if (_storage.ContainsKey(key))
        {
            _storage.Remove(key);
            return true;
        }
        return false;
    }

    public T? FindById(string id)
    {
        if (_storage.ContainsKey(id))
        {
            return _storage[id];
        }

        return null;
    }
    
    public IEnumerable<T> GetItems(Func<T, bool> filter, Func<T, object> sorter = null)
    {
        var query = _storage.Values.AsEnumerable();
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
    
    public IEnumerable<T> GetOverdueTasks(Func<T, bool> isOverdueFunc)
    {
        return _storage.Values.Where(isOverdueFunc);
    }
    
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Storage Contents:");
        foreach (var item in _storage)
        {
            sb.AppendLine($"({item.Key}, {item.Value})");
        }
        return sb.ToString();
    }
}