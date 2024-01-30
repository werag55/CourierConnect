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
using CourierConnectWeb.Services.Factory;
using Microsoft.Data.SqlClient;

namespace CourierConnectWeb.Controllers
{
	public class RequestController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IUnitOfWork _unitOfWork;
        //private readonly IRequestService _requestService;
        private List<IServiceFactory> _serviceFactories = new List<IServiceFactory>();
        private readonly IMapper _mapper;
		public RequestController(IUnitOfWork unitOfWork, /*IRequestService requestService,*/ OurServiceFactory ourServiceFactory, CurrierServiceFactory currierServiceFactory,
            CourierHubServiceFactory courierHubServiceFactory, IMapper mapper, UserManager<IdentityUser> userManager)
		{
			_unitOfWork = unitOfWork;
            //_requestService = requestService;
            _serviceFactories.Add(ourServiceFactory);
            _serviceFactories.Add(currierServiceFactory);
            _serviceFactories.Add(courierHubServiceFactory);
            _mapper = mapper;
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Create(int Id)
		{
			string? clientId = _userManager.GetUserId(User);
            PersonalData pd = new PersonalData();
			if (clientId != null)
                pd = _unitOfWork.PersonalData.Get(u => u.clientId == clientId, includeProperties: "address"); // ?? new PersonalData();
			Offer offer = _unitOfWork.Offer.Get(u => u.Id == Id); //, includeProperties: "inquiry,inquiry.sourceAddress,inquiry.destinationAddress,inquiry.package");

			if (offer == null) 
				return NotFound();

			RequestSendDto request = new RequestSendDto
            {
				companyOfferId	= offer.companyOfferId,
				personalData = _mapper.Map<PersonalDataDto>(pd),
			};

			RequestCreateVM requestCreateVM = new RequestCreateVM
			{
                requestSendDto = request,
				offerId = Id,
                companyId = offer.companyId,
			};

			return View(requestCreateVM);
		}

		[HttpPost]
		public async Task<IActionResult> Create(RequestCreateVM requestCreateVM)
		{
			RequestSendDto requestSendDto = requestCreateVM.requestSendDto;

            IServiceFactory serviceFactory = _serviceFactories.FindAll(u => u.serviceId == requestCreateVM.companyId).FirstOrDefault();
            var requestService = serviceFactory.createRequestService();
            var response = await requestService.GetRequestAsync<APIResponse>(requestSendDto);
            if (response != null && response.IsSuccess)
            {
                PersonalData pd = _mapper.Map<PersonalData>(requestSendDto.personalData);

                string? clientId = _userManager.GetUserId(User);
                PersonalData clientPd = _unitOfWork.PersonalData.Get(u => u.clientId == clientId, includeProperties: "address");
                if (clientPd == null && clientId != null)
                {
                    Address ad = pd.address;
                    _unitOfWork.Address.Add(ad);
                    _unitOfWork.Save();
                    pd.addressId = ad.Id;
                    pd.clientId = clientId;
                    _unitOfWork.PersonalData.Add(pd);
                    _unitOfWork.Save();
                }
                else
                {
                    if (clientPd != null && pd.name == clientPd.name && pd.surname == clientPd.surname && pd.companyName == clientPd.companyName
                        && pd.email == clientPd.email && pd.address.streetName == clientPd.address.streetName
                        && pd.address.houseNumber == clientPd.address.houseNumber && pd.address.flatNumber == clientPd.address.flatNumber
                        && pd.address.postcode == clientPd.address.postcode && pd.address.city == clientPd.address.city)
                        pd = clientPd;
                    else
                    {
                        Address ad = pd.address;
                        _unitOfWork.Address.Add(ad);
                        _unitOfWork.Save();
                        pd.addressId = ad.Id;
                        _unitOfWork.PersonalData.Add(pd);
                        _unitOfWork.Save();
                    }
                }

                RequestResponseDto requestResponseDto = JsonConvert.DeserializeObject<RequestResponseDto>(Convert.ToString(response.Result));

                Request request = new Request
                {
                    offerId = requestCreateVM.offerId,
                    offer = _unitOfWork.Offer.Get(u => u.Id == requestCreateVM.offerId, includeProperties:
                                                    "inquiry,inquiry.sourceAddress,inquiry.destinationAddress,inquiry.package"),
                    personalDataId = pd.Id,
                    personalData = pd,
                    companyRequestId = requestResponseDto.companyRequestId,
                    decisionDeadline = requestResponseDto.decisionDeadline,
                    requestStatus = RequestStatus.Pending
                };

                _unitOfWork.Request.Add(request);
                _unitOfWork.Save();

                request.offer.status = OfferStatus.Accepted;
                _unitOfWork.Offer.Update(request.offer);
                _unitOfWork.Save();

				return RedirectToRoute(new
				{
					controller = "Request",
					action = "Processing",
					id = request.Id
				});

			}
            return NotFound();

        }

        public IActionResult Rejected(int id)
        {
            Request request = _unitOfWork.Request.Get(u => u.Id == id, includeProperties:
                "personalData,personalData.address,offer,offer.inquiry,offer.inquiry.sourceAddress,offer.inquiry.destinationAddress,offer.inquiry.package");
            return View(request);
        }

		public IActionResult Processing(int id)
		{
			Request request = _unitOfWork.Request.Get(u => u.Id == id, includeProperties:
				"personalData,personalData.address,offer,offer.inquiry,offer.inquiry.sourceAddress,offer.inquiry.destinationAddress,offer.inquiry.package");
			return View(request);
		}

        #region Worker

        // [Authorize(Roles = SD.Role_User_Worker)]
        public async Task<IActionResult> IndexAll(string sortOrder, string searchString)
        {
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            IServiceFactory serviceFactory = _serviceFactories.FindAll(u => u.serviceId == 0).FirstOrDefault();
            var requestService = serviceFactory.createRequestService();
            var response = await requestService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                List<RequestDto>? requestDto = JsonConvert.DeserializeObject<List<RequestDto>>(Convert.ToString(response.Result));
                if (requestDto == null)
                    requestDto = new List<RequestDto>();

                if (!String.IsNullOrEmpty(searchString))
                {
                    requestDto = requestDto.Where(s => s.requestStatus.ToString() == searchString).ToList();
                }
                switch (sortOrder)
                {
                    case "Date":
                        requestDto = requestDto.OrderBy(s => s.decisionDeadline).ToList();
                        break;
                    default:
                        requestDto = requestDto.OrderByDescending(s => s.decisionDeadline).ToList();
                        break;
                }

                return View(requestDto);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["ErrorMessage"] = "There is no requests to display.";
                return RedirectToRoute(new
                {
                    controller = "Home",
                    action = "Index"
                });
            }

            return NotFound();
        }

