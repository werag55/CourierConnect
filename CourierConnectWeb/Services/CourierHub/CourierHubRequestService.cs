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

namespace CourierConnectWeb.Services.CourierHub
{
    public class CourierHubRequestService : CourierHubBaseService, IRequestService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private string apiUrl;
        private readonly IMapper _mapper;
        private readonly int _serviceId;

        public CourierHubRequestService(IHttpClientFactory clientFactory, IConfiguration configuration, IMapper mapper, int serviceId) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            apiUrl = configuration.GetValue<string>(SD.CourierHubApiUrlSectionName);
            _configuration = configuration;
            _mapper = mapper;
            _serviceId = serviceId;
        }

        public async Task<T> GetRequestAsync<T>(RequestSendDto requestSendDto)
        {
            CourierHubRequestSendDto courierHubRequestSendDto = _mapper.Map<CourierHubRequestSendDto>(requestSendDto);
            APIResponse apiResponse = await SendAsync<APIResponse>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = courierHubRequestSendDto,
                Url = apiUrl + "/api/Order",
            }, _configuration.GetValue<string>(SD.CourierHubApiKeySectionName));

            RequestResponseDto requestResponseDto = null;
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                requestResponseDto = _mapper.Map<RequestResponseDto>(courierHubRequestSendDto);
            }

            apiResponse.Result = requestResponseDto;

            var res = JsonConvert.SerializeObject(apiResponse);
            return JsonConvert.DeserializeObject<T>(res);
        }
        public async Task<T> GetRequestStatusAsync<T>(string companyRequestId)
        {
            APIResponse apiResponse = await SendAsync<APIResponse>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = apiUrl + $"/api/Order/Status/{companyRequestId}",
            }, _configuration.GetValue<string>(SD.CourierHubApiKeySectionName));

            RequestStatusDto requestStatusDto = null;
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                StatusType statusType = (StatusType)Enum.Parse(typeof(StatusType), Convert.ToString(apiResponse.Result)); //JsonConvert.DeserializeObject<StatusType>(Convert.ToString(apiResponse.Result));
                requestStatusDto = new RequestStatusDto
                {
                    isReady = (statusType == StatusType.Confirmed || statusType == StatusType.Denied) ? true : false
                };
            }
            apiResponse.Result = requestStatusDto;

            var res = JsonConvert.SerializeObject(apiResponse);
            return JsonConvert.DeserializeObject<T>(res);
        }

        public async Task<T> GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }
        public async Task<T> AcceptRequestAsync<T>(string companyRequestId)
        {
            throw new NotImplementedException();
        }
        public async Task<T> RejectRequestAsync<T>(string companyRequestId, string reason)
        {
            throw new NotImplementedException();
        }
    }
}
