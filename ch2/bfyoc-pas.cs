using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Company.Function
{
    public static class bfyoc_pas
    {
        [FunctionName("bfyoc_pas")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            string productId = req.Query["productId"];
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody); 
            productId = productId ?? data?.productId;
            string responseMessage =string.IsNullOrEmpty(productId)?"This HTTP triggered function executed successfully. Pass a productId in the query string or in the request body for a personalized response.": $"Hello, {productId}. This HTTP triggered function executed successfully.";
            return new OkObjectResult(responseMessage);
        }
    }
}
