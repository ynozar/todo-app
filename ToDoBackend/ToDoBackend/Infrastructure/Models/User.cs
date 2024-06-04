namespace ToDoBackend;

public class User: BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string HashedPassword { get; set; } = null!;
    public string Settings { get; set; } = "{}";
    public ICollection<Group> Groups { get; set; } = new List<Group>();
}