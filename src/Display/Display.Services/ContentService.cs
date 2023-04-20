using Display.Models;
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

        public ContentService(IRepository<ScreenModel> repository)
        {
            _repository = repository;
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
