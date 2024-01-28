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
using CourierCompanyApi.Responses;

namespace CourierCompanyApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InquiryController : Controller
    {
        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public InquiryController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
        }

        /// <summary>
        /// Returns all inquiries realated to the company (for the office worker)
        /// </summary>
        /// <response code="200">Returns list of all inquiries</response>
        /// <response code="404">There is no inquiry to return</response>
        [HttpGet]
        [ServiceFilter(typeof(SpecialApiKeyAuthFilter))]
        [ProducesResponseType(typeof(ListInquiryResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(APIResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetInquiries()
        {
            try
            {

                IEnumerable<Inquiry> InquiryList;
                InquiryList = await _unitOfWork.Inquiry.GetAllAsync(includeProperties: "sourceAddress,destinationAddress,package");

                if (InquiryList == null || InquiryList.Count() == 0)
                {
                    _response.IsSuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                _response.Result = _mapper.Map<List<InquiryDto>>(InquiryList);
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
