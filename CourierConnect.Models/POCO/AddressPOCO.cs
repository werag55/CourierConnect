using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.POCO
{
    public class AddressPOCO
    {
        public string streetName { get; set; }

        public int houseNumber { get; set; }

        public int? flatNumber { get; set; }

        public string postcode { get; set; }

        public string city { get; set; }
    }
}
