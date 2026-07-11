using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PersistanceLayer.DBContext;

namespace Persistance.Repositories
{
    public class LibrosRepository : GenericRepository<Libros>, ILibrosRepository
    {
        private readonly API_LibraryContext _API_LibraryContext;
        public LibrosRepository(API_LibraryContext API_LibraryContext) : base(API_LibraryContext)
        {
            _API_LibraryContext = API_LibraryContext;
        }

        public async Task<List<Libros>> GetLibrosPostedbefore2000()
        {
            var result = await _API_LibraryContext.Libros
                .Where(x => x.Año_publicacion < 2000).ToListAsync();

            return result;
        }
    }
}
