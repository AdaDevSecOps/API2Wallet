using API2Wallet.Class.Standard;
using API2Wallet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace API2Wallet.Class.Online
{
    /// <summary>
    /// Information class Online
    /// </summary>
    public class cIsOnline
    {
        /// <summary>
        /// ตรวจสอบสถานะ Online
        /// </summary>
        /// <param name="ptErrCode"></param>
        /// <param name="ptErrDesc"></param>
        /// <param name="poIsOnlineErr"></param>
        /// <returns></returns>
        public bool C_DATbVerifyIsOnline(out string ptErrCode
                                       , out string ptErrDesc, out cmlResIsOnline poIsOnlineErr)
        {
            cSP oFunc = new cSP();
            cMS oMsg = new cMS();
            cDatabase oDatabase;
            StringBuilder oSql;
            DataTable oTblComp;
            try
            {
                oDatabase = new cDatabase();
                oSql = new StringBuilder();
                oSql.Clear();
                oSql.AppendLine("SELECT TOP 1 FTCmpCode FROM TCNMComp WITH (NOLOCK)");
                oTblComp = oDatabase.C_GEToQuerySQLTbl(oSql.ToString());
                if (!(oTblComp.Rows.Count >= 0))
                {
                    poIsOnlineErr = new cmlResIsOnline();
                    ptErrCode = oMsg.tMS_RespCode905;
                    ptErrDesc = oMsg.tMS_RespDesc905;
                    return false;
                }

                ptErrCode = "";
                ptErrDesc = "";
                poIsOnlineErr = null;
                return true;
            }
            catch (Exception oEx)
            {
                poIsOnlineErr = new cmlResIsOnline();
                ptErrCode = oMsg.tMS_RespCode900;
                ptErrDesc = oMsg.tMS_RespDesc900;
                return false;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oDatabase = null;
                oSql = null;
                oTblComp = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}