using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.DataAccess.Data;
using CourierConnect.Models;
using CourierConnect.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourierConnectWeb.Email;
using Microsoft.AspNetCore.Identity;
using CourierConnect.Models.ViewModels;

namespace CourierConnectWeb.Controllers
{
    public class InquiryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailsender;
        private readonly UserManager<IdentityUser> _userManager;
        public InquiryController(IUnitOfWork unitOfWork, IEmailSender emailSender, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _emailsender = emailSender;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            List<Inquiry> objInquiryList = _unitOfWork.Inquiry.GetAll().ToList();
            return View(objInquiryList);
        }

        private bool hasDelivery(int inquiryId)
        {
            var offer = _unitOfWork.Offer.Get(u => u.Id == inquiryId);
            if (offer == default)
                return false;
            var request = _unitOfWork.Request.Get(u => u.Id == offer.Id);
            if (request == default)
                return false;
            var delivery = _unitOfWork.Delivery.Get(u => u.Id == request.Id);
            if (delivery == default)
                return false;
            return true;
        }

        public IActionResult ClientInquiries()
        {
            var id = _userManager.GetUserId(User);

            List<Inquiry> objInquiryList = _unitOfWork.Inquiry.FindAll(u => u.clientId.Equals(id),
                "sourceAddress,destinationAddress,package").ToList();

            List<ClientInquiryVM> objInquiryVMList = new List<ClientInquiryVM>();
            foreach (var inquiry in objInquiryList)
            {
                bool hasDelivery = this.hasDelivery(inquiry.Id);
                objInquiryVMList.Add(new ClientInquiryVM(inquiry, hasDelivery));
            }
            return View(objInquiryVMList);
        }

        public IActionResult Create()
        {
            return View();
        }

        //public IActionResult Create(Inquiry obj)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Inquiry.Add(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Inquiry created successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();

        //}

        public async Task<IActionResult> SendEmail(Inquiry obj) 
        {
            var receiver = "gina.grant@ethereal.email";
            var subject = "New Inquiry was created";
            var message = "hello!\n u have just created new inquiry at CourierConnect!\n" +
                "Delivery Date: " + obj.deliveryDate.ToShortDateString() + "\nInquiry ID: " + obj.Id.ToString() +
                "Destination Address:" + obj.destinationAddress.streetName.ToString() + obj.destinationAddress.houseNumber.ToString() +
                obj.destinationAddress.flatNumber.ToString() + obj.destinationAddress.postcode.ToString();

            await _emailsender.SendEmailAsync(receiver, subject, message);

            return View();
        }
        [HttpPost]
        public IActionResult Create(Inquiry obj)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Inquiry.Add(obj);
                _unitOfWork.Address.Add(obj.destinationAddress);
                _unitOfWork.Save();
                TempData["success"] = "Inquiry created successfully";
                _ = SendEmail(obj);
                return RedirectToAction("Index");
            }
            return View();

        }

        //public IActionResult Edit(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    Inquiry? inquiryFromDb = _unitOfWork.Inquiry.Get(u => u.Id == id);
        //    //Inquiry? inquiryFromDb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);
        //    //Inquiry? inquiryFromDb2 = _db.Categories.Where(u=>u.Id==id).FirstOrDefault();

        //    if (inquiryFromDb == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(inquiryFromDb);
        //}
        //[HttpPost]
        //public IActionResult Edit(Inquiry obj)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Inquiry.Update(obj);
        //        _unitOfWork.Save();
        //        TempData["success"] = "Inquiry updated successfully";
        //        return RedirectToAction("Index");
        //    }
        //    return View();

        //}

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Inquiry? InquiryFromDb = _unitOfWork.Inquiry.Get(u => u.Id == id);

            if (InquiryFromDb == null)
            {
                return NotFound();
            }
            return View(InquiryFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Inquiry? obj = _unitOfWork.Inquiry.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Inquiry.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Inquiry deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
