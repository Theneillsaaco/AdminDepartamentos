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
        _context = context;
        _entities = _context.Set<TEntity>();
    }

    #endregion
    
    public virtual async Task<TEntity> GetById(int id)
    {
        return await _entities.FindAsync(id);
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