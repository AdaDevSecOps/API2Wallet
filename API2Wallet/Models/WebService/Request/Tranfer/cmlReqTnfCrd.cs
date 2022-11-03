using API2Wallet.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Request.Tranfer
{
    /// <summary>
    /// 
    /// </summary>
    public class cmlReqTnfCrd
    {
        /// <summary>
        /// จากรหัสบัตร
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptFrmCrdCode { get; set; }

        /// <summary>
        /// ถึงรหัสบัตร
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptToCrdCode { get; set; }

        /// <summary>
        /// รหัสสาขา
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptBchCode { get; set; }

        /// <summary>
        /// ยอดทั้งหมดบัตรเก่า
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public decimal pcFrmCrdValue { get; set; }

        /// <summary>
        /// ยอดทั้งหมดบัตรใหม่
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public decimal pcToCrdValue { get; set; }

        /// <summary>
        /// เลขที่เอกสารอ้างอิง หรือ id กรณี Offline
        /// </summary>
        /// 
        /// <remarks>
        /// *[AnUBiS][][2019-02-13] - add new property.
        /// </remarks>
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptDocNoRef { get; set; }
    }
}