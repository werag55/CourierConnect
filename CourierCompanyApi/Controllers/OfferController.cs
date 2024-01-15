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

		/// <summary>
		/// Returns all offers realated to the company (for the office worker)
		/// </summary>
		/// <response code="200">Returns list of all offers</response>
		/// <response code="404">There is no offer to return</response>
		// GET: api/<OffersController>
		[HttpGet]
        [ServiceFilter(typeof(SpecialApiKeyAuthFilter))]
        [ProducesResponseType(typeof(ListOfferResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetOffers()
        {
            try
            {

                IEnumerable<Offer> OfferList;
                OfferList = await _unitOfWork.Offer.GetAllAsync(includeProperties:"inquiry,inquiry.sourceAddress,inquiry.destinationAddress,inquiry.package");
                
                if (OfferList == null || OfferList.Count() == 0)
                {
					_response.IsSuccess = false;
					_response.StatusCode = HttpStatusCode.NotFound;
					return NotFound();
				}
                
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

        //// GET api/<OffersController>/5
        //[HttpGet("{id}")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public async Task<ActionResult<APIResponse>> GetOffer(int id)
        //{
        //    try
        //    {
        //        if (id == 0)
        //        {
        //            _response.StatusCode = HttpStatusCode.BadRequest;
        //            return BadRequest(_response);
        //        }
        //        var Offer = await _unitOfWork.Offer.GetAsync(u => u.Id == id,includeProperties:"inquiry,inquiry.sourceAddress,inquiry.destinationAddress,inquiry.package");
        //        if (Offer == null)
        //        {
        //            _response.StatusCode = HttpStatusCode.NotFound;
        //            return NotFound(_response);
        //        }
        //        _response.Result = _mapper.Map<OfferDto>(Offer);
        //        _response.StatusCode = HttpStatusCode.OK;
        //        return Ok(_response);
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsSuccess = false;
        //        _response.ErrorMessages
        //             = new List<string>() { ex.ToString() };
        //    }
        //    return _response;
        //}

        private Offer createOffer(Inquiry inquiry)
        {
            Package package = inquiry.package; //_unitOfWork.Package.GetAsync(u => u.Id == inquiry.Id).Result;
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
            offer.creationDate = offer.creationDate.AddTicks(-offer.creationDate.Ticks % TimeSpan.TicksPerSecond);
            offer.updatedDate = offer.updatedDate.AddTicks(-offer.updatedDate.Ticks % TimeSpan.TicksPerSecond);
            offer.expirationDate = offer.expirationDate.AddTicks(-offer.expirationDate.Ticks % TimeSpan.TicksPerSecond);
            return offer;
        }

		/// <summary>
		/// Creates an offer based on a given inquiry
		/// </summary>
		/// <response code="201">Offer has been succesfully created. Returns the offer details.</response>
		/// <response code="400">Provided iquiry was not valid</response>
		// POST api/<OffersController>
		[HttpPost]
        [ServiceFilter(typeof(ApiKeyAuthFilter))]
        [ProducesResponseType(typeof(OfferResponse), StatusCodes.Status201Created)]
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
                OfferDto offerDto = _mapper.Map<OfferDto>(offer);
                offerDto.companyOfferId = offer.Id;
                _response.Result = offerDto;
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

/*        // PUT api/<OffersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OffersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }*/
    }
}
