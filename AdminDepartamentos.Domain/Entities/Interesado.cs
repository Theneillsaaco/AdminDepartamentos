using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDepartamentos.Domain.Entities;

public class Interesado
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdInteresado { get; set; }
    
    [Required]
    public string FirstName { get; set; }
    
    [Required]
    public string LastName { get; set; }
    
    [Required]
    [Phone]
    public string Telefono { get; set; }
    
    [Required]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required]
    public int IdUnidadHabitacional { get; set; }
    
    [Required]
    [DefaultValue(0)]
    public bool Deleted { get; set; }
    
    [ForeignKey("IdUnidadHabitacional")]
    public UnidadHabitacional? UnidadHabitacional { get; set; }
}