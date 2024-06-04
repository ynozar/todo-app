namespace ToDoBackend.DTO.User;

public class CreateAccountRequest
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
}