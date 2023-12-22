using System.ComponentModel.DataAnnotations;

namespace CourierCompanyApi.Models.Dto
{
    public class OfferDto
    {
        public InquiryDto inquiry { get; set; }
        public DateTime expirationDate { get; set; }
        public decimal price { get; set; }
        //public string currency {  get; set; }
        public decimal taxes { get; set; }
        public decimal fees { get; set; }
    }
}
