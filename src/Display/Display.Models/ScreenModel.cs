using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display.Models
{
    public class ScreenModel
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? DisplayName { get; set; }

        public string? MenuEntityId { get; set; }

        public string? MediaAssetEntityId { get; set; }

        public string? TemplateKey { get; set; }

        public IEnumerable<TemplatePropertyModel>? TemplateProperties { get; set; }

        public string? ExternalMediaSource { get; set; }
    }
}
