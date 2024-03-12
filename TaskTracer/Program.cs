// See https://aka.ms/new-console-template for more information
using TaskTracer.Application;
using TaskTracer.DataAccessor;
using TaskTracer.UserInput;
using TaskTracer.Validation;

var app = new Application(new FileStorageAccessor(), new ConsoleUserInput(), new Validator());
app.Run();