namespace AdminDepartamentos.API.Models.InteresadoModels;

public class InteresadoViewModel
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Telefono { get; set; }

    public DateTime Fecha { get; set; }

    public int UnidadHabitacionalId { get; set; }

    public string? UnidadHabitacionalName { get; set; }
}