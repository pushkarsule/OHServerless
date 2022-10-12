// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Azure.EventGrid.Models;

namespace Company.Function
{
    public class SmallReceiptsFunction
    {
        [FunctionName("SmallReceiptsFunction")]
         public async Task Run([EventGridTrigger] EventGridEvent eventGridEvent,
            [Blob("receipts/{rand-guid}.json", FileAccess.Write, Connection = "StorageAccountConnection")] Stream receipt,
            ILogger log)
        {
            var data = await new ReceiptHelper().ProcessReceipt<Contracts.SmallReceipt>(eventGridEvent);


            if (null != data)
            {
                //dump to storage

                string json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                byte[] ba = System.Text.Encoding.UTF8.GetBytes(json);
                await receipt.WriteAsync(ba, 0, ba.Length);
            }
        }
    }
}
