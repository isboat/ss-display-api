
using Display.Api.Middlewares;
using Display.Models;
using Display.Repositories;
using Display.Services;
using Display.Shared;
using Display.Shared.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Display.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddOptions<MongoSettings>()
                .Bind(builder.Configuration.GetSection("MongoSettings"))
                .Validate(settings => !string.IsNullOrWhiteSpace(settings.ConnectionString), "MongoSettings:ConnectionString is required")
                .ValidateOnStart();
            builder.Services.AddOptions<JwtSettings>()
                .Bind(builder.Configuration.GetSection(JwtSettings.SectionName))
                .Validate(settings => !string.IsNullOrWhiteSpace(settings.Issuer), "Jwt:Issuer is required")
                .Validate(settings => !string.IsNullOrWhiteSpace(settings.Audience), "Jwt:Audience is required")
                .Validate(settings => !string.IsNullOrWhiteSpace(settings.SigningKey) && settings.SigningKey.Length >= 32, "Jwt:SigningKey must be at least 32 characters")
                .ValidateOnStart();
            builder.Services.AddOptions<DeviceAuthenticationSettings>()
                .Bind(builder.Configuration.GetSection(DeviceAuthenticationSettings.SectionName))
                .Validate(settings => !string.IsNullOrWhiteSpace(settings.VerificationUrl), "DeviceAuthentication:VerificationUrl is required")
                .Validate(settings => settings.DeviceCodeExpiresInSeconds > 0, "DeviceAuthentication:DeviceCodeExpiresInSeconds must be positive")
                .Validate(settings => settings.PollingIntervalSeconds > 0, "DeviceAuthentication:PollingIntervalSeconds must be positive")
                .Validate(settings => settings.AccessTokenLifetimeDays > 0, "DeviceAuthentication:AccessTokenLifetimeDays must be positive")
                .Validate(settings => settings.RefreshTokenLifetimeDays > 0, "DeviceAuthentication:RefreshTokenLifetimeDays must be positive")
                .ValidateOnStart();
            builder.Services.AddOptions<AuthenticationSettings>()
                .Bind(builder.Configuration.GetSection(AuthenticationSettings.SectionName))
                .ValidateOnStart();
            builder.Services.AddOptions<SignalRSettings>()
                .Bind(builder.Configuration.GetSection(SignalRSettings.SectionName))
                .Validate(settings => !string.IsNullOrWhiteSpace(settings.AzureSignalRConnectionString), "SignalR:AzureSignalRConnectionString is required")
                .ValidateOnStart();

            // Add services to the container.
            builder.Services.AddSingleton<IContentService, ContentService>();
            builder.Services.AddSingleton<IDeviceAuthenticationService, DeviceAuthenticationService>();
            builder.Services.AddSingleton<IDeviceService, DeviceService>();
            builder.Services.AddSingleton<IDeviceRegistrationRepository, DeviceRegistrationRepository>();
            builder.Services.AddSingleton<IRepository<DetailedScreenModel>, ScreenRepository>();
            builder.Services.AddSingleton<IJwtService, JwtService>();
            builder.Services.AddSingleton<ISignalrConnectionService, SignalrConnectionService>();
            builder.Services.AddSingleton<ISignalRRepository, SignalrConnectionRepository>();
            builder.Services.AddSingleton<IDateTimeProvider, SystemDatetimeProvider>();

            builder.Services.AddControllers();
            builder.Services
                .AddSingleton<SignalRHostedService>()
                .AddHostedService(sp => sp.GetRequiredService<SignalRHostedService>())
                .AddSingleton<IHubContextStore>(sp => sp.GetRequiredService<SignalRHostedService>());

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            RegisterJwtAuth(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static void RegisterJwtAuth(WebApplicationBuilder builder)
        {
            var jwtSettings = builder.Configuration.GetRequiredSection(JwtSettings.SectionName).Get<JwtSettings>()
                ?? throw new InvalidOperationException($"{JwtSettings.SectionName} configuration is required");
            var authSettings = builder.Configuration.GetSection(AuthenticationSettings.SectionName).Get<AuthenticationSettings>() ?? new AuthenticationSettings();

            var isAuthenticationDisabled = authSettings.Disabled;

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy(TenantAuthorization.RequiredPolicy, policy =>
                {
                    if (isAuthenticationDisabled)
                    {
                        policy.RequireAssertion(_ => true);
                    }
                    else
                    {
                        policy.RequireAuthenticatedUser().RequireClaim("scope", TenantAuthorization.RequiredScope);
                    }
                });
            });

            if (!isAuthenticationDisabled)
            {
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                        .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>

                        {
                            options.SaveToken = true;
                            options.TokenValidationParameters = new()
                            {
                                RequireExpirationTime = true,
                                RequireSignedTokens = true,
                                ValidateAudience = true,
                                ValidateIssuer = true,
                                ValidateLifetime = true,

                                // Allow for some drift in server time
                                // (a lower value is better; we recommend two minutes or less)
                                ClockSkew = TimeSpan.FromSeconds(0),

                                ValidIssuer = jwtSettings.Issuer,
                                ValidAudience = jwtSettings.Audience,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SigningKey!))
                            };
                        });
            }
            else // authenticate anyone
            {
                builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddScheme<AuthenticationSchemeOptions, EmptyAuthHandler>
                        (JwtBearerDefaults.AuthenticationScheme, opts => { });
            }
        }
    }
}