namespace AdminDepartamentos.API.Models;

public class LoginModel
{
    public string Email { get; set; }
    
    public string Password { get; set; }
}

public class AuthResponse
{
    public string Token { get; set; }
    
    public string UserName { get; set; }
    
    public DateTime Expiration { get; set; }
}