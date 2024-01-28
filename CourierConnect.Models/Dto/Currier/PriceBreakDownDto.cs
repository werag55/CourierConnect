using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.Dto.Currier
{
    public class PriceBreakDownDto
    {
        public double amount {  get; set; }
        public string? currency {  get; set; }
        public string? description { get; set; }
    }
}
