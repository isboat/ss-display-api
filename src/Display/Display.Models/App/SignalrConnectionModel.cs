using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Display.Models.App
{
    public class SignalrConnectionModel
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? DeviceId { get; set; }
        public string? DeviceName { get; set; }
        public DateTime? ConnectionDateTime { get; set; }
    }
}
