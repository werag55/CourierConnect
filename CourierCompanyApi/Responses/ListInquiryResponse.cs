using CourierCompanyApi.Models.Dto;
using System.Net;

namespace CourierCompanyApi.Responses
{
    public class ListInquiryResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessages { get; set; }
        public List<InquiryDto> Result { get; set; }
    }
}
