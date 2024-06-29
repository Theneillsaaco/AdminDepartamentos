using AdminDepartamentos.API.Models.InquilinoModels.Core;

namespace AdminDepartamentos.API.Models.InquilinoModels;

public class InquilinoUpdateModel : InquilinoBaseViewModel
{
    public string FirstName { get; set; }

    public string Cedula { get; set; }

    public string NumTelefono { get; set; }
    
    public DateTime? ModifyDate { get; set; }
}