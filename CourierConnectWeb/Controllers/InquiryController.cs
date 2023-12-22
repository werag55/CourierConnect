using CourierConnect.DataAccess.Data;
using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.Models;
using CourierConnect.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace CourierConnectWeb.Controllers
{
    public class InquiryController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public InquiryController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            List<Inquiry> objInquiryList = _unitOfWork.Inquiry.GetAll().ToList();
            return View(objInquiryList);
        }

        public IActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Create(Inquiry obj)
        {
            string userId = _userManager.GetUserId(User);
            obj.clientId = userId;

            var sourceAddress = _context.Addresses
                .FirstOrDefault(a =>
                a.streetName == obj.sourceAddress.streetName &&
                a.flatNumber == obj.sourceAddress.flatNumber &&
                a.houseNumber == obj.sourceAddress.houseNumber &&
                a.postcode == obj.sourceAddress.postcode);

            if (sourceAddress == null)
            {

                sourceAddress = obj.sourceAddress;
                _context.Addresses.Add(sourceAddress);
            }
            else
            {

            }
          var destinationAddress = _context.Addresses
         .FirstOrDefault(a =>
         a.streetName == obj.destinationAddress.streetName &&
         a.flatNumber == obj.destinationAddress.flatNumber &&
         a.houseNumber == obj.destinationAddress.houseNumber &&
         a.postcode == obj.destinationAddress.postcode);

            if (destinationAddress== null)
            {
                destinationAddress = obj.destinationAddress;
                _context.Addresses.Add(destinationAddress);
            }

            destinationAddress = obj.destinationAddress;
            Package package = obj.package;
            _context.Packages.Add(package);
            
            
            _context.SaveChanges();

            
            obj.creationDate = DateTime.Now;
            obj.destinationAddressId = destinationAddress.Id;
            obj.sourceAddressId = sourceAddress.Id;
            obj.packageId = package.Id;

          

            //if (ModelState.IsValid)
            //{

                _unitOfWork.Inquiry.Add(obj);
                _unitOfWork.Save();
                TempData["success"] = "Inquiry created successfully";
                string emailSubject = "Created Inqury";
                string toEmail = _userManager.GetUserName(User);
                string message = "Your inquiry was created corectly\nThanks for using our Website";
                EmailSender email = new EmailSender();

                email.SendEmailAsync(emailSubject, toEmail, message).Wait();
                return RedirectToAction("Index");
            //}

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
