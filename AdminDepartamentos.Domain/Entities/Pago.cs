using System.ComponentModel;
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

    public int? FechaPagoInDays { get; set; }

    [DefaultValue(1)]
    public bool Retrasado { get; set; }

    [DefaultValue(0)]
    public bool Deleted { get; set; }

    [ForeignKey("IdInquilino")]
    public Inquilino Inquilino { get; set; }
}