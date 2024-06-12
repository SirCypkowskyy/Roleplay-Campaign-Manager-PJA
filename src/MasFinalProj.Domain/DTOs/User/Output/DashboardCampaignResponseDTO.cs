namespace MasFinalProj.Domain.DTOs.User.Output;

/// <summary>
///     DTO z danymi do wyświetlenia na dashboardzie kampanii.
/// </summary>
public class DashboardCampaignResponseDTO
{
    public required string CampaignGuid { get; set; }
    public required string CampaignHost { get; set; }
    public required string CampaignHostGuid { get; set; }
    public required string CampaignName { get; set; }
    public required DateOnly LastMessageDate { get; set; }
}