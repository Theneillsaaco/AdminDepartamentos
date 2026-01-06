namespace AdminDepartamentos.Infrastructure.Models.InteresadoModels;

public class InteresadoModel
{
    public int IdUnidadHabitacional { get; set; }

    public int IdInteresado { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Telefono { get; set; }

    public string TipoUnidadHabitacional { get; set; }

    public DateTime Fecha { get; set; }

    public string FullName => $"{FirstName} {LastName}";
}