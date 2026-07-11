using Domain.Entities;
using PersistanceLayer.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Seeds
{
    public static class AutorSeed
    {
        public static async Task SeedAsync(API_LibraryContext context)
        {
            if (context.Autores.Any())
                return;

            var autores = new List<Autores>
        {
            new()
            {
               
                Nombre = "Gabriel García Márquez",
                Nacionalidad = "Colombiana"
            },
            new()
            {
                
                Nombre = "Mario Vargas Llosa",
                Nacionalidad = "Peruana"
            },
            new()
            {
                
                Nombre = "Julio Cortázar",
                Nacionalidad = "Argentina"
            },
            new()
            {
               
                Nombre = "Jorge Luis Borges",
                Nacionalidad = "Argentina"
            },
            new()
            {
                
                Nombre = "Isabel Allende",
                Nacionalidad = "Chilena"
            }
        };

            await context.Autores.AddRangeAsync(autores);
            await context.SaveChangesAsync();
        }
    }
}
