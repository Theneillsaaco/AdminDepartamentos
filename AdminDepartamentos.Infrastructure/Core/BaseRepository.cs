using System.Linq.Expressions;
using AdminDepartamentos.Infrastucture.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartamentos.Infrastucture.Core;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    public virtual async Task<TEntity> GetById(int id)
    {
        return await _entities.FindAsync(id);
    }

    public virtual async Task Update(TEntity entity)
    {
        _entities.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Update(List<TEntity> entities)
    {
        _entities.UpdateRange(entities);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<bool> Exists(Expression<Func<TEntity, bool>> filter)
    {
        return await _entities.AnyAsync(filter);
    }

    #region Context

    private readonly DbContext _context;
    private readonly DbSet<TEntity> _entities;

    protected BaseRepository(DbContext context)
    {
        _context = context;
        _entities = _context.Set<TEntity>();
    }

    #endregion
}