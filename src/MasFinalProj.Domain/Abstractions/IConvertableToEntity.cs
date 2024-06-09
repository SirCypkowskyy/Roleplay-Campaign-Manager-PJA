namespace MasFinalProj.Domain.Abstractions;

/// <summary>
/// Interfejs do konwertowania DTO na encję
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public interface IConvertableToEntity<TEntity>
    where TEntity : class
{
    /// <summary>
    /// Konwertuje DTO na encję
    /// </summary>
    /// <returns>
    /// Encja
    /// </returns>
    TEntity ToEntity();
}