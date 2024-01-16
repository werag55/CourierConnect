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

    //public Task<T> GetAllAsync<T>()
    //{
    //    return SendAsync<T>(new APIRequest()
    //    {
    //        ApiType = SD.ApiType.GET,
    //        Url = apiUrl + "/api/Delivery/GetDeliveries",
    //    }, _configuration.GetValue<string>(SD.SpecialApiKeySectionName));
    //}

    //public Task<T> GetAsync<T>(string courier)
    //{
    //    return SendAsync<T>(new APIRequest()
    //    {
    //        ApiType = SD.ApiType.GET,
    //        Data = courier,
    //        Url = apiUrl + "/api/Delivery/GetCourierDeliveries",
    //    }, _configuration.GetValue<string>(SD.SpecialApiKeySectionName));
    //}

    public Task<T> GetDeliveryAsync<T>(int deliveryId)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.GET,
            Url = apiUrl + $"/api/Delivery/GetDelivery/{deliveryId}",
        }, _configuration.GetValue<string>(SD.ApiKeySectionName));
    }

    public Task<T> GetDeliveryAsync<T>(RequestSendDto requestDto)
    {
        return SendAsync<T>(new APIRequest()
        {
            ApiType = SD.ApiType.POST,
            Data = requestDto,
            Url = apiUrl + "/api/Delivery/PostDelivery",
        }, _configuration.GetValue<string>(SD.ApiKeySectionName));
    }
}
