using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.DataAccess.Data;
using CourierConnect.Models;
using CourierConnect.Models.Dto;
using CourierConnect.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourierConnectWeb.Services.IServices;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using CourierConnectWeb.Services.Factory;
using CourierConnect.Models.ViewModels;

namespace CourierConnectWeb.Controllers
{
    public class DeliveryController : Controller
    {
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IUnitOfWork _unitOfWork;
        //private readonly IDeliveryService _deliveryService;
        private List<IServiceFactory> _serviceFactories = new List<IServiceFactory>();
        //private readonly IRequestService _requestService;
        public DeliveryController(IUnitOfWork unitOfWork, /*IDeliveryService deliveryService, IRequestService requestService,*/
            OurServiceFactory ourServiceFactory, CurrierServiceFactory currierServiceFactory, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            //_deliveryService = deliveryService;
            _serviceFactories.Add(ourServiceFactory);
            _serviceFactories.Add(currierServiceFactory);
            //_requestService = requestService;
            _userManager = userManager;
        }

		public async Task<IActionResult> IndexAll(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            var pendingRequests = _unitOfWork.Request.FindAll(u => u.requestStatus == RequestStatus.Pending, includeProperties:
				"personalData,personalData.address,offer,offer.inquiry,offer.inquiry.sourceAddress,offer.inquiry.destinationAddress,offer.inquiry.package");

            foreach (var pendingRequest in pendingRequests)
            {
                IServiceFactory serviceFactory = _serviceFactories.FindAll(u => u.serviceId == pendingRequest.offer.companyId).FirstOrDefault();

                if (serviceFactory != null)
                {
                    var requestService = serviceFactory.createRequestService();
                    var response = await requestService.GetRequestStatusAsync<APIResponse>(pendingRequest.companyRequestId);

                    if (response != null && response.IsSuccess)
                    {
                        RequestStatusDto requestStatusDto = JsonConvert.DeserializeObject<RequestStatusDto>(Convert.ToString(response.Result));
                        if (requestStatusDto.isReady || pendingRequest.decisionDeadline < DateTime.Now)
                        {
                            var deliveryService = serviceFactory.createDeliveryService();
                            var responseDelivery = await deliveryService.GetNewDeliveryAsync<APIResponse>(pendingRequest.companyRequestId);
                            if (responseDelivery != null && responseDelivery.IsSuccess)
                            {
                                if (responseDelivery.StatusCode == System.Net.HttpStatusCode.NotAcceptable) // request rejected
                                {
                                    RequestRejectDto reject = JsonConvert.DeserializeObject<RequestRejectDto>(Convert.ToString(responseDelivery.Result));
                                    pendingRequest.requestStatus = reject.requestStatus;
                                    pendingRequest.rejectionReason = reject.rejectionReason;
                                    _unitOfWork.Request.Update(pendingRequest);
                                    _unitOfWork.Save();

                                    pendingRequest.offer.updatedDate = DateTime.Now;
                                    pendingRequest.offer.status = OfferStatus.Rejected;
                                    _unitOfWork.Offer.Update(pendingRequest.offer);
                                    _unitOfWork.Save();
                                }

                                if (responseDelivery.StatusCode == System.Net.HttpStatusCode.Created
                                    || responseDelivery.StatusCode == System.Net.HttpStatusCode.OK) // delivery created
                                {
                                    RequestAcceptDto accept = JsonConvert.DeserializeObject<RequestAcceptDto>(Convert.ToString(responseDelivery.Result));
                                    pendingRequest.requestStatus = accept.requestStatus;
                                    _unitOfWork.Request.Update(pendingRequest);
                                    _unitOfWork.Save();

                                    pendingRequest.offer.updatedDate = DateTime.Now;
                                    pendingRequest.offer.status = OfferStatus.Accepted;
                                    _unitOfWork.Offer.Update(pendingRequest.offer);
                                    _unitOfWork.Save();

                                    Delivery delivery = new Delivery
                                    {
                                        companyDeliveryId = accept.companyDeliveryId,
                                        companyId = pendingRequest.offer.companyId,
                                        requestId = pendingRequest.Id,
                                        request = pendingRequest
                                    };
                                    _unitOfWork.Delivery.Add(delivery);
                                    _unitOfWork.Save();
                                }
                            }
                        }
                    }
                }
			}

            List<DeliveryVM> deliveriesDto = new List<DeliveryVM>();

			var id = _userManager.GetUserId(User);
			var deliveries = _unitOfWork.Delivery.FindAll(u => u.request.offer.inquiry.clientId == id, includeProperties:
                "request,request.offer");

            foreach (var delivery in deliveries)
            {
                IServiceFactory serviceFactory = _serviceFactories.FindAll(u => u.serviceId == delivery.request.offer.companyId).FirstOrDefault();

                if (serviceFactory != null)
                {
                    var deliveryService = serviceFactory.createDeliveryService();
                    var response = await deliveryService.GetDeliveryAsync<APIResponse>(delivery.companyDeliveryId);
                    if (response != null && response.IsSuccess)
                    {
                        DeliveryDto deliveryDto = JsonConvert.DeserializeObject<DeliveryDto>(Convert.ToString(response.Result));
                        DeliveryVM deliveryVM = new DeliveryVM
                        {
                            delivery = delivery,
                            deliveryDto = deliveryDto
                        };
                        deliveriesDto.Add(deliveryVM);
                    }
                }

			}

			//if (!String.IsNullOrEmpty(searchString))
			//{
			//    objInquiryList = objInquiryList.Where(s => s.Id.ToString() == searchString);
			//}
			//       switch (sortOrder)
			//       {
			//           case "Date":
			//deliveriesDto = deliveriesDto.OrderBy(s => s.deliveryDate);
			//               break;
			//           case "date_desc":
			//deliveriesDto = deliveriesDto.OrderByDescending(s => s.deliveryDate);
			//               break;
			//           default:
			//deliveriesDto = deliveriesDto.OrderBy(s => s.Id);
			//               break;
			//       }
			//       return View(objInquiryList.ToList());
			return View(deliveriesDto);
			return NotFound();
        }
        public async Task<IActionResult> DeliveryStatus()
        {
			//var response = await _deliveryService.GetDeliveryAsync<APIResponse>(companyDeliveryId);
			//if (response != null && response.IsSuccess)
			//{
			//	DeliveryDto deliveryDto = JsonConvert.DeserializeObject<DeliveryDto>(Convert.ToString(response.Result));
			//	return View(deliveryDto);
			//}
			return NotFound();
		}

