using MongoDB.Bson.Serialization.Attributes;

namespace Display.Models
{
    [BsonIgnoreExtraElements]
    public class LayoutModel
    {
        public string? TemplateKey { get; set; }

        public IEnumerable<TemplatePropertyModel>? TemplateProperties { get; set; }

        public string? SubType { get; set; }
    }
}
