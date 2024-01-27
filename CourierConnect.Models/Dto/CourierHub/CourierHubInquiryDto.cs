using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.Dto.CourierHub
{
    public class CourierHubInquiryDto
    {
        public int depth {  get; set; }
        public int width { get; set; }
        public int length { get; set; }
        public int mass { get; set; }
        public CourierHubAddressDto sourceAddress { get; set; }
        public CourierHubAddressDto destinationAddress { get; set; }
        public DateTime datetime { get; set; }
        public bool isCompany { get; set; }
        public bool isWeekend { get; set; }
        public PriorityType priority { get; set; }
    }

    public enum PriorityType
    {
        Low,
        Medium,
        High
    }
}
