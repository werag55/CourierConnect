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

    }
}
