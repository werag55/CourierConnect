using CourierConnect.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.ViewModels
{
    public class DeliveryVM
    {
        public DeliveryDto deliveryDto { get; set; }
        public Delivery delivery { get; set; }
    }
}
