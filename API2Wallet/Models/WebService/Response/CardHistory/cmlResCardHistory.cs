using API2Wallet.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.CardHistory
{
    /// <summary>
    /// Card history information.
    /// </summary>
    public class cmlResCardHistory : cmlResBese
    {
        /// <summary>
        /// Card history list.
        /// </summary>
        public List<cmlResCardHistoryList> raoCrdHistory { get; set; }
    }
}