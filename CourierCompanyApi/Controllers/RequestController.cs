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
	public class RequestController : Controller
	{
		protected APIResponse _response;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public RequestController(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_response = new();
		}

		/// <summary>
		/// Informs company, that the client has selected a given offer
		/// </summary>
		/// <response code="201">Request has been succesfully created. Returns the processing request details.</response>
		/// <response code="400">Provided request was not valid (e.g. there is no offer with a given Id)</response>
		[HttpPost]
		[ServiceFilter(typeof(ApiKeyAuthFilter))]
		[ProducesResponseType(typeof(RequestResponse), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> PostRequest([FromBody] RequestSendDto requestDto)
		{
			try
			{

				if (requestDto == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
						= new List<string>() { "The required parameter has not been provided" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				Offer offer = await _unitOfWork.Offer.GetAsync(u => u.GUID == requestDto.companyOfferId);
				if (offer == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
						= new List<string>() { "There is no offer with provided offerId" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}

				Request request = _mapper.Map<Request>(requestDto);
				request.GUID = Guid.NewGuid().ToString();
				request.offerId = offer.Id;
				request.offer = offer;
				request.requestStatus = RequestStatus.Pending;
				request.decisionDeadline = DateTime.Now.AddMinutes(0.5);
				await _unitOfWork.Request.CreateAsync(request);


				RequestResponseDto requestResponse = _mapper.Map<RequestResponseDto>(request);
				requestResponse.companyRequestId = request.GUID;
				_response.Result = requestResponse;
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
		/// Returns information whether a decision regarding a given request has already been made
		/// </summary>
		/// <response code="200">Returns decision status of the given request.</response>
		/// <response code="400">Provided request was not valid (e.g. there is no request with a given Id)</response>
		[HttpGet("{requestId}")]
		[ServiceFilter(typeof(ApiKeyAuthFilter))]
		[ProducesResponseType(typeof(RequestStatusResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> GetRequestStatus(string requestId)
		{
			try
			{
				Request request = await _unitOfWork.Request.GetAsync(u => u.GUID == requestId);
				if (request == null)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
						= new List<string>() { "Provided requestId was not valid" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				
				RequestStatusDto requestStatus = new RequestStatusDto();
				requestStatus.isReady = !(request.requestStatus == RequestStatus.Pending);
				_response.IsSuccess = true;
				_response.Result = requestStatus;
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
		/// Changes the request status to Accepted (for the office worker)
		/// </summary>
		/// <response code="200">Request status has been succesfully updated</response>
		/// <response code="400">Provided request was not valid or decision cannot be made</response>
		[HttpPost("{requestId}")]
		[ServiceFilter(typeof(SpecialApiKeyAuthFilter))]
		[ProducesResponseType(typeof(RequestStatusResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> AcceptRequest(string requestId)
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
				if (request.requestStatus != RequestStatus.Pending)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
						= new List<string>() { "Decision about this request has already been made" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				if (request.decisionDeadline < DateTime.Now)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
						= new List<string>() { "Decision deadline has been exceeded" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}


				request.requestStatus = RequestStatus.Accepted;
				await _unitOfWork.Request.UpdateAsync(request);
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
		/// Changes the request status to Rejected (for the office worker)
		/// </summary>
		/// <response code="200">Request status has been succesfully updated</response>
		/// <response code="400">Provided request was not valid or decision cannot be made</response>
		[HttpPost("{requestId}")]
		[ServiceFilter(typeof(SpecialApiKeyAuthFilter))]
		[ProducesResponseType(typeof(RequestStatusResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(APIResponse), StatusCodes.Status400BadRequest)]
		public async Task<ActionResult<APIResponse>> RejectRequest(string requestId)
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
				if (request.requestStatus != RequestStatus.Pending)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
						= new List<string>() { "Decision about this request has already been made" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}
				if (request.decisionDeadline < DateTime.Now)
				{
					_response.IsSuccess = false;
					_response.ErrorMessages
						= new List<string>() { "Decision deadline has been exceeded" };
					_response.StatusCode = HttpStatusCode.BadRequest;
					return BadRequest(_response);
				}


				request.requestStatus = RequestStatus.Rejected;
				await _unitOfWork.Request.UpdateAsync(request);
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
