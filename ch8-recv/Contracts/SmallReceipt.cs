using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Function.Contracts
{
    public class SmallReceipt
    {
        /*
         {
    "Store": "00d8ea6f-935c-2cca-9bbc-f56b5a091621",
    "SalesNumber": "0c423398-3c7c-0682-7519-4701c445ed7a",
    "TotalCost": 6.58,
    "Items": 1,
    "SalesDate": "09/02/2019 10:36:17"
}        
         */

        public string Store { get; set; }
        public string SalesNumber { get; set; }
        public decimal TotalCost { get; set; }
        public int Items { get; set; }
        public DateTime SalesDate { get; set; }

    }
}
