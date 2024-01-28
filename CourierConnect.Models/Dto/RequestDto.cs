namespace CourierConnect.Models.Dto
{
    public class RequestDto
    {
        public string companyRequestId { get; set; }
        public OfferDto offer { get; set; }
        public RequestStatus requestStatus { get; set; }
        public DateTime decisionDeadline { get; set; }
        public PersonalDataDto personalData { get; set; }
        //TODO: Agreement
        //TODO: Receipt
        public string? rejectionReason { get; set; }
    }
}
