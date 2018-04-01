using Newtonsoft.Json;
using System;

namespace NickBuhro.Bitdefender.Models
{
    [Serializable]
    public sealed class AccountsList
    {
        /// <summary>
        /// The current page displayed.
        /// </summary>
        [JsonProperty("page")]
        public int Page { get; set; }

        /// <summary>
        /// The total number of available pages.
        /// </summary>
        [JsonProperty("pagesCount")]
        public int PagesCount { get; set; }

        /// <summary>
        /// The total number of returned items per page.
        /// </summary>
        [JsonProperty("perPage")]
        public int PerPage { get; set; }

        /// <summary>
        /// The list of user accounts.
        /// </summary>
        [JsonProperty("items")]
        public Account[] Items { get; set; }

        /// <summary>
        /// The total number of items.
        /// </summary>
        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
