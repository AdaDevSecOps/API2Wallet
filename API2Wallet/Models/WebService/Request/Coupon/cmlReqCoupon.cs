using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Request.Coupon
{
    public class cmlReqCoupon
    {
        /// <summary>
        /// ประเภทคูปอง
        /// </summary>
        public string ptCouponType { get; set; }

        /// <summary>
        /// เลขที่เอกสารคูปอง
        /// </summary>
        public string ptCpnDocNo { get; set; } //*Net 63-03-12 Create

        /// <summary>
        /// รหัสคูปอง
        /// </summary>
        public string ptBarCpn { get; set; } //*Net 63-03-12 Rename

        /// <summary>
        /// รหัสสาขา
        /// </summary>
        public string ptBranch { get; set; }

        /// <summary>
        /// กลุ่มราคา
        /// </summary>
        public string ptPriceGroup { get; set; }

        /// <summary>
        /// Merchant
        /// </summary>
        public string ptMerchant { get; set; }

        /// <summary>
        /// รหัสภาษา
        /// </summary>
        public int pnLangID { get; set; }

        /// <summary>
        /// รหัสลูกค้า
        /// </summary>
        public string ptCstCode { get; set; } //*Net 63-03-21
    }
}