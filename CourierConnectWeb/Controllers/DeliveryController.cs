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
        public IActionResult Index()
        {
            List<Delivery> objInquiryList = _unitOfWork.Delivery.GetAll().ToList();
            return View(objInquiryList);
        }
        public IActionResult DeliveryStatus(Delivery delivery)
        {
            return View(delivery);
        }
    }
}
