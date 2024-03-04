using MongoDB.Bson.Serialization.Attributes;

namespace Display.Models
{
    public class MenuModel : IModelItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? Name { get; set; } = null;

        public string? Description { get; set; }

        public string? Title { get; set; }

        public string? Currency { get; set; }

        public string? IconUrl { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public IEnumerable<MenuItem>? MenuItems { get; set; }
    }

    public class MenuItem : IModelItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? IconUrl { get; set; }

        public string? Price { get; set; }

        public string? DiscountPrice { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
