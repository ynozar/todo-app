using Microsoft.AspNetCore.Authorization;
using ToDoBackend.API.EndpointFilters;
using ToDoBackend.API.Validators.Group;
using ToDoBackend.Domain.Services.Interfaces;

namespace ToDoBackend;

using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ToDoBackend.Domain.Services.Interfaces;
using ToDoBackend.DTO;
using ToDoBackend.DTO.Group;

public static class GroupEndpoints
{
    
    public static IEndpointRouteBuilder MapGroupEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder route = app.MapGroup("/groups").WithTags("Groups");
        route.MapGet("", GetAllGroups).WithName(nameof(GetAllGroups));
        route.MapGet("{uid}", GetGroupById).WithName(nameof(GetGroupById));
        route.MapPost("", CreateGroup).WithName(nameof(CreateGroup))
           .AddEndpointFilter<ValidationFilter<CreateGroupRequest>>();
        route.MapPut("", UpdateGroup).WithName(nameof(UpdateGroup))
            .AddEndpointFilter<ValidationFilter<UpdateGroupRequest>>();
        route.MapDelete("{uid}", DeleteGroup).WithName(nameof(DeleteGroup));
        
        return app;
    }
    
    [Authorize]
    public static async Task<Results<Ok<IEnumerable<GroupResponse>>, NotFound<string>, BadRequest<string>>>  GetAllGroups([FromServices] IGroupService service, [FromQuery] bool? includeToDos)
    {
        return await service.GetAllGroups(includeToDos ?? false);
    }
    
    [Authorize]
    public static async Task<Results<Ok<GroupResponse>, NotFound<string>, BadRequest<string>>> GetGroupById([FromServices] IGroupService service,Guid uid)
    {
        return await service.GetGroupByUid(uid);
    }
    
    [Authorize]
    public static async Task<Results<Ok<GroupResponse>, NotFound<string>, BadRequest<string>>> CreateGroup([FromServices] IGroupService service,[FromBody] CreateGroupRequest request)
    {

        return await service.CreateGroup(request);
    }

    [Authorize]
    public static async Task<Results<Ok<GroupResponse>, NotFound<string>, BadRequest<string>>> UpdateGroup([FromServices] IGroupService service,[FromBody] UpdateGroupRequest request)
    {

        return await service.UpdateGroup(request);
    }

    [Authorize]
    public static async Task<Results<Ok<DeletionResponse>, NotFound<string>, BadRequest<string>>> DeleteGroup([FromServices] IGroupService service, Guid uid)
    {

        return await service.DeleteGroup(uid);
    }

}