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
    public class Card {

        [JsonProperty("addressCity")]
        public string AddressCity { get; set; }

        [JsonProperty("addressCountry")]
        public string AddressCountry { get; set; }

        [JsonProperty("addressLine1")]
        public string AddressLine1 { get; set; }

        [JsonProperty("addressLine2")]
        public string AddressLine2 { get; set; }

        [JsonProperty("addressState")]
        public string AddressState { get; set; }

        [JsonProperty("addressZip")]
        public string AddressZip { get; set; }

        [JsonProperty("androidPayData")]
        public APIAndroidPayCommand AndroidPayData { get; set; }

        [JsonProperty("applePayData")]
        public APIApplePayCommand ApplePayData { get; set; }

        [JsonProperty("cardEntryMode")]
        public string CardEntryMode { get; set; }

        [JsonProperty("cardPresentData")]
        public CardPresent CardPresentData { get; set; }

        [JsonProperty("customer")]
        public Customer Customer { get; set; }

        [JsonProperty("cvc")]
        public string Cvc { get; set; }

        [JsonProperty("dateCreated")]
        public long? DateCreated { get; set; }

        [JsonProperty("expMonth")]
        public int? ExpMonth { get; set; }

        [JsonProperty("expYear")]
        public int? ExpYear { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("indicator")]
        public string Indicator { get; set; }

        [JsonProperty("indicatorSource")]
        public string IndicatorSource { get; set; }

        [JsonProperty("last4")]
        public string Last4 { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("secure3DData")]
        public Secure3DData Secure3DData { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }


        public Card ()
        {
        }

        public Card (string Id)
        {
            this.Id = Id;
        }
    }
}
