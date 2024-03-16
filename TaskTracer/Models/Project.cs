namespace TaskTracer.Models;

public class Project 
{
    public string ID { get; set; }
    public string Title { get; set; }
    public DateTime DueDate { get; set; }

    public override string ToString()
    {
        return $"Title: {this.Title}, Due Date: {this.DueDate}";
    }
}