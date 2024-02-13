using Display.Models.App;
using Display.Shared.Exceptions;
using Display.Repositories;
using Display.Shared;
using Display.Shared.Constants;

namespace Display.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRegistrationRepository _repository;

        public DeviceService(IDeviceRegistrationRepository repository)
        {
            _repository = repository;
        }

        public async Task<string?> GetDeviceName(string id)
        {
            var device = await _repository.GetAsyncById(id);
            return device?.DeviceName;
        }
    }
}
