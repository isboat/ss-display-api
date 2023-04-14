using Display.Models;
using Display.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display.Services
{
    public class ScreenService: IScreenService
    {
        private readonly IRepository<ScreenModel> _repository;

        public ScreenService(IRepository<ScreenModel> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ScreenModel>> GetScreensAsync(string tenantId) =>
            await _repository.GetAllByTenantIdAsync(tenantId);

        public async Task<ScreenDetailModel?> GetDetailsAsync(string tenantId, string id)
        {
            ScreenDetailModel? screen = await _repository.GetAsync(tenantId, id) as ScreenDetailModel;
            if (screen == null) return null;

            if (!string.IsNullOrEmpty(screen.MenuEntityId))
            {
                screen.Menu = GetMenuDetails(screen.MenuEntityId);
            }

            if (!string.IsNullOrEmpty(screen.MediaAssetEntityId))
            {
                screen.MediaAsset = GetMediaAssetDetails(screen.MediaAssetEntityId);
            }

            return screen;
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
