using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.Dto.Currier
{
    public class CurrierRequestSendDto
    {
        public string inquiryId { get; set; }
        public string? name { get; set; }
        public string? email { get; set; }
        public CurrierAddressDto address { get; set; }
    }
}
