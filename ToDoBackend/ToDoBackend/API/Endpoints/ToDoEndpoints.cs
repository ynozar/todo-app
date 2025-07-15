using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ToDoBackend.API.EndpointFilters;
using ToDoBackend.Domain.Services.Interfaces;
using ToDoBackend.DTO;
using ToDoBackend.DTO.ToDoItem;

namespace ToDoBackend;

public static class ToDoEndpoints
{
    
    public static IEndpointRouteBuilder MapToDoEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder route = app.MapGroup("/todos").WithTags("ToDos");
        
        route.MapGet("", GetAllToDos).WithName(nameof(GetAllToDos));
        route.MapGet("{uid}", GetToDoById).WithName(nameof(GetToDoById));
        route.MapPost("", CreateToDo).WithName(nameof(CreateToDo))
            .AddEndpointFilter<ValidationFilter<CreateToDoItemRequest>>();
        route.MapPut("", UpdateToDo).WithName(nameof(UpdateToDo))
            .AddEndpointFilter<ValidationFilter<UpdateToDoItemRequest>>();
        route.MapDelete("{uid}", DeleteToDo).WithName(nameof(DeleteToDo));
        route.MapPatch("", PatchToDo).WithName(nameof(PatchToDo))
            .AddEndpointFilter<ValidationFilter<PatchToDoItemRequest>>();
        return app;
    }
    
    [Authorize]
    public static async Task<Results<Ok<IEnumerable<ToDoItemResponse>>, NotFound<string>, BadRequest<string>>> GetAllToDos([FromServices] IToDoService service, [FromQuery] Guid? groupUid, [FromQuery] bool? isCompleted, [FromQuery] int? priority, [FromQuery] DateTime? dueBefore)
    {
        return await service.GetAllToDos(groupUid, isCompleted, priority, dueBefore);
    }
    
    [Authorize]
    public static async Task<Results<Ok<ToDoItemResponse>, NotFound<string>, BadRequest<string>>> GetToDoById([FromServices] IToDoService service, Guid uid)
    {
  

        return await service.GetToDoById(uid);
    }
    
    [Authorize]
    public static async Task<Results<Ok<ToDoItemResponse>, NotFound<string>, BadRequest<string>>> CreateToDo([FromServices] IToDoService service, [FromBody] CreateToDoItemRequest request)
    {

        return await service.CreateToDo(request);
    }
    
    [Authorize]
    public static async Task<Results<Ok<ToDoItemResponse>, NotFound<string>, BadRequest<string>>> UpdateToDo([FromServices] IToDoService service, [FromBody] UpdateToDoItemRequest request)
    {

        return await service.UpdateToDo(request);
    }
    
    [Authorize]
    public static async Task<Results<Ok<DeletionResponse>, NotFound<string>, BadRequest<string>>> DeleteToDo([FromServices] IToDoService service, Guid uid)
    {
        
        return await service.DeleteToDo(uid);
    }

    [Authorize]
    public static async Task<Results<Ok<ToDoItemResponse>, NotFound<string>, BadRequest<string>>> PatchToDo([FromServices] IToDoService service, [FromBody] PatchToDoItemRequest request)
    {

        return await service.PatchToDo(request);
    }
    


}