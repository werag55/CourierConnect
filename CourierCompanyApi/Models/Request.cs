using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierCompanyApi.Models
{
    public class Request
    {
        public int Id { get; set; }
		public string GUID { get; set; }
		public int offerId { get; set; }
        public Offer offer { get; set; }
        public RequestStatus requestStatus { get; set; }
		public DateTime decisionDeadline { get; set; }
		public int personalDataId { get; set; }
        public PersonalData personalData { get; set; }
        //TODO: Agreement
        //TODO: Receipt
        public string? rejectionReason { get; set; }
    }

	public enum RequestStatus
	{
		Pending,
		Accepted,
		Rejected
	}
}
