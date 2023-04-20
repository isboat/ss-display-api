using Display.Models.App;
using Display.Shared.Exceptions;
using Display.Repositories;
using Display.Shared;

namespace Display.Services
{
    // https://developers.google.com/identity/protocols/oauth2/limited-input-device
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRegistrationRepository _repository;
        private readonly IJwtService _jwtService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DeviceService(IDeviceRegistrationRepository repository, IJwtService jwtService, IDateTimeProvider dateTimeProvider)
        {
            _repository = repository;
            _jwtService = jwtService;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<DeviceCodeModel?> GetDeviceCode()
        {
            var model =new DeviceCodeModel
            {
                DeviceCode = $"{Guid.NewGuid().ToString("N")}/{Guid.NewGuid().ToString("N")}",
                UserCode = GenerateUserCode(),
                ExpiresIn = 1800,
                Interval = 5,
                VerificationUrl = "https://www.isboatscreens.com/device"
            };

            var registrationModel = new DeviceCodeRegistrationModel
            {
                RegisteredDatetime = _dateTimeProvider.UtcNow,
                DeviceCode = model.DeviceCode,
                UserCode = model.UserCode,
                ExpiresIn = model.ExpiresIn,
                Interval = model.Interval,
                Id = Guid.NewGuid().ToString("N")
            };

            var res = await _repository.InsertAsync(registrationModel); 

            return res ? model : null;
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

            if(IsExpired(model))
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

            if (string.IsNullOrEmpty(codeStatusRequest?.DeviceCode))
            {
                throw new InvalidDevicecodeException();
            }

            try
            {
                _jwtService.ValidateToken(refreshToken);
            }
            catch (Exception ex)
            {
                // Refresh token is expired
                // log ex
                throw new RefreshTokenInvalidException();
            }

            var model = await _repository.GetAsync(codeStatusRequest.DeviceCode);

            if (model == null)
            {
                throw new AccessForbiddenException();
            }

            var access = GenerateAccessPermission(model);
            return access;
        }
        
        private string GenerateUserCode()
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
                Scope = "tenant.content"
            };
            var tokenData = new Dictionary<string, string>
            {
                {"tenantid",model.TenantId! },
                {"devicecode",model.DeviceCode! },
                {"scope",access.Scope! }
            };

            var accessToken = _jwtService.GenerateToken(tokenData, tokenExpiration);
            access.AccessToken = accessToken;

            var refreshToken = _jwtService.GenerateToken(tokenData, _dateTimeProvider.UtcNow!.Value.AddYears(1));
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
