using SimplifyCommerce.Payments;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO;
using System.Text;
using Microsoft.SqlServer.Server;
using Simplify.WebApi.Infrastructure.Helpers;
using NLog;
using Simplify.WebApi.Models;

namespace Simplify.WebApi.Infrastructure.Service
{
    public class WebhookService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public async Task<ResponseModel> GetInvoiceUpdate(string raw) {
            bool result = false;
            string message = "";
            ResponseModel model = new ResponseModel();
            try
            {

                //get the api keys from web config
                var publickey = WebConfigurationManager.AppSettings["SimplifyPublicApiKeys"];
                var privatekey = WebConfigurationManager.AppSettings["SimplifyPrivateApiKeys"];

                //assign the api keys
                PaymentsApi.PublicApiKey = publickey;
                PaymentsApi.PrivateApiKey = privatekey;
                 
                //get the raw JSON Web Signatures and decode the values
                PaymentsApi api = new PaymentsApi();
                JObject obj = api.JwsDecode(raw); // Raw Json
                string eventName = (string)obj["event"]["name"];
                string invoiceStatus = (string)obj["event"]["data"]["status"];

                //if (eventName != "invoice.update" || invoiceStatus != "PAID") return false;

                //get the jsondate and set to normal date format
                string jpaidDate = (string)obj["event"]["data"]["datePaid"];
                if (!string.IsNullOrEmpty(jpaidDate)) {
                    DateTime paidDate = new DateTime();
                    paidDate = DateTimeHelper.ParseDateTimeFromStringLiteral(jpaidDate);
                    JObject item = (JObject)obj["event"];

                    item.Add("paymentDate", paidDate.ToString("yyyy-MM-dd"));
                }

                //string jsonString = obj.ToString(Formatting.None);
                string jsonString = JsonConvert.SerializeObject(obj);

                #region podio process
                //send data to podio webhook api url
                //string webHookUrl = WebConfigurationManager.AppSettings["Webhook"];
                //Uri uri = new Uri(webHookUrl);
                //string baseUrl = uri.GetLeftPart(UriPartial.Authority);
                //string pathUrl = uri.AbsolutePath;

                //using (var client = new HttpClient())
                //{
                //    client.BaseAddress = new Uri(baseUrl);
                //    var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                //    await client.PostAsync(pathUrl, content);
                //}
                #endregion

                result = true;
                message = "Successfully sent to podio process";
                model.Data = jsonString.Replace(@"\", string.Empty);

                
            }
            catch (ApiException e)
            {

                result = false;

                logger.Info("Trigger WebhookService.GetInvoiceUpdate: apiErrMsg:" + e.Message + " " + Environment.NewLine + DateTime.Now);
                logger.Info("Trigger WebhookService.GetInvoiceUpdate: apiErrData:" + e.ErrorData + " " + Environment.NewLine + DateTime.Now);

                message = $"ApiErrMsg:{e.Message} ApiErrData:{e.ErrorData}";
            }
            catch (Exception e) {
                result = false;

                logger.Info("Trigger WebhookService.GetInvoiceUpdate: errMsg:" + e.Message + " " + Environment.NewLine + DateTime.Now);
                message = $"ErrMsg:{e.Message}";
            }

            model.Status = result;
            model.Message = message;

            return model;
        }

        public async Task<string> RawContentReader(HttpRequestMessage req)
        {
            using (var contentStream = await req.Content.ReadAsStreamAsync())
            {
                contentStream.Seek(0, SeekOrigin.Begin);
                using (var sr = new StreamReader(contentStream))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}