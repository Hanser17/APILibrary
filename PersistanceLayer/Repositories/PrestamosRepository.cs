using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PersistanceLayer.DBContext;

namespace Persistance.Repositories
{
    public class PrestamosRepository : GenericRepository<Prestamos>, IPrestamosRepository
    {
        private readonly API_LibraryContext _API_LibraryContext;
        public PrestamosRepository(API_LibraryContext API_LibraryContext) : base(API_LibraryContext)
        {
            _API_LibraryContext = API_LibraryContext;
        }

        public async Task<List<Prestamos>> GetPrestamosPendientesAsync()
        {
            return await _API_LibraryContext.Prestamos
            .Include(p => p.Libro)
            .ThenInclude(l=>l.Autor)
            .Where(p => p.Fecha_devolucion == null)
            .ToListAsync();
        }
    }
}
