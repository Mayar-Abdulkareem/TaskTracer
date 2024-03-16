using System.Text;
using TaskTracer.DataAccessor;
using TaskTracer.UserInput;

namespace TaskTracer.Storage;

public class Storage<T>(IDataStorageAccessor dataStorageAccessor, IUserInput userInput) where T : class
{
    private Dictionary<string, T> _storage = new Dictionary<string, T>();
    
    public bool Load(string filePath)
    {
        var success = true;
        try
        {
            _storage = dataStorageAccessor.LoadData<T>(filePath);
        }
        catch (ArgumentException ex)
        {
            success = false;
            userInput.ShowError($"Error loading data: {ex.Message}");
        }
        catch (Exception ex)
        {
            success = false;
            userInput.ShowError($"An unexpected error occurred: {ex.Message}");
        }

        return success;
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

    private IEnumerable<KeyValuePair<string, T>> GetStorage()
    {
        return _storage.AsEnumerable();
    }
    
    public IEnumerable<T> GetItems(Func<T, bool>? filter = null, Func<T, object>? sorter = null)
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

    public void Save(FileType fileType, bool append = false)
    {
        dataStorageAccessor.WriteData(fileType, _storage, append);
    }
    
    public bool ContainsID(string id)
    {
        return _storage.ContainsKey(id);
    }

    public void AppendStorage(Storage<T> newStorage)
    {
        foreach (var item in newStorage.GetStorage())
        {
            _storage[item.Key] = item.Value;
        }
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