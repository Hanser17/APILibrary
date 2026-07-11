using Domain.Entities;
using Domain.Interfaces;
using PersistanceLayer.DBContext;

namespace Persistance.Repositories
{
    public class AutoresRepository : GenericRepository<Autores>, IAutoresRepository
    {
        public AutoresRepository(API_LibraryContext API_LibraryContext) : base(API_LibraryContext)
        {
        }
    }
}
