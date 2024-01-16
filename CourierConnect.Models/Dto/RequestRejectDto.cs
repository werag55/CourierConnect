using Microsoft.AspNetCore.Mvc;

namespace CourierConnect.Models.Dto
{
    public class RequestRejectDto
    {
		public RequestStatus requestStatus { get; set; }

		public string? rejectionReason { get; set; }
	}
}
