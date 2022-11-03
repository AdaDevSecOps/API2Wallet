using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.Card
{
    /// <summary>
    /// Information class model response Array ChangeCard
    /// </summary>
    public class cmlResaoChangeCard
    {
        /// <summary>
        /// จากรหัสบบัตร
        /// </summary>
        public string rtFrmCrdCode { get; set; }

        /// <summary>
        /// ถึงรหัสบัตร
        /// </summary>
        public string rtToCrdCode { get; set; }

        /// <summary>
        /// สาขา
        /// </summary>
        public string rtBchCode { get; set; }

        /// <summary>
        /// สถานะ  1: สำเร็จ,
        /// 2 : ไม่สำเร็จ,
        /// 3 :  จากรหัสบัตร หมดอายุรหัสบัตร,
        /// 4 :  ถึงรหัสบัตร หมดอายุรหัสบัตร,
        /// 5 :  จากรหัสบัตร ยังไม่เบิกใช้งาน,
        /// 6 :  ถึงรหัสบัตร ยังไม่เบิกใช้งาน,
        /// 7 :  จากรหัสบัตร Reset Expire ไมได้,
        /// 8 : ถึงรหัสบัตร Reset Expire ไมได้,
        /// 9 : รหัสพนักงานของบัตรต้นทางและปลายทางไม่ตรงกัน,
        /// 10: มูลค่าคงเหลือของบัตรปลายทางไม่เท่ากับ 0
        /// </summary>
        public string rtStatus { get; set; }
    }
}