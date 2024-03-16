// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.DependencyInjection;
using TaskTracer.Application;
using TaskTracer.Commands;
using TaskTracer.Commands.CommandFactory;
using TaskTracer.DataAccessor;
using TaskTracer.Storage;
using TaskTracer.UserInput;
using TaskTracer.Validation;

void ConfigureServices(IServiceCollection services)
{
    services.AddTransient<AddProjectCommand>();
    services.AddTransient<AddTaskCommand>();
    services.AddTransient<EditProjectCommand>();
    services.AddTransient<EditTaskCommand>();
    services.AddTransient<DeleteProjectCommand>();
    services.AddTransient<DeleteTaskCommand>();
    services.AddTransient<DisplayProjectsCommand>();
    services.AddTransient<DisplayTasksCommand>();
    services.AddTransient<ImportTasksCommand>();
    services.AddTransient<SaveProjectsCommand>();
    services.AddTransient<SaveTasksCommand>();
    services.AddTransient<ShowMenuCommand>();
    services.AddTransient<ViewTasksCommand>();
    services.AddTransient<ViewTodayTasksCommand>();
    services.AddTransient<ViewOverdueTasksCommand>();
    
    services.AddSingleton<IUserInput, ConsoleUserInput>();
    services.AddSingleton<IDataStorageAccessor, FileStorageAccessor>();
    services.AddSingleton<IValidator, Validator>();
    services.AddSingleton<ICommandFactory, CommandFactory>();
    services.AddSingleton<StorageRepository>();
    services.AddSingleton<ModelFactory>();
    services.AddSingleton<Application>();
}

var serviceCollection = new ServiceCollection();
ConfigureServices(serviceCollection);
var serviceProvider = serviceCollection.BuildServiceProvider();

var app = serviceProvider.GetRequiredService<Application>();
app.Run();