namespace CourierCompanyApi.Models.Dto
{
    public class InquiryDto
    {
        public DateTime pickupDate { get; set; }
        public DateTime deliveryDate { get; set; }
        public bool isPriority { get; set; }
        public bool weekendDelivery { get; set; }
        public bool isCompany { get; set; }
        public Address sourceAddress { get; set; }
        public Address destinationAddress { get; set; }
        public Package package { get; set; }
    }
}
