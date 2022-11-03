using API2Wallet.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Request.Tranfer
{
    public class cmlReqTnfIn
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
        /// Remark
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptRemark { get; set; }


        /// <summary>
        /// ยอดมัดจำสินค้า
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public decimal pcCrdDeposit { get; set; }
    }
}