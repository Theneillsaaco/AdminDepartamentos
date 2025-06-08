namespace AdminDepartamentos.API.Models.UnidadHabitacional;

public class UnidadHabitacionalOccuppiedModel
{
    public int IdUnidadHabitacional { get; set; }
    public string Name { get; set; }
    public string Tipo { get; set; }
    public bool Occcupied { get; set; }
    public int? IdInquilinoActual { get; set; }
    
    public UnidadHabitacionalGetByInquilinoModel? InquilinoActual { get; set; }
}