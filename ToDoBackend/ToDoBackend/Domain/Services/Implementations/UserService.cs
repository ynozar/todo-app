using Microsoft.AspNetCore.Http.HttpResults;
using ToDoBackend.DataContext;
using ToDoBackend.Domain.Extensions;
using ToDoBackend.Domain.Helpers;
using ToDoBackend.Domain.Services.Interfaces;
using ToDoBackend.DTO.User;

namespace ToDoBackend.Domain.Services.Implementations;

public class UserService: IUserService
{
    private readonly ApplicationDataContext _context;
    private readonly IAuthHelper _authHelper;
    private const int MinPassLength = 8;
    private const int MinUsernameLength = 6;
    public UserService(ApplicationDataContext context, IAuthHelper authHelper)
    {
        _context = context;
        _authHelper = authHelper;
    }
    public async Task<Results<Ok<string>, NotFound<string>, BadRequest<string>>> Login(LoginRequest request)
    {
        request.Username= request.Username.ToLower();
        User? user = _context.Users.FirstOrDefault(i => i.Username == request.Username);
        
        if (user is null)
        {
            return TypedResults.BadRequest("Invalid username or password");
        }
        
        var validatePassword = BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.HashedPassword);
        if (!validatePassword)
        {
            return TypedResults.BadRequest("Invalid username or password");
        }
        return TypedResults.Ok($"{_authHelper.IssueToken(request.Username,user.Name, user.Uid )}");
    }
    
    public async Task<Results<Ok<string>, NotFound<string>, BadRequest<string>>> CreateAccount(CreateAccountRequest request)
    {
        request.Username= request.Username.ToLower();
        var user = _context.Users.FirstOrDefault(i => i.Username == request.Username);
        if (user is not null)
        {
            return TypedResults.BadRequest("Username already exists");
        }
        
        if (request.Username.Length<MinUsernameLength)
        {
            return TypedResults.BadRequest($"Username must be at least {MinUsernameLength} characters");
        }
        
        if (request.Password.Length<MinPassLength)
        {
            return TypedResults.BadRequest($"Password must be at least {MinPassLength} characters");
        }
        
        
        var enhancedHashPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password);
        user = request.ToUser(enhancedHashPassword);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return TypedResults.Ok($"{_authHelper.IssueToken(request.Username, user.Name,user.Uid)}");
    }

    public async Task<Results<Ok<string>, NotFound<string>, BadRequest<string>>> VerifyToken()
    {
        var userClaims = _authHelper.GetUserFromHeader();
        return TypedResults.Ok($"Hello, {userClaims["username"]}. You are authorized.");
    }
}