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

        public IList<PlaylistItemIdTypePair>? ItemIdAndTypePairs { get; set; }
        public IList<object>? Items { get; set; }

        public List<KeyValuePair<string, string>>? ItemsSerialized { get; set; }
    }

    public interface IPlaylistItem
    {
        PlaylistItemType PlaylistType { get; }
    }

    public class PlaylistItemIdTypePair
    {
        public PlaylistItemType ItemType { get; set; }
        public string? Id { get; set; }
    }

    public enum PlaylistItemType
    {
        Media,
        Text
    }
}
