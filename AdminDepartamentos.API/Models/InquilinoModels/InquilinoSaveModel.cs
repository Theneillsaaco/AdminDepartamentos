using AdminDepartamentos.Infrastructure.Models.InquilinoModels;
using AdminDepartamentos.Infrastructure.Models.PagoModels;

namespace AdminDepartamentos.API.Models.InquilinoModels;

public class InquilinoSaveModel
{
    public InquilinoDto InquilinoDto { get; set; }

    public PagoDto PagoDto { get; set; }
}