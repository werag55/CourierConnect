using System.ComponentModel.DataAnnotations;

namespace CourierCompanyApi.Models
{
    public class Offer
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime creationDate { get; set; }

        [Required]
        public float price { get; set; }
    }
}
