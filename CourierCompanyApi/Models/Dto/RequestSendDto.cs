using Microsoft.AspNetCore.Mvc;

namespace CourierCompanyApi.Models.Dto
{
    public class RequestSendDto
    {
        //public OfferDto offer { get; set; }
        public int companyOfferId { get; set; }

        //public bool isAccepted { get; set; }
        public PersonalDataDto personalData { get; set; }
        //TODO: Agreement
        //TODO: Receipt
        //public string? rejectionReason { get; set; }
    }
}
