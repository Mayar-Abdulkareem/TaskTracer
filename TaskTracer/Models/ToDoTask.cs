namespace TaskTracer.Models;

public class ToDoTask
{
    public string ID { get; init; }
    public string Title { get; set; }
    public string Description { get; set; }
     public DateTime StartDate { get; set; }
     public string EstimatedTime { get; set; }
     public DateTime DueTime { get; set; }
     public string Priority { get; set; }
     public string Status { get; set; }
     public string PID { get; set; } 
}
