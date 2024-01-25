using CourierConnect.Models.Dto;

namespace CourierConnectWeb.Services.IServices
{
    public interface IDeliveryService
    {
		Task<T> GetNewDeliveryAsync<T>(string companyRequestId);
		Task<T> GetDeliveryAsync<T>(string companyDeliveryId);
		Task<T> CancelDeliveryAsync<T>(string companyDeliveryId);

		// Office worker:
		Task<T> GetAllAsync<T>();

		// Courier:
		Task<T> GetAllCourierDeliveryAsync<T>(string courierUserName);
        Task<T> PickUpPackageAsync<T>(string companyDeliveryId);
		Task<T> DeliverPackageAsync<T>(string companyDeliveryId);
		Task<T> CannotDeliverPackageAsync<T>(string companyDeliveryId);

	}
}
