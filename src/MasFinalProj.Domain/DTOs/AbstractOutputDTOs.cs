using MasFinalProj.Domain.Models.Campaigns;
using MasFinalProj.Domain.Models.Campaigns.Characters;
using MasFinalProj.Domain.Models.Campaigns.Users;
using MasFinalProj.Domain.Models.Users.New;

namespace MasFinalProj.Domain.DTOs;

/// <summary>
/// Klasa abstrakcyjna z abstrakcyjnymi DTO wyjściowymi.
/// </summary>
public class AbstractOutputDTOs
{
    /// <summary>
    /// DTO wyjściowe użytkownika.
    /// </summary>
    public class UserDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string? DiscordUsername { get; set; }
        public string? Description { get; set; }
        public long? ProfileImageId { get; set; }
        public bool IsActive { get; set; }

        // Konstruktor bezargumentowy
        public UserDto() {}

        // Konstruktor, który przyjmuje model User
        public UserDto(Models.Users.User model)
        {
            Username = model.Username;
            Email = model.Email;
            DiscordUsername = model.DiscordUsername;
            Description = model.Description;
            ProfileImageId = model.ProfileImageId;
            IsActive = model.IsActive;
        }
    }
    
    /// <summary>
    /// DTO wyjściowe postaci.
    /// </summary>
    public class MessageDto
    {
        public string Text { get; set; }
        public long AuthorId { get; set; }
        public long? CharacterAuthorId { get; set; }

        // Konstruktor bezargumentowy
        public MessageDto() {}

        // Konstruktor, który przyjmuje model Message
        public MessageDto(Message model)
        {
            Text = model.Content;
            AuthorId = model.AuthorId;
            CharacterAuthorId = model.CharacterAuthorId;
        }
    }
    
    /// <summary>
    /// DTO wyjściowe kampanii.
    /// </summary>
    public class CampaignDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long? ImageId { get; set; }

        // Konstruktor bezargumentowy
        public CampaignDto() {}

        // Konstruktor, który przyjmuje model Campaign
        public CampaignDto(Campaign model)
        {
            Name = model.Name;
            Description = model.Description;
            ImageId = model.CampaignImageId;
        }
    }
    
    /// <summary>
    /// DTO wyjściowe postaci.
    /// </summary>
    public class CharacterDto
    {
        public string Name { get; set; }

        public string? Bio { get; set; }
        public string? Description { get; set; }

        // Konstruktor bezargumentowy
        public CharacterDto() {}

        // Konstruktor, który przyjmuje model Character
        public CharacterDto(Character model)
        {
            Name = model.Name;
            Description = model.Description;
            Bio = model.Bio;
        }
    }
    
    /// <summary>
    /// DTO wyjściowe obrazu.
    /// </summary>
    public class ImageDto
    {
        /// <summary>
        /// Nazwa zdjęcia
        /// </summary>
        public string ImageName { get; set; }
        
        public string Base64Image { get; set; }
        public string ImageFormat { get; set; }

        // Konstruktor bezargumentowy
        public ImageDto() {}

        // Konstruktor, który przyjmuje model Image
        public ImageDto(Models.Common.Image model)
        {
            Base64Image = model.Base64Image;
            ImageFormat = model.ImageFormat;
        }
    }
}