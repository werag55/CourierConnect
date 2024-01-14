namespace CourierCompanyApi.Models.Dto
{
    public class RequestAcceptDto
    {
        public int companyOfferId { get; set; }
        public int companyDeliveryId { get; set; }
        public bool isAccepted { get; set; }
        //TODO: Agreement
        //TODO: Receipt
    }
}
