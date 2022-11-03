using API2Wallet.Class.Standard;
using API2Wallet.Models;
using API2Wallet.Models.WebService.Request.SpotCheck;
using API2Wallet.Models.WebService.Response.SpotCheck;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace API2Wallet.Class.SpotCheck
{
    /// <summary>
    /// Information Spot Check.
    /// </summary>
    public class cSpotCheck
    {
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="poPara"></param>
        /// <param name="ptErrCode"></param>
        /// <param name="ptErrDesc"></param>
        /// <param name="poResult"></param>
        /// <returns></returns>
        public bool C_DATbProcSpotCheck(cmlReqSpotChk poPara,out string ptErrCode
                                      , out string ptErrDesc, out cmlResSpotChk poResult)
        {
            cSP oFunc = new cSP();
            cMS oMsg = new cMS();
            StringBuilder oSql;
            DateTime dExpireTopup = DateTime.Now;
            try
            {
                oSql = new StringBuilder();
                poResult = oFunc.SP_GEToValueCard(poPara.pnTxnOffline, poPara.ptCrdCode, poPara.pnLngID,poPara.pcAvailable);
                if (poResult != null)
                {
                    ptErrCode = "";
                    ptErrDesc = "";
                    return true;
                }
                else
                {
                    ptErrCode = oMsg.tMS_RespCode800;
                    ptErrDesc = oMsg.tMS_RespDesc800;
                    return false;
                }
            }
            catch (Exception oEx)
            {
                poResult = new cmlResSpotChk();
                ptErrCode = oMsg.tMS_RespCode900;
                ptErrDesc = oMsg.tMS_RespDesc900;
                return false;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oSql = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}