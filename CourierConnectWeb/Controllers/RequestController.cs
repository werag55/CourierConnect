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
		private readonly IOfferService _offerService;
		private readonly IMapper _mapper;
		public RequestController(IUnitOfWork unitOfWork, IOfferService offerService, IMapper mapper, UserManager<IdentityUser> userManager)
		{
			_unitOfWork = unitOfWork;
			_offerService = offerService;
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
			PersonalData pd = _unitOfWork.PersonalData.Get(u => u.clientId == clientId, includeProperties: "address") ?? new PersonalData();
			Offer offer = _unitOfWork.Offer.Get(u => u.Id  == Id, includeProperties: "inquiry");

			if (offer == null) 
				return NotFound();

			Request request = new Request
			{
				offerId	= Id,
				offer = offer,
				personalData = pd,
				personalDataId = pd.Id
			};

			return View(request);
		}

		[HttpPost]
		public IActionResult Create(Request request)
		{
            string? clientId = _userManager.GetUserId(User);
			PersonalData pd = request.personalData;
			if (_unitOfWork.PersonalData.Get(u => u.clientId == clientId) == null)
			{
				Address ad = request.personalData.address;
				_unitOfWork.Address.Add(ad);
                _unitOfWork.Save();
                pd.addressId = ad.Id;
				pd.clientId = clientId;
				_unitOfWork.PersonalData.Add(pd);
				_unitOfWork.Save();
			}
			request.offer = _unitOfWork.Offer.Get(u => u.Id == request.offerId, includeProperties: "inquiry");
			request.isAccepted = false;
			request.personalDataId = pd.Id;
			request.Id = 0;
			_unitOfWork.Request.Add(request);
			_unitOfWork.Save();

			// wysłać request do firm i czekać na odp

			return View(request);

        }
	}
}
