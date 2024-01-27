using AutoMapper;
using CourierConnect.Models;
using CourierConnect.Models.Dto.CourierHub;
using CourierConnect.Models.Dto;
using CourierConnect.Utility;
using CourierConnectWeb.Services.IServices;
using Newtonsoft.Json;
using CourierConnect.DataAccess.Repository.IRepository;

namespace CourierConnectWeb.Services.CourierHub
{
    public class CourierHubDeliveryService : CourierHubBaseService, IDeliveryService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _clientFactory;
        private string apiUrl;
        private readonly IMapper _mapper;
        private readonly int _serviceId;
        private readonly IUnitOfWork _unitOfWork;

        public CourierHubDeliveryService(IUnitOfWork unitOfWork, IHttpClientFactory clientFactory, IConfiguration configuration, IMapper mapper, int serviceId) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            apiUrl = configuration.GetValue<string>(SD.CourierHubApiUrlSectionName);
            _configuration = configuration;
            _mapper = mapper;
            _serviceId = serviceId;
            _unitOfWork = unitOfWork;
        }

        public async Task<T> GetNewDeliveryAsync<T>(string companyRequestId)
        {
            APIResponse apiResponse = await SendAsync<APIResponse>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = apiUrl + $"/api/Order/Status/{companyRequestId}",
            }, _configuration.GetValue<string>(SD.CourierHubApiKeySectionName));

            if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                StatusType statusType = JsonConvert.DeserializeObject<StatusType>(Convert.ToString(apiResponse.Result));

                if (statusType == StatusType.Confirmed)
                {
                    RequestAcceptDto requestAcceptDto = new RequestAcceptDto()
                    {
                        companyDeliveryId = companyRequestId,
                        requestStatus = RequestStatus.Accepted
                    };
                    apiResponse.Result = requestAcceptDto;
                }

                if (statusType == StatusType.Denied)
                {
                    RequestRejectDto requestRejectDto = new RequestRejectDto()
                    {
                        rejectionReason = "Not provided",
                        requestStatus = RequestStatus.Rejected
                    };
                    apiResponse.Result = requestRejectDto;
                    apiResponse.StatusCode = System.Net.HttpStatusCode.NotAcceptable;
                }

                var res1 = JsonConvert.SerializeObject(apiResponse);
                return JsonConvert.DeserializeObject<T>(res1);
            }
            apiResponse.StatusCode = System.Net.HttpStatusCode.BadRequest;
            apiResponse.IsSuccess = false;
            apiResponse.Result = null;

            var res = JsonConvert.SerializeObject(apiResponse);
            return JsonConvert.DeserializeObject<T>(res);

        }

        private DeliveryStatus mapStatus(StatusType statusType)
        {
            DeliveryStatus ret;
            switch (statusType)
            {
                case StatusType.NotConfirmed:
                    ret = DeliveryStatus.Proccessing; break;
                case StatusType.Confirmed:
                    ret = DeliveryStatus.Proccessing; break;
                case StatusType.Cancelled:
                    ret = DeliveryStatus.Canceled; break;
                case StatusType.Denied:
                    ret = DeliveryStatus.Proccessing; break;
                case StatusType.PickedUp:
                    ret = DeliveryStatus.PickedUp; break;
                case StatusType.Delivered:
                    ret = DeliveryStatus.Delivered; break;
                case StatusType.CouldNotDeliver:
                    ret = DeliveryStatus.CannotDeliver; break;
                default:
                    ret = DeliveryStatus.Proccessing; break;
            };
            return ret;
        }
        public async Task<T> GetDeliveryAsync<T>(string companyDeliveryId)
        {
            APIResponse apiResponse = await SendAsync<APIResponse>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = apiUrl + $"/api/Order/Status/{companyDeliveryId}",
            }, _configuration.GetValue<string>(SD.CourierHubApiKeySectionName));

            DeliveryDto deliveryDto = null;
            if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                StatusType statusType = JsonConvert.DeserializeObject<StatusType>(Convert.ToString(apiResponse.Result));
                deliveryDto = new DeliveryDto
                {
                    companyDeliveryId = companyDeliveryId,
                    courier = new CourierDto
                    {
                        name = "",
                        surname = ""
                    },
                    request = _mapper.Map<RequestDto>((_unitOfWork.Delivery.Get(u => u.companyDeliveryId == companyDeliveryId, includeProperties:
                        "request,request.personalData,request.personalData.address,request.offer,request.offer.inquiry,request.offer.inquiry.package,request.offer.inquiry.sourceAddress,request.offer.inquiry.destionationAddress"))
                        .request),
                    cancelationDeadline = DateTime.MaxValue,
                    deliveryStatus = mapStatus(statusType)
                };

            }

            apiResponse.Result = deliveryDto;
            var res = JsonConvert.SerializeObject(apiResponse);
            return JsonConvert.DeserializeObject<T>(res);
        }

        public async Task<T> CancelDeliveryAsync<T>(string companyDeliveryId)
        {
            CourierHubDeliveryCancelDto courierHubDeliveryCancelDto = new CourierHubDeliveryCancelDto
            {
                code = companyDeliveryId
            };
            return await SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = courierHubDeliveryCancelDto,
                Url = apiUrl + $"/api/Order/Withdraw",
            }, _configuration.GetValue<string>(SD.CourierHubApiKeySectionName));
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
