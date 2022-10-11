using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge7EventHub.Contracts
{
    public class Receipt
    {
        [JsonProperty("saleDate")]
        public DateTime SalesDate { get; set; }

        [JsonProperty("salesNumber")]
        public int SalesNumber { get; set; }

        [JsonProperty("storeLocation")]
        public string StoreLocation { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("totalCost")]
        public decimal TotalCost { get; set; }

        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }

    }
}
