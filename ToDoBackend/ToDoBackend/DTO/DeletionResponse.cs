namespace ToDoBackend.DTO;

public class DeletionResponse
{
    public Guid Uid { get; set; }
    public DateTime? DeletedAt { get; set; }
}
