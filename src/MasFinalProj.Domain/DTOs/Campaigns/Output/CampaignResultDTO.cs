using MasFinalProj.Domain.DTOs.Image.Output;
using MasFinalProj.Domain.DTOs.User.Output;
using MasFinalProj.Domain.Models.Campaigns;

namespace MasFinalProj.Domain.DTOs.Campaigns.Output;

/// <summary>
/// DTO wyniku kampanii
/// </summary>
public class CampaignResultDTO
{
    /// <summary>
    /// Id kampanii
    /// </summary>
    public string Id { get; set; }
    
    /// <summary>
    /// Nazwa kampanii
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Użytkownicy przypisani do kampanii
    /// </summary>
    public List<UserResponseDTO> Users { get; set; }
    
    /// <summary>
    /// Zdjęcie kampanii
    /// </summary>
    public ImageResponseDTO? CampaignImage { get; set; }

    /// <summary>
    /// Konstruktor DTO wyniku kampanii
    /// </summary>
    public CampaignResultDTO() {}
    
    /// <summary>
    /// Konstruktor DTO wyniku kampanii
    /// </summary>
    /// <param name="campaign">
    /// Kampania
    /// </param>
    public CampaignResultDTO(Campaign campaign)
    {
        Id = campaign.Id.ToString();
        Name = campaign.Name;
        var campaignUsers = campaign.CampaignPlayers.Select(u => u.User).Select(UserResponseDTO.FromUser).ToList();
        var campaignGameMaster = UserResponseDTO.FromUser(campaign.CampaignGameMaster.First().User);
        Users = new List<UserResponseDTO> { campaignGameMaster };
        Users.AddRange(campaignUsers);
        CampaignImage = campaign.CampaignImage is null ? null : new ImageResponseDTO(campaign.CampaignImage);
    }
}