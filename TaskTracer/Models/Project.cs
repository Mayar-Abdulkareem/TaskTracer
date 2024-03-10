namespace TaskTracer.Models;

public class Project : ITraceable
{
    public string ID { get; set; }
    public string Title { get; set; }
    public DateTime DueDate { get; set; }

    public override string ToString()
    {
        return $"Title: {this.Title}, Due Date: {this.DueDate}";
    }
}