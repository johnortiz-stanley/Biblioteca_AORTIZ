using System;

namespace Biblioteca_AORTIZ.Models
{
    public static class LibroMenu
    {
        private static readonly LibroService _service = new LibroService();

        public static void MostrarMenu(Libro libro)
        {
            int opc = 0;
            do
            {
                Console.Clear();
                Console.WriteLine($"--- Menú Libro: {libro.Titulo} ---");
                Console.WriteLine("1. Ficha del libro");
                Console.WriteLine("2. Reseñas");
                Console.WriteLine("3. Simbología");
                Console.WriteLine("4. Volver");
                Console.Write("Opción: ");

                if (!int.TryParse(Console.ReadLine(), out opc)) opc = 0;

                switch (opc)
                {
                    case 1:
                        Console.Clear();
                        libro.Imprimir();
                        Console.WriteLine("Presione Enter para volver...");
                        Console.ReadLine();
                        break;
                    case 2:
                        GestionResenas(libro);
                        break;
                    case 3:
                        MostrarSimbologia();
                        break;
                    case 4:
                        break;
                    default:
                        Console.WriteLine("Opción inválida.");
                        Console.ReadLine();
                        break;
                }
            } while (opc != 4);
        }

        private static void MostrarSimbologia()
        {
            Console.Clear();
            Console.WriteLine("Simbología:");
            Console.WriteLine("- Disponible: ✓ (El libro puede prestarse)");
            Console.WriteLine("- No disponible: ✗ (Actualmente prestado)");
            Console.WriteLine("- Categorías: ejemplo 'Ficción', 'No ficción', 'Infantil', etc.");
            Console.WriteLine("\nPresione Enter para volver...");
            Console.ReadLine();
        }

        private static void GestionResenas(Libro libro)
        {
            int opc = 0;
            do
            {
                Console.Clear();
                Console.WriteLine($"--- Reseñas de: {libro.Titulo} ---");
                var resenas = _service.ObtenerResenas(libro.Id);
                if (resenas.Count == 0) Console.WriteLine("No hay reseñas.");
                else
                {
                    foreach (var r in resenas)
                    {
                        Console.WriteLine($"[{r.Fecha:u}] {r.Autor}: {r.Texto}");
                        Console.WriteLine("---");
                    }
                }

                Console.WriteLine("1. Añadir reseña");
                Console.WriteLine("2. Volver");
                Console.Write("Opción: ");
                if (!int.TryParse(Console.ReadLine(), out opc)) opc = 0;

                if (opc == 1)
                {
                    Console.Write("Tu nombre: "); var autor = Console.ReadLine() ?? "Anónimo";
                    Console.Write("Texto: "); var texto = Console.ReadLine() ?? string.Empty;
                    _service.AgregarResena(libro.Id, autor, texto);
                    // recargar libro reseñas opcional
                }
            } while (opc != 2);
        }
    }
}