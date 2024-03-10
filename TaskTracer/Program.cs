// See https://aka.ms/new-console-template for more information
using TaskTracer.Application;
using TaskTracer.DataAccessor;
using TaskTracer.UserInput;

var app = new Application(new FileStorageAccessor(), new ConsoleUserInput());
app.Run();