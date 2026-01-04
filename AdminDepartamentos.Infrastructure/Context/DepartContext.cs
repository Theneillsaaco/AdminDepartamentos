using AdminDepartamentos.Domain.Entities;
using AdminDepartamentos.Infrastucture.Context.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartamentos.Infrastucture.Context;

public class DepartContext : IdentityDbContext<IdentityUser>
{
    public DepartContext(DbContextOptions<DepartContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<InquilinoEntity>()
            .HasOne(i => i.Pago)
            .WithOne(p => p.Inquilino)
            .HasForeignKey<PagoEntity>(p => p.IdInquilino)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<InquilinoEntity>()
            .HasQueryFilter(i => !i.Deleted);

        builder.Entity<PagoEntity>()
            .HasQueryFilter(p => !p.Deleted);

        builder.Entity<Interesado>()
            .HasQueryFilter(u => !u.Deleted);

        builder.Entity<UnidadHabitacional>()
            .HasQueryFilter(u => !u.Deleted);

        base.OnModelCreating(builder);
    }

    #region "Entities"

    public DbSet<InquilinoEntity> Inquilinos { get; set; }
    public DbSet<PagoEntity> Pagos { get; set; }

    public DbSet<UnidadHabitacional> UnidadHabitacionals { get; set; }

    public DbSet<Interesado> Interesados { get; set; }

    #endregion
}