using API2Wallet.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.Topup
{
    /// <summary>
    /// Respont Model ยกเลิกการเติมเงิน//Cancel Topup
    /// </summary>
    public class cmlResCancelTopup : cmlResBese
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
        /// จำนวนครั้งที่ใช้งาน offline จาก บัตร/wristband
        /// </summary>
        public int rnTxnOffline { get; set; }
    }
}