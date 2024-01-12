using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.Models;
using CourierConnect.Models.Dto;
using CourierConnect.Utility;
using CourierConnectWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using AutoMapper;

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
        public async Task<IActionResult> Index(int inquiryId)
        {
            Inquiry inquiry = _unitOfWork.Inquiry.GetAll(includeProperties:"sourceAddress,destinationAddress,package").FirstOrDefault();
            //Inquiry inquiry = _unitOfWork.Inquiry.Get(u => u.Id == inquiryId, includeProperties: "sourceAddress,destinationAddress,package");
            InquiryDto inquiryDto = _mapper.Map<InquiryDto>(inquiry);
            var response = await _offerService.GetOfferAsync<APIResponse>(inquiryDto);
            if (response != null && response.IsSuccess)
            {
                OfferDto offerDto = JsonConvert.DeserializeObject<OfferDto>(Convert.ToString(response.Result));
                return View(offerDto);
            }
            return NotFound();
        }
    }
}
