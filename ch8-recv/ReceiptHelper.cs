using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
//using Microsoft.Azure.EventGrid.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.EventGrid.Models;

namespace Company.Function
{
    internal class ReceiptHelper
    {
        public async Task<T> ProcessReceipt<T>(EventGridEvent e) where T : Contracts.SmallReceipt, new()
        {
            if (null == e) return default(T);

            if (null == e.Data) return default(T);

            Company.Function.Contracts.SmallReceipt receipt = Newtonsoft.Json.JsonConvert.DeserializeObject<Company.Function.Contracts.SmallReceipt>(e.Data.ToString());

            T data = new Company.Function.Contracts.SmallReceipt()
            {
                Items = receipt.TotalItems,
                SalesDate = receipt.SalesDate,
                SalesNumber = receipt.SalesNumber,
                Store = receipt.StoreLocation,
                TotalCost = receipt.TotalCost
            };

            if (typeof(T) == typeof(Contracts.LargeReceipt))
            {
                await ProcessReceiptImage(data as Contracts.LargeReceipt, receipt.Url);
            }

            return data;
        }

        private async Task ProcessReceiptImage(Contracts.LargeReceipt receipt, string imageUrl)
        {
            if (null == receipt) return;
            if (string.IsNullOrWhiteSpace(imageUrl)) return;

            HttpClient client = new HttpClient();
            var result = await client.GetAsync(imageUrl);

            if (result.IsSuccessStatusCode)
            {
                using (var stream = await result.Content.ReadAsStreamAsync())
                {
                    byte[] buffer = new byte[stream.Length];
                    await stream.ReadAsync(buffer, 0, buffer.Length);

                    receipt.ReceiptImage = Convert.ToBase64String(buffer);
                }

                //receipt.ReceiptImage = await result.Content.ReadAsStringAsync();
            }
        }
    }
}
