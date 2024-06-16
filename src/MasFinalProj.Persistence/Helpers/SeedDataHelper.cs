using System.Security.Cryptography;
using MasFinalProj.Domain.Models.Common;

namespace MasFinalProj.Persistence.Helpers;

/// <summary>
/// Klasa pomocnicza do seedowania danych
/// </summary>
public static class SeedDataHelper
{
    /// <summary>
    /// Zwraca listę obiektów Image do seedowania
    /// </summary>
    /// <returns></returns>
    public static List<Image> GetSeedImages()
    {
        return new List<Image>()
        {
            new Image()
            {
                ImageName = "Test Image",
                ImageFormat = "png",
                Checksum = "Test Checksum"
            },
            new Image()
            {
                ImageName = "Test Image 2",
                ImageFormat = "jpg",
            },
        };
    }
    
    
    /// <summary>
    /// Konwertuje obraz na base64
    /// </summary>
    /// <param name="path">
    /// Ścieżka do obrazu
    /// </param>
    /// <returns>
    /// Obraz w formacie base64 i jego checksum
    /// </returns>
    public static (string base64Img, string checksum) ImageToBase64(string path)
    {
        byte[] imageArray = System.IO.File.ReadAllBytes(path);
        return ImageToBase64(imageArray);
    }
    
    
    /// <summary>
    /// Konwertuje obraz na base64
    /// </summary>
    /// <param name="imageArray">
    /// Tablica bajtów obrazu
    /// </param>
    /// <returns>
    /// Obraz w formacie base64 i jego checksum
    /// </returns>
    public static  (string base64Img, string checksum) ImageToBase64(byte[] imageArray)
    {
        var base64Image = Convert.ToBase64String(imageArray);
        var checksum = GetChecksum(imageArray);
        return (base64Image, checksum);
    }

    /// <summary>
    /// Oblicza checksum obrazu
    /// </summary>
    /// <param name="imageArray">
    /// Tablica bajtów obrazu
    /// </param>
    /// <returns>
    /// Checksum obrazu
    /// </returns>
    private static string GetChecksum(byte[] imageArray)
    {
        using var md5 = MD5.Create();
        byte[] hash = md5.ComputeHash(imageArray);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }
}