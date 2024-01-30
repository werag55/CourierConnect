using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.POCO
{
    public class InquiryPOCO
    {
        public int Id { get; set; }
        public DateTime pickupDate { get; set; }
        public DateTime deliveryDate { get; set; }
        public bool isPriority { get; set; }
        public bool weekendDelivery { get; set; }
        public bool isCompany { get; set; }
        public AddressPOCO sourceAddress { get; set; }
        public AddressPOCO destinationAddress { get; set; }
        public PackagePOCO package { get; set; }
        public DateTime creationDate { get; set; }
    }
}
