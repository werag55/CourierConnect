using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.Models;
using CourierConnect.Models.Dto;
using CourierConnect.Utility;
using CourierConnectWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace CourierConnectWeb.Controllers
{
    public class OfferController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOfferService _offerService;
        private readonly IMapper _mapper;
        public OfferController(IUnitOfWork unitOfWork, IOfferService offerService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _offerService = offerService;
            _mapper = mapper;
        }

        [Authorize(Roles = SD.Role_User_Client)]
        public async Task<IActionResult> Index(int Id)
        {
            //Inquiry? inquiry = _unitOfWork.Inquiry.GetAll(includeProperties:"sourceAddress,destinationAddress,package").FirstOrDefault();
            Inquiry inquiry = _unitOfWork.Inquiry.Get(u => u.Id == Id, includeProperties: "sourceAddress,destinationAddress,package");
            InquiryDto inquiryDto = _mapper.Map<InquiryDto>(inquiry);
            var response = await _offerService.GetOfferAsync<APIResponse>(inquiryDto);
            if (response != null && response.IsSuccess)
            {
                OfferDto? offerDto = JsonConvert.DeserializeObject<OfferDto>(Convert.ToString(response.Result));
                //Offer offer = _mapper.Map<Offer>(offerDto);
                //_unitOfWork.Offer.Add(offer);
                return View(offerDto);
            }
            return NotFound();
        }

        [Authorize(Roles = SD.Role_User_Worker)]
        public async Task<IActionResult> IndexAll()
        {
            //var response = await _offerService.GetAllAsync<APIResponse>();
            //if (response != null && response.IsSuccess)
            //{
            //    List<OfferDto> offerDto = JsonConvert.DeserializeObject<OfferDto>(Convert.ToString(response.Result));
            //    return View(offerDto);
            //}
            return NotFound();
        }
    }
}
