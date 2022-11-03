using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.Database
{
    public class cmlDailyTopup
    {
        public long nSeqNo { get; set; }
        public DateTime FDCmmDateTime { get; set; }
        public string FTCmmType { get; set; }
        public double FCCmmValue { get; set; }
    }
}