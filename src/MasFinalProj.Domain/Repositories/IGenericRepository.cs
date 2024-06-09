using System.Linq.Expressions;
using MasFinalProj.Domain.Abstractions.Models;

namespace MasFinalProj.Domain.Repositories;

/// <summary>
/// Generyczny interfejs repozytorium
/// </summary>
/// <typeparam name="TKey">
/// Typ klucza głównego
/// </typeparam>
/// <typeparam name="TEntity">
/// Typ encji
/// </typeparam>
public interface IGenericRepository<TKey, TEntity> where TEntity : BaseEntity<TKey>
{
    /// <summary>
    /// Pobiera wszystkie encje
    /// </summary>
    /// <returns>
    /// Lista encji
    /// </returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// Pobiera encję po kluczu głównym
    /// </summary>
    /// <param name="id">
    /// Klucz główny
    /// </param>
    /// <param name="asNoTracking">
    /// Czy nie śledzić zmian
    /// </param>
    /// <returns>
    /// Encja
    /// </returns>
    Task<TEntity?> GetByIdAsync(TKey id, bool asNoTracking = false);
    
    /// <summary>
    /// Pobiera encję po predykacie
    /// </summary>
    /// <param name="predicate">
    /// Predykat
    /// </param>
    /// <param name="asNoTracking">
    /// Czy nie śledzić zmian
    /// </param>
    /// <returns></returns>
    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, bool asNoTracking = false);

    /// <summary>
    /// Dodaje encję
    /// </summary>
    /// <param name="entity">
    /// Encja
    /// </param>
    /// <returns>
    /// Encja
    /// </returns>
    Task<TEntity> AddAsync(TEntity entity);

    /// <summary>
    /// Aktualizuje encję
    /// </summary>
    /// <param name="entity">
    /// Encja
    /// </param>
    /// <returns>
    /// Encja
    /// </returns>
    Task<TEntity> UpdateAsync(TEntity entity);

    /// <summary>
    /// Usuwa encję
    /// </summary>
    /// <param name="entity">
    /// Encja
    /// </param>
    Task DeleteAsync(TEntity entity);
    
    /// <summary>
    /// Usuwa encję po kluczu głównym
    /// </summary>
    /// <param name="id">
    /// Klucz główny
    /// </param>
    /// <returns>
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Rzucane, gdy encja nie istnieje
    /// </exception>
    Task DeleteByIdAsync(TKey id);
}