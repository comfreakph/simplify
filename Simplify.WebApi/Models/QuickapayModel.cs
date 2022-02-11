using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Simplify.WebApi.Models
{
    public class QuickapayModel
    {
        public string customer_invoice_ref { get; set; }
        public string contact_name { get; set; }
        public string contact_email { get; set; }
        public string contact_number { get; set; }
        public string amount { get; set; }
        public string due_date { get; set; }
        public string[] payment_options { get; set; }
        public string invoice_pdf { get; set; }
        public string invoice_url { get; set; }
    }

    public class RequestModel {
        public string customer_invoice_ref { get; set; }
        public string contact_name { get; set; }
        public string contact_email { get; set; }
        public string contact_number { get; set; }
        public string amount { get; set; }
        public string due_date { get; set; }
    }
}