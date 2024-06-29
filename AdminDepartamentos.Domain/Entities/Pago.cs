using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDepartamentos.Domain.Entities;

public partial class Pago
{
    [Key]
    public int IdPago { get; set; }
    
    [Required]
    public int IdInquilino { get; set; }
    
    [Required]
    public int? NumDeposito { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Monto { get; set; }

    public DateOnly? FechaPago { get; set; }

    public bool Retrasado { get; set; }

    public bool Deleted { get; set; }

    public virtual Inquilino IdInquilinoNavigation { get; set; }
}