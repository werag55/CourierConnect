using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models
{
    public class Request
    {
        public int Id { get; set; }
        public int offerId { get; set; }
        public Offer offer { get; set; }
        public bool isAccepted { get; set; }
        public int personalDataId { get; set; }
        public PersonalData personalData { get; set; }
        //TODO: Agreement
        //TODO: Receipt
        public string? rejectionReason { get; set; }
    }
}
