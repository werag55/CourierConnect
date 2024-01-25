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
	public class RequestController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IRequestService _requestService;
		private readonly IMapper _mapper;
		public RequestController(IUnitOfWork unitOfWork, IRequestService requestService, IMapper mapper, UserManager<IdentityUser> userManager)
		{
			_unitOfWork = unitOfWork;
			_requestService = requestService;
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
				offerId = Id
			};

			return View(requestCreateVM);
		}

		[HttpPost]
		public async Task<IActionResult> Create(RequestCreateVM requestCreateVM)
		{
			RequestSendDto requestSendDto = requestCreateVM.requestSendDto;

            var response = await _requestService.GetRequestAsync<APIResponse>(requestSendDto);
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

        public IActionResult Reject(int id)
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
	}
}
