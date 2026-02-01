using System.ComponentModel.DataAnnotations;

namespace AdminDepartamentos.API.Models.LoginModels;

public class RegisterModel
{
    [EmailAddress] 
    public string Email { get; set; }

    [Required, MinLength(8), MaxLength(100)]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
    public string ConfirmPassword { get; set; }
}