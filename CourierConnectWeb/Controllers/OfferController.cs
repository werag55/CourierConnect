 using CourierConnect.Models;
using CourierConnect.Models.Dto;
using CourierConnect.Utility;
using CourierConnectWeb.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CourierConnectWeb.Controllers
{
    public class OfferController : Controller
    {
        private readonly IOfferService _OfferService;
        public OfferController(IOfferService OfferService)
        {
            _OfferService = OfferService;
        }
        public async Task<IActionResult> Index()
        {
            List<OfferDTO> list = new();

            var response = await _OfferService.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<OfferDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }
    }
}
