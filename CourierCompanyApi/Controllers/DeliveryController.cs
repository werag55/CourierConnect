using AutoMapper;
using CourierCompanyApi.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using CourierCompanyApi.Models;
using CourierCompanyApi.Models.Dto;
using CourierCompanyApi.Authentication;
using System.Diagnostics.Metrics;

namespace CourierCompanyApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DeliveryController : ControllerBase
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly bool autoAccept = true;
        public DeliveryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
        }

        [HttpGet("{courierUserName}")]
        [ServiceFilter(typeof(SpecialApiKeyAuthFilter))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetCourierDeliveries([FromBody] string courierUserName)
        {
            try
            {
                Courier courier = await _unitOfWork.Courier.GetAsync(u => u.userName == courierUserName);

                if (courier == null)
                    return BadRequest();

                IEnumerable<Delivery> DeliveryList;
                DeliveryList = await _unitOfWork.Delivery.GetAllAsync(u => u.courier.userName == courierUserName, includeProperties:
                    "request,request.offer,request.personalData,request.offer.inquiry,request.offer.inquiry.sourceAddress,request.offer.inquiry.destinationAddress,request.offer.inquiry.package");

                if (DeliveryList.Count() == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }

                List<DeliveryDto> deliveryDtoList = _mapper.Map<List<DeliveryDto>>(DeliveryList);
                for (int i = 0; i < deliveryDtoList.Count; i++)
                    deliveryDtoList[i].companyDeliveryId = (DeliveryList.ElementAt(i)).Id;
                _response.Result = deliveryDtoList;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet]
        [ServiceFilter(typeof(ApiKeyAuthFilter))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetDelivery([FromBody] int deliveryId)
        {
            try
            {
                Delivery delivery = await _unitOfWork.Delivery.GetAsync(u => u.Id == deliveryId, includeProperties:
                    "request,request.offer,request.personalData,request.offer.inquiry,request.offer.inquiry.sourceAddress,request.offer.inquiry.destinationAddress,request.offer.inquiry.package");

                if (delivery == null)
                    return BadRequest();

                DeliveryDto deliveryDto = _mapper.Map<DeliveryDto>(delivery);
                deliveryDto.companyDeliveryId = delivery.Id;
                _response.Result = deliveryDto;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet]
        [ServiceFilter(typeof(SpecialApiKeyAuthFilter))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAllDeliveries()
        {
            try
            {

                IEnumerable<Delivery> DeliveryList;
                DeliveryList = await _unitOfWork.Delivery.GetAllAsync(includeProperties:
                    "request,request.offer,request.personalData,request.offer.inquiry,request.offer.inquiry.sourceAddress,request.offer.inquiry.destinationAddress,request.offer.inquiry.package");

                if (DeliveryList.Count() == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound();
                }

                List<DeliveryDto> deliveryDtoList = _mapper.Map<List<DeliveryDto>>(DeliveryList);
                for (int i = 0; i < deliveryDtoList.Count; i++)
                    deliveryDtoList[i].companyDeliveryId = (DeliveryList.ElementAt(i)).Id;
                _response.Result = deliveryDtoList;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpPost]
        [ServiceFilter(typeof(ApiKeyAuthFilter))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> Get([FromBody] RequestSendDto requestDto)
        {
            try
            {
       
                if (requestDto == null)
                    return BadRequest(requestDto);

                Offer offer = await _unitOfWork.Offer.GetAsync(u => u.Id == requestDto.companyOfferId);
                if (offer == null)
                    return BadRequest();

                Request request = _mapper.Map<Request>(requestDto);
                request.offerId = offer.Id;
                request.offer = offer;

                Courier courier = null;
                var couriers = await _unitOfWork.Courier.GetAllAsync();
                if (couriers.Count != 0)
                {
                    Courier courierWithMaxId = couriers.OrderByDescending(c => c.Id).FirstOrDefault();
                    int maxCourierId = courierWithMaxId.Id;
/*                    int tried = 0;
                    do
                    {*/
                        Random random = new Random();
                        int randomCourierId = random.Next(1, maxCourierId + 1);
                        courier = couriers.FirstOrDefault(c => c.Id ==  randomCourierId);
                    /*    tried++;
                    } while (courier == null && tried < 1);*/
                    if (courier == null)
                        courier = courierWithMaxId;
                }

                if (autoAccept && courier != null)
                {
                    request.isAccepted = true;
                    //TODO: Agreement
                    //TODO: Receipt
                    await _unitOfWork.Request.CreateAsync(request);

                    request.offer.status = OfferStatus.Accepted;
                    request.offer.updatedDate = DateTime.Now;
                    await _unitOfWork.Offer.UpdateAsync(request.offer);

                    Delivery delivery = new Delivery
                    {
                        courier = courier,
                        courierId = courier.Id,
                        request = request,
                        requestId = request.Id,
                        cancelationDeadline = DateTime.Now.AddDays(1),
                        deliveryStatus = DeliveryStatus.Proccessing
                    };

                    await _unitOfWork.Delivery.CreateAsync(delivery);

                    RequestAcceptDto accept = _mapper.Map<RequestAcceptDto>(request);
                    accept.companyOfferId = requestDto.companyOfferId;
                    accept.companyDeliveryId = delivery.Id;

                    _response.Result = accept;
                    _response.StatusCode = HttpStatusCode.Created;
                    return Ok(_response);
                }
                else
                {
                    request.isAccepted = false;
                    request.rejectionReason = "We changed our mind ;*";             
                    await _unitOfWork.Request.CreateAsync(request);

                    request.offer.status = OfferStatus.Rejected;
                    request.offer.updatedDate = DateTime.Now;
                    await _unitOfWork.Offer.UpdateAsync(request.offer);

                    RequestRejectDto reject = _mapper.Map<RequestRejectDto>(request);
                    reject.companyOfferId = requestDto.companyOfferId;
                    _response.Result = reject;
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }
    }
}
