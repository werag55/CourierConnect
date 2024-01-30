using Microsoft.AspNetCore.Mvc;

namespace CourierConnect.Models.Dto
{
    public class RequestRejectDto
    {
        public int companyOfferId { get; set; }

        public bool isAccepted { get; set; }

        public string? rejectionReason { get; set; }
    }
}
