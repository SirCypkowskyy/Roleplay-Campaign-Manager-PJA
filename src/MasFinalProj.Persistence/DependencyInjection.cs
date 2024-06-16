using MasFinalProj.Domain.Abstractions.Options;
using MasFinalProj.Domain.Repositories;
using MasFinalProj.Persistence.Data;
using MasFinalProj.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MasFinalProj.Persistence;

/// <summary>
/// Klasa do wstrzykiwania zależności
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Dodaje wstrzykiwanie zależności dla warstwy persystancji
    /// </summary>
    /// <param name="services">
    /// Kolekcja serwisów
    /// </param>
    /// <param name="configurationOptions">
    /// Opcje konfiguracji
    /// </param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// Jeśli opcje konfiguracji są nullem
    /// </exception>
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        var configurationOptions = services.BuildServiceProvider().GetService<IOptions<ConfigurationOptions>>();
        ArgumentNullException.ThrowIfNull(configurationOptions);

        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlServer(configurationOptions.Value.DbConnectionString);
        });

        services.AddHttpClient("discordClient", c =>
        {
            c.BaseAddress = new Uri("https://discord.com/oauth2/");
        });

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDiscordAuthRepository, DiscordAuthRepository>();
        services.AddScoped<ICampaignRepository, CampaignRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IImageRepository, ImageRepository>();

        return services;
    }
}