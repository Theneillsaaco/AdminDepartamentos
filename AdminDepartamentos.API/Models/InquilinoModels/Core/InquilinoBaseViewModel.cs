namespace AdminDepartamentos.API.Models.InquilinoModels.Core;

public abstract class InquilinoBaseViewModel
{
    public string FirstName { get; set; }

    public string LastName { get; set; }
    
    public string Cedula { get; set; }
    
    public string NumTelefono { get; set; }
}