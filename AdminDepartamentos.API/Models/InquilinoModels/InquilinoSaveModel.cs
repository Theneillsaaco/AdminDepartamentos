using AdminDepartamentos.API.Models.InquilinoModels.Core;

namespace AdminDepartamentos.API.Models.InquilinoModels;

public class InquilinoSaveModel : InquilinoBaseViewModel
{
    public string Cedula { get; set; }
    
    public string NumTelefono { get; set; }
    
    public int? NumDeposito { get; set; }
    
    public decimal Monto { get; set; }
}