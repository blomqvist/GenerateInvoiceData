using Web.Models.JSON;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Logic.Models
{
    public class DateInfo
    {
        private static DateInfo _instance;
        private List<Day> _days;
        public List<Day> Days => _days ?? (_days = new List<Day>());

        /// <summary>
        /// Returns the instance of this class.
        /// </summary>
        public static DateInfo Instance => _instance ?? (_instance = new DateInfo());

        private DateInfo() { }

        /// <summary>
        /// Fetches date info from api.dryg.net using the v2.1 API
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        public async Task ReadDays(int year, int month)
        {
            var filepath = $"{Directory.GetCurrentDirectory()}\\{year}\\{month:d2}.json";
            string json;
            if (!File.Exists(filepath))
            {
                Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}\\{year}");
                var url = Common.Constants.DrygApiUrl + $"{year}/{month:d2}";
                using (var client = new HttpClient())
                {
                    json = await client.GetStringAsync(url);
                }
                var file = File.CreateText(filepath);
                file.Write(json);
                file.Dispose();
            }
            else
            {
                json = File.ReadAllText(filepath);
            }

            var jsonObject = JsonConvert.DeserializeObject<Dryg_V2_1>(json);
            Days.AddRange(jsonObject.Days);
        }

        public bool IsHoliday(int day)
        {
            return !Days.ElementAt(day - 1).WorkDay;
        }

        public bool IsWorkday(int day)
        {
            return Days.ElementAt(day - 1).WorkDay;
        }
    }
}
