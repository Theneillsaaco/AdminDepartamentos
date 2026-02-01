using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDepartamentos.Infrastructure.Context.Entities;

public class InteresadoEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdInteresado { get; set; }

    [Required, MaxLength(50)]
    public string FirstName { get; set; }

    [Required, MaxLength(50)]
    public string LastName { get; set; }

    [Required, Phone, MaxLength(20)]
    public string Telefono { get; set; }

    [Required]
    [Column(TypeName = "timestamp without time zone")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required]
    public string TipoUnidadHabitacional { get; set; }

    [Required]
    [DefaultValue(0)]
    public bool Deleted { get; set; }
}