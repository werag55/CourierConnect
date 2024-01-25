﻿using CourierConnect.Models;
using CourierConnect.Models.Dto;
using CourierConnect.Utility;
using CourierConnectWeb.Services.IServices;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CourierConnectWeb.Services
{
    public class OfferService : BaseService, IOfferService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private string apiUrl;

        public OfferService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
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
                Url = /*"https://couriercompanyapi.azurewebsites.net"*/ apiUrl + "/api/Offer/GetOffers",
            }, _configuration.GetValue<string>(SD.SpecialApiKeySectionName));
        }

        //public Task<T> GetAsync<T>(string courier)
        //{
        //    return SendAsync<T>(new APIRequest()
        //    {
        //        ApiType = SD.ApiType.GET,
        //        Data = courier,
        //        Url = offerUrl + "/api/Offer/GetourierOffers",
        //    }, _configuration.GetValue<string>(SD.SpecialApiKeySectionName));
        //}

        public async Task<T> GetOfferAsync<T>(InquiryDto inquiryDto)
        {
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = inquiryDto,
                Url = apiUrl + "/api/Offer/PostOffer",
            }, _configuration.GetValue<string>(SD.ApiKeySectionName));
        }

        public async Task<string> GetTokenAsync(string companyName)
        {
            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, _settings.TokenEndpointSzymon)
            {
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    ["grant_type"] = "client_credentials",
                    ["client_id"] = _settings.ClientIdSzymon,
                    ["client_secret"] = _settings.ClientSecretSzymon,
                    ["scope"] = _settings.Scope
                })
            };

            var tokenResponse = await _httpTokenSzymonClient.SendAsync(tokenRequest);
            tokenResponse.EnsureSuccessStatusCode();

            var tokenResult = await tokenResponse.Content.ReadAsStringAsync();


            using (var doc = JsonDocument.Parse(tokenResult))
            {
                var accessToken = doc.RootElement.GetProperty("access_token").GetString();
                return accessToken;
            }
        }


        public async Task<OfferRespondDTO> GetOfferOurID(OfferDTO offerdto)
        {

            OfferSzymonApiDTO data = new OfferSzymonApiDTO
            {
                Address = offerdto.Address,
                Name = offerdto.firstName,
                InquiryId = offerdto.InquiryId,
                Email = offerdto.Email

            };

            var response = await _httpOurClientOffer.PostAsJsonAsync("/offers", data);
            var responseBody = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            var respond = await response.Content.ReadFromJsonAsync<OfferRespondDTO>();

            return respond;
        }

        public async Task<OfferRespondDTO> GetOfferSzymonID(OfferDTO offerdto)
        {

            OfferSzymonApiDTO data = new OfferSzymonApiDTO
            {
                Address = offerdto.Address,
                Name = offerdto.firstName,
                InquiryId = offerdto.InquiryId,
                Email = offerdto.Email

            };

            var inquirymessage = new HttpRequestMessage(HttpMethod.Post, $"{_httpSzymonClientOffer.BaseAddress}Offers")
            {
                Content = JsonContent.Create(data)
            };


            var accessToken = await GetTokenAsync("SzymonCompany");

            inquirymessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await _httpSzymonClientOffer.SendAsync(inquirymessage);
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();


            var respond = await response.Content.ReadFromJsonAsync<OfferRespondDTO>();



            return respond;
        }

    }
}
