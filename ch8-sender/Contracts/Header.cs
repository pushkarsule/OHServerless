using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge7EventHub.Contracts
{
    public class Header
    {
        [JsonProperty("salesNumber")]
        public int SalesNumber { get; set; }

        [JsonProperty("dateTime")]
        public DateTime DateTime { get; set; }

        [JsonProperty("locationId")]
        public string LocationId { get; set; }

        [JsonProperty("locationName")]
        public string LocationName { get; set; }

        [JsonProperty("locationAddress")]
        public string LocationAddress { get; set; }

        [JsonProperty("locationPostcode")]
        public string LocationPostcode { get; set; }

        [JsonProperty("totalCost")]
        public decimal TotalCost { get; set; }

        [JsonProperty("totalTax")]
        public decimal TotalTax { get; set; }

        [JsonProperty("receiptUrl")]
        public string ReceiptUrl { get; set; }

        
        [JsonProperty("DateTime")]
        public DateTime dateTime { get; set; }

        [JsonProperty("SalesNumber")]
        public int salesNumber{get;set;}

    }
}