        public async Task<IActionResult> Index(int id)
        {
            
            Delivery delivery = _unitOfWork.Delivery.Get(u => u.Id == id);
            if (delivery == null)
                return NotFound();

            IServiceFactory serviceFactory = _serviceFactories.FindAll(u => u.serviceId == delivery./*request.offer.*/companyId).FirstOrDefault();
            var deliveryService = serviceFactory.createDeliveryService();

            var response = await deliveryService.GetDeliveryAsync<APIResponse>(delivery.companyDeliveryId);
            if (response != null && response.IsSuccess)
            {
                DeliveryDto deliveryDto = JsonConvert.DeserializeObject<DeliveryDto>(Convert.ToString(response.Result));
                DeliveryVM deliveryVM = new DeliveryVM
                {
                    delivery = delivery,
                    deliveryDto = deliveryDto
                };
                return View(deliveryVM);
            }
            return NotFound();
        }

        public async Task<IActionResult> Details(int id)
        {
            Delivery delivery = _unitOfWork.Delivery.Get(u => u.request.offer.inquiryId == id);
            if (delivery == null)
                return NotFound();

            return RedirectToRoute(new
            {
                controller = "Delivery",
                action = "Index",
                id = delivery.Id
            });
        }

        [HttpGet]
        public IActionResult Add()
		{
			return View();
		}

