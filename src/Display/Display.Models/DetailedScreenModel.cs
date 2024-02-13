namespace Display.Models
{
    public class DetailedScreenModel
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? DisplayName { get; set; }

        public string? MenuEntityId { get; set; }

        public string? MediaAssetEntityId { get; set; }

        public string? TextAssetEntityId { get; set; }

        public LayoutModel? Layout { get; set; }

        public string? ExternalMediaSource { get; set; }

        public string? TextEditorData { get; set; } = string.Empty; 
        
        public string? PlaylistId { get; set; }

        public MenuModel? Menu { get; set; }
        public AssetItemModel? MediaAsset { get; set; }

        public string? Checksum { get; set; }

        public PlaylistWithItemModel? PlaylistData { get; set; }
    }
}
