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
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Delivery date")]
        public DateTime DeliveryDate { get; set; }

        [Required]
        [DisplayName("Priority")]
        public bool isPriority { get; set; }

        [Required]
        [DisplayName("Delivery at weekend")]
        public bool weekendDelivery { get; set; }

        [Required]
        [DisplayName("Company")]
        public bool isCompany { get; set; }

        //[ForeignKey("Address")]
        //[Required]
        public int descAddressID { get; set; }
        //[DisplayName("Destination address")]
        [ForeignKey("descAddressID")]
        public Address descAddress { get; set; }
    }
}
