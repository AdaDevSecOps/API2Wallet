using API2Wallet.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Request.Topup
{
    public class cmlReqTopupList
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
        /// มูลค่า
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public decimal pcTxnValue { get; set; }

        /// <summary>
        /// สถานะเติมเงิน Auto or manual
        /// 0: manual, 1: Auto
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptAuto { get; set; }

        /// <summary>
        /// รหัสเครื่อง POS
        /// </summary>
        //[Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(3, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptTxnPosCode { get; set; }

        /// <summary>
        /// รหัสร้านค้า
        /// </summary>
        //[Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptShpCode { get; set; }

        /// <summary>
        /// เลขที่เอกสารอ้างอิง
        /// </summary>
        public string ptDocNoRef { get; set; }
    }
}