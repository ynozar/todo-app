using ToDoBackend.DTO.ToDoItem;

namespace ToDoBackend.Domain.Extensions;

internal static class ToDoExtensions
{
    internal static ToDoItemResponse ToToDoItemResponse(this ToDoItem toDoItem)
    {
        return new ToDoItemResponse()
        {
            Title = toDoItem.Title,
            GroupUid = toDoItem.Group.Uid,
            DueAt = toDoItem.DueAt,
            Description = toDoItem.Description,
            Priority = toDoItem.Priority,
            IsComplete = toDoItem.IsComplete,
            
            //Base
            Uid = toDoItem.Uid,
            CreatedBy = toDoItem.CreatedBy,
            CreatedAt = toDoItem.CreatedAt,
            ModifiedAt = toDoItem.ModifiedAt,
            ModifiedBy = toDoItem.ModifiedBy,
        };
    }
    
    internal static ToDoItem ToToDoItem(this CreateToDoItemRequest request, Group group, string username)
    {
        return new ToDoItem()
        {
            Title = request.Title,
            Group = group,
            GroupId = group.Id,
            DueAt = request.DueAt,
            Description = request.Description,
            Priority = request.Priority,
            IsComplete = request.IsComplete,
            CreatedBy = username,
            CreatedAt = DateTime.UtcNow,
        };
    }
    
    internal static ToDoItem ToToDoItem(this UpdateToDoItemRequest request, ToDoItem existing, Group group, string username)
    {
        existing.Title = request.Title;
        existing.Group = group;
        existing.GroupId = group.Id;
        existing.DueAt = request.DueAt;
        existing.Description = request.Description;
        existing.Priority = request.Priority;
        existing.IsComplete = request.IsComplete;
        existing.CreatedBy = username;
        existing.CreatedAt = DateTime.UtcNow;
        existing.ModifiedAt = DateTime.UtcNow;
                
        return existing;
    }
    internal static ToDoItem ToToDoItem(this PatchToDoItemRequest request, ToDoItem existing, Group group, string username)
    {
        existing.Title = request.Title ?? existing.Title;
        existing.Group = group;
        existing.GroupId = group.Id;
        existing.DueAt = request.DueAt ?? existing.DueAt;
        existing.Description = request.Description ?? existing.Description;
        existing.Priority = request.Priority ?? existing.Priority;
        existing.IsComplete = request.IsComplete ?? existing.IsComplete;
        SetModifiedInformation(existing, username); //test
                
        return existing;
    }
    
    private static ToDoItem SetModifiedInformation(this ToDoItem toDoItem, string username)
    {
        toDoItem.ModifiedBy = username;
        toDoItem.ModifiedAt = DateTime.UtcNow;
        return toDoItem;
    }
}