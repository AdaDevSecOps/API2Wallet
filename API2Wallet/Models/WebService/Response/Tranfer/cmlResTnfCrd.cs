using API2Wallet.Models.WebService.Response.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Response.Tranfer
{
    /// <summary>
    /// 
    /// </summary>
    public class cmlResTnfCrd : cmlResBese
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
        /// สถานะ
        /// </summary>
        public string rtStatus { get; set; }
    }
}