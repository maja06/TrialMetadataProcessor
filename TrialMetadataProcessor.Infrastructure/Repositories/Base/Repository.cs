using Microsoft.EntityFrameworkCore;
using TrialMetadataProcessor.Domain.Entities.Base;
using TrialMetadataProcessor.Domain.Interfaces.Repositories.Base;
using TrialMetadataProcessor.Infrastructure.Data.Context;

namespace TrialMetadataProcessor.Infrastructure.Repositories.Base
{
    public class Repository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public virtual async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TPrimaryKey id)
        {
            var entity = await GetAsync(id);
            if (entity != null)
            {
                await DeleteAsync(entity);
            }
        }

        public virtual IQueryable<TEntity> Query()
        {
            return (_dbSet.ToList()).AsQueryable();
        }
    }
}
