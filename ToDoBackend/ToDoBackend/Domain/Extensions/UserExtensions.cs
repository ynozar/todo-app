using ToDoBackend.DTO.User;

namespace ToDoBackend.Domain.Extensions;

internal static class UserExtensions
{
    
    internal static User ToUser(this CreateAccountRequest request, string hashedPassword)
    {
        return new User()
        {
            Username = request.Username,
            Name = request.Name,
            HashedPassword = hashedPassword,
            Email = request.Email,
            CreatedBy = "Admin",
            CreatedAt = DateTime.UtcNow,
        };
    }
    
    private static ToDoItem SetModifiedInformation(this ToDoItem toDoItem)
    {
        toDoItem.ModifiedBy = "Admin";
        toDoItem.ModifiedAt = DateTime.UtcNow;
        return toDoItem;
    }
}