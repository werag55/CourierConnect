namespace CourierConnect.Models.Dto
{
    public class DeliveryDto
    {
        public string companyDeliveryId { get; set; }
        public CourierDto courier { get; set; }
        public RequestDto request { get; set; }
        public DateTime cancelationDeadline { get; set; }
        public DateTime? pickUpDate { get; set; }
        public DateTime? deliveryDate { get; set; }
        public DeliveryStatus deliveryStatus { get; set; }
        public string? reason { get; set; }
    }
}
