using CourierConnect.Models.Dto;

namespace CourierConnectWeb.Services.IServices
{
	public interface IRequestService
	{
		Task<T> GetRequestAsync<T>(RequestSendDto requestSendDto);
		Task<T> GetRequestStatusAsync<T>(string companyRequestId);
		Task<T> AcceptRequestAsync<T>(string companyRequestId);
		Task<T> RejectRequestAsync<T>(string companyRequestId);


	}
}
