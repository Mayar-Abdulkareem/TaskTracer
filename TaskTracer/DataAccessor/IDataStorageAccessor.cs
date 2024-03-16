namespace TaskTracer.DataAccessor;

public interface IDataStorageAccessor
{
    public Dictionary<string, T> LoadData<T>(string filePath);

    public void WriteData<T>(FileType fileType, Dictionary<string, T> data, bool append = false);
}