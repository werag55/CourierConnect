using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.Dto.Currier
{
    public class CurrierAddressDto
    {
        public string? houseNumber {  get; set; }
        public string? apartamentNumber { get; set; }
        public string? street {  get; set; }
        public string? city { get; set; }
        public string? zipCode { get; set; }
        public string? country { get; set; }
    }
}
