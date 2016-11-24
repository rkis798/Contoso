using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Bot_Application1.Models
{
    public class Creditcard
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "decs")]
        public string Desc { get; set; }

        [JsonProperty(PropertyName = "img")]
        public string Img { get; set; }
    }
}