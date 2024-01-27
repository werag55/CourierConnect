using CourierConnectWeb.Services.IServices;

namespace CourierConnectWeb.Services.Factory
{
    public class OurServiceFactory : IServiceFactory
    {
        public int serviceId { get; } = 0;
        public IHttpClientFactory _httpClient { get; set; }

        private IConfiguration _configuration;
        public OurServiceFactory(IHttpClientFactory httpClient,IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }
        public IDeliveryService createDeliveryService()
        {
            return new DeliveryService(_httpClient, _configuration);
        }
        public IOfferService createOfferService()
        {
            return new OfferService(_httpClient, _configuration);
        }
        public IRequestService createRequestService()
        {
            return new RequestService(_httpClient, _configuration);
        }

        public IInquiryService createInquiryService()
        {
            return new InquiryService(_httpClient, _configuration);
        }
    }
}
