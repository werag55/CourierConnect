using CourierConnect.Models.Dto;

namespace CourierConnectWeb.Services.IServices
{
    public interface IOfferService
    {
        Task<T> GetAllAsync<T>();
        Task<T> GetAsync<T>(int id);
        Task<T> GetOfferAsync<T>(InquiryDto inquiryDto);
    }
}
