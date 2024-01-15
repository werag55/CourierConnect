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
using CourierCompanyApi.Responses;
using Microsoft.AspNetCore.Http;

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

		/// <summary>
		/// Returns all deliveries related to the courier with a given User Name (for the courier)
		/// </summary>
		/// <response code="200">Returns the deliveries</response>
		/// <response code="400">Provided User Name was not valid</response>
		/// /// <response code="404">There is no delivery related to provided courier</response>
		[HttpGet]
        [ServiceFilter(typeof(SpecialApiKeyAuthFilter))]
        [ProducesResponseType(typeof(ListDeliveryResponse),StatusCodes.Status200OK)]
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

		/// <summary>
		/// Returns delivery matching the provided delivery Id
		/// </summary>
		/// <response code="200">Returns the delivery</response>
		/// <response code="400">Provided delivery Id was not valid</response>
		[HttpGet("{deliveryId}")]
        [ServiceFilter(typeof(ApiKeyAuthFilter))]
        [ProducesResponseType(typeof(DeliveryResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetDelivery(int deliveryId)
        {
            try
            {
                Delivery delivery = await _unitOfWork.Delivery.GetAsync(u => u.Id == deliveryId, includeProperties:
                    "courier,request,request.offer,request.personalData,,request.personalData.address,request.offer.inquiry,request.offer.inquiry.sourceAddress,request.offer.inquiry.destinationAddress,request.offer.inquiry.package");

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

		/// <summary>
		/// Returns all deliveries realated to the company (for the office worker)
		/// </summary>
		/// <response code="200">Returns list of al deliveries</response>
		/// <response code="404">There is no delivery to return</response>
		[HttpGet]
        [ServiceFilter(typeof(SpecialApiKeyAuthFilter))]
        [ProducesResponseType(typeof(ListDeliveryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetAllDeliveries()
        {
            try
            {
                IEnumerable<Delivery> DeliveryList;
                DeliveryList = await _unitOfWork.Delivery.GetAllAsync(includeProperties:
                    "request,request.offer,request.personalData,request.offer.inquiry,request.offer.inquiry.sourceAddress,request.offer.inquiry.destinationAddress,request.offer.inquiry.package");

                if (DeliveryList == null || DeliveryList.Count() == 0)
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

		/// <summary>
		/// Changes the delivery status to Canceled (if the cancelation deadline has not been exceeded)
		/// </summary>
		/// <response code="200">Delivery status has been succesfully updated</response>
		/// <response code="400">Provided delivery Id was not valid or the cancelation deadline has been exceeded</response>
		[HttpPut("{deliveryId}")]
		[ServiceFilter(typeof(ApiKeyAuthFilter))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> CancelDelivery(int deliveryId)
        {
			try
			{
				Delivery delivery = await _unitOfWork.Delivery.GetAsync(u => u.Id == deliveryId, includeProperties:
					"courier,request,request.offer,request.personalData,,request.personalData.address,request.offer.inquiry,request.offer.inquiry.sourceAddress,request.offer.inquiry.destinationAddress,request.offer.inquiry.package");

				if (delivery == null)
					return BadRequest();

                if (delivery.cancelationDeadline < DateTime.Now)
                    return BadRequest();

                delivery.deliveryStatus = DeliveryStatus.Cancelled;
                _unitOfWork.Delivery.UpdateAsync(delivery);
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

		/// <summary>
		/// Creates a delivery based on a given request
		/// </summary>
		/// <response code="201">Delivery has been succesfully created. Returns the delivery details.</response>
        /// <response code="200">The company decided to reject request. Returns rjection reason.</response>
		/// <response code="400">Provided request was not valid (e.g. there is no offer with a given Id)</response>
		[HttpPost]
        [ServiceFilter(typeof(ApiKeyAuthFilter))]
		[ProducesResponseType(typeof(RequestRejectResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(RequestAcceptResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> Post([FromBody] RequestSendDto requestDto)
        {
            try
            {

                if (requestDto == null)
					return BadRequest(_response);

                Offer offer = await _unitOfWork.Offer.GetAsync(u => u.Id == requestDto.companyOfferId);
                if (offer == null)
					return BadRequest(_response);

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
