using Newtonsoft.Json;
using SmartHotel360.PublicWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace SmartHotel360.PublicWeb.Services
{
    public class HotelsService
    {
        //public static HotelsService Load()
        //{
        //    using (var client = new HttpClient())
        //    {
        //        using (var response = client.GetAsync(new Uri(localSettings.SettingsUrl)).Result)
        //        {
        //            response.EnsureSuccessStatusCode();
        //            string responseBody = response.Content.ReadAsStringAsync().Result;
        //            var model = JsonConvert.DeserializeObject<ServerSettings>(responseBody);
        //            model.Production = localSettings.Production;
        //            model.FakeAuth = localSettings.FakeAuth;

        //            return new SettingsService(model);
        //        }
        //    }
        //}

        public List<Hotel> Load()
        {
            string fileName = "C:\\SmartHotel360\\SmartHotel360.PublicWeb\\App_Data\\Hotels\\hotels.json";
            //string fileName = HttpContext.Current.Server.MapPath("~/App-Data/Hotels/hotels.json");
            string json = System.IO.File.ReadAllText(fileName);

            List<Hotel> hotels = JsonConvert.DeserializeObject<List<Hotel>>(json);
            return hotels;
        }
    }
}
