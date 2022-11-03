using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.Card
{
    /// <summary>
    /// 
    /// </summary>
    public class cmlResaoCardClearList
    {
        /// <summary>
        /// รหัสบัตร
        /// </summary>
        public string rtCrdCode { get; set; }

        /// <summary>
        /// สถานะ  
        /// 1: สำเร็จ
        /// 2: ไม่สำเร็จ
        /// 3: ไม่พบข้อมูลบัตร
        /// /// </summary>
        public string rtStatus { get; set; }
    }
}