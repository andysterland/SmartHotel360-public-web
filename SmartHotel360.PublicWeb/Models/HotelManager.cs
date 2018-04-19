using System.Collections.Generic;
using System.Linq;

namespace SmartHotel360.PublicWeb.Controllers
{
    public class HotelManager
    {
        public List<Hotel> AllHotels;

        public HotelManager(List<Hotel> hotels)
        {
            AllHotels = hotels;
        }

        public List<Hotel> GetBestHotels()
        {
            var bestHotels = AllHotels.Where(h =>
            {
                return h.rating == 4 || h.rating == 5;
            }).OrderBy(d => d.rating).ToList();
            return bestHotels;
        }

    };
}
