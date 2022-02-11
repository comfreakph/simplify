using NLog;
using Simplify.WebApi.Infrastructure.Service;
using Simplify.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace Simplify.WebApi.Controllers
{
    //[EnableCors(origins: "https://secure.globiflow.com/", headers: "*", methods: "get,post")]
    public class InvoiceController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public IHttpActionResult Post([FromBody] InvoiceModel requestItem)
        {
            logger.Info("Trigger POST: api/invoice" + Environment.NewLine + DateTime.Now);

            if (requestItem == null) return BadRequest();

            InvoiceService service = new InvoiceService();
            string result = service.Create(requestItem);

            //string result = "success";

            logger.Info("POST: api/invoice Result:" + result + " " + Environment.NewLine + DateTime.Now);

            return Ok(result);
        }


        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}