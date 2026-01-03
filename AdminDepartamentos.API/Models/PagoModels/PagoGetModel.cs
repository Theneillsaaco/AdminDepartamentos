using AdminDepartamentos.API.Models.PagoModels.Core;

namespace AdminDepartamentos.API.Models.PagoModels;

public class PagoGetModel : PagoInquilinoBaseModel
{
    public bool Email { get; set; }

    public bool Deleted { get; set; }
}