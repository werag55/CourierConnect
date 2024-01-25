using CourierConnectWeb.Services.IServices;

namespace CourierConnectWeb.Services.Factory
{
    public interface IServiceFactory
    {
        public int serviceId { get; }
        public IDeliveryService createDeliveryService();
        public IOfferService createOfferService();
        public IRequestService createRequestService();
    }
}
