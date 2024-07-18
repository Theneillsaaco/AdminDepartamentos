using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdminDepartamentos.Domain.Entities;

public partial class Inquilino
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int IdInquilino { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Cedula { get; set; }

    [Required]
    public int NumDepartamento { get; set; }

    [Required]
    public string NumTelefono { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.Now;

    [Column(TypeName = "datetime2")]
    public DateTime? ModifyDate { get; set; }

    public int CreationUser { get; set; }

    public bool Deleted { get; set; }
    
    public DateTime? DeletedDate { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; }
}