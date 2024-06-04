namespace ToDoBackend.Domain.Helpers;

public interface IAuthHelper
{
    public Dictionary<string,string> GetUserFromHeader();
    public string IssueToken(string username, string name,Guid uid);
}