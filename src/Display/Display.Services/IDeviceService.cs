using Display.Models.ViewModels;

namespace Display.Services
{
    public interface IDeviceService
    {
        Task<DeviceViewModel?> GetDevice(string id);
    }
}