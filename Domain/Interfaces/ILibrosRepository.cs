using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ILibrosRepository : IGenericRepository<Libros>
    {
        Task<List<Libros>> GetLibrosPostedbefore2000();
    }
}
