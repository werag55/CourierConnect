using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierCompanyApi.Models
{
    public class Inquiry
    {
        public int Id { get; set; }
        public DateTime pickupDate { get; set; }

        public DateTime deliveryDate { get; set; }

        public bool isPriority { get; set; }

        public bool weekendDelivery { get; set; }

        public bool isCompany { get; set; }

        public int sourceAddressId { get; set; }
        public Address sourceAddress { get; set; }

        public int destinationAddressId { get; set; }
        public Address destinationAddress { get; set; }

        public int packageId { get; set; }
        public Package package { get; set; }

    }
}
