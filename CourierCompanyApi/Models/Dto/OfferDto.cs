using System.ComponentModel.DataAnnotations;

namespace CourierCompanyApi.Models.Dto
{
    public class OfferDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public float price { get; set; }
    }
}
