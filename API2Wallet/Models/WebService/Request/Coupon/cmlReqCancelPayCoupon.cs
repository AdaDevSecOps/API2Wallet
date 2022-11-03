using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Request.Coupon
{
    public class cmlReqCancelPayCoupon
    {
        /// <summary>
        /// จากสาขา
        /// </summary>
        public string ptCpbFrmBch { get; set; }

        /// <summary>
        /// จากเครื่องจุดขาย
        /// </summary>
        public string ptCpbFrmPos { get; set; }

        /// <summary>
        /// เลขที่เอกสารอ้างอิง
        /// </summary>
        public string ptCpbFrmSaleRef { get; set; }
    }
}