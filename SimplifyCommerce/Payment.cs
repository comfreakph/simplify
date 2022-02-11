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
    public class Payment {

        [JsonProperty("agreementId")]
        public string AgreementId { get; set; }

        [JsonProperty("amount")]
        public long? Amount { get; set; }

        [JsonProperty("amountEstimated")]
        public bool? AmountEstimated { get; set; }

        [JsonProperty("amountRemaining")]
        public long? AmountRemaining { get; set; }

        [JsonProperty("authCode")]
        public string AuthCode { get; set; }

        [JsonProperty("authorization")]
        [JsonConverter(typeof(ObjectToIdConverter))]
        public Authorization Authorization { get; set; }

        [JsonProperty("avsAddressMatch")]
        public bool? AvsAddressMatch { get; set; }

        [JsonProperty("avsCvcMatch")]
        public bool? AvsCvcMatch { get; set; }

        [JsonProperty("avsZipMatch")]
        public bool? AvsZipMatch { get; set; }

        [JsonProperty("batchId")]
        public string BatchId { get; set; }

        [JsonProperty("card")]
        public Card Card { get; set; }

        [JsonProperty("createdBy")]
        public string CreatedBy { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("customer")]
        [JsonConverter(typeof(ObjectToIdConverter))]
        public Customer Customer { get; set; }

        [JsonProperty("customerSignature")]
        [JsonConverter(typeof(ObjectToIdConverter))]
        public BinaryFileData CustomerSignature { get; set; }

        [JsonProperty("dateCreated")]
        public long? DateCreated { get; set; }

        [JsonProperty("declineReason")]
        public string DeclineReason { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("disputed")]
        public bool? Disputed { get; set; }

        [JsonProperty("dynamicDescriptor")]
        public string DynamicDescriptor { get; set; }

        [JsonProperty("fee")]
        public long? Fee { get; set; }

        [JsonProperty("feeCurrency")]
        public string FeeCurrency { get; set; }

        [JsonProperty("feeEstimated")]
        public bool? FeeEstimated { get; set; }

        [JsonProperty("fraudResults")]
        public List<FraudResult> FraudResults { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("installment")]
        public APIInstallmentDetailsCommand Installment { get; set; }

        [JsonProperty("invoice")]
        public string Invoice { get; set; }

        [JsonProperty("order")]
        public APIOrderCreateCommand Order { get; set; }

        [JsonProperty("paymentDate")]
        public long? PaymentDate { get; set; }

        [JsonProperty("paymentStatus")]
        public string PaymentStatus { get; set; }

        [JsonProperty("receipt")]
        public APIReceiptCommand Receipt { get; set; }

        [JsonProperty("recurringType")]
        public string RecurringType { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }

        [JsonProperty("refunded")]
        public bool? Refunded { get; set; }

        [JsonProperty("refundedFees")]
        public long? RefundedFees { get; set; }

        [JsonProperty("refunds")]
        public List<Refund> Refunds { get; set; }

        [JsonProperty("replayId")]
        public string ReplayId { get; set; }

        [JsonProperty("review")]
        public TransactionReview Review { get; set; }

        [JsonProperty("settlementAmount")]
        public long? SettlementAmount { get; set; }

        [JsonProperty("settlementCurrency")]
        public string SettlementCurrency { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("statementDescription")]
        public APIDescriptorCommand StatementDescription { get; set; }

        [JsonProperty("tax")]
        [JsonConverter(typeof(ObjectToIdConverter))]
        public Tax Tax { get; set; }

        [JsonProperty("taxExempt")]
        public bool? TaxExempt { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("transactionData")]
        public Dictionary<string, object> TransactionData { get; set; }

        [JsonProperty("transactionDetails")]
        public TransactionDetails TransactionDetails { get; set; }


        public Payment ()
        {
        }

        public Payment (string Id)
        {
            this.Id = Id;
        }
    }
}
