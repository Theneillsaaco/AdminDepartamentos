using System.Linq.Expressions;
using AdminDepartamentos.Domain.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AdminDepartament.Infrastructure.Core;


public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity: class
{
    #region Context

    private readonly DbContext _context;
    private DbSet<TEntity> _entities;
    
    protected BaseRepository(DbContext context)
    {
        this._context = context;
        this._entities = this._context.Set<TEntity>();
    }

    #endregion
    
    public virtual async Task<List<TEntity>> GetAll(Expression<Func<TEntity, bool>> filter)
    {
        return await _entities.Where(filter).ToListAsync();
    }

    public virtual async Task<TEntity> GetById(int id)
    {
        return await _entities.FindAsync(id);
    }

    public virtual async Task Save(TEntity entity)
    {
        _entities.Add(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task Save(List<TEntity> entities)
    {
        _entities.AddRange(entities);
        await _context.SaveChangesAsync();
    }
    
    public virtual async Task Update(TEntity entity)
    {
        _entities.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<bool> Exists(Expression<Func<TEntity, bool>> filter)
    {
        return await _entities.AnyAsync(filter);
    }
}