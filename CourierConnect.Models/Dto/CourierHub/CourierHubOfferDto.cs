using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.Dto.CourierHub
{
    public class CourierHubOfferDto
    {
        public double price { get; set; }
        public string? code { get; set; }
        public DateTime expirationDate { get; set; }
    }
}
