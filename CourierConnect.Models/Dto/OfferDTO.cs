using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models.Dto
{
    public class OfferDTO
    {
        public int Id { get; set; }
        public float price { get; set; }
    }
}
