namespace AdminDepartamentos.Domain.Models;

public class InteresadoModel
{
    public int IdInteresado { get; set; }

    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string Telefono { get; set; }
    
    public DateTime Fecha { get; set; }

    public int IdUnidadHabitacional { get; set; }
}