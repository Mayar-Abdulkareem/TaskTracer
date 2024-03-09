// See https://aka.ms/new-console-template for more information
using TaskTracer.Application;
using TaskTracer.DataAccessor;
using TaskTracer.UserInput;

var app = new Application(new FileStorageAccessor("/Users/ftsmobileteam/Desktop/Backend/RiderProject/TaskTracer/TaskTracer/Files"), new ConsoleUserInput());
app.Run();