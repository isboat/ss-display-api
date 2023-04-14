using Display.Models.App;

namespace Display.Services
{
    public interface IAppRegistrationService
    {
        Task<RegisterModel?> GetStatus(int? regNumber);
        Task<RegisterResponseModel> Register(RegisterRequestModel model);
    }
}