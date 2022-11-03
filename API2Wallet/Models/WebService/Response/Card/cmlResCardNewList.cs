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
    public class cmlResCardNewList : cmlResBese
    {
        /// <summary>
        /// 
        /// </summary>
        public List<cmlResaoCardNewList> raoCardNewList { get; set; }

    }
}