namespace ToDoBackend;


public class Group: BaseEntity
{
    public string? Name { get; set; }
    public string? Color { get; set; }
    public bool isDefault { get; set; }
    public User User { get; set; }
    public long UserId { get; set; }
    public ICollection<ToDoItem> ToDoItems { get; set; } = new List<ToDoItem>();
}
