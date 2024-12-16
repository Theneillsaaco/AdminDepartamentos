using AdminDepartamentos.API.Models.InquilinoModels.Core;

namespace AdminDepartamentos.API.Models.InquilinoModels;

public class InquilinoViewModel : InquilinoBaseViewModel
{
    public int IdInquilino { get; set; }
    
    public string Cedula { get; set; }
    
    public string NumTelefono { get; set; }
}