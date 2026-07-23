using Display.Models;
using Display.Shared.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Display.Api
{
    public class EmptyAuthContext : ResultContext<AuthenticationSchemeOptions>
    {
        public EmptyAuthContext(HttpContext context, AuthenticationScheme scheme, AuthenticationSchemeOptions options)
            : base(context, scheme, options) { }
    }

    public class EmptyAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly AuthenticationSettings _authenticationSettings;

        public EmptyAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IOptions<AuthenticationSettings> authenticationSettings)
            : base(options, logger, encoder, clock)
        {
            _authenticationSettings = authenticationSettings.Value;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            await Task.Yield(); // now we're async

            var claims = new[]
            {
                new Claim("tenantid", _authenticationSettings.DisabledTenantId),
                new Claim("deviceId", _authenticationSettings.DisabledDeviceId),
                new Claim("scope", TenantAuthorization.RequiredScope)
            };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));
            var context = new EmptyAuthContext(Context, Scheme, Options) { Principal = principal };

            context.Success();
            return context.Result;
        }
    }
}
