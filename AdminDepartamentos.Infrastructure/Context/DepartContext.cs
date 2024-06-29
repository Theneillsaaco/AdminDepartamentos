using AdminDepartamentos.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace AdminDepartament.Infrastructure.Context
{
    public partial class DepartContext : DbContext
    {
        public DepartContext(DbContextOptions<DepartContext> options) : base(options) { }

        #region "Entities"
        
        public DbSet<Inquilino> Inquilinos { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.IdInquilinoNavigation)
                .WithMany(i => i.Pagos)
                .HasForeignKey(p => p.IdInquilino)
                .OnDelete(DeleteBehavior.Restrict);

            // Specify the column type for Monto to avoid precision issues
            modelBuilder.Entity<Pago>()
                .Property(p => p.Monto)
                .HasColumnType("decimal(18,2)");
        }
    }
}