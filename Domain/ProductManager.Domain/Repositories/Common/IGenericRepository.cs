using System.Linq.Expressions;
using ProductManager.Domain.Entities.Common;

namespace ProductManager.Domain.Repositories.Common;

public interface IGenericRepository<TEntity> : IDisposable where TEntity : BaseEntity
{
    IQueryable<TEntity> GetEntitiesQueryable();
    Task<IEnumerable<TEntity>> GetByCondition(Expression<Func<TEntity, bool>> query);
    Task<TEntity?> GetEntityByCondition(Expression<Func<TEntity, bool>> query, params Expression<Func<TEntity, object>>[] includeProperties);
    Task<IEnumerable<TEntity>> GetByConditionAsNoTracking(Expression<Func<TEntity, bool>> query);
    Task<TEntity?> GetEntityById(Guid entityId);
    Task AddEntity(TEntity entity);
    Task AddRange(IList<TEntity> entities);
    void UpdateEntity(TEntity entity);
    void UpdateRange(IList<TEntity> entities);
    void RemoveEntity(TEntity entity, bool forceDelete = false);
    Task RemoveEntityById(Guid entityId, bool forceDelete = false);
    Task RemoveRange(IList<TEntity> entities, bool forceDelete = false);
    Task RemoveRange(IQueryable<TEntity> entities, bool forceDelete = false);
    Task SaveChanges();
}