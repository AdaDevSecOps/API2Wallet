using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.Database
{
    public class cmlCardInfo
    {
        public string tCrdCode { get; set; }
        public string tCrdName { get; set; }
        public string tCrdTypeName { get; set; }
        public string tCrdStaType { get; set; }
        public DateTime dExpireDate { get; set; }
        public double cValue { get; set; }
        public double cAvailable { get; set; }
        public string tCodeRef { get; set; }  // *[ANUBIS][][2016-09-02] - รหัสอ้างอิง
        public double cMinBalRemainCrd { get; set; } // *[CHUCK][][2017-03-03] - ค่ามัดจำ
        public double cCtyDeptAmt { get; set; } // *CH 07-07-2017 -ค่ามัดจำตามประเภทบัตร
    }
}