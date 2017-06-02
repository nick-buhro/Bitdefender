using Newtonsoft.Json;
using System;

namespace NickBuhro.Bitdefender.Models
{
    [Serializable]
    public class Company
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
