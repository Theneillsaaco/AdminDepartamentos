using AdminDepartamentos.API.Models.InquilinoModels.Core;

namespace AdminDepartamentos.API.Models.InquilinoModels;

public class InquilinoUpdateModel : InquilinoBaseViewModel
{
    public string Cedula { get; set; }

    public string NumTelefono { get; set; }
}