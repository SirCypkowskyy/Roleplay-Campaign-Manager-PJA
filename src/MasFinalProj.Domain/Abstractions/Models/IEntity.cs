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
/// Interfejs reprezentujący encję
/// </summary>
internal interface IEntity<TKey>
{
    /// <summary>
    /// Identyfikator encji
    /// </summary>
    TKey Id { get; init; }
}