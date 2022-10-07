using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge3Functions.Contracts
{
    public class Rating : RatingBase
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("timestamp")]
        public DateTime TimeStamp { get; set; }
    }
}