        //[Authorize(Roles = SD.Role_User_Worker)]
        public async Task<IActionResult> Accept(string id)
        {

            IServiceFactory serviceFactory = _serviceFactories.FindAll(u => u.serviceId == 0).FirstOrDefault();
            var requestService = serviceFactory.createRequestService();

            var response = await requestService.AcceptRequestAsync<APIResponse>(id);
            if (response != null && response.IsSuccess)
            {
                TempData["SuccessMessage"] = "Status updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update status. Try again.";
            }

            return RedirectToAction("IndexAll");
        }

        [HttpGet]
        //[Authorize(Roles = SD.Role_User_Worker)]
        public IActionResult Reject(string id)
        {
            RequestRejectVM requestRejectVM = new RequestRejectVM
            {
                requestId = id
            };
            return View(requestRejectVM);
        }

        [HttpPost]
        //[Authorize(Roles = SD.Role_User_Worker)]
        public async Task<IActionResult> Reject(RequestRejectVM requestRejectVM)
        {

            IServiceFactory serviceFactory = _serviceFactories.FindAll(u => u.serviceId == 0).FirstOrDefault();
            var requestService = serviceFactory.createRequestService();

            var response = await requestService.RejectRequestAsync<APIResponse>(requestRejectVM.requestId, requestRejectVM.reason);
            if (response != null && response.IsSuccess)
            {
                TempData["SuccessMessage"] = "Status updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update status. Try again.";
            }

            return RedirectToAction("IndexAll");
        }

        #endregion
    }
}
