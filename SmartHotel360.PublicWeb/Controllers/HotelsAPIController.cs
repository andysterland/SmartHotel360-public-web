using System;
using System.Collections.Generic;
using System.Linq;
using System.IO; 
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SmartHotel360.PublicWeb.Controllers
{
    [Produces("application/json")]
    [Route("api/Hotels")]
    public class HotelsAPIController : Controller
    {
        // GET: api/Hotels/best
        [HttpGet("best")]
        public IEnumerable<Hotel> Get()
        {
            List<Hotel> allHotels = GetHotelList();

            return BestHotels(allHotels);
        }
        
        private List<Hotel> GetHotelList()
        {
            string fileName = "C:\\Users\\dech\\Source\\Repos\\SmartHotel360-public-web-ttd\\SmartHotel360.PublicWeb\\App_Data\\Hotels\\hotels.json";
            string json = System.IO.File.ReadAllText(fileName);

            List<Hotel> hotels = JsonConvert.DeserializeObject<List<Hotel>>(json);
            return hotels;
        }

        private List<Hotel> BestHotels(List<Hotel> allHotels)
        {
            HotelManager hotelManager = new HotelManager(allHotels);
            return hotelManager.GetBestHotels();

        }
    }

    public class HotelManager
    {
        public List<Hotel> AllHotels;

        public HotelManager(List<Hotel> hotels)
        {
            AllHotels = hotels;
        }

        public List<Hotel> GetBestHotels()
        {
            return AllHotels.Where(h =>
            {
                return h.rating == 4 || h.rating == 5;
            }).OrderBy(d => d.rating).ToList();

            //return AllHotels.Where(h =>
            //{
            //    return h.rating >= 4;
            //}).OrderBy(d => d.rating).ToList();
        }

    };
}
