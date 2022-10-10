using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Challenge7EventHub
{
    public class SalesOrderEventHubFunction
    {
        [FunctionName("SalesOrderEventHubFunction")]
        public async Task Run([EventHubTrigger("ohpossales", Connection = "EventHubConnectionString")] EventData[] events,
            [CosmosDB(
                    databaseName:"Sales",
                    collectionName:"Orders",
                    ConnectionStringSetting ="IceCreamRatingsCosmosDbConnection")] IAsyncCollector<Document> orders,
            ILogger log)
        {
            var exceptions = new List<Exception>();

            log.LogInformation($"Invoked with {events.Length} events...");

            foreach (EventData eventData in events)
            {
                try
                {
                    

                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                    var order = Newtonsoft.Json.JsonConvert.DeserializeObject<Document>(messageBody);
                    if (null != order)
                    {
                        await orders.AddAsync(order);
                    }

                    // Replace these two lines with your processing logic.
                    //log.LogInformation($"C# Event Hub trigger function processed a message: {messageBody}");
                    await Task.Yield();
                }
                catch (Exception e)
                {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }
    }
}
