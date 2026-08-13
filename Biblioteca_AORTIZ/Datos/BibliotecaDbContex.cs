using Microsoft.EntityFrameworkCore;
using Biblioteca_AORTIZ.Models;

namespace Biblioteca_AORTIZ.Datos
{
    public class BibliotecaDbContext : DbContext
    {
        public DbSet<Libro> Libros { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Ajusta la cadena de conexión según tu servidor o lee desde configuración.
                optionsBuilder.UseSqlServer(@"Server=Diana\SQLEXPRESS;Database=Biblioteca_AOrtiz;User Id=sa;Password=NICOLAS;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Libro>()
                .HasIndex(l => l.Isbn)
                .IsUnique();
        }
    }
}
