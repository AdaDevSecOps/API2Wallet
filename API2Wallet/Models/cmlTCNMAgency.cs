using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API2Wallet.Models
{
    /// <summary>
    /// Information claass model TCNMAgency
    /// </summary>
    public class cmlTCNMAgency
    {
        public string FTAgnCode { get; set;}
        public Guid FTAgnKeyAPI { get; set;}
        public string FTAgnPwd { get; set; }
        public string FTAgnEmail { get; set; }
        public string FTAgnTel { get; set; }
        public string FTAgnFax { get; set; }
        public string FTAgnMo { get; set;}
        public string FTAgnStaApv { get; set; }
        public string FTAgnStaActive { get; set;}
        public string FTPplCode { get; set; }
        public string FTPmgCode { get; set; }
        public int FNAtyID { get; set; }
        public int FNAggID { get; set; }
    }
}