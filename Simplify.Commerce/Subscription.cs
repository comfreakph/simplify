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
    public class Subscription {

        [JsonProperty("amount")]
        public long? Amount { get; set; }

        [JsonProperty("billingCycle")]
        public string BillingCycle { get; set; }

        [JsonProperty("billingCycleLimit")]
        public int? BillingCycleLimit { get; set; }

        [JsonProperty("coupon")]
        [JsonConverter(typeof(ObjectToIdConverter))]
        public Coupon Coupon { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("currentPeriodEnd")]
        public long? CurrentPeriodEnd { get; set; }

        [JsonProperty("currentPeriodStart")]
        public long? CurrentPeriodStart { get; set; }

        [JsonProperty("custom")]
        public bool? Custom { get; set; }

        [JsonProperty("customer")]
        [JsonConverter(typeof(ObjectToIdConverter))]
        public Customer Customer { get; set; }

        [JsonProperty("dateCreated")]
        public long? DateCreated { get; set; }

        [JsonProperty("frequency")]
        public string Frequency { get; set; }

        [JsonProperty("frequencyPeriod")]
        public int? FrequencyPeriod { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("latestInvoice")]
        public Invoice LatestInvoice { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pendingPayment")]
        public bool? PendingPayment { get; set; }

        [JsonProperty("plan")]
        [JsonConverter(typeof(ObjectToIdConverter))]
        public Plan Plan { get; set; }

        [JsonProperty("prorate")]
        public bool? Prorate { get; set; }

        [JsonProperty("quantity")]
        public int? Quantity { get; set; }

        [JsonProperty("renewalReminderLeadDays")]
        public int? RenewalReminderLeadDays { get; set; }

        [JsonProperty("start")]
        public long? Start { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }


        public Subscription ()
        {
        }

        public Subscription (string Id)
        {
            this.Id = Id;
        }
    }
}
