using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ToDoBackend.Domain.Services.Interfaces;
using ToDoBackend.DTO;
using ToDoBackend.DTO.Group;

namespace ToDoBackend;
/*
[ApiController]
[Route("controller/groups")]
public class GroupsController : ControllerBase

{
    [HttpGet("test")]
    public async Task<ActionResult<string>> GetTest()
    {
        return Ok("Hello, World!");
    }
    
    [HttpGet("")]
    public async Task<Results<Ok<IEnumerable<GroupResponse>>, NotFound<string>, BadRequest<string>>>  GetAllGroups([FromServices] IGroupService service)
    {
        return await service.GetAllGroups();
    }
    
    [HttpGet("{uid}")]
    public async Task<Results<Ok<GroupResponse>, NotFound<string>, BadRequest<string>>> GetGroupById([FromServices] IGroupService service,Guid uid)
    {
        return await service.GetGroupByUid(uid);
    }
    
    [HttpPost("")]
    public async Task<Results<Ok<GroupResponse>, NotFound<string>, BadRequest<string>>> CreateGroup([FromServices] IGroupService service,[FromBody] CreateGroupRequest request)
    {

        return await service.CreateGroup(request);
    }

    [HttpPut("")]
    public async Task<Results<Ok<GroupResponse>, NotFound<string>, BadRequest<string>>> UpdateGroup([FromServices] IGroupService service,[FromBody] UpdateGroupRequest request)
    {

        return await service.UpdateGroup(request);
    }

    [HttpDelete("")]
    public async Task<Results<Ok<DeletionResponse>, NotFound<string>, BadRequest<string>>> DeleteGroup([FromServices] IGroupService service,[FromBody] DeletionRequest request)
    {

        return await service.DeleteGroup(request);
    }
    
}
*/