using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.ViewModels
{
    public class ClientInquiryVM
    {
        public Inquiry Inquiry { get; set; }
        public bool hasDelivery { get; set; }

        public ClientInquiryVM(Inquiry inquiry, bool hasDelivery)
        {
            this.Inquiry = inquiry;
            this.hasDelivery = hasDelivery;
        }
    }
}
