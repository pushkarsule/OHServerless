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
using System.Net.Http;

namespace Challenge3Functions
{
    public static class CreateRating
    {
        /*
         
        Validate both userId and productId by calling the existing API endpoints. You can find a user id to test with from the sample payload above
Add a property called id with a GUID value
Add a property called timestamp with the current UTC date time
Validate that the rating field is an integer from 0 to 5
Use a data service to store the ratings information to the backend
Return the entire review JSON payload with the newly created id and timestamp
         
         */


        [FunctionName("CreateRating")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            [CosmosDB(
            databaseName:"IceCream",
            collectionName:"Ratings",
            ConnectionStringSetting ="IceCreamRatingsCosmosDbConnection")] out Contracts.Rating rating,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            rating = new Contracts.Rating();


            //string name = req.Query["name"];

            string requestBody = new StreamReader(req.Body).ReadToEnd();



            Contracts.RatingBase rb = JsonConvert.DeserializeObject<Contracts.RatingBase>(requestBody);


            if (null == rb)
            {
                //validate, return if not there
            }

            if (false == IsProductValid(rb.ProductId))
            {
                return new NotFoundObjectResult("Product id was not found");
            }

            if (false == IsUserValid(rb.UserId))
            {
                return new NotFoundObjectResult("User id was not found");
            }

            rating.UserId = rb.UserId;
            rating.ProductId = rb.ProductId;
            rating.UserNotes = rb.UserNotes;
            rating.LocationName = rb.LocationName;
            rating.Rating = rb.Rating;

            rating.TimeStamp = DateTime.UtcNow;
            rating.Id = Guid.NewGuid();

            return new OkObjectResult(rating);
        }

        private static bool IsUserValid(Guid userId)
        {
            //https://serverlessohapi.azurewebsites.net/api/GetUser?userId=cc20a6fb-a91f-4192-874d-132493685376
            if (Guid.Empty == userId) return false;

            HttpClient client = new HttpClient();
            var response = client.GetAsync($"https://serverlessohapi.azurewebsites.net/api/GetUser?userId={userId}")
                .ConfigureAwait(false).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode) return true;


            return false;

            //Content.ReadAsStringAsync().ConfigureAwait(false).GetAwaiter().GetResult();


        }

        private static bool IsProductValid(Guid productId)
        {
            //https://serverlessohapi.azurewebsites.net/api/GetProduct?productId=75542e38-563f-436f-adeb-f426f1dabb5c

            if (Guid.Empty == productId) return false;

            HttpClient client = new HttpClient();
            var response = client.GetAsync($"https://serverlessohapi.azurewebsites.net/api/GetProduct?productId={productId}")
                .ConfigureAwait(false).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode) return true;

            return false;
        }
    }
}
