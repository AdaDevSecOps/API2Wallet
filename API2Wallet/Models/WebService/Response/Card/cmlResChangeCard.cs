using API2Wallet.Models.WebService.Response.Base;
using API2Wallet.Models.WebService.Response.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.ChangeCard
{
    /// <summary>
    /// Information class model response  ChangeCard
    /// </summary>
    public class cmlResChangeCard : cmlResBese
    {
        /// <summary>
        /// Information class model response Array ChangeCard
        /// </summary>
        public List<cmlResaoChangeCard> raoChangeCard { get; set; }
    }
}