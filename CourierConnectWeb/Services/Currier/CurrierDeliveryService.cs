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
    public class CurrierDeliveryService : IDeliveryService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private string apiUrl;
        private readonly IMapper _mapper;
        private readonly int _serviceId;

        public CurrierDeliveryService(IHttpClientFactory clientFactory, IConfiguration configuration, IMapper mapper, int serviceId)
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

        public async Task<T> GetNewDeliveryAsync<T>(string companyRequestId)
        {
            var client = _clientFactory.CreateClient();

            string token = await GetTokenAsync();

            HttpRequestMessage message = new HttpRequestMessage();
            message.RequestUri = new Uri(apiUrl + $"/offer/request/{companyRequestId}/status");
            message.Method = HttpMethod.Get;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage apiResponse = null;
            apiResponse = await client.SendAsync(message);

            RequestAcceptDto requestAcceptDto = null;
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                CurrierRequestStatusDto currierRequestStatusDto = JsonConvert.DeserializeObject<CurrierRequestStatusDto>(apiContent);
                requestAcceptDto = _mapper.Map<RequestAcceptDto>(currierRequestStatusDto);
            }

            APIResponse APIResponse = new APIResponse
            {
                StatusCode = apiResponse.StatusCode,
                IsSuccess = apiResponse.StatusCode == System.Net.HttpStatusCode.OK,
                Result = requestAcceptDto
            };

            var res = JsonConvert.SerializeObject(APIResponse);
            return JsonConvert.DeserializeObject<T>(res);
        }
        public async Task<T> GetDeliveryAsync<T>(string companyDeliveryId)
        {
            var client = _clientFactory.CreateClient();

            string token = await GetTokenAsync();

            HttpRequestMessage message = new HttpRequestMessage();
            message.RequestUri = new Uri(apiUrl + $"/offer/{companyDeliveryId}");
            message.Method = HttpMethod.Get;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage apiResponse = null;
            apiResponse = await client.SendAsync(message);

            DeliveryDto deliveryDto = null;
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                CurrierDeliveryDto currierDeliveryDto = JsonConvert.DeserializeObject<CurrierDeliveryDto>(apiContent);
                deliveryDto = _mapper.Map<DeliveryDto>(currierDeliveryDto);
            }

            APIResponse APIResponse = new APIResponse
            {
                StatusCode = apiResponse.StatusCode,
                IsSuccess = apiResponse.StatusCode == System.Net.HttpStatusCode.OK,
                Result = deliveryDto
            };

            var res = JsonConvert.SerializeObject(APIResponse);
            return JsonConvert.DeserializeObject<T>(res);
        }
        public async Task<T> CancelDeliveryAsync<T>(string companyDeliveryId)
        {
            var client = _clientFactory.CreateClient();

            string token = await GetTokenAsync();

            HttpRequestMessage message = new HttpRequestMessage();
            message.RequestUri = new Uri(apiUrl + $"/offer/{companyDeliveryId}/cancel");
            message.Method = HttpMethod.Delete;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage apiResponse = null;
            apiResponse = await client.SendAsync(message);

            APIResponse APIResponse = new APIResponse
            {
                StatusCode = apiResponse.StatusCode,
                IsSuccess = apiResponse.StatusCode == System.Net.HttpStatusCode.OK,
            };

            var res = JsonConvert.SerializeObject(APIResponse);
            return JsonConvert.DeserializeObject<T>(res);
        }

        // Office worker:
        public async Task<T> GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        // Courier:
        public async Task<T> GetAllCourierDeliveryAsync<T>(string courierUserName)
        {
            throw new NotImplementedException();
        }
        public async Task<T> PickUpPackageAsync<T>(string companyDeliveryId)
        {
            throw new NotImplementedException();
        }
        public async Task<T> DeliverPackageAsync<T>(string companyDeliveryId)
        {
            throw new NotImplementedException();
        }
        public async Task<T> CannotDeliverPackageAsync<T>(string companyDeliveryId, string reason)
        {
            throw new NotImplementedException();
        }
    }
}
