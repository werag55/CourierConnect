using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.DataAccess.Data;
using CourierConnect.Models;
using CourierConnect.Models.Dto;
using CourierConnect.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourierConnectWeb.Services.IServices;
using Newtonsoft.Json;

namespace CourierConnectWeb.Controllers
{
    public class DeliveryController : Controller
    {
    
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDeliveryService _deliveryService;
        public DeliveryController(IUnitOfWork unitOfWork, IDeliveryService deliveryService)
        {
            _unitOfWork = unitOfWork;
            _deliveryService = deliveryService;
        }
        public IActionResult IndexAll(string sortOrder, string searchString)
        {
            //ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            //var objInquiryList = _unitOfWork.Delivery.GetAll();
            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    objInquiryList = objInquiryList.Where(s => s.Id.ToString() == searchString);
            //}
            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        objInquiryList = objInquiryList.OrderByDescending(s => s.Id);
            //        break;
            //    case "Date":
            //        objInquiryList = objInquiryList.OrderBy(s => s.deliveryDate);
            //        break;
            //    case "date_desc":
            //        objInquiryList = objInquiryList.OrderByDescending(s => s.deliveryDate);
            //        break;
            //    default:
            //        objInquiryList = objInquiryList.OrderBy(s => s.Id);
            //        break;
            //}
            //return View(objInquiryList.ToList());
            return NotFound();
        }
        public IActionResult DeliveryStatus(Delivery delivery)
        {
            if(delivery.deliveryStatus == CourierConnect.Models.DeliveryStatus.CannotDeliver)
            {
                return View("CannotDeliverView", delivery);
            }
            else
            {
                return View("CanDeliverView", delivery);
            }
        }

        public async Task<IActionResult> Index(int id)
        {
            Delivery delivery = _unitOfWork.Delivery.Get(u => u.Id == id);
            if (delivery == null)
                return NotFound();

            var response = await _deliveryService.GetDeliveryAsync<APIResponse>(id);
            if (response != null && response.IsSuccess)
            {
                DeliveryDto deliveryDto = JsonConvert.DeserializeObject<DeliveryDto>(Convert.ToString(response.Result));
                return View(deliveryDto);
            }
            return NotFound();

        }
    }
}
