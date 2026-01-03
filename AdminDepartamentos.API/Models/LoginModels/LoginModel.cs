using System.ComponentModel.DataAnnotations;

namespace AdminDepartamentos.API.Models.LoginModels;

public class LoginModel
{
    [EmailAddress] public string Email { get; set; }

    public string Password { get; set; }
}