using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDepartamentos.Domain.Entities;

public partial class UnidadHabitacional
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdUnidadHabitacional { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string LightCode { get; set; }
    
    [Required]
    public string Tipo { get; set; }

    public bool Occupied => IdInquilinoActual.HasValue;
    
    public int? IdInquilinoActual { get; set; }
    
    [ForeignKey("IdInquilinoActual")]
    public Inquilino? InquilinoActual { get; set; }
    
    [Required]
    [DefaultValue(0)]
    public bool Deleted { get; set; }
    
    public ICollection<Interesado> Interesados { get; set; } = new List<Interesado>();
}