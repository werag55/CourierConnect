namespace CourierConnect.Models.Dto
{
    public class InquiryDto
    {
        public DateTime pickupDate { get; set; }
        public DateTime deliveryDate { get; set; }
        public bool isPriority { get; set; }
        public bool weekendDelivery { get; set; }
        public bool isCompany { get; set; }
        public AddressDto sourceAddress { get; set; }
        public AddressDto destinationAddress { get; set; }
        public PackageDto package { get; set; }
    }
}
