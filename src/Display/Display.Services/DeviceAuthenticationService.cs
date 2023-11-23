using Display.Models.App;
using Display.Shared.Exceptions;
using Display.Repositories;
using Display.Shared;
using Display.Shared.Constants;

namespace Display.Services
{
    // https://developers.google.com/identity/protocols/oauth2/limited-input-device
    public class DeviceAuthenticationService : IDeviceAuthenticationService
    {
        private readonly IDeviceRegistrationRepository _repository;
        private readonly IJwtService _jwtService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DeviceAuthenticationService(IDeviceRegistrationRepository repository, IJwtService jwtService, IDateTimeProvider dateTimeProvider)
        {
            _repository = repository;
            _jwtService = jwtService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<DeviceCodeModel?> GetDeviceCode(CodeRequest codeRequest)
        {
            var model =new DeviceCodeModel
            {
                DeviceCode = $"{Guid.NewGuid():N}/{Guid.NewGuid():N}",
                UserCode = GenerateUserCode(),
                ExpiresIn = 1800, // seconds
                Interval = 5,
                VerificationUrl = "https://www.isboatscreens.com/device/auth",
                DeviceName = GenerateDeviceName(),
                ClientId = codeRequest.ClientId,
            };

            var registrationModel = new DeviceCodeRegistrationModel
            {
                RegisteredDatetime = _dateTimeProvider.UtcNow,
                DeviceCode = model.DeviceCode,
                UserCode = model.UserCode,
                ExpiresIn = model.ExpiresIn,
                Interval = model.Interval,
                Id = Guid.NewGuid().ToString("N"),
                DeviceName = model.DeviceName,
            };

            var res = await _repository.InsertAsync(registrationModel); 

            return res ? model : null;
        }

        private static string GenerateDeviceName()
        {
            var rnd = new Random();
            return $"device-name-{rnd.Next()}";
        }

        public async Task<AccessPermission?> GetAccessToken(TokenRequest codeStatusRequest)
        {
            if (string.IsNullOrEmpty(codeStatusRequest?.DeviceCode))
            {
                throw new InvalidDevicecodeException();
            }

            var model = await _repository.GetAsync(codeStatusRequest.DeviceCode);

            if (model == null)
            {
                throw new AccessForbiddenException();
            }

            if(IsExpired(model) && !IsApproved(model))
            {
                // rate_limit_exceeded
                throw new AccessExpiredException();

            }

            if (!IsApproved(model))
            {
                throw new AuthorizationPendingException();
            }

            var access = GenerateAccessPermission(model);
            return access;
        }

        public async Task<AccessPermission?> RefreshToken(TokenRequest codeStatusRequest, string refreshToken)
        {
            try
            {
                _jwtService.ValidateToken(refreshToken);
            }
            catch (Exception ex)
            {
                // Refresh token is expired
                // log ex
                throw new InvalidRefreshTokenException();
            }

            var token = _jwtService.ReadToken(refreshToken) ?? throw new InvalidRefreshTokenException();
            var scopeClaim =  token.Claims.FirstOrDefault(x => x.Type.Equals("scope", StringComparison.OrdinalIgnoreCase));
            if (scopeClaim == null || scopeClaim.Value != "refresh_token") throw new InvalidRefreshTokenException();

            var deviceCodeClaim = token.Claims.FirstOrDefault(x => x.Type.Equals("devicecode", StringComparison.OrdinalIgnoreCase));
            if (string.IsNullOrEmpty(deviceCodeClaim?.Value)) throw new InvalidRefreshTokenException();

            var model = await _repository.GetAsync(deviceCodeClaim.Value) ?? throw new AccessForbiddenException();
            var access = GenerateAccessPermission(model);
            return access;
        }
        
        private static string GenerateUserCode()
        {
            var splits = Guid.NewGuid().ToString("D").Split('-');

            // return 12 dash characters
            return $"{splits[1]}-{splits[2]}-{splits[3]}".ToUpperInvariant();
        }

        private bool IsExpired(DeviceCodeRegistrationModel model)
        {
            if (model?.RegisteredDatetime == null || model?.ExpiresIn == null) return true;

            var expirationDatetime = model.RegisteredDatetime.Value.AddSeconds(model.ExpiresIn.Value);
            return expirationDatetime < _dateTimeProvider.UtcNow;
        }

        private AccessPermission GenerateAccessPermission(DeviceCodeRegistrationModel model)
        {
            var tokenExpiration = _dateTimeProvider.UtcNow!.Value.AddHours(1);
            var access = new AccessPermission
            {
                TokenType = "Bearer",
                ExpiresIn = ConvertToSeconds(tokenExpiration),
                Scope = TenantAuthorization.RequiredScope
            };
            var tokenData = new Dictionary<string, string>
            {
                {"tenantid",model.TenantId! },
                {"deviceId",model.Id! },
                {"devicecode",model.DeviceCode! },
                {"devicename",model.DeviceName! },
                {"scope",access.Scope! }
            };

            var accessToken = _jwtService.GenerateToken(tokenData, tokenExpiration);
            access.AccessToken = accessToken;

            var refreshTokenData = new Dictionary<string, string>
            {
                {"devicecode",model.DeviceCode! },
                {"scope","refresh_token" }
            };

            var refreshToken = _jwtService.GenerateToken(refreshTokenData, _dateTimeProvider.UtcNow!.Value.AddYears(1));
            access.RefreshToken = refreshToken;
            return access;
        }

        private double? ConvertToSeconds(DateTime tokenExpiration)
        {
            var totalSecs = tokenExpiration.Subtract(_dateTimeProvider.UnixEpoch!.Value).TotalSeconds;
            return Math.Floor(totalSecs);
        }

        private bool IsApproved(DeviceCodeRegistrationModel model)
        {
            return model?.ApprovedDatetime != null 
                && model.ApprovedDatetime > _dateTimeProvider.UnixEpoch!.Value
                && model.ApprovedDatetime > model.RegisteredDatetime;

        }
    }
}
