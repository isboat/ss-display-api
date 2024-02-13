using Display.Models.App;
using Display.Shared.Exceptions;
using Display.Repositories;
using Display.Shared;
using Display.Shared.Constants;
using Display.Models.ViewModels;

namespace Display.Services
{
    public class DeviceService : IDeviceService
    {
        private readonly IDeviceRegistrationRepository _repository;

        public DeviceService(IDeviceRegistrationRepository repository)
        {
            _repository = repository;
        }

        public async Task<DeviceViewModel?> GetDevice(string id)
        {
            var device = await _repository.GetAsyncById(id);
            return device == null ? null : new DeviceViewModel
            {
                DeviceName = device.DeviceName,
                Id = device.Id,
                TenantId = device.TenantId
            };
        }
    }
}
