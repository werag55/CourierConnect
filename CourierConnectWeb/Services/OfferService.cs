using CourierConnect.Models;
using CourierConnect.Models.Dto;
using CourierConnect.Utility;
using CourierConnectWeb.Services.IServices;

namespace CourierConnectWeb.Services
{
    public class OfferService : BaseService, IOfferService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string offerUrl;

        public OfferService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            offerUrl = configuration.GetValue<string>("ServiceUrls:OfferAPI");

        }

        public Task<T> GetAllAsync<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = offerUrl + "/api/Offer/GetOffers",
            });
        }

        public Task<T> GetAsync<T>(int id)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = offerUrl + "/api/Offer/GetOffer/" + id,
            });
        }

        public Task<T> GetOfferAsync<T>(InquiryDto inquiryDto)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = inquiryDto,
                Url = offerUrl + "/api/Offer/Post",
            });
        }

    }
}
