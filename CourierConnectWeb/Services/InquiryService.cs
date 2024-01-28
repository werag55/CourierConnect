using CourierConnect.Models;
using CourierConnect.Utility;
using CourierConnectWeb.Services.IServices;

namespace CourierConnectWeb.Services
{
    public class InquiryService : BaseService, IInquiryService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private string apiUrl;

        public InquiryService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            apiUrl = configuration.GetValue<string>(SD.ApiUrlSectionName);
            _configuration = configuration;
        }

        public async Task<T> GetAllAsync<T>()
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = apiUrl + "/api/Inquiry/GetInquiries",
            }, _configuration.GetValue<string>(SD.SpecialApiKeySectionName));
        }
    }
}
