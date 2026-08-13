using Biblioteca_AORTIZ.Models;
using System;

class Program
{
    private static readonly LibroService _service = new LibroService();

    static void Main()
    {
        int opcion = 0;
        do
        {
            Console.Clear();
            Console.WriteLine("==================================================");
            Console.WriteLine("        SISTEMA DE GESTIÓN DE BIBLIOTECA          ");
            Console.WriteLine("==================================================");
            Console.WriteLine("1.- Crear Libro");
            Console.WriteLine("2.- Listar Libros");
            Console.WriteLine("3.- Buscar Libro por ISBN");
            Console.WriteLine("4.- Actualizar Libro");
            Console.WriteLine("5.- Eliminar Libro");
            Console.WriteLine("6.- Salir");
            Console.WriteLine("==================================================");
            Console.Write("Ingrese una opción: ");

            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                opcion = 0;
            }

            switch (opcion)
            {
                case 1: Crear(); break;
                case 2: Listar(); break;
                case 3: Buscar(); break;
                case 4: Actualizar(); break;
                case 5: Eliminar(); break;
                case 6: Console.WriteLine("Saliendo..."); break;
                default: Console.WriteLine("Opción inválida."); Console.ReadLine(); break;
            }
        } while (opcion != 6);
    }

    static void Crear()
    {
        Console.Clear();
        Console.WriteLine("********** Crear Libro **********");
        Console.Write("ISBN: "); string isbn = Console.ReadLine()!;
        Console.Write("Título: "); string titulo = Console.ReadLine()!;
        Console.Write("Autor: "); string autor = Console.ReadLine()!;
        Console.Write("Año de Publicación: "); int anio = int.TryParse(Console.ReadLine(), out var a) ? a : 0;
        Console.Write("Categoría: "); string cat = Console.ReadLine()!;
        Console.Write("Precio: "); decimal precio = decimal.TryParse(Console.ReadLine(), out var p) ? p : 0;

        Libro nuevo = new Libro(isbn, titulo, autor, anio, cat, precio);
        _service.Crear(nuevo);

        Console.WriteLine("\n¡Libro registrado exitosamente!");
        Console.ReadLine();
    }

    static void Listar()
    {
        Console.Clear();
        Console.WriteLine("********** Listado de Libros **********");
        var lista = _service.ObtenerTodos();

        if (lista.Count == 0)
        {
            Console.WriteLine("No hay libros registrados.");
        }
        else
        {
            foreach (var libro in lista)
            {
             libro.Imprimir();
             Console.WriteLine("\nPresione Enter para abrir menú del libro...");
	         Console.ReadLine();
	         LibroMenu.MostrarMenu(libro);
             Console.WriteLine("----------------------------------");
            }
        }
        Console.ReadLine();
    }

    static void Buscar()
    {
        Console.Clear();
        Console.WriteLine("********** Buscar Libro **********");
        Console.Write("Ingrese ISBN: "); string isbn = Console.ReadLine()!;

        var libro = _service.BuscarPorIsbn(isbn);
        if (libro != null)
        {
            Console.WriteLine("\nLibro Encontrado:");
            libro.Imprimir();
        }
        else
        {
            Console.WriteLine("\nLibro no encontrado.");
        }
        Console.ReadLine();
    }

    static void Actualizar()
    {
        Console.Clear();
        Console.WriteLine("********** Actualizar Libro **********");
        Console.Write("Ingrese ISBN del libro a actualizar: "); string isbn = Console.ReadLine()!;

        var libro = _service.BuscarPorIsbn(isbn);
        if (libro != null)
        {
            Console.Write($"Nuevo Título ({libro.Titulo}): "); string nTitulo = Console.ReadLine()!;
            Console.Write($"Nuevo Autor ({libro.Autor}): "); string nAutor = Console.ReadLine()!;
            Console.Write($"Nuevo Precio ({libro.Precio}): "); string pStr = Console.ReadLine()!;
            decimal nPrecio = string.IsNullOrEmpty(pStr) ? libro.Precio : decimal.TryParse(pStr, out var np) ? np : libro.Precio;

            Console.Write("¿Está Disponible? (S/N): ");
            bool nDisp = Console.ReadLine()!.ToUpper() == "S";

            _service.Actualizar(isbn, nTitulo, nAutor, nPrecio, nDisp);
            Console.WriteLine("\n¡Libro actualizado correctamente!");
        }
        else
        {
            Console.WriteLine("\nLibro no encontrado.");
        }
        Console.ReadLine();
    }

    static void Eliminar()
    {
        Console.Clear();
        Console.WriteLine("********** Eliminar Libro **********");
        Console.Write("Ingrese ISBN del libro a eliminar: "); string isbn = Console.ReadLine()!;

        var libro = _service.BuscarPorIsbn(isbn);
        if (libro != null)
        {
            libro.Imprimir();
            Console.Write("\n¿Está seguro de eliminar este libro? S/N: ");
            if (Console.ReadLine()!.ToUpper() == "S")
            {
                _service.Eliminar(isbn);
                Console.WriteLine("Libro eliminado.");
            }
        }
        else
        {
            Console.WriteLine("Libro no encontrado.");
        }
        Console.ReadLine();
    }
}

