using NLog;
using Simplify.WebApi.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Simplify.WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "GET, POST, PUT, DELETE")]
    public class ProxyController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // GET: api/Proxy
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Proxy/5
        public string Get(int id)
        {
            return "value";
        }

        //public IHttpActionResult Post([NakedBody] string raw)
        //{
        //    logger.Info("Trigger POST NakedBody: api/proxy" + Environment.NewLine + DateTime.Now);

        //    WebhookService webHookService = new WebhookService();
        //    //capture the raw content here...
        //    //string raw = webHookService.RawContentReader(this.Request).Result;
        //    var result = webHookService.GetInvoiceUpdate(raw).Result;

        //    logger.Info("POST NakedBody: api/proxy Result:" + result + " " + Environment.NewLine + DateTime.Now);

        //    return Ok(result);
        //}

        //POST: api/Proxy
        public IHttpActionResult Post()
        {
            logger.Info("Trigger POST: api/proxy" + Environment.NewLine + DateTime.Now);

            WebhookService webHookService = new WebhookService();
            //capture the raw content here...
            string raw = webHookService.RawContentReader(this.Request).Result;
            string jsonString = raw.Replace(@"%34",@"""");
            jsonString = jsonString.Replace(@"%40", "@");
            jsonString = jsonString.Replace("+", " ");
            //var result = webHookService.GetInvoiceUpdate(raw).Result;


            logger.Info("POST: api/proxy Result:" + jsonString + " " + Environment.NewLine + DateTime.Now);
            return Ok();
        }
        //[HttpPost]
        //public string Post([FromBody] string text)
        //{
        //    logger.Info("Trigger POST: api/proxy" + Environment.NewLine + DateTime.Now);
        //    logger.Info("Trigger POST: api/proxy data: " + text + " " + Environment.NewLine + DateTime.Now);
        //    return text;
        //}

        // PUT: api/Proxy/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Proxy/5
        public void Delete(int id)
        {
        }
    }
}
