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
    public class CurrierOfferService : IOfferService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private string apiUrl;
        private readonly IMapper _mapper;
        private readonly int _serviceId;

        public CurrierOfferService(IHttpClientFactory clientFactory, IConfiguration configuration, IMapper mapper, int serviceId)
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

        public async Task<T> GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetOfferAsync<T>(InquiryDto inquiryDto)
        {
            var client = _clientFactory.CreateClient();

            string token = await GetTokenAsync();
            CurrierInquiryDto currierInquiryDto = _mapper.Map<CurrierInquiryDto>(inquiryDto);

            HttpRequestMessage message = new HttpRequestMessage();
            message.RequestUri = new Uri(apiUrl + "/Inquires");
            message.Content = new StringContent(JsonConvert.SerializeObject(currierInquiryDto),
                        Encoding.UTF8, "application/json");
            message.Method = HttpMethod.Post;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage apiResponse = null;
            apiResponse = await client.SendAsync(message);
            apiResponse.EnsureSuccessStatusCode();

            OfferDto offerDto = null;
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                CurrierOfferDto currierOfferDto = JsonConvert.DeserializeObject<CurrierOfferDto>(apiContent);
                offerDto = _mapper.Map<OfferDto>(currierOfferDto);
                offerDto.companyId = _serviceId;
            }

            APIResponse APIResponse = new APIResponse
            {
                StatusCode = apiResponse.StatusCode,
                IsSuccess = apiResponse.StatusCode == System.Net.HttpStatusCode.OK,
                Result = offerDto
            };

            var res = JsonConvert.SerializeObject(APIResponse);
            return JsonConvert.DeserializeObject<T>(res);
        }
        


    }
}
