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
            List<Hotel> bestHotels = new List<Hotel>();

            for(int i = 0; i < AllHotels.Count; i ++)
            {
               
                if (AllHotels[i].rating == 4 || AllHotels[i].rating == 5)
                {
                    bestHotels.Add(AllHotels[i]);
                }
            }

            bestHotels.OrderBy(d => d.rating);
            return bestHotels;
        }

    };
}
