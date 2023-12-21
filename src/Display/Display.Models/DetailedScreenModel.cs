using MongoDB.Bson.Serialization.Attributes;

namespace Display.Models
{
    [BsonIgnoreExtraElements]
    public class DetailedScreenModel // : ScreenModel
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? DisplayName { get; set; }

        public string? MenuEntityId { get; set; }

        public string? MediaAssetEntityId { get; set; }

        public LayoutModel? Layout { get; set; }

        public string? ExternalMediaSource { get; set; }

        public MenuModel? Menu { get; set; }
        public AssetItemModel? MediaAsset { get; set; }

        public string? Checksum { get; set; }

        public PlaylistWithItemModel? PlaylistData { get; set; }
    }
}
