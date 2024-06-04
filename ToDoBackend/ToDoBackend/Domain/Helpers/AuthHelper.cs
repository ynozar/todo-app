using System.Security.Cryptography.X509Certificates;
using System.Text;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using Microsoft.Extensions.Options;

namespace ToDoBackend.Domain.Helpers;

public class AuthHelper: IAuthHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IOptions<ToDoServiceOptions> _options;
    
    public AuthHelper(IHttpContextAccessor httpContextAccessor, IOptions<ToDoServiceOptions> options)
    {
        _httpContextAccessor = httpContextAccessor;
        _options = options;
    }
    public Dictionary<string,string> GetUserFromHeader()
    {
        var user = _httpContextAccessor.HttpContext.User;
        var claims = user.Claims.ToDictionary(i => i.Type, i => i.Value);
        return claims;
    }
    
    public string IssueToken(string username, string name,Guid uid)
    {
        var payload = new Dictionary<string, object>
        {
            { "exp", new DateTimeOffset(DateTime.UtcNow.AddHours(1)).ToUnixTimeSeconds() },
            { "username", username },
            { "full_name", name },
            { "sub", uid },
            { "aud", "claim2-value" }
        };
        
        var certificate = new X509Certificate2(Convert.FromBase64String(_options.Value.Certificate), _options.Value.Cert_Password);

        IJwtAlgorithm algorithm = new RS256Algorithm(certificate);
        IJsonSerializer serializer = new JsonNetSerializer();
        IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
        IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
        const string key = null; // not needed if algorithm is asymmetric

        var token = encoder.Encode(payload, key);
        //Console.WriteLine(token);
        return token;
    }
    
}