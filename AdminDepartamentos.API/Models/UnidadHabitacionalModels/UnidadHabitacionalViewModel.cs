using AdminDepartamentos.API.Models.UnidadHabitacional.Core;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.API.Models.UnidadHabitacional;

public class UnidadHabitacionalViewModel : UnidadHabitacionalBaseModel
{
    public bool occcupied { get; set; }

    public InquilinoModel? InquilinoActual { get; set; }

    public List<InteresadoModel> Interesados { get; set; } = new();
}