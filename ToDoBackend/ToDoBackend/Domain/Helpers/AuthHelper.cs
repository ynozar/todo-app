using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
namespace ToDoBackend.Domain.Helpers;
public class AuthHelper: IAuthHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
    private readonly RSA _rsa;
    
    public AuthHelper(IHttpContextAccessor httpContextAccessor, IOptions<ToDoServiceOptions> options)
    {
        _httpContextAccessor = httpContextAccessor;
        _rsa = RSA.Create();
        var privateKeyBytes = Convert.FromBase64String(options.Value.RSA_PRIVATE);
        _rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
    }
    public Dictionary<string,string> GetUserFromHeader()
    {
        var user = _httpContextAccessor.HttpContext?.User ;
        var claims = user?.Claims.ToDictionary(i => i.Type, i => i.Value);
        return claims?? new Dictionary<string, string>();
    }
    
    public string IssueToken(string username, string name,Guid uid)
    {
        
        var credentials = new SigningCredentials(new RsaSecurityKey(_rsa), SecurityAlgorithms.RsaSha256);
        var token = new JwtSecurityToken(
            issuer: "api.todo.yoel.app",
            audience: "todo.yoel.app",
            claims: new List<Claim>()
            {
                new ("username",username),
                new ("full_name",name),
                new ("sub",uid.ToString())
            },
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: credentials);

        return _jwtSecurityTokenHandler.WriteToken(token);
    }
    
}