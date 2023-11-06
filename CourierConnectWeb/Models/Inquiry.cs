using System.ComponentModel.DataAnnotations;

namespace CourierHubWeb.Models
{
    public class Inquiry
    {
        [Key] public int Id { get; set; }
        public int MyProperty { get; set; }
    }
}
