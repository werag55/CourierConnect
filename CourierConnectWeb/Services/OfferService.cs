using CourierConnect.Models;
using CourierConnect.Models.Dto;
using CourierConnect.Utility;
using CourierConnectWeb.Services.IServices;

namespace CourierConnectWeb.Services
{
    public class OfferService : BaseService, IOfferService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private string offerUrl;

        public OfferService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            offerUrl = configuration.GetValue<string>("ServiceUrls:OfferAPI");
            _configuration = configuration;
        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = offerUrl + "/api/Offer/GetOffers",
            }, _configuration.GetValue<string>(SD.SpecialApiKeySectionName));
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = offerUrl + "/api/Offer/GetOffer/" + id,
            }, _configuration.GetValue<string>(SD.SpecialApiKeySectionName));
        }

        public Task<T> GetOfferAsync<T>(InquiryDto inquiryDto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = inquiryDto,
                Url = offerUrl + "/api/Offer/Get",
            }, _configuration.GetValue<string>(SD.ApiKeySectionName));
        }

    }
}
