using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Street name")]
        public string streetName { get; set; }

        [Required]
        [DisplayName("House number")]
        public int houseNumber { get; set; }

        [Required]
        [DisplayName("Flat number")]
        public int flatNumber { get; set; }

        [Required]
        [DisplayName("Postcode")]
        public string postcode { get; set; }
    }
}
