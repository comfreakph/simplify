using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simplify.WebApi.Models
{
    public class ResponseModel
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public string Data { get; set; }
    }
}