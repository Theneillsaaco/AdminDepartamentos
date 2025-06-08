namespace AdminDepartamentos.Domain.Models;

public class UnidadHabitacionalModel
{
    public int IdUnidadHabitacional { get; set; }
    
    public string Name { get; set; }

    public string Tipo { get; set; }

    public bool occcupied { get; set; }

    public InquilinoModel? InquilinoActual { get; set; }

    public List<InteresadoModel> Interesados { get; set; } = new();
}