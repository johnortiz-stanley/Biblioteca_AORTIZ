using System;

namespace Biblioteca_AORTIZ.Models
{
    public class Resena
    {
        public int Id { get; set; }
        public int LibroId { get; set; }
        public string Autor { get; set; } = string.Empty;
        public string Texto { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        // Navegación
        public Libro? Libro { get; set; }
    }
}
