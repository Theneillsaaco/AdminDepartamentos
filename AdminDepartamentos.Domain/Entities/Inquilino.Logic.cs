namespace AdminDepartamentos.Domain.Entities;

public partial class Inquilino
{
    public bool IsDeleted => Deleted;

    public void AsignPago(Pago pago)
    {
        Pago = pago ?? throw new ArgumentNullException(nameof(pago));
    }

    public static Inquilino Create(
        string firstName,
        string lastName,
        string cedula,
        string telefono
    )
    {
        if (string.IsNullOrWhiteSpace(cedula))
            throw new Exception("La cedula es obligatoria");
        
        return new Inquilino
        {
            FirstName = firstName,
            LastName = lastName,
            Cedula = cedula,
            Telefono = telefono,
        };
    }
    
    public void Update(string firstName, string lastName, string cedula, string telefono)
    {
        FirstName = firstName;
        LastName = lastName;
        Cedula = cedula;
        Telefono = telefono;
        ModifyDate = DateTime.Now;
    }
    
    public void MarkDeleted()
    {
        if (Deleted)
            return;
        
        Deleted = true;
        DeletedDate = DateTime.Now;
        ModifyDate = DateTime.Now;

        Pago?.MarkDeleted();
    }
}