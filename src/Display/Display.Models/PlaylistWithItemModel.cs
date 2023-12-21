using MongoDB.Bson.Serialization.Attributes;

namespace Display.Models
{
    [BsonIgnoreExtraElements]
    public class PlaylistWithItemModel : IModelItem 
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? Name { get; set; }

        public TimeSpan? ItemDuration { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public IList<string>? AssetIds { get; set; }
        public IList<AssetItemModel>? AssetItems { get; set; }
    }
}
