using Newtonsoft.Json;
using NLog;
using RestSharp;
using Simplify.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simplify.WebApi.Infrastructure.Service
{
    public class QuickapayService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static string _accessKey = "";
        public static string _businessId = "";

        public string Create(QuickapayModel model) {
            var client = new RestClient("https://my.quicka.co/v1/invoices");
            
            client.Timeout = -1;
            
            var request = new RestRequest(Method.POST);
            request.AddHeader("X-Access-Key", _accessKey);
            request.AddHeader("X-Business-Id", _businessId);
            request.AddHeader("Content-Type", "application/json");

            var body = JsonConvert.SerializeObject(model);

            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            return response.Content;
        }
    }
}