using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.Dto.Currier
{
    public class DimensionsDto
    {
        public float width { get; set; }
        public float height { get; set; }
        public float length { get; set; }
        public string? dimensionUnit { get; set; }
    }
}
