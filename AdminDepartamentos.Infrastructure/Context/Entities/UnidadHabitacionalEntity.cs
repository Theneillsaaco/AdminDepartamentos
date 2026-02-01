using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDepartamentos.Infrastructure.Context.Entities;

public class UnidadHabitacionalEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdUnidadHabitacional { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; }

    [Required, MaxLength(20)]
    public string LightCode { get; set; }

    [Required, MaxLength(50)]
    public string Tipo { get; set; }

    public bool Occupied => IdInquilinoActual.HasValue;

    public int? IdInquilinoActual { get; set; }

    [ForeignKey("IdInquilinoActual")]
    public InquilinoEntity? InquilinoActual { get; set; }

    [Required, DefaultValue(false)] 
    public bool Deleted { get; set; }

    [NotMapped]
    public List<InteresadoEntity> Interesados { get; set; } = new();
}