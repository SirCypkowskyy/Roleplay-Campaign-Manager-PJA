using System.Reflection;
using System.Text;
using System.Text.Json;
using Asp.Versioning;
using MasFinalProj.Domain.Abstractions.Options;
using MasFinalProj.Domain.Models.Users;
using MasFinalProj.Persistence;
using MasFinalProj.Persistence.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

namespace MasFinalProj.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddOptions<ConfigurationOptions>()
            .Bind(builder.Configuration)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var configurationOptions = new ConfigurationOptions();
        builder.Configuration.Bind(configurationOptions);

        builder.Services.AddPersistence();
        
        builder.Services.AddControllers()
            .AddJsonOptions(opts => { opts.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase; });

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opts =>
            {
                opts.RequireHttpsMetadata = false;
                opts.SaveToken = true;
                opts.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configurationOptions.JwtIssuer,
                    ValidAudience = configurationOptions.JwtIssuer,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configurationOptions.JwtSecret)),
                    ValidateIssuerSigningKey = true
                };
            });

        builder.Services.AddAuthorization();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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

            opts.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Description = "Please enter into field the word 'Bearer' following by space and JWT"
            });

            opts.OperationFilter<SecurityRequirementsOperationFilter>();

            // Dokumentacja XML z komentarzów do kodu
            var sharedXmlFile = $"{typeof(DatabaseContext).Assembly.GetName().Name}.xml";
            var webApiXmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var persistenceXmlFile = $"{typeof(User).Assembly.GetName().Name}.xml";

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            
            // opts.IncludeXmlComments(webApiXmlFile);
            // opts.IncludeXmlComments(sharedXmlFile);
            // opts.IncludeXmlComments(persistenceXmlFile);
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
        });

        builder.Services.AddCors(opts =>
        {
            if (builder.Environment.IsDevelopment())
            {
                opts.AddDefaultPolicy(policy =>
                {
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                    policy.AllowAnyOrigin();
                });
                return;
            }

            opts.AddDefaultPolicy(policy =>
            {
                policy.WithOrigins(configurationOptions.AllowedOrigins);
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
            });
        });

        // Logowanie odczytu z konsoli
        builder.Logging.ClearProviders();
        builder.Host.UseSerilog((context, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration);
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseAuthentication();

        app.MapControllers();

        app.Run();
    }
}