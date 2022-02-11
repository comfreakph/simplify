/*
 * Copyright (c) 2013 - 2020 MasterCard International Incorporated
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are 
 * permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of 
 * conditions and the following disclaimer.
 * Redistributions in binary form must reproduce the above copyright notice, this list of 
 * conditions and the following disclaimer in the documentation and/or other materials 
 * provided with the distribution.
 * Neither the name of the MasterCard International Incorporated nor the names of its 
 * contributors may be used to endorse or promote products derived from this software 
 * without specific prior written permission.
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
 * SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
 * TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; 
 * OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER 
 * IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING 
 * IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF 
 * SUCH DAMAGE.
 */


using System.Collections.Generic;
using Newtonsoft.Json;

namespace SimplifyCommerce.Payments {
    public class Invoice {

        [JsonProperty("billingAddress")]
        public Address BillingAddress { get; set; }

        [JsonProperty("businessAddress")]
        public Address BusinessAddress { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("customer")]
        [JsonConverter(typeof(ObjectToIdConverter))]
        public Customer Customer { get; set; }

        [JsonProperty("customerName")]
        public string CustomerName { get; set; }

        [JsonProperty("customerTaxNo")]
        public string CustomerTaxNo { get; set; }

        [JsonProperty("dateCreated")]
        public long? DateCreated { get; set; }

        [JsonProperty("datePaid")]
        public long? DatePaid { get; set; }

        [JsonProperty("dateSent")]
        public long? DateSent { get; set; }

        [JsonProperty("discountRate")]
        public string DiscountRate { get; set; }

        [JsonProperty("dueDate")]
        public long? DueDate { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("invoiceDate")]
        public long? InvoiceDate { get; set; }

        [JsonProperty("invoiceId")]
        public string InvoiceId { get; set; }

        [JsonProperty("invoiceToCopy")]
        public string InvoiceToCopy { get; set; }

        [JsonProperty("isLate")]
        public bool? IsLate { get; set; }

        [JsonProperty("items")]
        public List<InvoiceItem> Items { get; set; }

        [JsonProperty("lastViewed")]
        public long? LastViewed { get; set; }

        [JsonProperty("lateFee")]
        public long? LateFee { get; set; }

        [JsonProperty("memo")]
        public string Memo { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("note")]
        public string Note { get; set; }

        [JsonProperty("payment")]
        [JsonConverter(typeof(ObjectToIdConverter))]
        public Payment Payment { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("shippingAddress")]
        public Address ShippingAddress { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("subTotal")]
        public long? SubTotal { get; set; }

        [JsonProperty("suppliedDate")]
        public long? SuppliedDate { get; set; }

        [JsonProperty("taxNo")]
        public string TaxNo { get; set; }

        [JsonProperty("total")]
        public long? Total { get; set; }

        [JsonProperty("totalDiscount")]
        public long? TotalDiscount { get; set; }

        [JsonProperty("totalFees")]
        public long? TotalFees { get; set; }

        [JsonProperty("totalTax")]
        public long? TotalTax { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("uuid")]
        public string Uuid { get; set; }

        [JsonProperty("sendMail")]
        public string SendMail { get; set; }

        public Invoice ()
        {
        }

        public Invoice (string Id)
        {
            this.Id = Id;
        }
    }
}
