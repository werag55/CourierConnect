using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.Dto.Currier
{
    public class CurrierRequestResponseDto
    {
        public string offerRequestId { get; set; }
        public DateTime validTo { get; set; }
    }
}
