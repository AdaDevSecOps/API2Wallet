using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class cmlTFNMCardType
    {
        /// <summary>
        /// รหัส
        /// </summary>
        public string FTCtyCode { get; set; }

        /// <summary>
        /// เงินมัดจำบัตร
        /// </summary>
        public decimal FCCtyDeposit { get; set; }

        /// <summary>
        /// ยอดเติมเงิน อัตโนมัติ
        /// </summary>
        public decimal FCCtyTopupAuto { get; set; }

        /// <summary>
        /// จำนวนวันหมดอายุบัตร (0:No check)
        /// </summary>
        public Int64 FNCtyExpirePeriod { get; set; }

        /// <summary>
        /// 1:hour 2:day 3:month 4:year
        /// </summary>
        public int FNCtyExpiredType { get; set; }

        /// <summary>
        /// สถานะอนุญาตคืน
        /// </summary>
        public string FTCtyStaAlwRet { get; set; }

        /// <summary>
        /// วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> FDLastUpdOn { get; set; }

        /// <summary>
        /// วันที่ปรับปรุงรายการล่าสุด
        /// </summary>
        public Nullable<DateTime> FDCreateOn { get; set; }

        /// <summary>
        /// ผู้ปรับปรุงรายการล่าสุด
        /// </summary>
        public string FTLastUpdBy { get; set; }

        /// <summary>
        /// ผู้สร้างรายการ
        /// </summary>
        public string FTCreateBy { get; set; }
    }
}