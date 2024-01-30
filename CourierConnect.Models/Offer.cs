 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public string companyOfferId { get; set; }
        public int companyId { get; set; }
        public int inquiryId { get; set; }
        public Inquiry inquiry { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime updatedDate { get; set; } 
        public DateTime expirationDate { get; set; }
        public OfferStatus status { get; set; }
        public decimal price { get; set; }
        public decimal taxes { get; set; }
        public decimal fees { get; set; }
        public Currency currency { get; set; }

    }

    public enum OfferStatus
    {
        Pending,
        Accepted,
        Rejected
    }

    public enum Currency
    {
        PLN,
        EUR,
        USD,
        GBP
    }
}
