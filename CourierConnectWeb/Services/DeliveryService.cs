using CourierConnect.Models;
using CourierConnect.Models.Dto;
using CourierConnect.Utility;
using CourierConnectWeb.Services.IServices;
using CourierConnectWeb.Services;

public class DeliveryService : BaseService, IDeliveryService
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _clientFactory;
    private string apiUrl;

    public DeliveryService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
    {
        _clientFactory = clientFactory;
        apiUrl = configuration.GetValue<string>(SD.ApiUrlSectionName);
        _configuration = configuration;
    }

	public async Task<T> GetNewDeliveryAsync<T>(string companyRequestId)
    {
		return await SendAsync<T>(new APIRequest()
		{
			ApiType = SD.ApiType.POST,
			Url = apiUrl + $"/api/Delivery/PostDelivery/{companyRequestId}",
		}, _configuration.GetValue<string>(SD.ApiKeySectionName));
	}
	public async Task<T> GetDeliveryAsync<T>(string companyDeliveryId)
    {
		return await SendAsync<T>(new APIRequest()
		{
			ApiType = SD.ApiType.GET,
			Url = apiUrl + $"/api/Delivery/GetDelivery/{companyDeliveryId}",
		}, _configuration.GetValue<string>(SD.ApiKeySectionName));
	}
	public async Task<T> CancelDeliveryAsync<T>(string companyDeliveryId)
	{
		return await SendAsync<T>(new APIRequest()
		{
			ApiType = SD.ApiType.DELETE,
			Url = apiUrl + $"/api/Delivery/CancelDelivery/{companyDeliveryId}",
		}, _configuration.GetValue<string>(SD.ApiKeySectionName));
	}

	// Office worker:
	public async Task<T> GetAllAsync<T>()
	{
		return await SendAsync<T>(new APIRequest()
		{
			ApiType = SD.ApiType.GET,
			Url = apiUrl + "/api/Delivery/GetAllDeliveries",
		}, _configuration.GetValue<string>(SD.SpecialApiKeySectionName));
	}

	// Courier:
	public async Task<T> GetAllCourierDeliveryAsync<T>(string courierUserName)
	{
		return await SendAsync<T>(new APIRequest()
		{
			ApiType = SD.ApiType.GET,
			Data = courierUserName,
			Url = apiUrl + "/api/Delivery/GetCourierDeliveries",
		}, _configuration.GetValue<string>(SD.SpecialApiKeySectionName));
	}

	public async Task<T> PickUpPackageAsync<T>(string companyDeliveryId)
	{
		return await SendAsync<T>(new APIRequest()
		{
			ApiType = SD.ApiType.POST,
			Url = apiUrl + $"/api/Delivery/PickUpPackage/{companyDeliveryId}",
		}, _configuration.GetValue<string>(SD.SpecialApiKeySectionName));
	}

	public async Task<T> DeliverPackageAsync<T>(string companyDeliveryId)
	{
		return await SendAsync<T>(new APIRequest()
		{
			ApiType = SD.ApiType.POST,
			Url = apiUrl + $"/api/Delivery/DeliverPackage/{companyDeliveryId}",
		}, _configuration.GetValue<string>(SD.SpecialApiKeySectionName));
	}

	public async Task<T> CannotDeliverPackageAsync<T>(string companyDeliveryId)
	{
		return await SendAsync<T>(new APIRequest()
		{
			ApiType = SD.ApiType.POST,
			Url = apiUrl + $"/api/Delivery/CannotDeliverPackage/{companyDeliveryId}",
		}, _configuration.GetValue<string>(SD.SpecialApiKeySectionName));
	}
}
