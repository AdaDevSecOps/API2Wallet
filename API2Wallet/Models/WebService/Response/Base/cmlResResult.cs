using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.Base
{
    /// <summary>
    /// Result
    /// </summary>
    /// <typeparam name="Model">Model of result.</typeparam>
    public class cmlResResult<Model> : cmlResBese
    {
        /// <summary>
        /// Item.
        /// </summary>
        public Model roItem { get; set; }

        /// <summary>
        /// List items.
        /// </summary>
        public List<Model> raoItems { get; set; }

        /// <summary>
        /// Count of items.
        /// </summary>
        public int rnCount { get; set; }
    }
}