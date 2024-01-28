namespace CourierConnectWeb.Services.IServices
{
    public interface IInquiryService
    {
        Task<T> GetAllAsync<T>();
    }
}
