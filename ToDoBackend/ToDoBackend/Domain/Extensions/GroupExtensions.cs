using ToDoBackend.DTO.Group;

namespace ToDoBackend.Domain.Extensions;

internal static class GroupExtensions
{
    internal static GroupResponse ToGroupResponse(this Group group, bool includeToDos=false)
    {
        return new GroupResponse()
        {
            Name = group.Name,
            Color = group.Color,
            isDefault = group.isDefault,
            ToDoItems = (includeToDos)?group.ToDoItems.Select(x=>x.ToToDoItemResponse()).ToList():null,
            //Base
            Uid = group.Uid,
            CreatedBy = group.CreatedBy,
            CreatedAt = group.CreatedAt,
            ModifiedAt = group.ModifiedAt,
            ModifiedBy = group.ModifiedBy
        };
    }
    
    internal static Group ToGroup(this CreateGroupRequest request,  User user, bool isDefault=false)
    {
        return new Group()
        {
            
            Name = request.Name,
            Color = request.Color,
            isDefault = isDefault,
            //User
            User = user,
            UserId = user.Id,
            //Base
            CreatedBy = user.Username,
            CreatedAt = DateTime.UtcNow,
        };
    }
    internal static Group ToGroup(this UpdateGroupRequest request, Group group, string username)
    {
        group.Color= request.Color;
        group.Name= request.Name;
        SetModifiedInformation(group, username);
        return group;
    }
    private static Group SetModifiedInformation(this Group group, string username)
    {
        group.ModifiedBy = username;
        group.ModifiedAt = DateTime.UtcNow;
        return group;
    }
}