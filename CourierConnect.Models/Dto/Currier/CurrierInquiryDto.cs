using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.Dto.Currier
{
    public class CurrierInquiryDto
    {
        public DimensionsDto dimensions {  get; set; }
        string? currency {  get; set; }
        public float weight { get; set; }
        public string? weightUnit { get; set; }
        public CurrierAddressDto source {  get; set; }
        public CurrierAddressDto destination { get; set; }
        public DateTime pickupDate { get; set; }
        public DateTime deliveryDay { get; set;}
        public bool deliveryInWeekend { get; set; }
        public string? priority { get; set; }
        public bool vipPackage { get; set; }
        public bool isCompany { get; set; }
    }
}
