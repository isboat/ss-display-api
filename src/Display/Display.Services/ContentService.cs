using Display.Models;
using Display.Models.App;
using Display.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display.Services
{
    public class ContentService: IContentService
    {
        private readonly IRepository<DetailedScreenModel> _repository;
        private  readonly IDeviceRegistrationRepository _deviceRegistrationRepository;

        public ContentService(IRepository<DetailedScreenModel> repository, IDeviceRegistrationRepository deviceRegistrationRepository)
        {
            _repository = repository;
            _deviceRegistrationRepository = deviceRegistrationRepository;
        }

        public async Task<DetailedScreenModel?> GetDetailsAsync(string tenantId, string id)
        {
            var screen = await _repository.GetAsync(tenantId, id);
            return screen;
        }

        public Task<DeviceCodeRegistrationModel?> GetDeviceAsync(string id) =>
            _deviceRegistrationRepository.GetAsyncById(id);
    }
}
