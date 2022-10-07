using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Challenge3Functions.Contracts
{
    public class RatingBase
    {
        [JsonProperty("userId")]
        public Guid UserId { get; set; }

        [JsonProperty("productId")]
        public Guid ProductId { get; set; }

        [JsonProperty("locationName")]
        public string LocationName { get; set; }

        [JsonProperty("rating")]
        public int Rating { get; set; }

        [JsonProperty("userNotes")]
        public string UserNotes { get; set; }
    }
}
