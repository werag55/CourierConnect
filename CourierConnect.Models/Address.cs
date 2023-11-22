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
        public required string StreetName { get; set; }

        [Required]
        [DisplayName("House number")]
        public required string HouseNumber { get; set; }

        [DisplayName("Flat number")]
        public required string FlatNumber { get; set; }

        [DisplayName("Postcode")]
        public required string Postcode { get; set; }
    }
}
