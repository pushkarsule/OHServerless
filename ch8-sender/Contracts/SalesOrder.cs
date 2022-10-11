using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge7EventHub.Contracts
{
    public class SalesOrder
    {
        [JsonProperty("header")]
        public Header Header { get; set; }

        [JsonProperty("details")]
        public IEnumerable<Detail> Details { get; set; }
    }
}
