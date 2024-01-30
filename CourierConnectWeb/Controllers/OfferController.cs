using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.Models;
using CourierConnect.Models.Dto;
using CourierConnect.Utility;
using CourierConnectWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using CourierConnect.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace CourierConnectWeb.Controllers
{
    public class OfferController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOfferService _offerService;
        private readonly IMapper _mapper;
        public OfferController(IUnitOfWork unitOfWork, IOfferService offerService, IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _offerService = offerService;
            _mapper = mapper;
            _userManager = userManager;
        }

        private Offer GetOfferToSave(OfferDto offerDto, Inquiry inquiry, int companyId)
        {
            Offer offer = _mapper.Map<Offer>(offerDto);
            offer.inquiry = inquiry;
            offer.inquiryId = inquiry.Id;
            offer.updatedDate = DateTime.Now;
            offer.companyId = companyId;
            return offer;
        }

        //[Authorize(Roles = SD.Role_User_Client)]
        public async Task<IActionResult> Create(int Id)
        {
            //Inquiry? inquiry = _unitOfWork.Inquiry.GetAll(includeProperties:"sourceAddress,destinationAddress,package").FirstOrDefault();
            Inquiry inquiry = _unitOfWork.Inquiry.Get(u => u.Id == Id, includeProperties: "sourceAddress,destinationAddress,package");
            InquiryDto inquiryDto = _mapper.Map<InquiryDto>(inquiry);
            var response = await _offerService.GetOfferAsync<APIResponse>(inquiryDto);
            if (response != null && response.IsSuccess)
            {
                OfferDto? offerDto = JsonConvert.DeserializeObject<OfferDto>(Convert.ToString(response.Result));
                Offer offer = GetOfferToSave(offerDto, inquiry, 1);
                _unitOfWork.Offer.Add(offer);
                _unitOfWork.Save();

                return View(offer);
            }
            return NotFound();
        }

        [Authorize(Roles = SD.Role_User_Client)]
        public IActionResult Index(int Id)
        {
            IEnumerable<Offer> offers = _unitOfWork.Offer.FindAll(u => u.inquiry.Id == Id, 
                includeProperties:"inquiry,inquiry.sourceAddress,inquiry.destinationAddress,inquiry.package");
            if (offers != null)
            {
                return View(offers);
            }
            return NotFound();
        }

        //[Authorize(Roles = SD.Role_User_Worker)]
        public async Task<IActionResult> IndexAll()
        {
            var response = await _offerService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                List<OfferDto>? offerDto = JsonConvert.DeserializeObject<List<OfferDto>>(Convert.ToString(response.Result));
                return View(offerDto);
            }
            return NotFound();
        }

        ////[Authorize(Roles = SD.Role_User_Courier)]
        //public async Task<IActionResult> IndexAllCourier()
        //{
        //    string? courier = _userManager.GetUserName(User);

        //    var response = await _offerService.GetAsync<APIResponse>(courier);
        //    if (response != null && response.IsSuccess)
        //    {
        //        List<OfferDto>? offerDto = JsonConvert.DeserializeObject<List<OfferDto>>(Convert.ToString(response.Result));
        //        return View(offerDto);
        //    }
        //    return NotFound();
        //}
    }
}
