using API2Wallet.Class.Standard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.Topup
{
    /// <summary>
    /// Information class model Response Return topup list
    /// </summary>
    public class cmlResReturnTopupList
    {
        /// <summary>
        /// รหัสบัตร
        /// </summary>
        public string rtCrdCode { get; set; }

        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        /// สถานะ 1  สำเร็จ, 2 ไม่สำเร็จ, 3.สถานนะบัตรยังไม่ถูกเปิดใช้งาน,4 ไม่พบข้อมูลบัตร ,5 ไม่สามารถ ResetExpired
        /// </summary>
        public string rtStatus { get; set; }
    }
}