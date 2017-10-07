using Newtonsoft.Json;
using System.Collections.Generic;

namespace Web.JsonModels
{
    public class Autocomplete
    {
        [JsonProperty("customers")]
        public List<string> Customers { get; set; } = new List<string>();
        [JsonProperty("projects")]
        public List<string> Projects { get; set; } = new List<string>();
        [JsonProperty("activities")]
        public List<string> Activities { get; set; } = new List<string>();
    }
}
