using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models
{
    public class Package
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DisplayName("Package width")]
        public double PackageWidth { get; set; }

        [Required]
        [DisplayName("Package height")]
        public double PackageHeight { get; set; }

        [Required]
        [DisplayName("Package length")]
        public double PackageLength { get; set; }

        [Required]
        [DisplayName("Package unit")]
        public required string PackageUnit { get; set; }

        [Required]
        [DisplayName("Package Weight")]
        public double PackageWeight { get; set; }
    }
}
