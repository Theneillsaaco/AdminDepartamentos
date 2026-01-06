using AdminDepartamentos.API.Models.UnidadHabitacional.Core;
using AdminDepartamentos.Infrastructure.Models.InquilinoModels;
using AdminDepartamentos.Infrastructure.Models.InteresadoModels;

namespace AdminDepartamentos.API.Models.UnidadHabitacional;

public class UnidadHabitacionalViewModel : UnidadHabitacionalBaseModel
{
    public int IdUnidadHabitacional { get; set; }

    public bool Occupied { get; set; }

    public InquilinoModel? InquilinoActual { get; set; }

    public List<InteresadoModel> Interesados { get; set; } = new();
}