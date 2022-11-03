using API2Wallet.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models
{
    /// <summary>
    /// Information Is online ตรวจสอบ connection
    /// </summary>
    public class cmlResIsOnline : cmlResBese
    {
        /// <summary>
        /// Response result
        /// </summary>
        public string rtResult { get; set; }
    }
}