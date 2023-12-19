using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models
{
    public class Package
    {
        public int Id { get; set; }
        public double width { get; set; }
        public double height { get; set; }
        public double length { get; set; }
        public string dimensionsUnit { get; set; }
        public double weight { get; set; }
        public string weightUnit { get; set; }
    }
}
