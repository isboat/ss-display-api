namespace Display.Models
{
    public class ScreenDetailModel: ScreenModel
    {
        public MenuModel? Menu { get; set; }
        public AssetItemModel? MediaAsset { get; set; }

        public static ScreenDetailModel ToDetails(ScreenModel screen)
        {
            var details = new ScreenDetailModel();
            details.DisplayName = screen.DisplayName;
            details.Id = screen.Id;
            details.TenantId = screen.TenantId;
            details.MenuEntityId = screen.MenuEntityId;
            details.MediaAssetEntityId = screen.MediaAssetEntityId;
            details.TemplateKey = screen.TemplateKey;
            details.TemplateProperties = screen.TemplateProperties;

            return details;
        }
    }
}
