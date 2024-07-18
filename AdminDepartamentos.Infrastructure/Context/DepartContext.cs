using AdminDepartamentos.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
            modelBuilder.Entity<Inquilino>()
                .HasMany(i => i.Pagos)
                .WithOne(p => p.Inquilino)
                .HasForeignKey(p => p.IdInquilino);
            
            base.OnModelCreating(modelBuilder);
        }
    }
}