using AutoMapper;
using CourierConnect.Models.Dto.Currier;
using CourierConnect.Utility;
using CourierConnectWeb.Services.Currier;
using CourierConnectWeb.Services.IServices;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace CourierConnectWeb.Services.CourierHub
{
    public class CourierHubDeliveryService : IDeliveryService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private string apiUrl;
        private readonly IMapper _mapper;
        private readonly int _serviceId;

        public CourierHubDeliveryService(IHttpClientFactory clientFactory, IConfiguration configuration, IMapper mapper, int serviceId)
        {
            _clientFactory = clientFactory;
            apiUrl = configuration.GetValue<string>(SD.CurrierApiUrlSectionName);
            _configuration = configuration;
            _mapper = mapper;
            _serviceId = serviceId;
        }

        public async Task<T> GetNewDeliveryAsync<T>(string companyRequestId)
        {

        }
        public async Task<T> GetDeliveryAsync<T>(string companyDeliveryId)
        {

        }
        public async Task<T> CancelDeliveryAsync<T>(string companyDeliveryId)
        {

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
