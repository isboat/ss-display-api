using Display.Models;
using Display.Models.App;
using Display.Repositories;

namespace Display.Services
{
    public interface IContentService
    {
        Task<IEnumerable<ScreenModel>> GetScreensAsync(string tenantId);

        Task<ScreenDetailModel?> GetDetailsAsync(string tenantId, string id);

        Task<DeviceCodeRegistrationModel?> GetDeviceAsync(string id);
    }
}