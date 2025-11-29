using AdminDepartamentos.API.Models.UnidadHabitacional.Core;

namespace AdminDepartamentos.API.Models.UnidadHabitacional;

public class UnidadHabitacionalOccuppiedModel : UnidadHabitacionalBaseModel
{
    public int IdUnidadHabitacional { get; set; }
    
    public bool Occupied { get; set; }
    
    public int? IdInquilinoActual { get; set; }
    
    
    public UnidadHabitacionalGetByInquilinoModel? InquilinoActual { get; set; }
}