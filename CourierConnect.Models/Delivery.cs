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
        //TODO: courierId
        public int requestId { get; set; }
        public Request request { get; set; }
        public DateTime pickUpDate { get; set; }
        public DateTime? deliveryDate { get; set; }
        public DeliveryStatus deliveryStatus { get; set; }
        public string? reason { get; set; }

    }

    public enum DeliveryStatus
    {
        PickedUp,
        Deliverd,
        CannotDeliver
    }
}
