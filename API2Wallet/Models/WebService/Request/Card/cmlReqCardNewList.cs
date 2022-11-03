using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models.WebService.Request.Card
{
    /// <summary>
    /// 
    /// </summary>
    public class cmlReqCardNewList
    {
        /// <summary>
        /// รหัสภาษา
        /// </summary>
        public int pnLngID { get; set; }

        /// <summary>
        ///  รหัสสาขา
        /// </summary>
        public string ptBchCode { get; set; }

        /// <summary>
        /// รหัสบัตร
        /// </summary>
        public string ptCrdCode { get; set; }

        /// <summary>
        /// รหัสประเภทบัตร
        /// </summary>
        public string ptCtyCode { get; set; }

        /// <summary>
        /// รหัสผู้ถือบัตร
        /// </summary>
        public string ptCrdHolderID  { get; set; }

        /// <summary>
        /// รหัสแผนก
        /// </summary>
        public string ptDptCode { get; set; }

        /// <summary>
        /// ชื่อผู้ถือบัตร
        /// </summary>
        public string ptCrdName { get; set; } 
    }
}