using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.Dto
{
    public class RequestSendDto
    {
        //public OfferDto offer { get; set; }
        public string companyOfferId { get; set; }

        //public bool isAccepted { get; set; }
        public PersonalDataDto personalData { get; set; }
        //TODO: Agreement
        //TODO: Receipt
        //public string? rejectionReason { get; set; }
    }
}
