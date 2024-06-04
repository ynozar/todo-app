namespace ToDoBackend;

public class ToDoItem: BaseEntity
{
    public string Title { get; set; }
    public Group Group { get; set; } = null!;
    public long GroupId { get; set; }
    public DateTime? DueAt { get; set; }
    public int Priority { get; set; }
    public string Description { get; set; }
    public bool IsComplete { get; set; }
}