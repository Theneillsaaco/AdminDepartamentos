using System.ComponentModel.DataAnnotations;

namespace AdminDepartamentos.API.Models.LoginModels;

public class RegisterModel
{
    [EmailAddress] public string Email { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }
}