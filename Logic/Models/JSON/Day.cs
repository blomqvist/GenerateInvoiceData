using System;
using Newtonsoft.Json;

namespace Web.Models.JSON
{
    public class Day
    {
        [JsonProperty("datum")]
        public DateTime Date { get; set; }

        [JsonProperty("arbetsfri dag")]
        public string _WorkDay;
        public bool WorkDay
        {
            get
            {
                return _WorkDay == "Nej";
            }
        }
    }
}
