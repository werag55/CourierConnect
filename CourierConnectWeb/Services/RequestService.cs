using CourierConnect.Models;
using CourierConnect.Models.Dto;
using CourierConnect.Utility;
using CourierConnectWeb.Services.IServices;

namespace CourierConnectWeb.Services
{
	public class RequestService : BaseService, IRequestService
	{
		private readonly IConfiguration _configuration;
		private readonly IHttpClientFactory _clientFactory;
		private string apiUrl;

		public RequestService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
		{
			_clientFactory = clientFactory;
			apiUrl = configuration.GetValue<string>(SD.ApiUrlSectionName);
			_configuration = configuration;
		}

		public async Task<T> GetRequestAsync<T>(RequestSendDto requestSendDto)
		{
			return await SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.POST,
				Data = requestSendDto,
				Url = apiUrl + "/api/Request/PostRequest",
			}, _configuration.GetValue<string>(SD.ApiKeySectionName));
		}

		public async Task<T> GetRequestStatusAsync<T>(string companyRequestId)
		{
			return await SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.GET,
				Url = apiUrl + $"/api/Request/GetRequestStatus/{companyRequestId}",
			}, _configuration.GetValue<string>(SD.ApiKeySectionName));
		}

		public async Task<T> AcceptRequestAsync<T>(string companyRequestId)
		{
			return await SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.POST,
				Url = apiUrl + $"/api/Request/AcceptRequest/{companyRequestId}",
			}, _configuration.GetValue<string>(SD.SpecialApiKeySectionName));
		}
		public async Task<T> RejectRequestAsync<T>(string companyRequestId)
		{
			return await SendAsync<T>(new APIRequest()
			{
				ApiType = SD.ApiType.POST,
				Url = apiUrl + $"/api/Request/RejectRequest/{companyRequestId}",
			}, _configuration.GetValue<string>(SD.SpecialApiKeySectionName));
		}
	}
}
