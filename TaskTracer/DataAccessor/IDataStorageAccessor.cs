namespace TaskTracer.DataAccessor;

public interface IDataStorageAccessor
{
    public Dictionary<string, T> LoadData<T>(FileType fileType);
}