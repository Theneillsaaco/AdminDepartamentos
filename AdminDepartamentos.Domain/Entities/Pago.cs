using System.ComponentModel.DataAnnotations;

namespace AdminDepartamentos.Domain.Entities;

public partial class Pago
{
    [Key]
    public int IdPago { get; set; }

    public int IdInquilino { get; set; }

    public int? NumDeposito { get; set; }

    public decimal Monto { get; set; }

    public DateOnly FechaPago { get; set; }

    public bool Retrasado { get; set; }

    public bool Deleted { get; set; }

    public virtual Inquilino IdInquilinoNavigation { get; set; }
}