namespace AdminDepartamentos.App.Models.InquilinoModels;

public class InquilinoGetByIdModel
{
    public int IdInquilino { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Cedula { get; set; }

    public int NumDepartamento { get; set; }
    
    public string NumTelefono { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;
    
    public DateTime? ModifyDate { get; set; }
}