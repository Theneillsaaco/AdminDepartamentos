using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDepartamentos.Domain.Entities;

public partial class Interesado
{
    protected Interesado() {}
    
    public Interesado(string firstName, string lastName, string telefono, string tipoUnidad)
    {
        FirstName = firstName;
        LastName = lastName;
        Telefono = telefono;
        TipoUnidadHabitacional = tipoUnidad;
        Deleted = false;
    }
    
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
    [Column(TypeName = "timestamp without time zone")]
    public DateTime Fecha { get; set; } = DateTime.Now;

    [Required]
    public string TipoUnidadHabitacional { get; set; }
    
    [Required]
    [DefaultValue(0)]
    public bool Deleted { get; set; }
    
    [ForeignKey("IdUnidadHabitacional")]
    public UnidadHabitacional? UnidadHabitacional { get; set; }
}