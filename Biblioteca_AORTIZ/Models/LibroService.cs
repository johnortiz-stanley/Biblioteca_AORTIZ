using System;
using System.Collections.Generic;
using System.Linq;
using Biblioteca_AORTIZ.Datos;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca_AORTIZ.Models
{
    public class LibroService
    {
        private readonly BibliotecaDbContext _db;

        public LibroService()
        {
            // BibliotecaDbContext configura la conexión en OnConfiguring
            _db = new BibliotecaDbContext();
        }

        public void Crear(Libro libro)
        {
            _db.Libros.Add(libro);
            _db.SaveChanges();
        }

        public List<Libro> ObtenerTodos()
        {
            return _db.Libros.Include(l => l.Resenas).ToList();
        }

        public Libro? BuscarPorIsbn(string isbn)
        {
            return _db.Libros.Include(l => l.Resenas).FirstOrDefault(l => l.Isbn == isbn);
        }

        public bool Actualizar(string isbn, string titulo, string autor, decimal precio, bool disponible)
        {
            var libro = BuscarPorIsbn(isbn);
            if (libro == null) return false;
            if (!string.IsNullOrEmpty(titulo)) libro.Titulo = titulo;
            if (!string.IsNullOrEmpty(autor)) libro.Autor = autor;
            libro.Precio = precio;
            libro.Disponible = disponible;
            _db.SaveChanges();
            return true;
        }

        public bool Eliminar(string isbn)
        {
            var libro = BuscarPorIsbn(isbn);
            if (libro == null) return false;
            _db.Libros.Remove(libro);
            _db.SaveChanges();
            return true;
        }

        public List<Resena> ObtenerResenas(int libroId)
        {
            return _db.Set<Resena>().Where(r => r.LibroId == libroId).OrderByDescending(r => r.Fecha).ToList();
        }

        public void AgregarResena(int libroId, string autor, string texto)
        {
            var res = new Resena { LibroId = libroId, Autor = autor, Texto = texto, Fecha = DateTime.UtcNow };
            _db.Set<Resena>().Add(res);
            _db.SaveChanges();
        }
    }
}





