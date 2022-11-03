using API2Wallet.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Request.SpotCheck
{
    /// <summary>
    /// Information class Model Request cmlReqSpotChk
    /// </summary>
    public class cmlReqSpotChk
    {
        /// <summary>
        /// (รหัสบัตร/wristband)
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptCrdCode { get; set; }

        /// <summary>
        /// (รหัสสาขา)
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptBchCode { get; set; }

        /// <summary>
        /// (รหัสภาษา)
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnLngID  { get; set; }

        /// <summary>
        /// (ยอดเงินใช้ได้จาก บัตร/wristband)
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public decimal pcAvailable { get; set; }

        /// <summary>
        /// (จำนวนครั้งที่ใช้งาน offline จาก บัตร/wristband)
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnTxnOffline { get; set; }
    }
}