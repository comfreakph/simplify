using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simplify.WebApi.Models
{
    public class InvoiceModel
    {
        public string InvoiceNo { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Reference { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string Note { get; set; }
        public string SuppliedDate { get; set; }

    }
}