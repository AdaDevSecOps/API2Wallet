using API2Wallet.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.Card
{
    /// <summary>
    /// 
    /// </summary>
    public class cmlResChangeSta : cmlResBese
    {
        /// <summary>
        /// 
        /// </summary>
        public List<cmlResaoChangeSta> raoCancelCard { get; set; }
    }
}