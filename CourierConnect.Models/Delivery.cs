using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models
{
    public class Delivery
    {
        [Key]
        public int Id { get; set; }


        [Required]
        [DisplayName("Courier")]
        public Courier Courier { get; set; }

        [Required]
        [DisplayName("Delivery date")]
        public DateTime DeliveryDate { get; set; }

        [Required]
        [DisplayName("Client")]
        public Client Client { get; set; }

        [Required]
        [DisplayName("Deadline delivery date")]
        public DateTime DeadlineDeliveryDate { get; set; }

        [Required]
        [DisplayName("Address")]
        public Address Address { get; set; }

        // request ID

        [Required]
        [DisplayName("Delivery status")]
        public int DeliveryStatus { get; set; }

        // required??
        [DisplayName("Receiving date")]
        public DateTime ReceiveingDate { get; set; }
    }
}
