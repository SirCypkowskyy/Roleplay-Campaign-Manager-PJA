/* @2024 Cyprian Gburek.
 * Proszę nie kopiować kodu bez zgody autora.
 * Kod jest własnością uczelni (Polsko-Japońska Akademia Technik Komputerowych, PJATK),
 * i jest udostępniany wyłącznie w celach edukacyjnych.
 *
 * Wykorzystanie kodu we własnych projektach na zajęciach jest zabronione, a jego wykorzystanie
 * może skutkować oznanieniem projektu jako plagiat.
 */
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