using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.Models;
using CourierConnect.Models.Dto;
using CourierConnect.Utility;
using CourierConnectWeb.Services.IServices;
using CourierConnectWeb.Services.Factory;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using CourierConnect.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using CourierConnectWeb.Services;

namespace CourierConnectWeb.Controllers
{
    public class OfferController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IOfferService _offerService;
        private List<IServiceFactory> _serviceFactories = new List<IServiceFactory>();
        private readonly IMapper _mapper;
        public OfferController(IUnitOfWork unitOfWork, /*IOfferService offerService,*/ OurServiceFactory ourServiceFactory, CurrierServiceFactory currierServiceFactory,
            IMapper mapper, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            //_offerService = offerService;
            _serviceFactories.Add(ourServiceFactory);
            _serviceFactories.Add(currierServiceFactory);
            _mapper = mapper;
            _userManager = userManager;
        }

        private Offer GetOfferToSave(OfferDto offerDto, Inquiry inquiry)
        {
            Offer offer = _mapper.Map<Offer>(offerDto);
            offer.inquiry = inquiry;
            offer.inquiryId = inquiry.Id;
            offer.updatedDate = DateTime.Now;
            return offer;
        }

        //[Authorize(Roles = SD.Role_User_Client)]
        public async Task<IActionResult> Create(int Id)
        {
            //Inquiry? inquiry = _unitOfWork.Inquiry.GetAll(includeProperties:"sourceAddress,destinationAddress,package").FirstOrDefault();
            Inquiry inquiry = _unitOfWork.Inquiry.Get(u => u.Id == Id, includeProperties: "sourceAddress,destinationAddress,package");
            InquiryDto inquiryDto = _mapper.Map<InquiryDto>(inquiry);
            List<Task<APIResponse>> tasks = new List<Task<APIResponse>>();

            foreach (var serviceFactory in  _serviceFactories)
            {
                var offerService = serviceFactory.createOfferService();
                tasks.Add(offerService.GetOfferAsync<APIResponse>(inquiryDto));
            }

            var responses = await Task.WhenAll(tasks);
            foreach (var response in responses)
            {
                if (response != null && response.IsSuccess)
                {
                    OfferDto? offerDto = JsonConvert.DeserializeObject<OfferDto>(Convert.ToString(response.Result));
                    Offer offer = GetOfferToSave(offerDto, inquiry);
                    _unitOfWork.Offer.Add(offer);
                    _unitOfWork.Save();
                }
            }

            return RedirectToRoute(new
            {
                controller = "Offer",
                action = "Index",
                id = inquiry.Id
            });

            //return NotFound();
        }

        //[Authorize(Roles = SD.Role_User_Client)]
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
            //foreach (var serviceFactory in _serviceFactories)
            //{
                IServiceFactory serviceFactory = _serviceFactories.FindAll(u => u.serviceId == 0).FirstOrDefault();
                var offerService = serviceFactory.createOfferService();
                var response = await offerService.GetAllAsync<APIResponse>();
                if (response != null && response.IsSuccess)
                {
                    List<OfferDto>? offerDto = JsonConvert.DeserializeObject<List<OfferDto>>(Convert.ToString(response.Result));
                    return View(offerDto);
                }
            //}
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
