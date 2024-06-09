namespace MasFinalProj.Domain.Abstractions.Models;

/// <summary>
/// Encja bazowa dla wszystkich encji w systemie
/// </summary>
/// <typeparam name="TKey">
/// Typ klucza głównego
/// </typeparam>
public abstract class BaseEntity<TKey> : IEntity<TKey>
{
    /// <inheritdoc />
    public TKey Id { get; init; }
}