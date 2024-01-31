using CourierConnect.Models.POCO;

namespace CourierConnect.Models.ViewModels
{
    public class ClientInquiryVM
    {
        public InquiryPOCO Inquiry { get; set; }
        public bool hasDelivery { get; set; }

        public ClientInquiryVM(InquiryPOCO inquiry, bool hasDelivery)
        {
            this.Inquiry = inquiry;
            this.hasDelivery = hasDelivery;
        }
    }
}
