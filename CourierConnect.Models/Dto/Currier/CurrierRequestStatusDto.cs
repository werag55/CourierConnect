using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.Dto.Currier
{
    public class CurrierRequestStatusDto
    {
        public string? offerId { get; set; }
        public bool isReady { get; set; }
        public DateTime timeStamp { get; set; }
    }
}
