using AdminDepartamentos.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartament.Infrastructure.Context
{
    public partial class DepartContext : IdentityDbContext<IdentityUser>
    {
        public DepartContext(DbContextOptions<DepartContext> options) : base(options) { }

        #region "Entities"
        
        public DbSet<Inquilino> Inquilinos { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Inquilino>()
                .HasMany(i => i.Pagos)
                .WithOne(p => p.Inquilino)
                .HasForeignKey(p => p.IdInquilino);
            
            base.OnModelCreating(builder);
        }
    }
}