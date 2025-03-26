namespace crm_dashboard.Models;

public class LoginRequest
{
    public LoginRequest(string email, string password)
    {
        Email = email;
        Password = password;
    }
    public LoginRequest() { }

    public string Email { get; set; }
    public string Password { get; set; }
}