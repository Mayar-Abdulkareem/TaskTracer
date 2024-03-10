using TaskTracer.Enums;

namespace TaskTracer.Models;

public class ToDoTask : ITraceable
{
    public string ID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
     public DateTime StartDate { get; set; }
     public string EstimatedTime { get; set; }
     public DateTime DueDate { get; set; }
     public PriorityLevel Priority { get; set; }
     public ToDoTaskStatus Status { get; set; }
     public string PID { get; set; } 
     
     public override string ToString()
     {
         return $"Title: {this.Title}, Description: {this.Description}, Start Date: {this.StartDate}" +
                $", EstimatedTime: {this.EstimatedTime}, Due Date: {this.DueDate}, Priority: {this.Priority}" +
                $", Status: {this.Status}";
     }
}
