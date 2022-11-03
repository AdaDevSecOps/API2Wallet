using API2Wallet.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.Topup
{
    public class cmlResTopupList
    {
        /// <summary>
        /// รหัสบัตร
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(30, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string rtCrdCode { get; set; }

        /// <summary>
        /// รหัสสาขา
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(5, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string rtBchCode { get; set; }

        /// <summary>
        /// สถานะ  1  สำเร็จ, 2 ไม่สำเร็จ, 3ไม่สามารถ ResetExpired, 4 ไม่พบข้อมูลบัตร, 5.บัตรหมดอายุ, 6. บัตรยังไม่ถูกเบิกใช้งาน
        /// </summary>
        [Required(ErrorMessage = cCS.tCS_MsgAtrRequired)]
        [MaxLength(1, ErrorMessage = cCS.tCS_MsgAtrMaxLength)]
        public string rtStatus { get; set; }
    }
}