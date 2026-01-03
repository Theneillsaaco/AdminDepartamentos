using AdminDepartamentos.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartamentos.Infrastructure.Context;

public class DepartContext : IdentityDbContext<IdentityUser>
{
    public DepartContext(DbContextOptions<DepartContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Inquilino>()
            .HasOne(i => i.Pago)
            .WithOne(p => p.Inquilino)
            .HasForeignKey<Pago>(p => p.IdInquilino)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.Entity<Inquilino>()
            .HasQueryFilter(i => !i.Deleted);

        builder.Entity<Pago>()
            .HasQueryFilter(p => !p.Deleted);
        
        base.OnModelCreating(builder);
    }

    #region "Entities"

    public DbSet<Inquilino> Inquilinos { get; set; }
    public DbSet<Pago> Pagos { get; set; }
    
    public DbSet<UnidadHabitacional> UnidadHabitacionals { get; set; }
    
    public DbSet<Interesado> Interesados { get; set; }

    #endregion
}