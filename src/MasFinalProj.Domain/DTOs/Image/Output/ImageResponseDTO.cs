namespace MasFinalProj.Domain.DTOs.Image.Output;

/// <summary>
/// DTO odpowiedzi z obrazkiem
/// </summary>
public class ImageResponseDTO
{
    /// <summary>
    /// Nazwa zdjęcia
    /// </summary>
    public string ImageName { get; set; }

    /// <summary>
    /// Format zdjęcia
    /// </summary>
    public string ImageFormat { get; set; }

    /// <summary>
    /// Zdjęcie w formacie Base64
    /// </summary>
    public string Base64Image { get; set; }
    
    /// <summary>
    /// Suma kontrolna zdjęcia (MD5)
    /// </summary>
    public string Checksum { get; set; }
    
    /// <summary>
    /// Konstruktor DTO odpowiedzi z obrazkiem
    /// </summary>
    public ImageResponseDTO() {}
    
    /// <summary>
    /// Konstruktor DTO odpowiedzi z obrazkiem
    /// </summary>
    /// <param name="image">
    /// Obrazek
    /// </param>
    public ImageResponseDTO(Models.Common.Image image)
    {
        ImageName = image.ImageName;
        ImageFormat = image.ImageFormat;
        Base64Image = image.Base64Image;
        Checksum = image.Checksum;
    }
}