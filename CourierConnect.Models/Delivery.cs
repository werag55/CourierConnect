using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public string companyDeliveryId { get; set; }
        public int companyId { get; set; }
        public int requestId { get; set; }
        public Request request { get; set; }

    }

    public enum DeliveryStatus
    {
        Proccessing,
        PickedUp,
        Deliverd,
        CannotDeliver,
        Cancelled
    }
}
