namespace AdminDepartamentos.App.Models.InquilinoModels;

public class InquilinoGetModel
{
    public int IdInquilino { get; set; }
    
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Cedula { get; set; }
    
    public string NumTelefono { get; set; }
    
    public int NumDepartamento { get; set; }
}