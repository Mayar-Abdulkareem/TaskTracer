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
}