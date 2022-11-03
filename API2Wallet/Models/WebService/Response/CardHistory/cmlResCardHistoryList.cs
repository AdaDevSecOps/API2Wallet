using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.CardHistory
{
    /// <summary>
    /// INformation Model ResCardHistoryList
    /// </summary>
    public class cmlResCardHistoryList
    {
        /// <summary>
        /// ประเภทรายการ
        /// </summary>
        public string rtType { get; set; }

        /// <summary>
        /// วันที่เอกสาร
        /// </summary>
        public DateTime? rdDocDate { get; set; }

        /// <summary>
        /// รหัสสาขาอ้างอิง
        /// </summary>
        public string rtBchRef { get; set; }

        /// <summary>
        /// เลขที่เอกสารอ้างอิง
        /// </summary>
        public string rtDocRef { get; set; }

        ///// <summary>
        ///// เครื่องจุดขาย
        ///// </summary>
        //public string rtPosCode { get; set; }

        /// <summary>
        /// มูลค่า
        /// </summary>
        public decimal rcTxnValue { get; set; }

        /// <summary>
        /// สถานะ Offline
        /// </summary>
        public int rnStaOffline { get; set; }

        /// <summary>
        /// ชื่อร้านค้า
        /// </summary>
        public string rtShopName { get; set; }

        /// <summary>
        /// ยอดเงินมัดจำบัตร
        /// </summary>
        public decimal rcTxnDeposit { get; set; }

        /// <summary>
        /// มูลค่าก่อนใช้
        /// </summary>
        /// <remarks>
        /// *[AnUBiS][][2018-12-03] - add new property
        /// </remarks>
        public Nullable<decimal> rcTxnCrdValue { get; set; }
    }
}