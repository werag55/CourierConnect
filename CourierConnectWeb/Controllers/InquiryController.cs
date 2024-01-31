using CourierConnect.DataAccess.Data;
using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.Models;
using CourierConnect.Models.POCO;
using CourierConnect.Models.Dto;
using CourierConnect.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CourierConnect.Models.ViewModels;
using Google.Apis.Admin.Directory.directory_v1.Data;
using ICSharpCode.Decompiler.CSharp.Syntax;
using CourierConnectWeb.Services.Factory;
using Newtonsoft.Json;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace CourierConnectWeb.Controllers
{
    public class InquiryController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private List<IServiceFactory> _serviceFactories = new List<IServiceFactory>();
        private readonly IMapper _mapper;
        public InquiryController(IUnitOfWork unitOfWork, OurServiceFactory ourServiceFactory, CurrierServiceFactory currierServiceFactory, 
            CourierHubServiceFactory courierHubServiceFactory, IMapper mapper, UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _context = context;
            _serviceFactories.Add(ourServiceFactory);
            _serviceFactories.Add(currierServiceFactory);
            _serviceFactories.Add(courierHubServiceFactory);
            _mapper = mapper;
        }

        [Authorize(Roles = SD.Role_User_Worker)]
        public async Task<IActionResult> IndexAll(string sortOrder)
        {
            ViewBag.PickUpDateSortParm = sortOrder == "PickUpDate" ? "pickup_date_desc" : "PickUpDate";
            ViewBag.DeliveryDateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            IServiceFactory serviceFactory = _serviceFactories.FindAll(u => u.serviceId == 0).FirstOrDefault();
            var inquiryService = serviceFactory.createInquiryService();
            var response = await inquiryService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                List<InquiryDto>? inquiryDto = JsonConvert.DeserializeObject<List<InquiryDto>>(Convert.ToString(response.Result));
                if (inquiryDto == null)
                    inquiryDto = new List<InquiryDto>();

                switch (sortOrder)
                {
                    case "Date":
                        inquiryDto = inquiryDto.OrderBy(s => s.deliveryDate).ToList();
                        break;
                    case "date_desc":
                        inquiryDto = inquiryDto.OrderByDescending(s => s.deliveryDate).ToList();
                        break;
                    case "PickUpDate":
                        inquiryDto = inquiryDto.OrderBy(s => s.pickupDate).ToList();
                        break;
                    case "pickup_date_desc":
                        inquiryDto = inquiryDto.OrderByDescending(s => s.pickupDate).ToList();
                        break;
                    default:
                        inquiryDto = inquiryDto.OrderByDescending(s => s.pickupDate).ToList();
                        break;
                }

                return View(inquiryDto);
            }
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                TempData["ErrorMessage"] = "There is no offers to display.";
                return RedirectToRoute(new
                {
                    controller = "Home",
                    action = "Index"
                });
            }

            return NotFound();
        }

        private bool hasDelivery(int inquiryId)
        {
            var offer = _unitOfWork.Offer.Get(u => u.inquiryId == inquiryId);
            if (offer == default)
                return false;
            var request = _unitOfWork.Request.Get(u => u.offerId == offer.Id);
            if (request == default)
                return false;
            var delivery = _unitOfWork.Delivery.Get(u => u.requestId == request.Id);
            if (delivery == default)
                return false;
            return true;
        }

        [Authorize(Roles = SD.Role_User_Client)]
        public IActionResult ClientInquiries()
        {
            var id = _userManager.GetUserId(User);

            List<Inquiry> objInquiryList = _unitOfWork.Inquiry.FindAll(u => u.clientId.Equals(id),
                "sourceAddress,destinationAddress,package").ToList();

            List<ClientInquiryVM> objInquiryVMList = new List<ClientInquiryVM>();
            foreach (var inquiry in objInquiryList)
            {
                InquiryPOCO inquiryPOCO = _mapper.Map<InquiryPOCO>(inquiry);
                bool hasDelivery = this.hasDelivery(inquiry.Id);
                objInquiryVMList.Add(new ClientInquiryVM(inquiryPOCO, hasDelivery));
            }
            return View(objInquiryVMList.OrderByDescending(s => s.Inquiry.creationDate).ToList());
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
            var destinationAddress = _context.Addresses.FirstOrDefault(a =>
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
				//return RedirectToAction("Index");
				return RedirectToRoute(new
				{
					controller = "Offer",
					action = "Create",
					id = obj.Id
				});
			//}

            return View(obj);

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
