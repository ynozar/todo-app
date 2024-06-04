namespace ToDoBackend.DTO.ToDoItem;

public class ToDoItemResponse
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Guid GroupUid { get; set; }
    public DateTime? DueAt { get; set; }
    public int Priority { get; set; }
    public bool IsComplete { get; set; }
    
    //base entity
    public Guid Uid { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public bool Deleted { get; set; }
}