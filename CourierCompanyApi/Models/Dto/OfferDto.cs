using System.ComponentModel.DataAnnotations;

namespace CourierCompanyApi.Models.Dto
{
    public class OfferDto
    {
        public int companyOfferId { get; set; }
        public InquiryDto inquiry { get; set; }
        public DateTime creationDate { get; set; }
        public DateTime expirationDate { get; set; }
        public decimal price { get; set; }
        public decimal taxes { get; set; }
        public decimal fees { get; set; }
        public Currency currency { get; set; }
    }
}
