using Domain.Entities;
using PersistanceLayer.DBContext;

namespace Persistance.Seeds
{
    public static class PrestamoSeed
    {
        public static async Task SeedAsync(API_LibraryContext context)
        {
            if (context.Prestamos.Any())
                return;

            var prestamos = new List<Prestamos>
        {
            new()
            {

                Libro_id = 1,
                Fecha_prestamo = DateTime.UtcNow.AddDays(-15),
                Fecha_devolucion = DateTime.UtcNow.AddDays(-5)
            },
            new()
            {

                Libro_id = 2,
                Fecha_prestamo = DateTime.UtcNow.AddDays(-10),
                Fecha_devolucion = null
            },
            new()
            {

                Libro_id = 3,
                Fecha_prestamo = DateTime.UtcNow.AddDays(-8),
                Fecha_devolucion = DateTime.UtcNow.AddDays(-2)
            },
            new()
            {

                Libro_id = 4,
                Fecha_prestamo = DateTime.UtcNow.AddDays(-4),
                Fecha_devolucion = null
            },
            new()
            {

                Libro_id = 5,
                Fecha_prestamo = DateTime.UtcNow.AddDays(-20),
                Fecha_devolucion = DateTime.UtcNow.AddDays(-12)
            }
        };

            await context.Prestamos.AddRangeAsync(prestamos);
            await context.SaveChangesAsync();
        }
    }
}
