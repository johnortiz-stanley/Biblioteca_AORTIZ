using System;
using System.Collections.Generic;

namespace Biblioteca_AORTIZ.Models
{
    public class Libro
    {
        public int Id { get; set; }
        public string Isbn { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public int AnioPublicacion { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public bool Disponible { get; set; } = true;

        public List<Resena> Resenas { get; set; } = new List<Resena>();

        public Libro() { }

        public Libro(string isbn, string titulo, string autor, int anioPublicacion, string categoria, decimal precio)
        {
            Isbn = isbn;
            Titulo = titulo;
            Autor = autor;
            AnioPublicacion = anioPublicacion;
            Categoria = categoria;
            Precio = precio;
            Disponible = true;
        }

        public void Imprimir()
        {
            Console.WriteLine($"ID: {Id}");
            Console.WriteLine($"ISBN: {Isbn}");
            Console.WriteLine($"Título: {Titulo}");
            Console.WriteLine($"Autor: {Autor}");
            Console.WriteLine($"Año de Publicación: {AnioPublicacion}");
            Console.WriteLine($"Categoría: {Categoria}");
            Console.WriteLine($"Precio: ${Precio:F2}");
            Console.WriteLine($"Disponible: {(Disponible ? "Sí" : "No")}");
        }
    }
}