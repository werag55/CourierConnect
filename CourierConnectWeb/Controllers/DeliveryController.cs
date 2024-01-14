using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.DataAccess.Data;
using CourierConnect.Models;
using CourierConnect.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourierConnectWeb.Controllers
{
    public class DeliveryController : Controller
    {
    
        private readonly IUnitOfWork _unitOfWork;
        public DeliveryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var objInquiryList = _unitOfWork.Delivery.GetAll();
            if (!String.IsNullOrEmpty(searchString))
            {
                objInquiryList = objInquiryList.Where(s => s.Id.ToString() == searchString);
            }
            switch (sortOrder)
            {
                case "name_desc":
                    objInquiryList = objInquiryList.OrderByDescending(s => s.Id);
                    break;
                case "Date":
                    objInquiryList = objInquiryList.OrderBy(s => s.deliveryDate);
                    break;
                case "date_desc":
                    objInquiryList = objInquiryList.OrderByDescending(s => s.deliveryDate);
                    break;
                default:
                    objInquiryList = objInquiryList.OrderBy(s => s.Id);
                    break;
            }
            return View(objInquiryList.ToList());
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
    }
}
