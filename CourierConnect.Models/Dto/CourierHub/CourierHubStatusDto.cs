using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.Dto.CourierHub
{
    public enum StatusType
    {
        NotConfirmed,
        Confirmed,
        Cancelled,
        Denied,
        PickedUp,
        Delivered,
        CouldNotDeliver
    }
}
