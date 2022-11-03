using API2Wallet.Models.Database;
using API2Wallet.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.SpotCheck
{
    /// <summary>
    /// Information class response spot Check
    /// </summary>
    public class cmlResSpotChk : cmlResBese
    {
        /// <summary>
        /// ยอดทั้งหมด
        /// </summary>
        public decimal rcTxnValue { get; set; }

        /// <summary>
        /// เงินมัดจำบัตร
        /// </summary>
        public decimal rcCrdDeposit { get; set; }

        /// <summary>
        /// เงินมัดจำบัตรสินค้า
        /// </summary>
        public decimal rcCrdDepositPdt { get; set; }

        /// <summary>
        /// ยอดใช้ได้
        /// </summary>
        public decimal rcTxnValueAvb { get; set; }

        /// <summary>
        /// ชื่อบัตร
        /// </summary>
        public string rtCrdName { get; set; }

        /// <summary>
        /// ประเภทบัตร
        /// </summary>
        public string rtCtyName { get; set; }

        /// <summary>
        /// วันที่บัตรหมดอายุ
        /// </summary>
        public Nullable<DateTime> rdCrdExpireDate { get; set; }

        /// <summary>
        /// วันที่เติมเงินล่าสุด
        /// </summary>
        public Nullable<DateTime> rdCrdLastTopup { get; set; }

        /// <summary>
        /// จำนวนครั้งที่ใช้งาน offline จาก บัตร/wristband
        /// </summary>
        public int rnTxnOffline { get; set; }

        /// <summary>
        /// ประเภทหมดอายุบัตร 
        /// </summary>
        public int rnCtyExpiredType { get; set; }

        /// <summary>
        /// จำนวนอายุ บัตร
        /// </summary>
        public int rnCtyExpirePeriod { get; set; }

        /// <summary>
        /// รหัสพนักงาน, หมายเลขผู้ถือบัตร
        /// </summary>
        public string rtCrdHolderID { get; set; }

        /// <summary>
        /// สถานะบัตร 1: เคลื่อนไหว, 2: ไม่เคลื่อนไหว, 3: ยกเลิก
        /// </summary>
        /// 
        /// <remarks>
        /// *[AnUBiS][][2019-02-21] - pandora.
        /// </remarks>
        public string rtCrdStaActive { get; set; }
    }
}