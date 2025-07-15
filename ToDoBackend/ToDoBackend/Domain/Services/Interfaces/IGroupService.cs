
using Microsoft.AspNetCore.Http.HttpResults;
using ToDoBackend.DTO;
using ToDoBackend.DTO.Group;

namespace ToDoBackend.Domain.Services.Interfaces;

public interface IGroupService
{

    public Task<Results<Ok<IEnumerable<GroupResponse>>, NotFound<string>, BadRequest<string>>> GetAllGroups(bool includeToDos = false);
    
    public Task<Results<Ok<GroupResponse>, NotFound<string>, BadRequest<string>>> GetGroupByUid(Guid uid);
    
    public Task<Results<Ok<GroupResponse>, NotFound<string>, BadRequest<string>>> CreateGroup(CreateGroupRequest request);
    
    public Task<Results<Ok<GroupResponse>, NotFound<string>, BadRequest<string>>> UpdateGroup(UpdateGroupRequest request);
    
    public Task<Results<Ok<DeletionResponse>, NotFound<string>, BadRequest<string>>> DeleteGroup(Guid uid);
}