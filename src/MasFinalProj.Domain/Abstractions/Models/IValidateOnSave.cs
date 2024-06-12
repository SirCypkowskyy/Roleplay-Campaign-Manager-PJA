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