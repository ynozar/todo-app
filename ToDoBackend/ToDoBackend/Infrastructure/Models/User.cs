using System.ComponentModel.DataAnnotations;

namespace ToDoBackend;

public class User: BaseEntity
{
    [StringLength(20, MinimumLength = 4, ErrorMessage = "Name must be between 4 and 20 characters.")]
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string HashedPassword { get; set; } = null!;
    public string Settings { get; set; } = "{}";
    public ICollection<Group> Groups { get; set; } = new List<Group>();
}