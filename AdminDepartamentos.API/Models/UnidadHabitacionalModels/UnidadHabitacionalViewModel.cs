using AdminDepartamentos.API.Models.InquilinoModels;
using AdminDepartamentos.API.Models.InteresadoModels;
using AdminDepartamentos.Domain.Models;

namespace AdminDepartamentos.API.Models.UnidadHabitacional;

public class UnidadHabitacionalViewModel
{
    public string Name { get; set; }

    public string Tipo { get; set; }

    public bool occcupied { get; set; }

    public InquilinoModel? InquilinoActual { get; set; }

    public List<InteresadoModel> Interesados { get; set; } = new();
}