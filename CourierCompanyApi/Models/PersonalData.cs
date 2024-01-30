using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierCompanyApi.Models
{
    public class PersonalData
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string surname { get; set; }
        public string companyName { get; set; }
        public int addressId { get; set; }
        public Address address { get; set; }
        public string email { get; set; }
	}
}
