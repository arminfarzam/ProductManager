using Microsoft.EntityFrameworkCore;
using ProductManager.Domain.Repositories.Common;
using System.Linq.Expressions;
using ProductManager.Data.Context;
using ProductManager.Domain.Entities.Common;

namespace ProductManager.Data.Repositories.Common;

public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    #region constructor

    private readonly ProductManagerContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(ProductManagerContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    #endregion

    public IQueryable<TEntity> GetEntitiesQueryable()
    {
        return _dbSet.AsQueryable();
    }

    public async Task<IEnumerable<TEntity>> GetByCondition(Expression<Func<TEntity, bool>> query)
    {
        return await _dbSet.Where(query).ToListAsync();
    }

    public async Task<TEntity?> GetEntityByCondition(Expression<Func<TEntity, bool>> query, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        var getQuery = _dbSet.Where(query);
        getQuery = includeProperties.Aggregate(getQuery, (current, includeProperty) => current.Include(includeProperty));
        return await getQuery.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<TEntity>> GetByConditionAsNoTracking(Expression<Func<TEntity, bool>> query)
    {
        return await _dbSet.Where(query).AsNoTracking().ToListAsync();
    }

    public async Task<TEntity?> GetEntityById(Guid entityId)
    {
        return await _dbSet.SingleOrDefaultAsync(e => e.Id == entityId);
    }

    public async Task AddEntity(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task AddRange(IList<TEntity> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public void UpdateEntity(TEntity entity)
    {
        _dbSet.Update(entity);
    }

    public void UpdateRange(IList<TEntity> entities)
    {
        _dbSet.UpdateRange(entities);
    }

    public void RemoveEntity(TEntity entity, bool forceDelete = false)
    {
        if (forceDelete)
        {
            _dbSet.Remove(entity);
        }
        else
        {
            entity.IsDeleted = true;
            UpdateEntity(entity);
        }
    }

    public async Task RemoveEntityById(Guid entityId, bool forceDelete = false)
    {
        var entity = await GetEntityById(entityId);
        if (entity != null)
        {
            if (forceDelete)
            {
                RemoveEntity(entity, true);
            }
            else
            {
                RemoveEntity(entity);
            }
        }
    }

    public async Task RemoveRange(IList<TEntity> entities, bool forceDelete = false)
    {
        if (forceDelete)
        {
            _dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }
        else
        {
            foreach (var entity in entities)
            {
                entity.IsDeleted = true;
            }
            UpdateRange(entities);
            await Task.CompletedTask;
        }
    }

    public async Task RemoveRange(IQueryable<TEntity> entities, bool forceDelete = false)
    {
        if (forceDelete)
        {
            _dbSet.RemoveRange(entities);
            await Task.CompletedTask;
        }
        else
        {
            var entitiesList = await entities.ToListAsync();
            foreach (var entity in entitiesList)
            {
                entity.IsDeleted = true;
            }
            UpdateRange(entitiesList);
            await Task.CompletedTask;
        }
    }

    public async Task SaveChanges()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}