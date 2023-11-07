using Display.Models.App;

namespace Display.Services
{
    // https://developers.google.com/identity/protocols/oauth2/limited-input-device

    public interface IDeviceAuthenticationService
    {
        Task<DeviceCodeModel?> GetDeviceCode();

        Task<AccessPermission?> GetAccessToken(TokenRequest codeStatusRequest);
        Task<AccessPermission?> RefreshToken(TokenRequest codeStatusRequest, string refreshToken);
    }
}