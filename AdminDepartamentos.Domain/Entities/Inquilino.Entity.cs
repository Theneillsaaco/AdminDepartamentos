using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDepartamentos.Domain.Entities;

public partial class Inquilino
{
    protected Inquilino()
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdInquilino { get; set; }

    [Required] public string FirstName { get; set; }

    [Required] public string LastName { get; set; }

    [Required] public string Cedula { get; set; }

    [Required] public string Telefono { get; set; }

    public int? IdUnidadHabitacional { get; set; }

    [Required]
    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreationDate { get; set; } = DateTime.Now;

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ModifyDate { get; set; }

    [DefaultValue(1)] public int CreationUser { get; set; }

    [Required] [DefaultValue(0)] public bool Deleted { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? DeletedDate { get; set; }

    public Pago Pago { get; private set; }
    public UnidadHabitacional? UnidadHabitacional { get; set; }
}