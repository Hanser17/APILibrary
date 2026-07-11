using Domain.Entities;
using PersistanceLayer.DBContext;

namespace Persistance.Seeds
{
    public static class LibroSeed
    {
        public static async Task SeedAsync(API_LibraryContext context)
        {
            if (context.Libros.Any())
                return;

            var libros = new List<Libros>
        {
            new()
            {
               
                Titulo = "Cien años de soledad",
                Autor_id = 1,
                Año_publicacion = 1967,
                Genero = "Realismo mágico"
            },
            new()
            {
                Titulo = "El amor en los tiempos del cólera",
                Autor_id = 1,
                Año_publicacion = 1985,
                Genero = "Novela romántica"
            },
            new()
            {
               
                Titulo = "La ciudad y los perros",
                Autor_id = 2,
                Año_publicacion = 1963,
                Genero = "Novela"
            },
            new()
            {
               
                Titulo = "Rayuela",
                Autor_id = 3,
                Año_publicacion = 1963,
                Genero = "Novela experimental"
            },
            new()
            {
               
                Titulo = "Ficciones",
                Autor_id = 4,
                Año_publicacion = 1944,
                Genero = "Cuentos"
            },
            new()
            {
               
                Titulo = "La casa de los espíritus",
                Autor_id = 5,
                Año_publicacion = 1982,
                Genero = "Realismo mágico"
            }
        };

            await context.Libros.AddRangeAsync(libros);
            await context.SaveChangesAsync();
        }
    }
}
