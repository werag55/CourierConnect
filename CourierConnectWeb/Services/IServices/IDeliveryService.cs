using CourierConnect.Models.Dto;

namespace CourierConnectWeb.Services.IServices
{
    public interface IDeliveryService
    {
        //Task<T> GetAllAsync<T>();
        //Task<T> GetAsync<T>(string courier);
        public Task<T> GetDeliveryAsync<T>(int deliveryId);
        Task<T> GetDeliveryAsync<T>(RequestSendDto requestDto);
    }
}
