using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using ToDoBackend.DataContext;
using ToDoBackend.Domain.Extensions;
using ToDoBackend.Domain.Helpers;
using ToDoBackend.Domain.Services.Interfaces;
using ToDoBackend.DTO;
using ToDoBackend.DTO.ToDoItem;

namespace ToDoBackend;

public class ToDoService: IToDoService
{
   private readonly ApplicationDataContext _context;
   private readonly IAuthHelper _authHelper;
    public ToDoService(ApplicationDataContext context, IAuthHelper authHelper)
    {
        _context = context;
        _authHelper = authHelper;
    }
    
    public async Task<Results<Ok<IEnumerable<ToDoItemResponse>>, NotFound<string>, BadRequest<string>>> GetAllToDos( Guid? groupUid,  bool? isCompleted, int? priority,DateTime? dueBefore)
    {
        var userClaims= _authHelper.GetUserFromHeader();
        var group= _context.Groups.FirstOrDefault(i => i.Uid == groupUid);
        var x = _context.ToDoItems.Where(todo =>
                (todo.CreatedBy.Equals(userClaims["username"])) &&
                (groupUid == null || group != null && (todo.GroupId == group.Id)) &&
                (isCompleted == null || todo.IsComplete == isCompleted) && 
                (priority == null || todo.Priority == priority) &&
                (dueBefore == null || todo.DueAt < dueBefore))
            .OrderBy(s => s.IsComplete)
            .ThenBy(s => s.CreatedAt)
            .ToList();
       return TypedResults.Ok(x.Select(y=>y.ToToDoItemResponse()));
    }

    public async Task<Results<Ok<ToDoItemResponse>, NotFound<string>, BadRequest<string>>> GetToDoById(Guid uid)
    {
        var userClaims= _authHelper.GetUserFromHeader();
        var toDoItem= _context.ToDoItems.FirstOrDefault(i => i.Uid == uid && i.CreatedBy.Equals(userClaims["username"]));
        if (toDoItem == null)
        {
            return TypedResults.NotFound($"ToDo with id {uid} not found");
        }
        return TypedResults.Ok(toDoItem.ToToDoItemResponse());
    }

    public async Task<Results<Ok<ToDoItemResponse>, NotFound<string>, BadRequest<string>>> CreateToDo(CreateToDoItemRequest request)
    {
        var userClaims= _authHelper.GetUserFromHeader();
        var user = _context.Users.FirstOrDefault(i => i.Username.Equals(userClaims["username"]));
        if (user is null)
        {
            return TypedResults.BadRequest($""); //Not possible unless token was issues and then user was deleted
        }
        var group = _context.Groups.FirstOrDefault(i => i.Uid == request.GroupUid);
        if (group is null && request.GroupUid is not null)
        {
            return TypedResults.BadRequest($"Group with id {request.GroupUid} not found");
        }

        if (group is null) //recently changed, double check
        {
            group = _context.Groups.FirstOrDefault(i => i.isDefault);
            if (group is null)
            {
                //create default group
                group = new Group()
                {
                    Name = "Ungrouped Items",
                    isDefault = true,
                    
                    User = user,
                    UserId = user.Id,
                    
                    CreatedBy = user.Username,
                    CreatedAt = DateTime.UtcNow,
                };
                _context.Groups.Add(group);
            }
        }
        var toDoItem = request.ToToDoItem(group,userClaims["username"]);
        _context.ToDoItems.Add(toDoItem);
        _ = await _context.SaveChangesAsync();
        return TypedResults.Ok(toDoItem.ToToDoItemResponse());
    }

    public async Task<Results<Ok<ToDoItemResponse>, NotFound<string>, BadRequest<string>>> UpdateToDo(UpdateToDoItemRequest request)
    {
        var userClaims = _authHelper.GetUserFromHeader();
        var toDoItem = _context.ToDoItems.FirstOrDefault(i => i.Uid == request.Uid && i.CreatedBy.Equals(userClaims["username"]));
        if (toDoItem is null)
        {
            return TypedResults.NotFound($"ToDo with id {request.Uid} not found");
        }
        
        var group = _context.Groups.FirstOrDefault(i => i.Uid == request.GroupUid);
        if (group is null)
        {
            return TypedResults.BadRequest($"Group with id {request.GroupUid} not found");
        }
        
        toDoItem = request.ToToDoItem(toDoItem, group, userClaims["username"]);
        _ = await _context.SaveChangesAsync();
        return TypedResults.Ok(toDoItem.ToToDoItemResponse());
    }

    public async Task<Results<Ok<DeletionResponse>, NotFound<string>, BadRequest<string>>> DeleteToDo(Guid uid)
    {
        var userClaims = _authHelper.GetUserFromHeader();
        var toDoItem = _context.ToDoItems.FirstOrDefault(i => i.Uid == uid && i.CreatedBy.Equals(userClaims["username"]));
        if (toDoItem is null)
        {
            return TypedResults.NotFound($"ToDo with id {uid} not found");
        }
        _context.ToDoItems.Remove(toDoItem);
        _ = await _context.SaveChangesAsync();
        return TypedResults.Ok(new DeletionResponse()
        {
            Uid = uid,
            DeletedAt = DateTime.UtcNow,
        });
    }

    public async Task<Results<Ok<ToDoItemResponse>, NotFound<string>, BadRequest<string>>> PatchToDo(PatchToDoItemRequest request)
    {
        var userClaims = _authHelper.GetUserFromHeader();
        var toDoItem = _context.ToDoItems.Include(toDoItem => toDoItem.Group)
            .FirstOrDefault(i => i.Uid == request.Uid && i.CreatedBy.Equals(userClaims["username"]));
        
        if (toDoItem is null)
        {
            return TypedResults.NotFound($"ToDo with id {request.Uid} not found");
        }
        
        var newGroup = _context.Groups.FirstOrDefault(i => i.Uid == request.GroupUid);
        if (request.GroupUid is not null && newGroup is null)
        {
            return TypedResults.BadRequest("Group not found");
        }

        toDoItem = request.ToToDoItem(toDoItem, toDoItem.Group, userClaims["username"]);
        _ = await _context.SaveChangesAsync();
        return TypedResults.Ok(toDoItem.ToToDoItemResponse());
    }
}