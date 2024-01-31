using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.POCO
{
    public class OfferPOCO
    {
        public string companyOfferId { get; set; }
        public int companyId { get; set; }
        public InquiryPOCO inquiry { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime expirationDate { get; set; }
        public decimal price { get; set; }
        public decimal taxes { get; set; }
        public decimal fees { get; set; }
        public Currency currency { get; set; }
    }
}
