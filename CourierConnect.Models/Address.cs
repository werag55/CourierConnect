﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierConnect.Models
{
    public class Address
    {
        public int Id { get; set; }

        public string streetName { get; set; }

        public int houseNumber { get; set; }

        public int? flatNumber { get; set; }

        public string postcode { get; set; }

        public string city { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append($"{streetName}, {houseNumber}");

            if (flatNumber != null)
            {
                sb.Append($"/{flatNumber}");
            }

            sb.Append("\n");
            sb.Append($"{postcode}, {city}");

            return sb.ToString();
        }
    }
}
