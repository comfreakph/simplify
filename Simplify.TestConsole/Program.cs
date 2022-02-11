using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SimplifyCommerce.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Simplify.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            invoice();
        }

        static void invoice() {
            PaymentsApi.PublicApiKey = "";
            PaymentsApi.PrivateApiKey = "";

            PaymentsApi api = new PaymentsApi();

            Invoice invoice = new Invoice();
            invoice.Currency = "AUD";
            invoice.Email = "comfreakph@gmail.com";
            List<InvoiceItem> items = new List<InvoiceItem>();
            InvoiceItem items1 = new InvoiceItem();
            items1.Amount = 5504;
            items1.Quantity = 1;
            items1.Description = "Course fee";
            //items1.Tax = new Tax("[TAX ID]");
            items.Add(items1);
            invoice.Items = items;
            invoice.Memo = "This is a memo";
            invoice.Name = "John Doe";
            invoice.Note = "This is a note";
            invoice.Reference = "Ref2";

            JsonSerializerSettings microsoftDateFormatSettings = new JsonSerializerSettings
            {
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            };

            DateTime now = new DateTime(2045, 11, 19,0,0,0,DateTimeKind.Utc);

            string microsoftJsonDate = JsonConvert.SerializeObject(now, microsoftDateFormatSettings);
            microsoftJsonDate = microsoftJsonDate.Replace("\"\\/Date(", "").Replace(")\\/\"", "");

            //invoice.SuppliedDate = 2394839384000;
            invoice.SuppliedDate = Convert.ToInt64(microsoftJsonDate);
            try
            {
                invoice = (Invoice)api.Create(invoice);
            }
            catch (ApiException e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Message: " + e.Message);
                Console.WriteLine("Error details: " + e.ErrorData);
            }
        }
    }
}
