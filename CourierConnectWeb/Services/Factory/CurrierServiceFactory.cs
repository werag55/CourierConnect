using CourierConnectWeb.Services.IServices;
using CourierConnectWeb.Services.Currier;
using AutoMapper;

namespace CourierConnectWeb.Services.Factory
{
    public class CurrierServiceFactory : IServiceFactory
    {
        public int serviceId { get; } = 1;
        public IHttpClientFactory _httpClient { get; set; }

        private IConfiguration _configuration;
        private IMapper _mapper;
        public CurrierServiceFactory(IHttpClientFactory httpClient, IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _mapper = mapper;
        }
        public IDeliveryService createDeliveryService()
        {
            return new CurrierDeliveryService(_httpClient, _configuration, _mapper, serviceId);
        }
        public IOfferService createOfferService()
        {
            return new CurrierOfferService(_httpClient, _configuration, _mapper, serviceId);
        }
        public IRequestService createRequestService()
        {
            return new CurrierRequestService(_httpClient, _configuration, _mapper, serviceId);
        }

        public IInquiryService createInquiryService()
        {
            throw new NotImplementedException();
        }
    }
}
