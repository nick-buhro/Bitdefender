using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NickBuhro.Bitdefender.Models
{
    [Serializable]
    public sealed class Account
    {
        /// <summary>
        /// The ID of the user account.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("email")]
        public int Email { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("profile")]
        public int Profile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("role")]
        public AccountRole Role { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("rights")]
        public Dictionary<string, bool> Rights { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("companyName")]
        public string CompanyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("companyId")]
        public string CompanyId { get; set; }
    }
}
