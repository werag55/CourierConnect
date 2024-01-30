using Microsoft.AspNetCore.Mvc;

namespace CourierCompanyApi.Models.Dto
{
    public class RequestRejectDto
    {
        public RequestStatus requestStatus { get; set; }

        public string? rejectionReason { get; set; }
    }
}
