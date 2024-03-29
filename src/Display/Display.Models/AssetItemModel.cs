﻿using MongoDB.Bson.Serialization.Attributes;

namespace Display.Models
{
    [BsonIgnoreExtraElements]
    public class AssetItemModel : IModelItem, IPlaylistItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? Name { get; set; } = null;

        public string? Description { get; set; }

        public string? AssetUrl { get; set; }

        public string? FileName { get; set; }

        public AssetType? Type { get; set; }

        public PlaylistItemType PlaylistType => PlaylistItemType.Media;
    }

    public enum AssetType
    {
        None,
        Image,
        Video
    }
}
