using AutoMapper;
using AutoMapper.Internal;
using CourierConnect.Models;
using CourierConnect.Models.Dto;
using CourierConnect.Models.Dto.Currier;
using CourierConnect.Utility;
using CourierConnectWeb.Services.IServices;
using Microsoft.CodeAnalysis.Elfie.Model.Strings;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CourierConnectWeb.Services.Currier
{
    public class CurrierRequestService : IRequestService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private string apiUrl;
        private readonly IMapper _mapper;
        private readonly int _serviceId;

        public CurrierRequestService(IHttpClientFactory clientFactory, IConfiguration configuration, IMapper mapper, int serviceId)
        {
            _clientFactory = clientFactory;
            apiUrl = configuration.GetValue<string>(SD.CurrierApiUrlSectionName);
            _configuration = configuration;
            _mapper = mapper;
            _serviceId = serviceId;
        }

        public async Task<string> GetTokenAsync()
        {
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, _configuration.GetValue<string>(SD.TokenUrlSectionName))
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = _configuration.GetValue<string>(SD.CurrierGrandTypeSectionName),
                    ["client_id"] = _configuration.GetValue<string>(SD.CurrierClientIdSectionName),
                    ["client_secret"] = _configuration.GetValue<string>(SD.CurrierClientSecretSectionName),
                    ["scope"] = _configuration.GetValue<string>(SD.CurrierScopeSectionName)
                })
            };

            var client = _clientFactory.CreateClient();
            var tokenResponse = await client.SendAsync(tokenRequest);
            tokenResponse.EnsureSuccessStatusCode();

            var tokenResult = await tokenResponse.Content.ReadAsStringAsync();


            using (var doc = JsonDocument.Parse(tokenResult))
            {
                var accessToken = doc.RootElement.GetProperty("access_token").GetString();
                return accessToken;
            }
        }

        public async Task<T> GetRequestAsync<T>(RequestSendDto requestSendDto)
        {
            var client = _clientFactory.CreateClient();

            string token = await GetTokenAsync();
            CurrierRequestSendDto currierRequestSendDto = _mapper.Map<CurrierRequestSendDto>(requestSendDto);

            HttpRequestMessage message = new HttpRequestMessage();
            message.RequestUri = new Uri(apiUrl + "/Offers");
            message.Content = new StringContent(JsonConvert.SerializeObject(currierRequestSendDto),
                        Encoding.UTF8, "application/json");
            message.Method = HttpMethod.Post;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage apiResponse = null;
            apiResponse = await client.SendAsync(message);

            RequestResponseDto requestResponseDto = null;
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                CurrierRequestResponseDto currierRequestResponseDto = JsonConvert.DeserializeObject<CurrierRequestResponseDto>(apiContent);
                requestResponseDto = _mapper.Map<RequestResponseDto>(currierRequestResponseDto);
                //requestResponseDto.companyId = _serviceId;
            }

            APIResponse APIResponse = new APIResponse
            {
                StatusCode = apiResponse.StatusCode,
                IsSuccess = apiResponse.StatusCode == System.Net.HttpStatusCode.OK,
                Result = requestResponseDto
            };

            var res = JsonConvert.SerializeObject(APIResponse);
            return JsonConvert.DeserializeObject<T>(res);
        }
        public async Task<T> GetRequestStatusAsync<T>(string companyRequestId)
        {
            var client = _clientFactory.CreateClient();

            string token = await GetTokenAsync();

            HttpRequestMessage message = new HttpRequestMessage();
            message.RequestUri = new Uri(apiUrl + $"/offer/request/{companyRequestId}/status");
            message.Method = HttpMethod.Get;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage apiResponse = null;
            apiResponse = await client.SendAsync(message);

            RequestStatusDto requestStatusDto = null;
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                CurrierRequestStatusDto currierRequestStatusDto = JsonConvert.DeserializeObject<CurrierRequestStatusDto>(apiContent);
                requestStatusDto = _mapper.Map<RequestStatusDto>(currierRequestStatusDto);
            }

            APIResponse APIResponse = new APIResponse
            {
                StatusCode = apiResponse.StatusCode,
                IsSuccess = apiResponse.StatusCode == System.Net.HttpStatusCode.OK,
                Result = requestStatusDto
            };

            var res = JsonConvert.SerializeObject(APIResponse);
            return JsonConvert.DeserializeObject<T>(res);
        }


        public async Task<T> AcceptRequestAsync<T>(string companyRequestId)
        {
            throw new NotImplementedException();
        }
        public async Task<T> RejectRequestAsync<T>(string companyRequestId)
        {
            throw new NotImplementedException();
        }
    }
}
