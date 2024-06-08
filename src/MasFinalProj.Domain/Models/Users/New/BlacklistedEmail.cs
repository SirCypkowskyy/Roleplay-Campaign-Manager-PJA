using System.ComponentModel.DataAnnotations;
using MasFinalProj.Domain.Abstractions.Models;

namespace MasFinalProj.Domain.Models.Users.New;

/// <summary>
/// Encja zablokowanego adresu email przy rejestracji
/// </summary>
public class BlacklistedEmail : BaseEntity<Guid>
{
    /// <summary>
    /// Adres email
    /// </summary>
    [EmailAddress]
    public string Email { get; set; }
}