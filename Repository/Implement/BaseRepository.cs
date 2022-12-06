using JwtApplication.Data;
using JwtApplication.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JwtApplication.Repository.Implement
{
    public class BaseRepository<T, ID> : IBaseRepository<T, ID> where T : class
    {
        private readonly DatabaseContext _context;
        protected DbSet<T> _dbSet;

        protected BaseRepository(DatabaseContext context)
        {
            this._context = context;
            this._dbSet = context.Set<T>();
        }

        public virtual async Task<bool> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            return true;
        }
        public virtual async Task<bool> AddRange(List<T> entityList)
        {
            await _dbSet.AddRangeAsync(entityList);
            return true;
        }
        public virtual Task<bool> Delete(T entity)
        {
            _dbSet.Remove(entity);
            return Task.FromResult(true);
        }


        public virtual async Task<IEnumerable<T>> FindAll()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public virtual async Task<T> FindById(ID entityId)
        {
            T? entity = await _dbSet.FindAsync(entityId);
            return entity;

        }

        public virtual async Task<bool> IsExists(ID entityId)
        {
            T? entity = await _dbSet.FindAsync(entityId);
            if (entity != null)
            {
                return true;
            }
            return false;
        }

        public virtual Task<bool> Update(T entity)
        {

            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                return Task.FromResult(true);
            }
            catch (Exception)
            {

                throw;
            }


        }



    }
}
