using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.Dto.CourierHub
{
    public class CourierHubRequestSendDto
    {
        public string? inquireCode { get; set; }
        public string? clientName { get; set; }
        public string? clientSurname { get; set; }
        public string? clientEmail { get; set; }
        public string? clientPhoneNumber { get; set;}
        public string? clientCompany { get; set; }
        public CourierHubAddressDto clientAddress { get; set; }
    }
}
