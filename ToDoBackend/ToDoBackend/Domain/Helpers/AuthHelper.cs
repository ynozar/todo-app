using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ToDoBackend.Domain.Helpers;

public class AuthHelper: IAuthHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptions<ToDoServiceOptions> _options;
    private RSA _rsa;
    
    public AuthHelper(IHttpContextAccessor httpContextAccessor, IOptions<ToDoServiceOptions> options)
    {
        _httpContextAccessor = httpContextAccessor;
        _options = options;
        _rsa = RSA.Create();
        var privateKeyBytes = Convert.FromBase64String(_options.Value.RSA_PRIVATE);

        _rsa.ImportRSAPrivateKey(privateKeyBytes, out _);
    }
    public Dictionary<string,string> GetUserFromHeader()
    {
        var user = _httpContextAccessor.HttpContext.User;
        var claims = user.Claims.ToDictionary(i => i.Type, i => i.Value);
        return claims;
    }
    
    public string IssueToken(string username, string name,Guid uid)
    {

        
        
        var creds = new SigningCredentials(new RsaSecurityKey(_rsa), SecurityAlgorithms.RsaSha256);

        var token = new JwtSecurityToken(
            issuer: "ssss",
            audience: "todoapp",
            claims: new List<Claim>()
            {
                new ("username",username),
                new ("full_name",name),
                new ("sub",uid.ToString())
            },
            expires: DateTime.Now.AddMinutes(60),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
}