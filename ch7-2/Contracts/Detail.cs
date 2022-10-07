using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge7EventHub.Contracts
{
    public class Detail
    {
        [JsonProperty("productId")]
        public Guid ProductId { get; set; }

        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        [JsonProperty("unitCost")]
        public decimal UnitCost { get; set; }

        [JsonProperty("totalCost")]
        public decimal TotalCost { get; set; }

        [JsonProperty("totalTax")]
        public decimal TotalTax { get; set; }

        [JsonProperty("productName")]
        public string ProductName { get; set; }

        [JsonProperty("productDescription")]
        public string ProductDescription { get; set; }


    }
}
