using System.Linq.Expressions;

namespace AdminDepartamentos.Domain.Core.Interfaces;

/// <summary>
///     Interfaz genérica para el repositorio base que define operaciones comunes de acceso a datos.
/// </summary>
/// <typeparam name="TEntity">Tipo de la entidad que representa en la base de datos.</typeparam>
public interface IBaseRepository<TEntity> where TEntity : class
{
    /// <summary>
    ///     Obtiene una entidad específica a partir de su Id.
    /// </summary>
    /// <param name="id">Requiere el Id de la entidad</param>
    /// <returns>Una tarea que representa la operación asíncrona, con la entidad encontrada como resultado.</returns>
    Task<TEntity> GetById(int id);

    /// <summary>
    ///     Actualiza una entidad existente en la base de datos.
    /// </summary>
    /// <param name="entity">Instancia de la entidad que contiene los datos actualizados.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    Task Update(TEntity entity);

    /// <summary>
    ///     Actualiza múltiples entidades en la base de datos.
    /// </summary>
    /// <param name="entities">Lista de entidades con los datos actualizados.</param>
    /// <returns>Una tarea que representa la operación asíncrona.</returns>
    Task Update(List<TEntity> entities);

    /// <summary>
    ///     Verifica si existe al menos una entidad que cumpla con el filtro especificado.
    /// </summary>
    /// <param name="filter">Expresión lambda que define la condición de filtrado.</param>
    /// <returns>Una tarea que representa la operación asíncrona, con un valor booleano que indica si la entidad existe.</returns>
    Task<bool> Exists(Expression<Func<TEntity, bool>> filter);
}