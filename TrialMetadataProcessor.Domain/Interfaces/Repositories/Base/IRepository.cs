using TrialMetadataProcessor.Domain.Entities.Base;

namespace TrialMetadataProcessor.Domain.Interfaces.Repositories.Base
{
    public interface IRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>
    {
        Task<TEntity> GetAsync(TPrimaryKey id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task DeleteAsync(TPrimaryKey id);
        IQueryable<TEntity> Query();
    }
}
