using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.API.Models.InquilinoModels;

public class InquilinoSaveModel
{
    public InquilinoDto InquilinoDto { get; set; }
    
    public PagoDto PagoDto { get; set; }
}