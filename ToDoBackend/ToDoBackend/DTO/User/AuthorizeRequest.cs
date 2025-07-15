namespace ToDoBackend.DTO.User;

public class AuthorizeRequest
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string ClientId { get; set; }
    public required string RedirectUri { get; set; }
}