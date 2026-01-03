namespace AdminDepartamentos.API.Models.LoginModels;

public class AuthResponse
{
    public string Token { get; set; }

    public string UserName { get; set; }

    public DateTime Expiration { get; set; }
}