using API2Wallet.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.Pay
{
    public class cmlResReturnDeposit : cmlResBese
    {
        /// <summary>
        /// ยอดทั้งหมด
        /// </summary>
        public decimal rcTxnValue { get; set; }

        /// <summary>
        /// เงินมัดจำบัตร
        /// </summary>
        public decimal rcCtyDeposit { get; set; }

        /// <summary>
        /// ยอดใช้ได้
        /// </summary>
        public decimal rcTxnValueAvb { get; set; }

        /// <summary>
        /// ประเภทบัตร
        /// </summary>
        public string rtCtyCode { get; set; }

        /// <summary>
        /// วันที่บัตรหมดอายุ
        /// </summary>
        public DateTime rdCrdExpireDate { get; set; }

        /// <summary>
        /// ชื่อบัตร
        /// </summary>
        public string rtCrdName { get; set; }

        /// <summary>
        /// เงินมัดจำสินค้า
        /// </summary>
        public decimal rcCrdDeposit { get; set; }
    }
}