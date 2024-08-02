using System.Linq.Expressions;

namespace AdminDepartamentos.Domain.Core.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task<TEntity> GetById(int id);

    Task Update(TEntity entity);
    
    Task Update(List<TEntity> entity);
    
    Task<bool> Exists(Expression<Func<TEntity, bool>> filter);
}