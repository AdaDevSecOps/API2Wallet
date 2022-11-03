using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.Database
{
    public class cmlFingerScan
    {
        public int topup_ID { get; set; }
        public string UserID { get; set; }
        public string EmployeeID { get; set; }
        public DateTime TransactionTime { get; set; }
        public int TerminalID { get; set; }
    }
}