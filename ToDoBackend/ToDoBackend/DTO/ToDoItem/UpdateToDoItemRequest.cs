namespace ToDoBackend.DTO.ToDoItem;

public class UpdateToDoItemRequest
{
    public Guid Uid { get; set; }
    public string Title { get; set; }
    public Guid GroupUid { get; set; }
    public DateTime? DueAt { get; set; }
    public int Priority { get; set; }
    public string Description { get; set; }
    public bool IsComplete { get; set; }
}