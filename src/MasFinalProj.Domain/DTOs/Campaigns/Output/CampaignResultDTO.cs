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
    /// Opis kampanii
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Id zdjęcia kampanii
    /// </summary>
    public long? ImageId { get; set; }
    
    /// <summary>
    /// Nazwa kampanii
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Użytkownicy przypisani do kampanii
    /// </summary>
    public List<UserResponseDTO> Users { get; set; }
    
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
        Description = campaign.Description;
        ImageId = campaign.CampaignImageId;
        var campaignUsers = campaign.CampaignPlayers.Select(u => u.User).Select(UserResponseDTO.FromUser).ToList();
        if(!campaign.CampaignGameMaster.Any())
            throw new Exception($"Campaign {campaign.Id} has no game master");
        var campaignGameMaster = UserResponseDTO.FromUser(campaign.CampaignGameMaster.First().User);
        Users = new List<UserResponseDTO> { campaignGameMaster };
        Users.AddRange(campaignUsers);
    }
}