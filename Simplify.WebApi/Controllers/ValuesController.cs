using NLog;
using Simplify.WebApi.Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Simplify.WebApi.Controllers
{
    public class ValuesController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        //POST api/values
        //public void Post([FromBody] string value)
        //{
        //}

        /// <summary>
        ///POST api/values/postrawbuffer 
        ///this is only to test if thie api is called or triggered
        /// </summary>
        /// <param name="raw">capture the raw json data from the portal</param>
        /// <returns>return the captured raw data</returns>
        [HttpPost]
        public string PostRawBuffer([NakedBody] string raw)
        {
            logger.Info("Trigger POST: api/values/PostRawBuffer" + Environment.NewLine + DateTime.Now);

            logger.Info("POST: api/values/PostRawBuffer data: " + raw + " " + Environment.NewLine + DateTime.Now);

            return raw;
        }

        //[HttpPost]
        //public IHttpActionResult Post()
        //{
        //    logger.Info("Trigger POST: api/values" + Environment.NewLine + DateTime.Now);
        //    WebhookService webHookService = new WebhookService();
        //    string raw = webHookService.RawContentReader(this.Request).Result;

        //    logger.Info("Trigger POST: api/values data: " + raw + " " + Environment.NewLine + DateTime.Now);
        //    return Ok(raw);
        //}
        //public IHttpActionResult Post()
        //{
        //    logger.Info("Trigger POST: api/proxy" + Environment.NewLine + DateTime.Now);

        //    WebhookService webHookService = new WebhookService();
        //    string raw = webHookService.RawContentReader(this.Request).Result;
        //    //bool result = webHookService.GetInvoiceUpdate(raw).Result;

        //    logger.Info("POST: api/proxy Result:" + raw + " " + Environment.NewLine + DateTime.Now);
        //    return Ok(raw);
        //}

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
