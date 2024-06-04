using Microsoft.AspNetCore.Http.HttpResults;
using ToDoBackend.DTO.User;

namespace ToDoBackend.Domain.Services.Interfaces;

public interface IUserService
{
    public Task<Results<Ok<string>, NotFound<string>, BadRequest<string>>> Login(LoginRequest request);

    public Task<Results<Ok<string>, NotFound<string>, BadRequest<string>>> CreateAccount(CreateAccountRequest request);
    
    public Task<Results<Ok<string>, NotFound<string>, BadRequest<string>>> VerifyToken();
}