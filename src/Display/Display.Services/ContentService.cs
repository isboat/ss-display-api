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
        private readonly IRepository<ScreenModel> _repository;
        private  readonly IDeviceRegistrationRepository _deviceRegistrationRepository;

        public ContentService(IRepository<ScreenModel> repository, IDeviceRegistrationRepository deviceRegistrationRepository)
        {
            _repository = repository;
            _deviceRegistrationRepository = deviceRegistrationRepository;
        }

        public async Task<IEnumerable<ScreenModel>> GetScreensAsync(string tenantId) =>
            await _repository.GetAllByTenantIdAsync(tenantId);

        public async Task<ScreenDetailModel?> GetDetailsAsync(string tenantId, string id)
        {
            var screen = await _repository.GetAsync(tenantId, id);
            if (screen == null) return null;

            var screenDetails = ScreenDetailModel.ToDetails(screen);

            if (!string.IsNullOrEmpty(screenDetails.MenuEntityId))
            {
                screenDetails.Menu = GetMenuDetails(screenDetails.MenuEntityId);
            }

            if (!string.IsNullOrEmpty(screenDetails.MediaAssetEntityId))
            {
                screenDetails.MediaAsset = GetMediaAssetDetails(screenDetails.MediaAssetEntityId);
            }

            return screenDetails;
        }

        public Task<DeviceCodeRegistrationModel?> GetDeviceAsync(string id) =>
            _deviceRegistrationRepository.GetAsyncById(id);

        private MenuModel? GetMenuDetails(string? itemId)
        {
            throw new NotImplementedException();
        }

        private AssetItemModel? GetMediaAssetDetails(string? itemId)
        {
            throw new NotImplementedException();
        }
    }
}
