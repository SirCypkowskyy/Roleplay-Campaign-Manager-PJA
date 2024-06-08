using System.ComponentModel.DataAnnotations;
using MasFinalProj.Domain.Abstractions.Models;
using MasFinalProj.Domain.Models.Campaigns.Characters;
using MasFinalProj.Domain.Models.Campaigns.Users;

namespace MasFinalProj.Domain.Models.Campaigns;

/// <summary>
/// Notatka kampanii
/// </summary>
public class Note : BaseEntity<long>
{
    /// <summary>
    /// Nazwa notatki
    /// </summary>
    [MaxLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// Opis notatki
    /// </summary>
    [MaxLength(2000)]
    public string Description { get; set; }

    /// <summary>
    /// Użytkownik kampanii
    /// </summary>
    public CampaignUser CampaignUser { get; set; }

    /// <summary>
    /// Id użytkownika kampanii
    /// </summary>
    public long CampaignUserId { get; set; }

    /// <summary>
    /// Postać, która jest powiązana z notatką
    /// </summary>
    /// <remarks>
    /// Notatka może nie być powiązana z postacią (jeśli dotyczy przedmiotu, XOR z <see cref="ItemId"/>)
    /// </remarks>
    public Character? Character { get; set; }

    /// <summary>
    /// Id postaci, która jest powiązana z notatką
    /// </summary>
    private long? _characterId;
    
    /// <summary>
    /// Id postaci, która jest powiązana z notatką
    /// </summary>
    /// <remarks>
    /// Notatka może nie być powiązana z postacią (jeśli dotyczy przedmiotu, XOR z <see cref="ItemId"/>)
    /// </remarks>
    public long? CharacterId
    {
        get => _characterId;
        set
        {
            if (value.HasValue)
                _characterAttributeId = null;
            _characterId = value;
        }
    }
    
    /// <summary>
    /// Attrybut postaci, który jest powiązany z notatką
    /// </summary>
    /// <remarks>
    /// Notatka może nie być powiązana z atrybutem postaci (jeśli dotyczy postaci, XOR z <see cref="CharacterId"/>)
    /// </remarks>
    public CharacterAttribute? CharacterAttribute { get; set; }

    /// <summary>
    /// Id przedmiotu, który jest powiązany z notatką
    /// </summary>
    private long? _characterAttributeId;
    
    /// <summary>
    /// Id przedmiotu, który jest powiązany z notatką
    /// </summary>
    /// <remarks>
    /// Notatka może nie być powiązana z przedmiotem (jeśli dotyczy postaci, XOR z <see cref="CharacterId"/>)
    /// </remarks>
    public long? CharacterAttributeId
    {
        get => _characterAttributeId;
        set
        {
            if (value.HasValue)
                _characterId = null;
            _characterAttributeId = value;
        }
    }
}