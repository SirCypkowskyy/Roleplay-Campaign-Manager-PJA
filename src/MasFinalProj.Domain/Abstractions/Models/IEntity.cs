namespace MasFinalProj.Domain.Abstractions.Models;

/// <summary>
///     Interfejs reprezentujący encję
/// </summary>
internal interface IEntity<TKey>
{
    /// <summary>
    ///     Identyfikator encji
    /// </summary>
    TKey Id { get; init; }
}