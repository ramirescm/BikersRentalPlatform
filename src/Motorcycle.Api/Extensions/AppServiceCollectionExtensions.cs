using System.Text;
using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Motorcycle.Api.Endpoints;
using Motorcycle.Api.Middlewares;
using Motorcycle.Core;
using Motorcycle.Core.Repositories;
using Motorcycle.Core.Services;
using Motorcycle.Core.UoW;
using Motorcycle.Data;
using Motorcycle.Data.Repositories;
using Motorcycle.Shared;
using Motorcycle.Shared.Common.Behavior;
using Motorcycle.Shared.Config;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Motorcycle.Api.Extensions;

public static class AppServiceCollectionExtensions
{
    public static void ConfigureAppDependencies(this IServiceCollection services, IConfiguration configuration,
        AppSettings appSettings)
    {
        services.AddRouting(options => options.LowercaseUrls = true);
        services.Configure<JsonOptions>(o =>
            o.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull);

        services.AddEndpointsApiExplorer();
        services.AddTransient<ExceptionHandlingMiddleware>();

        services.AddHttpContextAccessor();
        services.AddAntiforgery(options => options.SuppressXFrameOptionsHeader = true);

        services.AddAuthorization(options =>
        {
            // Define a policy
            options.AddPolicy("AdminOnly", policy =>
                policy.RequireRole("admin"));
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!)),
                ValidateIssuer = false,
                ValidateAudience = false
            };
        });

        services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
            options.Providers.Add<GzipCompressionProvider>();
        });

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Motorcyle.Api",
                Version = "v1",
                Description = "bikers management"
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                }
            });
        });

        // Register MediatR
        services.AddMediatR(config => config.RegisterServicesFromAssemblies(typeof(ICoreAssemblyMarker).Assembly)
            .AddOpenBehavior(typeof(ValidationBehavior<,>)));

        // Register Validators
        services.AddValidatorsFromAssemblyContaining<ISharedAssemblyMarker>();

        // Config Database
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DatabaseSettings")!)
                .EnableSensitiveDataLogging()
                .LogTo(Console.WriteLine, LogLevel.Information));
        
        ConfigureServices(services);
        ConfigureRabbit(services, configuration);
    }

    private static void ConfigureRabbit(IServiceCollection services, IConfiguration configuration)
    {
        var rabbitMqSettings = configuration.GetSection("RabbitMQ");
        services.AddSingleton<IMessageBus>(provider =>
        {
            var settings = rabbitMqSettings.Get<RabbitMQSettings>();
            return new MessageBus(settings.HostName, settings.UserName, settings.Password);
        });
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddTransient<IMotorycycleRepository, MotorycycleRepository>();
        services.AddTransient<IRentalPlanRepository, RentalPlanRepository>();
        services.AddTransient<IDelivererRepository, DelivererRepository>();
        services.AddTransient<IMotorcycleRentalPlanRepository, MotorcycleRentalPlanRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IJwtBearerService, JwtBearerService>();
    }

    public static void UseApiConfiguration(this WebApplication app)
    {
        // Swagger/OpenAPI services
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.DocumentTitle = "Motorcycle.Api Swagger";
            c.DisplayRequestDuration();
            c.DocExpansion(DocExpansion.None);
            c.EnableDeepLinking();
            c.ShowExtensions();
            c.ShowCommonExtensions();
        });

        app.UseMiddleware<ExceptionHandlingMiddleware>();

        app.UseResponseCompression();

        app.UseAntiforgery();
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseHttpsRedirection();

        app.MapEndpoints();
    }
}