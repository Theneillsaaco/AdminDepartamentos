using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDepartamentos.Domain.Entities;

public partial class Pago
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdPago { get; set; }
    
    [Required]
    public int IdInquilino { get; set; }
    
    [Required]
    public int? NumDeposito { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Monto { get; set; }

    [Required]
    public int FechaPagoInDays { get; set; }

    [Required]
    [DefaultValue(true)]
    public bool Retrasado { get; set; }
    
    [Column(TypeName = "timestamp without time zone")]
    public DateTime? RetrasadoDate { get; set; }
    
    [Required]
    [DefaultValue(true)]
    public bool Email { get; set; }

    [Required]
    [DefaultValue(false)]
    public bool Deleted { get; set; }

    [ForeignKey("IdInquilino")]
    public Inquilino Inquilino { get; set; }
}