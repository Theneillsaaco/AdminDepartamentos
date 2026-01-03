namespace AdminDepartamentos.Domain.Entities;

public partial class Interesado
{
    public void Update(string firstName, string lastName, string telefono, string tipoUnidad)
    {
        FirstName = firstName;
        LastName = lastName;
        Telefono = telefono;
        TipoUnidadHabitacional = tipoUnidad;
    }

    public void MarkDeleted()
    {
        if (Deleted)
            return;
        
        Deleted = true;
    }
}