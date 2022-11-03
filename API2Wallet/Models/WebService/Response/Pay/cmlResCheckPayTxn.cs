using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.Pay
{
    /// <summary>
    /// Payment transaction information.
    /// </summary>
    public class cmlResCheckPayTxn
    {
        /// <summary>
        /// Transaction ID.
        /// </summary>
        public Nullable<long> rnTxnID { get; set; }

        /// <summary>
        /// Document no.
        /// </summary>
        public string rtTxnDocNoRef { get; set; }

        /// <summary>
        /// Document date.
        /// </summary>
        public Nullable<DateTime> rdTxnDocDate { get; set; }

        /// <summary>
        /// Card code.
        /// </summary>
        public string rtCrdCode { get; set; }

        /// <summary>
        /// Card name.
        /// </summary>
        public string rtCrdName { get; set; }

        /// <summary>
        /// Card value.
        /// </summary>
        public Nullable<decimal> rcTxnCrdValue { get; set; }

        /// <summary>
        /// Payment value.
        /// </summary>
        public Nullable<decimal> rcTxnValue { get; set; }

        /// <summary>
        /// Document no. in case cancel payment.
        /// </summary>
        public Nullable<long> rnTxnIDRef { get; set; }

        /// <summary>
        /// Card holder ID.
        /// </summary>
        public string rtCrdHolderID { get; set; }
    }
}