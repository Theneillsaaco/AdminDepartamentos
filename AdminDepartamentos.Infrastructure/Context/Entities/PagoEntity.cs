using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDepartamentos.Infrastructure.Context.Entities;

public class PagoEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdPago { get; set; }

    [Required] 
    public int IdInquilino { get; set; }

    [MaxLength(50)]
    public int? NumDeposito { get; set; }

    [Required, MaxLength(18), Column(TypeName = "decimal(18,2)")]
    public decimal Monto { get; set; }

    [Required, MaxLength(3)] 
    public int FechaPagoInDays { get; set; }

    [Required]
    public bool Retrasado { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? RetrasadoDate { get; set; }

    [Required, DefaultValue(true)] 
    public bool Email { get; set; }

    [Required, DefaultValue(false)] 
    public bool Deleted { get; set; }

    [ForeignKey("IdInquilino")] 
    public InquilinoEntity Inquilino { get; set; }
}