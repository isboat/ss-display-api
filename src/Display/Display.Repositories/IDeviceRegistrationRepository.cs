using Display.Models.App;

namespace Display.Repositories
{
    public interface IDeviceRegistrationRepository
    {
        Task<DeviceCodeRegistrationModel> GetAsync(string regCode);

        Task<DeviceCodeRegistrationModel?> GetAsyncById(string deviceId);

        Task<bool> InsertAsync(DeviceCodeRegistrationModel model);
    }
}
