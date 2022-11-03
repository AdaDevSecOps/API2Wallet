using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models
{
    public class cmlTFNTCrdSale
    {
        public int FNTxnID { get; set; }
        public string FTBchCode { get; set; }
        public string FTTxnDocType { get; set; }
        public string FTCrdCode { get; set; }
        public DateTime FDTxnDocDate { get; set; }
        public string FTBchCodeRef { get; set; }
        public string FTTxnDocNoRef { get; set; }
        public string FTTxnPosCode { get; set; }
        public decimal FCTxnValue { get; set; }
        public Int64 FNTxnIDRef { get; set; }
        public string FTTxnStaPrc { get; set; }
        public string FTTxnStaCancel { get; set; }
        public decimal FCTxnDeposit { get; set; }
       
    }
}