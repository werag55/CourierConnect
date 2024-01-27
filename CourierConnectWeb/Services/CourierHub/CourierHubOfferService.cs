using AutoMapper;
using CourierConnect.Models.Dto;
using CourierConnect.Models.Dto.CourierHub;
using CourierConnect.Utility;
using CourierConnectWeb.Services.IServices;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using CourierConnect.Models;
using CourierConnect.Models.Dto.Currier;

namespace CourierConnectWeb.Services.CourierHub
{
    public class CourierHubOfferService : CourierHubBaseService, IOfferService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private string apiUrl;
        private readonly IMapper _mapper;
        private readonly int _serviceId;

        public CourierHubOfferService(IHttpClientFactory clientFactory, IConfiguration configuration, IMapper mapper, int serviceId) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            apiUrl = configuration.GetValue<string>(SD.CourierHubApiUrlSectionName);
            _configuration = configuration;
            _mapper = mapper;
            _serviceId = serviceId;
        }

        public async Task<T> GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetOfferAsync<T>(InquiryDto inquiryDto)
        {
            CourierHubInquiryDto courierHubInquiryDto = _mapper.Map<CourierHubInquiryDto>(inquiryDto);
            APIResponse apiResponse = await SendAsync<APIResponse>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = courierHubInquiryDto,
                Url = apiUrl + "/api/Inquire",
            }, _configuration.GetValue<string>(SD.CourierHubApiKeySectionName));

            OfferDto offerDto = null;
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.Created)
            {
                CourierHubOfferDto courierHubOfferDto = JsonConvert.DeserializeObject<CourierHubOfferDto>(Convert.ToString(apiResponse.Result));
                offerDto = _mapper.Map<OfferDto>(courierHubOfferDto);
                offerDto.inquiry = inquiryDto;
                offerDto.companyId = _serviceId;
            }

            apiResponse.Result = offerDto;

            var res = JsonConvert.SerializeObject(apiResponse);
            return JsonConvert.DeserializeObject<T>(res);
        }
    }
}
