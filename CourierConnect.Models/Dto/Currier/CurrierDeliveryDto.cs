using CourierConnect.Models.Dto.Currier;

namespace CourierConnectWeb.Services.Currier
{
    public class CurrierDeliveryDto
    {
        public string offerId { get; set; }
        public DimensionsDto dimensions { get; set; }
        public CurrierAddressDto source { get; set; }
        public CurrierAddressDto destination { get; set; }
        public float weight { get; set; }
        public string? weightUnit { get; set; }
        public DateTime pickupDate { get; set; }
        public DateTime deliveryDate { get; set; }
        public DateTime validTo { get; set; }
        public bool deliveryInWeekend { get; set; }
        public string? priority { get; set; }
        public bool vipPackage { get; set; }
        //public List<PriceBreakDownDto> priceBreakDown { get; set; }
        public double totalPrice { get; set; }
        public string? currency { get; set; }
        public DateTime inquireDate { get; set; }
        public DateTime offerRequestDate { get; set; }
        public DateTime decisionDate { get; set; }
        public string? offerStatus { get; set; }
        public string? buyerName { get; set; }
        public CurrierAddressDto buyerAddress { get; set; }
    }
}
