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