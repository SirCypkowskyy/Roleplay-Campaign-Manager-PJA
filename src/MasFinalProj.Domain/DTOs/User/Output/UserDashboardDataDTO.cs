namespace MasFinalProj.Domain.DTOs.User.Output;

/// <summary>
/// DTO z danymi do wyświetlenia na prywatnym dashboardzie użytkownika.
/// </summary>
public class UserDashboardDataDTO
{
    /// <summary>
    /// Liczba aktywnych kampanii w sumie, w których użytkownik uczestniczy.
    /// </summary>
    public required int NumberOfActiveCampaignsTotal { get; set; }
    /// <summary>
    /// Liczba aktywnych kampanii w ciągu ostatnich 30 dni, w których użytkownik uczestniczy.
    /// </summary>
    public required int NumberOfActiveCampaignsInLast30Days { get; set; }
    /// <summary>
    /// Liczba wysłanych wiadomości w sumie przez użytkownika.
    /// </summary>
    public required int MessagesSentTotal { get; set; }
    /// <summary>
    /// Liczba wysłanych wiadomości w ciągu ostatnich 30 dni przez użytkownika.
    /// </summary>
    public required int MessagesSentInLast30Days { get; set; }
    /// <summary>
    /// Liczba postaci stworzonych przez użytkownika w sumie.
    /// </summary>
    public required int CreatedCharactersTotal { get; set; }
    /// <summary>
    /// Lista aktywnych kampanii, w których użytkownik uczestniczy.
    /// </summary>
    public required DashboardCampaignResponseDTO[] ActiveCampaigns { get; set; }
}