using Microsoft.AspNetCore.Http.HttpResults;
using ToDoBackend.DataContext;
using ToDoBackend.Domain.Extensions;
using ToDoBackend.Domain.Helpers;
using ToDoBackend.Domain.Services.Interfaces;
using ToDoBackend.DTO;
using ToDoBackend.DTO.Group;

namespace ToDoBackend.Domain.Services.Implementations;

public class GroupService(ApplicationDataContext context, IAuthHelper authHelper) : IGroupService
{
    public async Task<Results<Ok<IEnumerable<GroupResponse>>, NotFound<string>, BadRequest<string>>> GetAllGroups(bool includeToDos=false)
    {
        var userClaims = authHelper.GetUserFromHeader();
        var groups = context.Groups.Where(i => i.CreatedBy.Equals(userClaims["username"])).ToList();

        groups = groups.OrderBy(i=>!i.isDefault).ThenBy(i=>i.Name).ToList();
        return  TypedResults.Ok(groups.Select(i => i.ToGroupResponse(includeToDos)));
    }

    public async Task<Results<Ok<GroupResponse>, NotFound<string>, BadRequest<string>>> GetGroupByUid(Guid uid)
    {
        var userClaims = authHelper.GetUserFromHeader();
        var group = context.Groups.FirstOrDefault(i => i.Uid == uid && i.CreatedBy.Equals(userClaims["username"]));
        if (group is null)
        {
            return TypedResults.NotFound($"Group with uid {uid} not found");
        }

        return TypedResults.Ok(group.ToGroupResponse());
    }

    public async Task<Results<Ok<GroupResponse>, NotFound<string>, BadRequest<string>>> CreateGroup(CreateGroupRequest request)
    {
        var userClaims = authHelper.GetUserFromHeader();
        var user = context.Users.FirstOrDefault(i => i.Username.Equals(userClaims["username"])); //maybe get by Guid
        var newGroup = request.ToGroup(user);
        context.Groups.Add(newGroup);
        _ = await context.SaveChangesAsync();
        return TypedResults.Ok(newGroup.ToGroupResponse());
    }

    public async Task<Results<Ok<GroupResponse>, NotFound<string>, BadRequest<string>>> UpdateGroup(UpdateGroupRequest request)
    {
        var userClaims = authHelper.GetUserFromHeader();
        var group = context.Groups.FirstOrDefault(i => i.Uid == request.Uid);
        if (group is null)
        {
            return TypedResults.NotFound($"Group with uid {request.Uid} not found");
        }
        group = request.ToGroup(group, userClaims["username"]);
        _ = await context.SaveChangesAsync();
        return TypedResults.Ok(group.ToGroupResponse());
    }

    public async Task<Results<Ok<DeletionResponse>, NotFound<string>, BadRequest<string>>> DeleteGroup(Guid uid)
    {
        var userClaims = authHelper.GetUserFromHeader();
        var group = context.Groups.FirstOrDefault(i => i.Uid == uid && i.CreatedBy.Equals(userClaims["username"]));
        if (group is null)
        {
            return TypedResults.NotFound($"Group with uid {uid} not found");
        }
        context.Groups.Remove(group);
        _ = await context.SaveChangesAsync();
        return TypedResults.Ok(new DeletionResponse()
        {
            Uid = uid,
            DeletedAt = DateTime.UtcNow
        });
    }
}