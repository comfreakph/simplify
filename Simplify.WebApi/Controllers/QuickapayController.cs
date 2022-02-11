using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Simplify.WebApi.Infrastructure.Helpers;
using Simplify.WebApi.Infrastructure.Service;
using Simplify.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Simplify.WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE")]
    public class QuickapayController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: api/Quickapay
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Quickapay/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Quickapay
        [HttpPost]
        public IHttpActionResult Post()
        {
            logger.Info("Trigger POST: api/Quickapay" + Environment.NewLine + DateTime.Now);
            string result = string.Empty;
            try
            {
                WebhookService webHookService = new WebhookService();
                //capture the raw content here...
                string raw = webHookService.RawContentReader(this.Request).Result;
                

                if (string.IsNullOrEmpty(raw)) return BadRequest();


                string jsonString = raw.Replace(@"%34", @"""");
                jsonString = jsonString.Replace(@"%40", "@");
                jsonString = jsonString.Replace("+", " ");

                logger.Info($"Trigger POST: api/Quickapay raw:{jsonString} {Environment.NewLine + DateTime.Now}");

                string[] data = jsonString.Split(',');



                QuickapayService service = new QuickapayService();

                DateTime dueDate = Convert.ToDateTime(data[5].ToString());
                //dueDate = dueDate.AddDays(14);
                string invoiceHeader = data[6].Trim();
                string isTest = data[7].Trim();

                QuickapayModel model = new QuickapayModel()
                {
                    contact_email = data[2].Trim(),
                    contact_name = data[0].Trim(),
                    contact_number = data[1].Trim(),
                    amount = data[4].Trim(),
                    customer_invoice_ref = data[3].Trim(),
                    due_date = DateTimeHelper.ToRfc3339String(dueDate),
                    payment_options = new string[] { "Instalments" },
                    invoice_pdf = string.Empty,
                    invoice_url = "http://payments.qualifyme.edu.au/Home/Invoice"
                };

                var body = JsonConvert.SerializeObject(model);
                logger.Info("POST: api/Quickapay Result:" + body.ToString() + " " + Environment.NewLine + DateTime.Now);

                

                if (!string.IsNullOrEmpty(isTest) && isTest == "false")
                {
                    result = service.Create(model);
                }
                else
                {
                    result = body.ToString();
                }


                JObject jsonResult = JObject.Parse(result);

                logger.Info("POST: api/Quickapay Result:" + result + " " + Environment.NewLine + DateTime.Now);

                return Ok(jsonResult);
            }
            catch (Exception ex)
            {
                logger.Info("POST: api/Quickapay Error Result:" + ex.Message + " " + Environment.NewLine + DateTime.Now);

                result = "error";

                JObject jsonResult = JObject.Parse(result);

                return Ok(jsonResult);
            }
            
        }

        // PUT: api/Quickapay/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Quickapay/5
        public void Delete(int id)
        {
        }
    }
}
