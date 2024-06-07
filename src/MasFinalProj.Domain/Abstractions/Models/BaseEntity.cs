namespace MasFinalProj.Domain.Abstractions.Models;

/// <summary>
///     Encja bazowa dla wszystkich encji w systemie
/// </summary>
/// <typeparam name="TKey">
///     Typ klucza głównego
/// </typeparam>
public abstract class BaseEntity<TKey> : IEntity<TKey>, IAuditableEntity where TKey : struct
{
    /// <inheritdoc />
    public string CreatedBy { get; set; }

    /// <inheritdoc />
    public string? ModifiedBy { get; set; }

    /// <inheritdoc />
    public DateTime CreatedAtUtc { get; set; }

    /// <inheritdoc />
    public DateTime? ModifiedAtUtc { get; set; }

    /// <inheritdoc />
    public TKey Id { get; init; }
}