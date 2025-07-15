namespace ToDoBackend.DTO.Group;

public class UpdateGroupRequest
{
    public Guid Uid { get; set; }
    public string? Name { get; set; }
    public string? Color { get; set; }
}