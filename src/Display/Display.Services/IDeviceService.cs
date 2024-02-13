using Display.Models.App;

namespace Display.Services
{
    public interface IDeviceService
    {
        Task<string?> GetDeviceName(string id);
    }
}