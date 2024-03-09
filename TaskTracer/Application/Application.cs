using TaskTracer.DataAccessor;
using TaskTracer.Storage;
using TaskTracer.UserInput;

namespace TaskTracer.Application;

public class Application(IDataStorageAccessor dataStorageAccessor, IUserInput userInput)
{
    private readonly IDataStorageAccessor _dataStorageAccessor = dataStorageAccessor;
    private readonly IUserInput _userInput = userInput;

    public void Run()
    {
        Console.WriteLine("here");
        StorageFactory storage = new StorageFactory(_dataStorageAccessor);
        storage.Load();
    }
}