using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.Database
{
    public class cmlCardHis
    {
        public long nSeqNo { get; set; }
        public string tPosCode { get; set; }
        public string tPosName { get; set; }
        public string tUsrCode { get; set; }
        public string tCmmType { get; set; }
        public DateTime dCmmDateTime { get; set; }
        public double cValue { get; set; }
        public string tCmpName { get; set; }  // *[ANUBIS][][2016-09-02] - ชื่อร้าน
        public string tUsrName { get; set; }  // *[ANUBIS][][2016-09-02] - ชื่อแคชเชียร์
        public string tCstName { get; set; }  // *[ANUBIS][][2016-09-02] - ซื่อพนักงาน
        public string tCmmDocNo { get; set; }
    }
}