using System.ComponentModel.DataAnnotations;

namespace AdminDepartamentos.API.Models;

public class RegisterModel
{
    [EmailAddress]
    public string Email { get; set; }
    
    public string Password { get; set;}
    
    public string ConfirmPassword { get; set; }
}

public class LoginModel
{
    [EmailAddress]
    public string Email { get; set; }
    
    public string Password { get; set; }
}

public class AuthResponse
{
    public string Token { get; set; }
    
    public string UserName { get; set; }
    
    public DateTime Expiration { get; set; }
}