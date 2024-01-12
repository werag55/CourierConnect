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

namespace CourierCompanyApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [ServiceFilter(typeof(ApiKeyAuthFilter))]
    public class OfferController : ControllerBase
    {

        protected APIResponse _response;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OfferController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _response = new();
        }


        // GET: api/<OffersController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetOffers()
        {
            int pageSize = 0, pageNumber = 1;
            try
            {

                IEnumerable<Offer> OfferList;
                OfferList = await _unitOfWork.Offer.GetAllAsync(pageSize: pageSize,
                    pageNumber: pageNumber);

                Pagination pagination = new() { PageNumber = pageNumber, PageSize = pageSize };

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(pagination));
                _response.Result = _mapper.Map<List<OfferDto>>(OfferList);
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

        // GET api/<OffersController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetOffer(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var Offer = await _unitOfWork.Offer.GetAsync(u => u.Id == id,includeProperties:"inquiry");
                if (Offer == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Result = _mapper.Map<OfferDto>(Offer);
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


        private Offer createOffer(Inquiry inquiry)
        {
            Package package = _unitOfWork.Package.GetAsync(u => u.Id == inquiry.Id).Result;
            var weight = package.weight;
            var length = package.length;
            var offer = new Offer()
            {
                Id = 0,
                inquiryId = inquiry.Id,
                inquiry = inquiry,
                creationDate = DateTime.Now,
                updatedDate = DateTime.Now,
                expirationDate = DateTime.Now.AddDays(1),
                status = OfferStatus.Pending,
                price = (decimal)(weight + length * 2.5),
                taxes = (decimal)(weight + length * 2.5 * 0.23),
                fees = (decimal)(weight + length * 2.5 * 0.1),
            };
            return offer;
        }
        // POST api/<OffersController>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> Post([FromBody] InquiryDto inquiryDto)
        {
            try
            {

                if (inquiryDto == null)
                {
                    return BadRequest(inquiryDto);
                }

                Inquiry inquiry = _mapper.Map<Inquiry>(inquiryDto);

                await _unitOfWork.Inquiry.CreateAsync(inquiry);
                //await _unitOfWork.Address.CreateAsync(inquiry.destinationAddress);
                //await _unitOfWork.Address.CreateAsync(inquiry.sourceAddress);
                //await _unitOfWork.Package.CreateAsync(inquiry.package);
                //await _unitOfWork.SaveAsync();

                Offer offer = createOffer(inquiry);
                await _unitOfWork.Offer.CreateAsync(offer);
                //await _unitOfWork.SaveAsync();

                _response.Result = _mapper.Map<OfferDto>(offer);
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
                //return CreatedAtRoute("Offer/GetOffer", new { id = offer.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages
                     = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        // PUT api/<OffersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OffersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
