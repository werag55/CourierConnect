using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
