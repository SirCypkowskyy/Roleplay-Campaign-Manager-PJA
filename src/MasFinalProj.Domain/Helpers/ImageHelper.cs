/* @2024 Cyprian Gburek.
 * Proszę nie kopiować kodu bez zgody autora.
 * Kod jest własnością uczelni (Polsko-Japońska Akademia Technik Komputerowych, PJATK),
 * i jest udostępniany wyłącznie w celach edukacyjnych.
 *
 * Wykorzystanie kodu we własnych projektach na zajęciach jest zabronione, a jego wykorzystanie
 * może skutkować oznanieniem projektu jako plagiat.
 */
using System.Security.Cryptography;

namespace MasFinalProj.Domain.Helpers;

                                                /// <summary>
                                                /// Klasa pomocnicza do obsługi obrazów
                                                /// </summary>
                                                public static class ImageHelper
                                                {
                                                    /// <summary>
                                                    /// Metoda do konwertowania obrazu na base64 i zwrócenia go razem z sumą kontrolną (w formie base64)
                                                    /// </summary>
                                                    /// <param name="image">Obraz</param>
                                                    /// <returns>Obraz w formie base64</returns>
                                                    public static (string base64, string checksum) ImageToBase64(byte[] image)
                                                    {
                                                        var checksum = MD5.HashData(image);
                                                        return (Convert.ToBase64String(image), Convert.ToBase64String(checksum));
                                                    }

                                                    /// <summary>
                                                    /// Metoda do konwertowania base64 na obraz w formie bajtów oraz zwrócenia go razem z sumą kontrolną (w formie base64)
                                                    /// </summary>
                                                    /// <param name="base64">Obraz w formie base64</param>
                                                    /// <returns>Obraz</returns>
                                                    public static (byte[] image, string checksum) Base64ToImage(string base64)
                                                    {
                                                        var image = Convert.FromBase64String(base64);
                                                        var checksum = MD5.HashData(image);
                                                        return (image, Convert.ToBase64String(checksum));
                                                    }
                                                }