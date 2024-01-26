using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierCompanyApi.Models
{
    public class Delivery
    {
        public int Id { get; set; }
        public string GUID { get; set; }
        public int courierId { get; set; }
        public Courier courier { get; set; }
        public int requestId { get; set; }
        public Request request { get; set; }
        public DateTime cancelationDeadline { get; set; }
        public DateTime? pickUpDate { get; set; }
        public DateTime? deliveryDate { get; set; }
        public DeliveryStatus deliveryStatus { get; set; }
        public string? reason { get; set; }

    }

    public enum DeliveryStatus
    {
        Proccessing,
        PickedUp,
        Delivered,
        CannotDeliver,
        Cancelled
    }
}
