/* @2024 Cyprian Gburek.
 * Proszę nie kopiować kodu bez zgody autora.
 * Kod jest własnością uczelni (Polsko-Japońska Akademia Technik Komputerowych, PJATK),
 * i jest udostępniany wyłącznie w celach edukacyjnych.
 *
 * Wykorzystanie kodu we własnych projektach na zajęciach jest zabronione, a jego wykorzystanie
 * może skutkować oznanieniem projektu jako plagiat.
 */
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