        [HttpPost]
        public IActionResult Add(string id)
        {
			Delivery delivery = _unitOfWork.Delivery.Get(u => u.companyDeliveryId == id,
                includeProperties:"request,request.offer,request.offer.inquiry");

            if (delivery != null)
            {
                var clientId = _userManager.GetUserId(User);
                delivery.request.offer.inquiry.clientId = clientId;
                _unitOfWork.Delivery.Update(delivery);
                _unitOfWork.Save();

				TempData["SuccessMessage"] = "Delivery added successfully.";
			}
			else
			{
				TempData["ErrorMessage"] = "Failed to add delivery. Delivery not found.";
			}

			return RedirectToAction("IndexAll");
		}

        public async Task<IActionResult> Cancel(int id)
        {
            Delivery delivery = _unitOfWork.Delivery.Get(u => u.Id == id);
            if (delivery == null)
                return NotFound();

            IServiceFactory serviceFactory = _serviceFactories.FindAll(u => u.serviceId == delivery./*request.offer.*/companyId).FirstOrDefault();
            var deliveryService = serviceFactory.createDeliveryService();

            var response = await deliveryService.CancelDeliveryAsync<APIResponse>(delivery.companyDeliveryId);
            if (response != null && response.IsSuccess)
            {
                TempData["SuccessMessage"] = "Delivery canceled successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to cancel delivery. Try again.";
            }

            return RedirectToAction("IndexAll");
        }


        #region Courier

        //[Authorize(Roles = SD.Role_User_Courier)]
        public async Task<IActionResult> IndexAllCourier()
        {
            string courierUserName = _userManager.GetUserName(User); //= "hryshko.alina@gmail.com";

            IServiceFactory serviceFactory = _serviceFactories.FindAll(u => u.serviceId == 0).FirstOrDefault();
            var deliveryService = serviceFactory.createDeliveryService();

            var response = await deliveryService.GetAllCourierDeliveryAsync<APIResponse>(courierUserName);
            if (response != null && response.IsSuccess)
            {
                List<DeliveryDto>? offerDto = JsonConvert.DeserializeObject<List<DeliveryDto>>(Convert.ToString(response.Result));
                return View(offerDto);
            }

            return View(null);
        }

        //[Authorize(Roles = SD.Role_User_Courier)]
        public async Task<IActionResult> PickUp(string id)
        {

            IServiceFactory serviceFactory = _serviceFactories.FindAll(u => u.serviceId == 0).FirstOrDefault();
            var deliveryService = serviceFactory.createDeliveryService();

            var response = await deliveryService.PickUpPackageAsync<APIResponse>(id);
            if (response != null && response.IsSuccess)
            {
                TempData["SuccessMessage"] = "Status updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update status. Try again.";
            }

            return RedirectToAction("IndexAllCourier");
        }

        //[Authorize(Roles = SD.Role_User_Courier)]
        public async Task<IActionResult> Deliver(string id)
        {

            IServiceFactory serviceFactory = _serviceFactories.FindAll(u => u.serviceId == 0).FirstOrDefault();
            var deliveryService = serviceFactory.createDeliveryService();

            var response = await deliveryService.DeliverPackageAsync<APIResponse>(id);
            if (response != null && response.IsSuccess)
            {
                TempData["SuccessMessage"] = "Status updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update status. Try again.";
            }

            return RedirectToAction("IndexAllCourier");
        }


        [HttpGet]
        //[Authorize(Roles = SD.Role_User_Courier)]
        public IActionResult CannotDeliver(string id)
        {
            CannotDeliverVM cannotDeliverVM = new CannotDeliverVM
            {
                deliveryId = id
            };
            return View(cannotDeliverVM);
        }

        [HttpPost]
        //[Authorize(Roles = SD.Role_User_Courier)]
        public async Task<IActionResult> CannotDeliver(CannotDeliverVM cannotDeliverVM)
        {

            IServiceFactory serviceFactory = _serviceFactories.FindAll(u => u.serviceId == 0).FirstOrDefault();
            var deliveryService = serviceFactory.createDeliveryService();

            var response = await deliveryService.CannotDeliverPackageAsync<APIResponse>(cannotDeliverVM.deliveryId, cannotDeliverVM.reason);
            if (response != null && response.IsSuccess)
            {
                TempData["SuccessMessage"] = "Status updated successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to update status. Try again.";
            }

            return RedirectToAction("IndexAllCourier");
        }

        #endregion
    }
}
