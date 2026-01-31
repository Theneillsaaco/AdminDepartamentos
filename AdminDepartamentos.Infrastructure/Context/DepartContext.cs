using AdminDepartamentos.Infrastructure.Context.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartamentos.Infrastructure.Context;

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

        builder.Entity<InteresadoEntity>()
            .HasQueryFilter(u => !u.Deleted);
        
        builder.Entity<UnidadHabitacionalEntity>()
            .HasQueryFilter(u => !u.Deleted);

        #region "Indexes"

        builder.Entity<InquilinoEntity>()
            .HasIndex(inq => inq.Cedula)
            .IsUnique();

        builder.Entity<PagoEntity>()
            .HasIndex(pa => pa.Retrasado)
            .IsUnique();

        builder.Entity<InteresadoEntity>()
            .HasIndex(ints => ints.TipoUnidadHabitacional)
            .IsUnique();

        builder.Entity<UnidadHabitacionalEntity>()
            .HasIndex(uni => uni.IdInquilinoActual)
            .IsUnique();

        #endregion
        
        base.OnModelCreating(builder);
    }

    #region "Entities"

    public DbSet<InquilinoEntity> Inquilinos { get; set; }
    public DbSet<PagoEntity> Pagos { get; set; }

    public DbSet<UnidadHabitacionalEntity> UnidadHabitacionals { get; set; }

    public DbSet<InteresadoEntity> Interesados { get; set; }

    #endregion
}