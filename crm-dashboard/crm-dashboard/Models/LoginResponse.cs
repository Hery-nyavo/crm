namespace crm_dashboard.Models;

public class LoginResponse
{
    public LoginResponse(string token, string username, List<string> roles)
    {
        Token = token;
        Username = username;
        Roles = roles;
    }
    public LoginResponse() { }

    public string Token { get; set; }
    public string Username { get; set; }
    public List<string> Roles { get; set; }
}