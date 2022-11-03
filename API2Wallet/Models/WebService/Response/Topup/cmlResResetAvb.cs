using API2Wallet.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.Topup
{
    public class cmlResResetAvb : cmlResBese
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
        /// เงินมัดจำสินค้า
        /// </summary>
        public decimal rcCrdDepositPdt { get; set; }

        /// <summary>
        /// ยอดที่คืนได้
        /// </summary>
        public decimal rcTxnValueRtn { get; set; }

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
    }
}