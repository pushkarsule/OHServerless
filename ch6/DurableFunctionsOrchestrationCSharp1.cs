using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Company.Function
{
    public static class DurableFunctionsOrchestrationCSharp1
    {
        [FunctionName("DurableFunctionsOrchestrationCSharp1")]
        public static async Task<List<string>> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var outputs = new List<string>();
            string blobname = context.GetInput<string>();
            var file1 = blobname + "-OrderHeaderDetails.csv";
            var file2 = blobname + "-OrderLineItems.csv";
            var file3 = blobname + "-ProductInformation.csv";

            // Replace "hello" with the name of your Durable Activity Function.
            outputs.Add(await context.CallActivityAsync<string>("DurableFunctionsOrchestrationCSharp1_Hello", file1));
            outputs.Add(await context.CallActivityAsync<string>("DurableFunctionsOrchestrationCSharp1_Hello", file2));
            outputs.Add(await context.CallActivityAsync<string>("DurableFunctionsOrchestrationCSharp1_Hello", file3));

            // returns ["Hello Tokyo!", "Hello Seattle!", "Hello London!"]
            return outputs;
        }

        [FunctionName("DurableFunctionsOrchestrationCSharp1_Hello")]
        public static string SayHello([ActivityTrigger] string name, ILogger log)
        {
            log.LogInformation($"Saying hello to {name}.");
            return $"Hello {name}!";
        }

        //filter on file type
        //"path": "samples/{name}.png",
        [FunctionName("BlobTrigger")]
        public static async Task Run(
            [BlobTrigger("fntest/{name}-OrderHeaderDetails.csv", Connection ="mystoracct")] Stream myItem,
            string name,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            StreamReader reader = new StreamReader(myItem);
            string jsonContent = reader.ReadToEnd();
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("DurableFunctionsOrchestrationCSharp1", null, name);

            //log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            //return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}