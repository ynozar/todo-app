using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ToDoBackend.Domain.Services.Interfaces;
using ToDoBackend.DTO.User;

namespace ToDoBackend;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder route = app.MapGroup("/users").WithTags("Users");
        route.MapPost("login", Login).WithName(nameof(Login));
        route.MapPost("signup", Create).WithName(nameof(Create));
        route.MapGet("verify", Verify).WithName(nameof(Verify));
        //add user modification endpoints and services
        return app;
    }
    [AllowAnonymous]
    public static async Task<Results<Ok<string>, NotFound<string>, BadRequest<string>>> Login([FromServices] IUserService service,[FromBody] LoginRequest request)
    {

        return await service.Login(request);
    }
    [AllowAnonymous]
    public static async Task<Results<Ok<string>, NotFound<string>, BadRequest<string>>> Create([FromServices] IUserService service,[FromBody] CreateAccountRequest request)
    {

        return await service.CreateAccount(request);
    }
    [Authorize]
    public static async Task<Results<Ok<string>, NotFound<string>, BadRequest<string>>> Verify([FromServices] IUserService service)
    {

        return await service.VerifyToken();
    }
}