using System.Reflection;
using System.Text;
using System.Text.Json;
using Asp.Versioning;
using MasFinalProj.API.Hubs;
using MasFinalProj.Domain.Abstractions.Options;
using MasFinalProj.Domain.Models.NoDbModels;
using MasFinalProj.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;

namespace MasFinalProj.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddOptions<ConfigurationOptions>()
            .Bind(builder.Configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var configurationOptions = new ConfigurationOptions();
        builder.Configuration.Bind(configurationOptions);

        builder.Services.AddPersistence();

        builder.Services.AddControllers()
            .AddJsonOptions(opts =>
            {
                // opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            });
        builder.Services.AddSignalR();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opts =>
            {
                opts.ClaimsIssuer = configurationOptions.JwtIssuer;
                opts.RequireHttpsMetadata = false;
                opts.SaveToken = true;
                opts.UseSecurityTokenValidators = true;
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = configurationOptions.JwtIssuer,
                    ValidIssuer = configurationOptions.JwtIssuer,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationOptions.JwtSecret)),
                    ValidateIssuerSigningKey = true
                };
            });

        builder.Services.AddAuthorization();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(opts =>
        {
            opts.EnableAnnotations();

            opts.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "MasFinalProj.API",
                Version = "v1",
                Description = "Aplikacja backendowa web API dla projektu z przedmiotu MAS",
                Contact = new OpenApiContact
                {
                    Name = "Cyprian Gburek",
                    Email = "s24759@pjwstk.edu.pl",
                    Url = new Uri("https://cg-personal.vercel.app/")
                }
            });

            opts.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Description = "Wprowadź token JWT w formacie `Bearer {token}",
            });
            
            opts.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
            
            // opts.OperationFilter<SecurityRequirementsOperationFilter>();

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
        });

        builder.Services.AddRouting(opts =>
        {
            opts.LowercaseUrls = true;
            opts.LowercaseQueryStrings = true;
        });

        builder.Services.AddApiVersioning(opts =>
        {
            opts.DefaultApiVersion = new ApiVersion(1, 0);
            opts.AssumeDefaultVersionWhenUnspecified = true;
            opts.ReportApiVersions = true;
            opts.ApiVersionReader = new UrlSegmentApiVersionReader();
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        builder.Services.AddCors(opts =>
        {
            if (builder.Environment.IsDevelopment())
            {
                // opts.AddDefaultPolicy(policy =>
                // {
                //     policy.AllowAnyOrigin();
                //
                //     policy.AllowAnyHeader();
                //     policy.AllowAnyMethod();
                //     // policy.AllowCredentials();
                // });
                
                opts.AddDefaultPolicy(policy =>
                {
                    policy.WithOrigins(configurationOptions.AllowedOrigins);
                
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowCredentials();
                });
                
                return;
            }

            opts.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(configurationOptions.AllowedOrigins);
                
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowCredentials();
            });
        });

        builder.Logging.ClearProviders();
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration);
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseCors();
        app.MapHub<CampaignChatHub>("/api/hubs/campaign");

        app.MapControllers();
      
        app.Run();
    }
}
