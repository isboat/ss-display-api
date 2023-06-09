﻿namespace Display.Models
{
    public class MenuModel : IModelItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? Name { get; set; } = null;

        public string? Description { get; set; }

        public string? Title { get; set; }

        public IEnumerable<MenuItem>? MenuItems { get; set; }
    }

    public class MenuItem : IModelItem
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? IconUrl { get; set; }

        public string? Title { get; set; }

        public decimal? Price { get; set; }
    }
}
