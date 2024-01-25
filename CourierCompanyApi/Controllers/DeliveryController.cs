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
		/// Creates a delivery based on a given request
		/// </summary>
		/// <response code="201">Delivery has been succesfully created. Returns the delivery details.</response>
		/// <response code="406">The company decided to reject request. Returns rejection reason.</response>
		/// <response code="400">Provided request was not valid (e.g. there is no offer with a given Id) r the decision has not been made yet</response>
		[HttpPost("{requestId}")]
		[ServiceFilter(typeof(ApiKeyAuthFilter))]
		[ProducesResponseType(typeof(RequestRejectResponse), StatusCodes.Status406NotAcceptable)]
		[ProducesResponseType(typeof(RequestAcceptResponse), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> PostDelivery(string requestId)
		{
			try
			{
				Request request = await _unitOfWork.Request.GetAsync(u => u.GUID == requestId, includeProperties:
					"offer,personalData,personalData.address,offer.inquiry,offer.inquiry.sourceAddress,offer.inquiry.destinationAddress,offer.inquiry.package");

				if (request == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
						 = new List<string>() { "Provided requestId was not valid" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				if (request.requestStatus == RequestStatus.Pending && request.decisionDeadline > DateTime.Now)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
						 = new List<string>() { "Decision about this request has not been made yet" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				if (request.requestStatus == RequestStatus.Pending)
					request.requestStatus = RequestStatus.Accepted;
				if (request.requestStatus == RequestStatus.Rejected)
					request.rejectionReason = "We changed our mind ;*";

				Courier courier = null;
				if (request.requestStatus == RequestStatus.Accepted)
				{
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
						courier = couriers.FirstOrDefault(c => c.Id == randomCourierId);
						/*    tried++;
                        } while (courier == null && tried < 1);*/
						if (courier == null)
							courier = courierWithMaxId;
					}

					if (courier == null)
					{
						request.requestStatus = RequestStatus.Rejected;
						request.rejectionReason = "There are currently no couriers available";
					}
				}
				await _unitOfWork.Request.UpdateAsync(request);

				if (request.requestStatus == RequestStatus.Accepted)
				{
					//TODO: Agreement
					//TODO: Receipt

					request.offer.status = OfferStatus.Accepted;
					request.offer.updatedDate = DateTime.Now;
					await _unitOfWork.Offer.UpdateAsync(request.offer);

					Delivery delivery = new Delivery
					{
						GUID = (Guid.NewGuid()).ToString(),
						courier = courier,
						courierId = courier.Id,
						request = request,
						requestId = request.Id,
						cancelationDeadline = DateTime.Now.AddDays(1),
						deliveryStatus = DeliveryStatus.Proccessing
					};

					await _unitOfWork.Delivery.CreateAsync(delivery);

					RequestAcceptDto accept = _mapper.Map<RequestAcceptDto>(request);
					accept.companyDeliveryId = delivery.GUID;

					_response.Result = accept;
					_response.StatusCode = HttpStatusCode.Created;
					return Ok(_response);
				}
				else
				{

					request.offer.status = OfferStatus.Rejected;
					request.offer.updatedDate = DateTime.Now;
					await _unitOfWork.Offer.UpdateAsync(request.offer);

					RequestRejectDto reject = _mapper.Map<RequestRejectDto>(request);
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

		/// <summary>
		/// Returns delivery matching the provided delivery Id
		/// </summary>
		/// <response code="200">Returns the delivery</response>
		/// <response code="400">Provided delivery Id was not valid</response>
		[HttpGet("{deliveryId}")]
        [ServiceFilter(typeof(ApiKeyAuthFilter))]
        [ProducesResponseType(typeof(DeliveryResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetDelivery(string deliveryId)
        {
            try
            {
                Delivery delivery = await _unitOfWork.Delivery.GetAsync(u => u.GUID == deliveryId, includeProperties:
                    "courier,request,request.offer,request.personalData,request.personalData.address,request.offer.inquiry,request.offer.inquiry.sourceAddress,request.offer.inquiry.destinationAddress,request.offer.inquiry.package");

                if (delivery == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
					    = new List<string>() { "Provided deliveryId was not valid" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				DeliveryDto deliveryDto = _mapper.Map<DeliveryDto>(delivery);
                deliveryDto.companyDeliveryId = delivery.GUID;
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
		/// Changes the delivery status to Canceled (if the cancelation deadline has not been exceeded)
		/// </summary>
		/// <response code="200">Delivery status has been succesfully updated</response>
		/// <response code="400">Provided delivery Id was not valid or the cancelation deadline has been exceeded</response>
		[HttpDelete("{deliveryId}")]
		[ServiceFilter(typeof(ApiKeyAuthFilter))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> CancelDelivery(string deliveryId)
        {
			try
			{
				Delivery delivery = await _unitOfWork.Delivery.GetAsync(u => u.GUID == deliveryId, includeProperties:
					"courier,request,request.offer,request.personalData,,request.personalData.address,request.offer.inquiry,request.offer.inquiry.sourceAddress,request.offer.inquiry.destinationAddress,request.offer.inquiry.package");

				if (delivery == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
					    = new List<string>() { "Provided deliveryId was not valid" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				if (delivery.cancelationDeadline < DateTime.Now)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
					     = new List<string>() { "The cancelation deadline has been exceeded" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				delivery.deliveryStatus = DeliveryStatus.Cancelled;
                await _unitOfWork.Delivery.UpdateAsync(delivery);
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
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<APIResponse>> GetAllDeliveries()
		{
			try
			{
				IEnumerable<Delivery> DeliveryList;
				DeliveryList = await _unitOfWork.Delivery.GetAllAsync(includeProperties:
					"courier,request,request.offer,request.personalData,request.personalData.address,request.offer.inquiry,request.offer.inquiry.sourceAddress,request.offer.inquiry.destinationAddress,request.offer.inquiry.package");

				if (DeliveryList == null || DeliveryList.Count() == 0)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}

				List<DeliveryDto> deliveryDtoList = _mapper.Map<List<DeliveryDto>>(DeliveryList);
				for (int i = 0; i < deliveryDtoList.Count; i++)
					deliveryDtoList[i].companyDeliveryId = (DeliveryList.ElementAt(i)).GUID;
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
		/// Returns all deliveries related to the courier with a given User Name (for the courier)
		/// </summary>
		/// <response code="200">Returns the deliveries</response>
		/// <response code="400">Provided User Name was not valid</response>
		/// /// <response code="404">There is no delivery related to provided courier</response>
		[HttpGet]
		[ServiceFilter(typeof(SpecialApiKeyAuthFilter))]
		[ProducesResponseType(typeof(ListDeliveryResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
		public async Task<ActionResult<APIResponse>> GetCourierDeliveries([FromBody] string courierUserName)
		{
			try
			{
				Courier courier = await _unitOfWork.Courier.GetAsync(u => u.userName == courierUserName);

				if (courier == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
						= new List<string>() { "Provided courier UserName was not valid" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				IEnumerable<Delivery> DeliveryList;
				DeliveryList = await _unitOfWork.Delivery.GetAllAsync(u => u.courier.userName == courierUserName, includeProperties:
					"courier,request,request.offer,request.personalData,request.personalData.address,request.offer.inquiry,request.offer.inquiry.sourceAddress,request.offer.inquiry.destinationAddress,request.offer.inquiry.package");

				if (DeliveryList.Count() == 0)
				{
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound(_response);
				}

				List<DeliveryDto> deliveryDtoList = _mapper.Map<List<DeliveryDto>>(DeliveryList);
				for (int i = 0; i < deliveryDtoList.Count; i++)
					deliveryDtoList[i].companyDeliveryId = (DeliveryList.ElementAt(i)).GUID;
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
		/// Changes the delivery status to PickedUp (for the courier)
		/// </summary>
		/// <response code="200">Delivery status has been succesfully updated</response>
		/// <response code="400">Provided delivery Id was not valid or the package cannot be picked up</response>
		[HttpPost("{deliveryId}")]
		[ServiceFilter(typeof(SpecialApiKeyAuthFilter))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> PickUpPackage(string deliveryId)
		{
			try
			{
				Delivery delivery = await _unitOfWork.Delivery.GetAsync(u => u.GUID == deliveryId, includeProperties:
					"courier,request,request.offer,request.personalData,,request.personalData.address,request.offer.inquiry,request.offer.inquiry.sourceAddress,request.offer.inquiry.destinationAddress,request.offer.inquiry.package");

				if (delivery == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
						= new List<string>() { "Provided deliveryId was not valid" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				if (delivery.deliveryStatus != DeliveryStatus.Proccessing)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
						 = new List<string>() { "Due to the current delivery status, the package cannot be picked up" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				delivery.deliveryStatus = DeliveryStatus.PickedUp;
                delivery.pickUpDate = DateTime.Now;
				await _unitOfWork.Delivery.UpdateAsync(delivery);
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
		/// Changes the delivery status to Delivered (for the courier)
		/// </summary>
		/// <response code="200">Delivery status has been succesfully updated</response>
		/// <response code="400">Provided delivery Id was not valid or the package cannot be delivered</response>
		[HttpPost("{deliveryId}")]
		[ServiceFilter(typeof(SpecialApiKeyAuthFilter))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> DeliverPackage(string deliveryId)
		{
			try
			{
				Delivery delivery = await _unitOfWork.Delivery.GetAsync(u => u.GUID == deliveryId, includeProperties:
					"courier,request,request.offer,request.personalData,,request.personalData.address,request.offer.inquiry,request.offer.inquiry.sourceAddress,request.offer.inquiry.destinationAddress,request.offer.inquiry.package");

				if (delivery == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
						= new List<string>() { "Provided deliveryId was not valid" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				if (delivery.deliveryStatus != DeliveryStatus.PickedUp)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
						 = new List<string>() { "Due to the current delivery status, the package cannot be delivered" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				delivery.deliveryStatus = DeliveryStatus.Deliverd;
				delivery.deliveryDate = DateTime.Now;
				await _unitOfWork.Delivery.UpdateAsync(delivery);
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
		/// Changes the delivery status to CannotDeliver (for the courier)
		/// </summary>
		/// <response code="200">Delivery status has been succesfully updated</response>
		/// <response code="400">Provided delivery Id was not valid or the package cannot be droped</response>
		[HttpPost("{deliveryId}")]
		[ServiceFilter(typeof(SpecialApiKeyAuthFilter))]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> CannotDeliverPackage(string deliveryId, [FromBody] string reason)
		{
			try
			{
				Delivery delivery = await _unitOfWork.Delivery.GetAsync(u => u.GUID == deliveryId, includeProperties:
					"courier,request,request.offer,request.personalData,,request.personalData.address,request.offer.inquiry,request.offer.inquiry.sourceAddress,request.offer.inquiry.destinationAddress,request.offer.inquiry.package");

				if (delivery == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
						= new List<string>() { "Provided deliveryId was not valid" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				if (delivery.deliveryStatus == DeliveryStatus.Deliverd)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
						 = new List<string>() { "Due to the current delivery status, the package cannot be delivered (the package was already delivered)" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				delivery.deliveryStatus = DeliveryStatus.CannotDeliver;
				delivery.deliveryDate = DateTime.Now;
				delivery.reason = reason;
				await _unitOfWork.Delivery.UpdateAsync(delivery);
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
	}
}
