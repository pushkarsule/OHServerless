using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.Function.Contracts
{
    public class LargeReceipt : SmallReceipt
    {
        /*
         
        {
    "Store": "00d8ea6f-935c-2cca-9bbc-f56b5a091621",
    "SalesNumber": "0c423398-3c7c-0682-7519-4701c445ed7a",
    "TotalCost": 123.40,
    "Items": 8,
    "SalesDate": "09/11/2019 06:04:43",
    "ReceiptImage":"V2VsY29tZSB0byBTZXJ2ZXJsZXNzIE9wZW5IYWNrIQ=="
}
         
         */


        public string ReceiptImage { get; set; }
    }
}
