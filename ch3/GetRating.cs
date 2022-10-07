using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Challenge3Functions
{
    /*
     
     Verb: GET
Query string or route parameter: ratingId
Requirements
Get the rating from your database and return the entire JSON payload for the review identified by the id
Additional route parameters or query string values may be used if necessary.
Output payload example:
{
  "id": "79c2779e-dd2e-43e8-803d-ecbebed8972c",
  "userId": "cc20a6fb-a91f-4192-874d-132493685376",
  "productId": "4c25613a-a3c2-4ef3-8e02-9c335eb23204",
  "timestamp": "2018-05-21 21:27:47Z",
  "locationName": "Sample ice cream shop",
  "rating": 5,
  "userNotes": "I love the subtle notes of orange in this ice cream!"
}
     
     */
    public static class GetRating
    {
        [FunctionName("GetRating")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "ratings/{ratingId}")] HttpRequest req,
             [CosmosDB(
                databaseName:"IceCream",
                collectionName:"Ratings",
                ConnectionStringSetting = "IceCreamRatingsCosmosDbConnection",
                SqlQuery = "select * from Ratings r where r.id = {ratingId}")
            ] IEnumerable<Contracts.Rating> ratings,
            ILogger log)
        {
            return new OkObjectResult(ratings.FirstOrDefault());
        }
    }
}
