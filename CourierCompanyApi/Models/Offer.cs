 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierCompanyApi.Models
{
    public class Offer
    {
        public int Id { get; set; }
        public int inquiryId { get; set; }
        public Inquiry inquiry { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime updatedDate { get; set; } 
        public DateTime expirationDate { get; set; }
        public OfferStatus status { get; set; }
        public decimal price { get; set; }
        //public string currency {  get; set; }
        public decimal taxes { get; set; }
        public decimal fees { get; set; }   

    }

    public enum OfferStatus
    {
        Pending,
        Accepted,
        Rejected
    }
}
