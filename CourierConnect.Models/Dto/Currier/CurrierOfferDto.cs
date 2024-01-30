using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.Dto.Currier
{
    public class CurrierOfferDto
    {
        public string inquiryId {  get; set; }
        public double totalPrice { get; set; }
        public string? currency {  get; set; }
        public DateTime expiringAt { get; set; }
        //public List<PriceBreakDownDto> priceBreakDown { get; set; }
    }
}
