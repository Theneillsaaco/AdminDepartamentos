﻿using System.ComponentModel;
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
    public string Telefono { get; set; }

    public int? IdUnidadHabitacional { get; set; }
    
    [Required]
    public DateTime CreationDate { get; set; } = DateTime.Now;

    [Column(TypeName = "datetime2")]
    public DateTime? ModifyDate { get; set; }

    [DefaultValue(1)]
    public int CreationUser { get; set; }

    [Required]
    [DefaultValue(0)]
    public bool Deleted { get; set; }
    
    public DateTime? DeletedDate { get; set; }

    public virtual ICollection<Pago> Pagos { get; set; }
    
    public UnidadHabitacional? UnidadHabitacional { get; set; }
}