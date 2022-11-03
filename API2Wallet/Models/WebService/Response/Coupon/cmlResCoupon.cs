using API2Wallet.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.Coupon
{
    public class cmlResCoupon : cmlResBese
    {
        public List<cmlResCouponList> raoCoupon { get; set; }
        
    }
}