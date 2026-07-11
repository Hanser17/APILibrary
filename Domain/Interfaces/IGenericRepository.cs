

namespace Domain.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity: class
    {
        

        Task<TEntity> GetById(int? id);

        Task<TEntity> Add(TEntity entity);

        Task Update(TEntity entity);
        Task Delete(TEntity entity);

    }
}
