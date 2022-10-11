using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;

namespace Company.Function
{
    public class SalesOrderEventHubExtFunction
    {
        [FunctionName("SalesOrderEventHubExtFunction")]
        public async Task Run([EventHubTrigger("ehposdata2", Connection = "ohpossales_RootManageSharedAccessKey_EVENTHUB")] EventData[] events, 
                [CosmosDB(
                     databaseName:"Sales",
                     collectionName:"Orders",
                     ConnectionStringSetting ="IceCreamRatingsCosmosDbConnection")] IAsyncCollector<Document> orders,
                [EventGrid(TopicEndpointUri = "EventGridEndPoint", TopicKeySetting = "EventGridKey")] IAsyncCollector<EventGridEvent> receipts,
                ILogger log)
        {
            var exceptions = new List<Exception>();

            log.LogInformation($"Invoked with {events.Length} events...");

            foreach (EventData eventData in events)
            {
                try
                {


                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                    var order = Newtonsoft.Json.JsonConvert.DeserializeObject<Challenge7EventHub.Contracts.SalesOrder>(messageBody);
                    var doc = Newtonsoft.Json.JsonConvert.DeserializeObject<Document>(messageBody);
                    if (null != order)
                    {
                        if (null != order.Header && false == string.IsNullOrWhiteSpace(order.Header.ReceiptUrl))
                        {
                            Challenge7EventHub.Contracts.Receipt receipt = new Challenge7EventHub.Contracts.Receipt()
                            {
                                SalesDate = order.Header.DateTime,
                                SalesNumber = order.Header.SalesNumber,
                                StoreLocation = order.Header.LocationId,
                                Url = order.Header.ReceiptUrl,
                                TotalCost = order.Header.TotalCost,
                                TotalItems = GetItemCount(order)
                            };
                            await receipts.AddAsync(new EventGridEvent("receipts", "Receipt", "Receipt", new BinaryData(Newtonsoft.Json.JsonConvert.SerializeObject(receipt))));
                            //Newtonsoft.Json.JsonConvert.SerializeObject(receipt)
                        }
                        //string receiptUrl = order.GetPropertyValue<string>("header.receiptUrl");
                        //if (false == string.IsNullOrWhiteSpace(receiptUrl)) { 
                        //    Contracts.Receipt 
                        //}

                        await orders.AddAsync(doc);
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

        private int GetItemCount(Challenge7EventHub.Contracts.SalesOrder order)
        {
            if (null == order) return 0;
            if (null == order.Details) return 0;

            int items = 0;
            foreach (var d in order.Details)
            {
                items += d.Quantity;
            }

            return items;
        }
    }
}
