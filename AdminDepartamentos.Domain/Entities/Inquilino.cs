using System.ComponentModel.DataAnnotations;

namespace AdminDepartamentos.Domain.Entities;

public partial class Inquilino
{
    [Key]
    public int IdInquilino { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Cedula { get; set; }

    public int NumDepartamento { get; set; }

    public string NumTelefono { get; set; }

    public DateTime CreationDate { get; set; }

    public DateTime? ModifyDate { get; set; }

    public int CreationUser { get; set; }

    public bool Deleted { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}