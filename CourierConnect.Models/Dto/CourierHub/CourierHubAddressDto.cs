using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.Dto.CourierHub
{
    public class CourierHubAddressDto
    {
        public string? city { get; set; }
        public string? postalCode { get; set; }
        public string? street { get; set; }
        public string? number { get; set; }
        public string? flat { get; set; }
    }
}
