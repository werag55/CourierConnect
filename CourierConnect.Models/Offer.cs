using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models
{
    public class Offer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Inquiry")]
        public Inquiry Inquiry { get; set; }

        [Required]
        [DisplayName("Creation date")]
        public DateTime creationDate { get; set; }

        [Required]
        [DisplayName("Update date")]
        public DateTime updateDate { get; set; }

        [Required]
        [DisplayName("Offer status")]
        public string offerStatus { get; set; }

        // wtf
        [Required]
        [DisplayName("Requested value")]
        public int requestedValue { get; set; }

        [Required]
        [DisplayName("Offer time period")]
        public int offerTimePeriod { get; set; }

        // company id

        // czy nie zmienic na trzy rozne wymiary?
        [Required]
        [DisplayName("Package info")]
        public string packageInfo { get; set; }


        // source info??

        // delivery info??

        [Required]
        [DisplayName("Delivery price")]
        public int deliveryPrice { get; set; }

        [Required]
        [DisplayName("Taxes")]
        public int Taxes { get; set; }
    }
}
