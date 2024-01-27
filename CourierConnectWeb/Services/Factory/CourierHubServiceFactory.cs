using AutoMapper;
using CourierConnectWeb.Services.CourierHub;
using CourierConnectWeb.Services.IServices;
using CourierConnect.DataAccess.Repository.IRepository;

namespace CourierConnectWeb.Services.Factory
{
    public class CourierHubServiceFactory : IServiceFactory
    {
        public int serviceId { get; } = 2;
        public IHttpClientFactory _httpClient { get; set; }
        private IUnitOfWork _unitOfWork;
        private IConfiguration _configuration;
        private IMapper _mapper;
        public CourierHubServiceFactory(IUnitOfWork unitOfWork, IHttpClientFactory httpClient, IConfiguration configuration, IMapper mapper)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public IDeliveryService createDeliveryService()
        {
            return new CourierHubDeliveryService(_unitOfWork, _httpClient, _configuration, _mapper, serviceId);
        }
        public IOfferService createOfferService()
        {
            return new CourierHubOfferService(_httpClient, _configuration, _mapper, serviceId);
        }
        public IRequestService createRequestService()
        {
            return new CourierHubRequestService(_httpClient, _configuration, _mapper, serviceId);
        }

        public IInquiryService createInquiryService()
        {
            throw new NotImplementedException();
        }
    }
}
