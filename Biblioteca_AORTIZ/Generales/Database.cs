using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Biblioteca_AORTIZ.Models;

namespace Biblioteca_AORTIZ.Generales
{
    public static class Database
    {
        private static readonly string rutaCarpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Datos");
        private static readonly string rutaArchivoLibros = Path.Combine(rutaCarpeta, "libros.json");

        public static List<Libro> Libros { get; private set; } = new List<Libro>();

        public static void CargarDatos()
        {
            if (!Directory.Exists(rutaCarpeta))
            {
                Directory.CreateDirectory(rutaCarpeta);
            }

            if (!File.Exists(rutaArchivoLibros))
            {
                Libros = new List<Libro>();
                return;
            }

            var json = File.ReadAllText(rutaArchivoLibros);
            Libros = string.IsNullOrWhiteSpace(json)
                ? new List<Libro>()
                : JsonSerializer.Deserialize<List<Libro>>(json) ?? new List<Libro>();
        }

        public static void GuardarDatos()
        {
            if (!Directory.Exists(rutaCarpeta))
            {
                Directory.CreateDirectory(rutaCarpeta);
            }

            var json = JsonSerializer.Serialize(Libros, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(rutaArchivoLibros, json);
        }
    }
}
