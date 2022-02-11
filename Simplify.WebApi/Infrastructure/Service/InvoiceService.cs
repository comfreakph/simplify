using NLog;
using Simplify.WebApi.Infrastructure.Helpers;
using Simplify.WebApi.Models;
using SimplifyCommerce.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace Simplify.WebApi.Infrastructure.Service
{
    public class InvoiceService
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public string Create(InvoiceModel model) {
            string sucess = "";
            var publickey = WebConfigurationManager.AppSettings["SimplifyPublicApiKeys"];
            var privatekey = WebConfigurationManager.AppSettings["SimplifyPrivateApiKeys"];

            PaymentsApi.PublicApiKey = publickey;
            PaymentsApi.PrivateApiKey = privatekey;

            DateTime suppliedDate = Convert.ToDateTime(model.SuppliedDate);
            model.SuppliedDate = suppliedDate.ToString("MM/dd/yyyy");

            string strAmount = model.Amount.ToString();
            string newAmount = string.Empty;
            int intValue = 0;
            if (int.TryParse(strAmount, out intValue))
            {
                // x is an int
                newAmount = strAmount + "00";
            }
            else
            {
                // x is not an int
                newAmount = strAmount.Replace(".", "");
            }
            
            long amount = Convert.ToInt64(newAmount);

            string[] emails = model.Email.Split(',');
            string email = emails[0];

            PaymentsApi api = new PaymentsApi();

            Invoice invoice = new Invoice();
            invoice.InvoiceId = model.InvoiceNo;
            invoice.Currency = "AUD";
            invoice.Email = email;
            List<InvoiceItem> items = new List<InvoiceItem>();
            InvoiceItem items1 = new InvoiceItem();
            items1.Amount = amount;
            items1.Quantity = 1;
            items1.Description = model.Description;
            //items1.Tax = new Tax("[TAX ID]");
            items.Add(items1);
            invoice.Items = items;
            invoice.Memo = Memo();
            invoice.Name = model.Name;
            invoice.Note = model.Note;
            invoice.Reference = model.Reference;
            invoice.SuppliedDate = DateTimeHelper.ConvertDateTimeToJsonDateLong(model.SuppliedDate);

            try
            {
                //create invoice
                invoice = (Invoice)api.Create(invoice);

                //update invoice 
                invoice = (Invoice)api.Find(typeof(Invoice), invoice.Id);
                invoice.Status = "OPEN";
                invoice.SendMail = "true";
                invoice = (Invoice)api.Update(invoice);


                sucess = "true, success";
            }
            catch (ApiException e)
            {
                //Console.WriteLine(e.ToString());
                //Console.WriteLine("Message: " + e.Message);
                //Console.WriteLine("Error details: " + e.ErrorData);
                //throw new Exception(e.Message);
                sucess = "error, " + e.Message;
                logger.Info("Trigger InvoiceService.Create: errMsg:" + e.Message + " " + Environment.NewLine + DateTime.Now);
            }

            return sucess;
        }

        private string Memo() {
            string memo = "Qualify Me!® Pty Ltd is an authorised partner of Train n Trade a Registered Training Organisation (RTO No. 41135) to recruit and provide student support for all courses on the scope of registration of the RTO excluding government funded courses. All rights reserved. ABN: 56160476844";
            return memo;
        }
    }
}