using Microsoft.EntityFrameworkCore;
using ProjetoTCC.Models;

namespace ProjetoTCC.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options)
        {             
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Medida> Medidas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                .HasMany(u => u.Medidas)
                .WithOne(m => m.Usuario)
                .HasForeignKey(m => m.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
