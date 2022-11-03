using API2Wallet.Class;
using API2Wallet.Class.Online;
using API2Wallet.Class.Standard;
using API2Wallet.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Threading;

namespace API2Wallet.Controllers
{
    /// <summary>
    /// Information Class online
    /// </summary>
    [Route(cCS.tCS_APIVer + "/Online")] //*Net 63-03-18
    public class cOnlineController : ControllerBase
    {
        /// <summary>
        /// Is online ตรวจสอบ connection
        /// </summary>
        /// <returns>
        /// System process status.<br/>
        ///&#8195;     1   : success.<br/>    
        ///&#8195;     900 : service process false.<br/>
        ///&#8195;     905 : cannot connect database.<br/>
        ///&#8195;     906 : this time not allowed to use method.<br/>
        /// </returns>
        //[Route("IsOnline")]  //*Net 63-03-18
        [Route("CheckOnline")]
        [HttpPost]
        public cmlResIsOnline POST_CHKoIsOnline()
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cIsOnline oIsOnline;
            cmlResIsOnline oResIsOnlineErr;
            cmlResIsOnline oResult;
            string tErrCode, tErrDesc;
            bool bVerifyPara;
            string tDBName = "";
            cDatabase oDatabase;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oResult = new cmlResIsOnline();
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();

                // Varify parameter value.
                oIsOnline = new cIsOnline();
                bVerifyPara = oIsOnline.C_DATbVerifyIsOnline(out tErrCode, out tErrDesc, out oResIsOnlineErr);
                if (bVerifyPara == true)
                {
                    oDatabase = new cDatabase();
                    tDBName = oDatabase.C_GETtSQLScalarString("SELECT NAME FROM SYS.sysdatabases WHERE DBID=db_id()");
                    oResult.rtResult = tDBName + " :Ready";
                    oResult.rtCode = oMsg.tMS_RespCode1;
                    oResult.rtDesc = oMsg.tMS_RespDesc1;
                    return oResult;
                }
                else
                {
                    // Varify parameter value false.
                    oResult.rtCode = tErrCode;
                    oResult.rtDesc = tErrDesc;
                    return oResult;
                }
            }
            catch (Exception)
            {
                // Return error.
                oResult = new cmlResIsOnline();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                oResIsOnlineErr = null;
                oResult = null;
                oResult = null;
                oDatabase = null;
                tDBName = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
