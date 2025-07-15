using ToDoBackend.DTO.ToDoItem;

namespace ToDoBackend.DTO.Group;

public class GroupResponse
{
    public string? Name { get; set; }
    public string? Color { get; set; }
    public bool isDefault { get; set; }
    
    public ICollection<ToDoItemResponse>? ToDoItems { get; set; } = null!;
    
    //base entity
    public Guid Uid { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; } = null!;
    public DateTime? ModifiedAt { get; set; }
    public string? ModifiedBy { get; set; }
}