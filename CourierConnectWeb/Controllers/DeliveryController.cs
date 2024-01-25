﻿using CourierConnect.DataAccess.Repository.IRepository;
using CourierConnect.DataAccess.Data;
using CourierConnect.Models;
using CourierConnect.Models.Dto;
using CourierConnect.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CourierConnectWeb.Services.IServices;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;

namespace CourierConnectWeb.Controllers
{
    public class DeliveryController : Controller
    {
		private readonly UserManager<IdentityUser> _userManager;
		private readonly IUnitOfWork _unitOfWork;
        private readonly IDeliveryService _deliveryService;
        private readonly IRequestService _requestService;
        public DeliveryController(IUnitOfWork unitOfWork, IDeliveryService deliveryService, IRequestService requestService, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _deliveryService = deliveryService;
            _requestService = requestService;
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
                var response = await _requestService.GetRequestStatusAsync<APIResponse>(pendingRequest.companyRequestId);

				if (response != null && response.IsSuccess)
				{
                    RequestStatusDto requestStatusDto = JsonConvert.DeserializeObject<RequestStatusDto>(Convert.ToString(response.Result));
                    if (requestStatusDto.isReady || pendingRequest.decisionDeadline < DateTime.Now)
                    {
						var responseDelivery = await _deliveryService.GetNewDeliveryAsync<APIResponse>(pendingRequest.companyRequestId);
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

                            if (responseDelivery.StatusCode == System.Net.HttpStatusCode.Created) // delivery created
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

            List<DeliveryDto> deliveriesDto = new List<DeliveryDto>();

			var id = _userManager.GetUserId(User);
			var deliveries = _unitOfWork.Delivery.FindAll(u => u.request.offer.inquiry.clientId == id);

            foreach (var delivery in deliveries)
            {
				var response = await _deliveryService.GetDeliveryAsync<APIResponse>(delivery.companyDeliveryId);
				if (response != null && response.IsSuccess)
                {
                    DeliveryDto deliveryDto = JsonConvert.DeserializeObject<DeliveryDto>(Convert.ToString(response.Result));
                    deliveriesDto.Add(deliveryDto);
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

            var response = await _deliveryService.GetDeliveryAsync<APIResponse>(delivery.companyDeliveryId);
            if (response != null && response.IsSuccess)
            {
                DeliveryDto deliveryDto = JsonConvert.DeserializeObject<DeliveryDto>(Convert.ToString(response.Result));
                return View(deliveryDto);
            }
            return NotFound();

        }
    }
}
