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
/// Interfejs markerowy dla encji, które wymagają walidacji przed zapisem
/// </summary>
public interface IValidateOnSave
{
    /// <summary>
    /// Metoda walidująca encję przed zapisem
    /// </summary>
    void ValidateBeforeSave();
}