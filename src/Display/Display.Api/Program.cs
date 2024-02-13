
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

            builder.Services.Configure<MongoSettings>(
                builder.Configuration.GetSection("MongoSettings"));

            // Add services to the container.
            builder.Services.AddSingleton<IContentService, ContentService>();
            builder.Services.AddSingleton<IDeviceAuthenticationService, DeviceAuthenticationService>();
            builder.Services.AddSingleton<IDeviceService, DeviceService>();
            builder.Services.AddSingleton<IDeviceRegistrationRepository, DeviceRegistrationRepository>();
            builder.Services.AddSingleton<IRepository<DetailedScreenModel>, ScreenRepository>();
            builder.Services.AddSingleton<IJwtService, JwtService>();
            builder.Services.AddSingleton<IDateTimeProvider, SystemDatetimeProvider>();

            builder.Services.AddControllers();
            builder.Services
                .AddSingleton<SignalRHostedService>()
                .AddHostedService(sp => sp.GetService<SignalRHostedService>())
                .AddSingleton<IHubContextStore>(sp => sp.GetService<SignalRHostedService>());

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

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlingMiddleware>();
            app.MapControllers();

            app.Run();
        }

        private static void RegisterJwtAuth(WebApplicationBuilder builder)
        {
            var jwtIssuer = "http://mysite.com";
            var jwtAudience = "http://myaudience.com";
            var jwtSigningKey = "asdv234234^&%&^%&^hjsdfb2%%%";

            var isAuthenticationDisabled = false;

            if (!isAuthenticationDisabled)
            {
                builder.Services.AddAuthorization(options =>
                {
                    options.AddPolicy(TenantAuthorization.RequiredPolicy, policy =>
                        policy.RequireAuthenticatedUser().RequireClaim("scope", TenantAuthorization.RequiredScope));
                });
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

                                ValidIssuer = jwtIssuer,
                                ValidAudience = jwtAudience,
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSigningKey))
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