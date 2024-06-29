using AdminDepartamentos.API.Models.InquilinoModels.Core;

namespace AdminDepartamentos.API.Models.InquilinoModels;

public class InquilinoDetailModel : InquilinoBaseViewModel
{
    public int IdInquilino { get; set; }
    
    public string Cedula { get; set; }
    
    public string NumTelefono { get; set; }
    
    public DateTime CreationDate { get; set; }
}