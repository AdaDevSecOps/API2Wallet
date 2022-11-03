using API2Wallet.Class.Standard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace API2Wallet.Models.WebService.Request.CardHistory
{
    /// <summary>
    /// Information class model Request Card history
    /// </summary>
    public class cmlReqCardHistory
    {
        /// <summary>
        /// รหัสบัตร
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptCrdCode { get; set; }

        /// <summary>
        /// สาขา
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public string ptBchCode { get; set; }

        ///// <summary>
        ///// วันที่เอกสาร
        ///// </summary>
        //[Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        //public DateTime pdDocDate { get; set; }

        /// <summary>
        /// จำนวนรายการที่ต้องการแสดง
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public Int32 pnQty { get; set; }

        /// <summary>
        /// Sort การเรียงลำดับข้อมูล 0 = น้อย , 1 = มาก 
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string ptSort { get; set; }

        /// <summary>
        /// จำนวนครั้งที่ใช้งาน offline จาก บัตร/wristband
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        public int pnTxnOffline { get; set; }

        /// <summary>
        /// วันที่เอกสาร formate yyyy-MM-dd
        /// </summary>
        public string ptTxnDocDate{ get; set; }
    }
}