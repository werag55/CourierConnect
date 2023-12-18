using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CourierConnect.Models
{
    public class Inquiry
    {
        public int Id { get; set; }

        public DateTime DeliveryDate { get; set; }

        public bool isPriority { get; set; }

        public bool weekendDelivery { get; set; }

        public bool isCompany { get; set; }

        public int descAddressID { get; set; }

        public Address descAddress { get; set; }
    }
}
