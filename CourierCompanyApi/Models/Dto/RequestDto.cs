namespace CourierCompanyApi.Models.Dto
{
    public class RequestDto
    {
        public OfferDto offer { get; set; }
        public RequestStatusDto requestStatus { get; set; }
        public PersonalDataDto personalData { get; set; }
        //TODO: Agreement
        //TODO: Receipt
        public string? rejectionReason { get; set; }
    }
}
