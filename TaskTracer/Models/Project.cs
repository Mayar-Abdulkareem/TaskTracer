namespace TaskTracer.Models;

public class Project
{
    public string ID { get; init; }
    public string Title { get; set; }
    public DateTime DueDate { get; set; }
}