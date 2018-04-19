using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SmartHotel360.PublicWeb.Services;

namespace SmartHotel360.PublicWeb.Controllers
{
    [Produces("application/json")]
    [Route("api/Hotels")]
    public class HotelsAPIController : Controller
    {
        private readonly HotelsService _hotelsService;

        public HotelsAPIController(HotelsService hotelsService)
        {
            _hotelsService = hotelsService;
        }

        // GET: api/Hotels/best
        [HttpGet("best")]
        public IActionResult Get()
        {
            var allHotels = _hotelsService.Load();
            var bestHotels = BestHotels(allHotels);

            return Ok(bestHotels);
        }

        private List<Hotel> BestHotels(List<Hotel> allHotels)
        {
            HotelManager hotelManager = new HotelManager(allHotels);
            return hotelManager.GetBestHotels();

        }
    }
}
