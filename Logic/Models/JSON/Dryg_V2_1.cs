using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Web.Models.JSON
{
    public class Dryg_V2_1
    {
        [JsonProperty("dagar")]
        public List<Day> Days;
    }
}
