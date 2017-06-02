using Newtonsoft.Json;
using System;

namespace NickBuhro.Bitdefender.Models
{
    [Serializable]
    public sealed class CompanyDetails: Company
    {
        [JsonProperty("type")]
        public CompanyType Type { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("phone")]
        public string Phone { get; set; }

        [JsonProperty("canBeManagedByAbove")]
        public bool CanBeManagedByAbove { get; set; }

        [JsonProperty("isSuspended")]
        public bool IsSuspended { get; set; }
    }
}
