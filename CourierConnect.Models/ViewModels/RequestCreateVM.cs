using CourierConnect.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.ViewModels
{
    public class RequestCreateVM
    {
        public RequestSendDto requestSendDto { get; set; }
        public int offerId { get; set; }
    }
}
