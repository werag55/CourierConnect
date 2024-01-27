using AutoMapper;
using CourierConnect.Models.Dto;
using CourierConnect.Models.Dto.CourierHub;
using CourierConnect.Utility;
using CourierConnectWeb.Services.IServices;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace CourierConnectWeb.Services.CourierHub
{
    public class CourierHubOfferService : IOfferService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private string apiUrl;
        private readonly IMapper _mapper;
        private readonly int _serviceId;

        public CourierHubOfferService(IHttpClientFactory clientFactory, IConfiguration configuration, IMapper mapper, int serviceId)
        {
            _clientFactory = clientFactory;
            apiUrl = configuration.GetValue<string>(SD.CurrierApiUrlSectionName);
            _configuration = configuration;
            _mapper = mapper;
            _serviceId = serviceId;
        }

        public async Task<T> GetAllAsync<T>()
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetOfferAsync<T>(InquiryDto inquiryDto)
        {
            
        }
    }
}
