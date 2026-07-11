using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using PersistanceLayer.DBContext;

namespace Persistance.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly API_LibraryContext _API_LibraryContext;
        public GenericRepository(API_LibraryContext API_LibraryContext)
        {
            _API_LibraryContext = API_LibraryContext;
        }

        public virtual async  Task<TEntity> Add(TEntity entity)
        {
            await _API_LibraryContext.Set<TEntity>().AddAsync(entity);
            await _API_LibraryContext.SaveChangesAsync();
            return entity;
        }

        public virtual async Task Delete(TEntity entity)
        {
            _API_LibraryContext.Set<TEntity>().Remove(entity);
            await _API_LibraryContext.SaveChangesAsync();
        }


        public virtual async Task<TEntity> GetById(int? id)
        {
            return await _API_LibraryContext.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task Update(TEntity entity)
        {
            _API_LibraryContext.Entry(entity).State = EntityState.Modified;
            await _API_LibraryContext.SaveChangesAsync();
        }
    }
}
