using Microsoft.AspNetCore.Http.HttpResults;
using ToDoBackend.DTO;
using ToDoBackend.DTO.Group;
using ToDoBackend.DTO.ToDoItem;

namespace ToDoBackend.Domain.Services.Interfaces;

public interface IToDoService
{
    public Task<Results<Ok<IEnumerable<ToDoItemResponse>>, NotFound<string>, BadRequest<string>>> GetAllToDos(Guid? groupUid, bool? isCompleted, int? priority,
        DateTime? dueBefore);
    public Task<Results<Ok<ToDoItemResponse>, NotFound<string>, BadRequest<string>>> GetToDoById(Guid uid);

    public Task<Results<Ok<ToDoItemResponse>, NotFound<string>, BadRequest<string>>> CreateToDo(CreateToDoItemRequest request);
    
    public Task<Results<Ok<ToDoItemResponse>, NotFound<string>, BadRequest<string>>> UpdateToDo(UpdateToDoItemRequest request);
    
    public Task<Results<Ok<DeletionResponse>, NotFound<string>, BadRequest<string>>> DeleteToDo(Guid uid);
    
    public Task<Results<Ok<ToDoItemResponse>, NotFound<string>, BadRequest<string>>> PatchToDo(PatchToDoItemRequest request);
}