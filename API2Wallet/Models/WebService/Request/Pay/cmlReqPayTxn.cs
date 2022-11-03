using API2Wallet.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Request.Pay
{
    /// <summary>
    /// Request Model ตัดจ่าย//PayTxn
    /// </summary>
    public class cmlReqPayTxn
    {
        /// <summary>
        /// รหัสบัตร
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCrdCode { get; set; }

        /// <summary>
        /// รหัสสาขา
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptBchCode { get; set; }

        /// <summary>
        /// รหัสเครื่อง POS
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(3, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptTxnPosCode { get; set; }

        /// <summary>
        /// เลขที่เอกสารอ้างอิง
        /// </summary>
        public string ptTxnDocNoRef { get; set; }

        /// <summary>
        /// ยอดเงิน
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public decimal pcTxnValue { get; set; }

        /// <summary>
        /// ยอดมัดจำบัตร
        /// </summary>
        public decimal pcCrdDeposit { get; set; }

        /// <summary>
        /// ยอดมัดจำสินค้า
        /// </summary>
        public decimal pcCrdDepositPdt { get; set; }

        /// <summary>
        /// รหัสภาษา
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnLngID { get; set; }

        /// <summary>
        /// ยอดเงินใช้ได้จาก บัตร/wristband
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public decimal pcAvailable { get; set; }

        /// <summary>
        /// จำนวนครั้งที่ใช้งาน offline จาก บัตร/wristband
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnTxnOffline { get; set; }

        /// <summary>
        /// รหัสร้านค้า
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptShpCode { get; set; }

        /// <summary>
        /// ลำดับเอกสารอ้างอิง
        /// </summary>
        public int pnTxnRef { get; set; }
    }
}