namespace MasFinalProj.Domain.Abstractions.Models;

/// <summary>
/// Interfejs dla encji, która ma być śledzona przez system audytowy.
/// </summary>
public interface IAuditableEntity
{
    /// <summary>
    /// Data utworzenia encji.
    /// </summary>
    DateTime CreatedAtUtc { get; set; }

    /// <summary>
    /// Ostatnia modyfikacja encji.
    /// </summary>
    /// <remarks>
    /// Wartość <c>null</c> oznacza, że encja nie była modyfikowana.
    /// </remarks>
    DateTime? ModifiedAtUtc { get; set; }
}