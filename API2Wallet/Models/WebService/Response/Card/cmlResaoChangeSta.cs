using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.Card
{
    /// <summary>
    /// 
    /// </summary>
    public class cmlResaoChangeSta
    {
        /// <summary>
        /// รหัสบัตร
        /// </summary>
        public string rtCrdCode { get; set; }

        /// <summary>
        /// สาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        /// สถานะ  1: สำเร็จ,
        /// 2 : ไม่สำเร็จ,
        /// 3 :  จากรหัสบัตร หมดอายุรหัสบัตร,
        /// 4 :  ถึงรหัสบัตร หมดอายุรหัสบัตร,
        /// 5 :  ถึงรหัสบัตร ยังไม่เบิกใช้งาน,
        /// 6 :  ถึงรหัสบัตร ยังไม่เบิกใช้งาน,
        /// 7 :  จากรหัสบัตร Reset Expire ไมได้,
        /// 8 :   ถึงรหัสบัตร Reset Expire ไมได้
        /// </summary>
        public string rtStatus { get; set; }
    }
}