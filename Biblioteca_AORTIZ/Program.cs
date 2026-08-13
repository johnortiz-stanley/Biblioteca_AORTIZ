using Biblioteca_AORTIZ.Models;
using Biblioteca_AORTIZ.Datos;
using Microsoft.EntityFrameworkCore;
using System;

class Program
{
    private static readonly LibroService _service = new LibroService();

    static void Main()
    {
        // Asegurar la existencia de tablas adicionales en la BD
        EnsureExtraTables();

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
            Console.WriteLine("6.- Gestionar Reseñas");
            Console.WriteLine("7.- Gestionar LibroMenu (guardar entradas)");
            Console.WriteLine("8.- Registrar acción en LibroService (log)");
            Console.WriteLine("9.- Salir");
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
                case 6: GestionarResenas(); break;
                case 7: GestionarLibroMenu(); break;
                case 8: RegistrarAccionService(); break;
                case 9: Console.WriteLine("Saliendo..."); break;
                default: Console.WriteLine("Opción inválida."); Console.ReadLine(); break;
            }
        } while (opcion != 9);
    }

    static void EnsureExtraTables()
    {
        try
        {
            using var db = new BibliotecaDbContext();
            // Crear tabla LibroMenu si no existe
            var sqlCreateMenu = @"
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LibroMenu]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[LibroMenu](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [LibroId] INT NULL,
        [Tipo] NVARCHAR(100) NULL,
        [Data] NVARCHAR(MAX) NULL,
        [CreatedAt] DATETIME2 NOT NULL
    );
END";
            db.Database.ExecuteSqlRaw(sqlCreateMenu);

            // Crear tabla LibroService si no existe
            var sqlCreateService = @"
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LibroService]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[LibroService](
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [Accion] NVARCHAR(100) NULL,
        [Detalle] NVARCHAR(MAX) NULL,
        [Fecha] DATETIME2 NOT NULL
    );
END";
            db.Database.ExecuteSqlRaw(sqlCreateService);
        }
        catch (Exception ex)
        {
            Console.WriteLine("No se pudieron crear/asegurar las tablas adicionales: " + ex.Message);
            Console.WriteLine("Presione Enter para continuar...");
            Console.ReadLine();
        }
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
        Console.WriteLine("Presiona Enter para volver al menú...");
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

    static void GestionarResenas()
    {
        Console.Clear();
        Console.WriteLine("********** Gestionar Reseñas **********");
        Console.Write("Ingrese ISBN del libro para añadir reseña: ");
        var isbn = Console.ReadLine()!;
        var libro = _service.BuscarPorIsbn(isbn);
        if (libro == null)
        {
            Console.WriteLine("Libro no encontrado.");
            Console.ReadLine();
            return;
        }

        Console.Write("Autor de la reseña: "); var autor = Console.ReadLine() ?? "Anónimo";
        Console.Write("Texto de la reseña: "); var texto = Console.ReadLine() ?? string.Empty;

        _service.AgregarResena(libro.Id, autor, texto);
        Console.WriteLine("Reseña agregada correctamente.");
        Console.ReadLine();
    }

    static void GestionarLibroMenu()
    {
        Console.Clear();
        Console.WriteLine("********** Gestionar LibroMenu **********");
        Console.WriteLine("1. Añadir entrada en LibroMenu");
        Console.WriteLine("2. Listar entradas LibroMenu");
        Console.Write("Opción: ");
        if (!int.TryParse(Console.ReadLine(), out var opc)) opc = 0;

        using var db = new BibliotecaDbContext();
        if (opc == 1)
        {
            Console.Write("Id de libro (Enter para NULL): ");
            var libroIdInput = Console.ReadLine();
            int? libroId = int.TryParse(libroIdInput, out var lid) ? lid : (int?)null;
            Console.Write("Tipo (ej. Simbologia, Ficha): "); var tipo = Console.ReadLine() ?? string.Empty;
            Console.Write("Data (texto o JSON): "); var data = Console.ReadLine() ?? string.Empty;
            var created = DateTime.UtcNow;

            db.Database.ExecuteSqlInterpolated($@"
            INSERT INTO [dbo].[LibroMenu] (LibroId, Tipo, Data, CreatedAt)
            VALUES ({libroId}, {tipo}, {data}, {created});");
            Console.WriteLine("Entrada LibroMenu creada.");
            Console.ReadLine();
        }
        else if (opc == 2)
        {
            try
            {
                var conn = db.Database.GetDbConnection();
                if (conn.State != System.Data.ConnectionState.Open) conn.Open();

                using var cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT Id, LibroId, Tipo, Data, CreatedAt FROM [dbo].[LibroMenu]";

                using var reader = cmd.ExecuteReader();
                Console.WriteLine("Entradas LibroMenu:");
                while (reader.Read())
                {
                    var id = reader.GetInt32(0);
                    var lidStr = reader.IsDBNull(1) ? "NULL" : reader.GetInt32(1).ToString();
                    var tipo = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    var data = reader.IsDBNull(3) ? "" : reader.GetString(3);
                    var created = reader.IsDBNull(4) ? DateTime.MinValue : reader.GetDateTime(4);
                    Console.WriteLine($"Id:{id} LibroId:{lidStr} Tipo:{tipo} CreatedAt:{created:u}");
                    Console.WriteLine($" Data: {data}");
                    Console.WriteLine("----");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error listando LibroMenu: " + ex.Message);
            }
            Console.WriteLine("Presione Enter para volver...");
            Console.ReadLine();
        }
        else
        {
            Console.WriteLine("Opción inválida.");
            Console.ReadLine();
        }
    }
    static void RegistrarAccionService()
    {
        Console.Clear();
        Console.WriteLine("********** Registrar acción en LibroService **********");
        Console.Write("Acción (ej. Crear, Actualizar, Eliminar): "); var accion = Console.ReadLine() ?? string.Empty;
        Console.Write("Detalle (opcional): "); var detalle = Console.ReadLine() ?? string.Empty;
        var fecha = DateTime.UtcNow;

        using var db = new BibliotecaDbContext();
        db.Database.ExecuteSqlInterpolated($@"
            INSERT INTO [dbo].[LibroService] (Accion, Detalle, Fecha)
            VALUES ({accion}, {detalle}, {fecha});");

        Console.WriteLine("Registro agregado a LibroService.");
        Console.ReadLine();
    }
}


