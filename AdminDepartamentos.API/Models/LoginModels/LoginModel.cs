using System.ComponentModel.DataAnnotations;

namespace AdminDepartamentos.API.Models.LoginModels;

public class LoginModel
{
    [Required, EmailAddress, MaxLength(100)]
     public string Email { get; set; }

    [Required, MinLength(8), MaxLength(100)]
    public string Password { get; set; }
}