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
    //[Route(cCS.tCS_APIVer + "/Online")] //*Net 63-03-18 เปลี่ยนให้เหมือน API2Master
    [Route(cCS.tCS_APIVer + "/CheckOnline")]
    public class cCheckOnlineController: ControllerBase
    {
        /// <summary>
        /// Function check online (เช็คผ่าน URL)
        /// </summary>
        /// <returns>
        /// System process status.<br/>
        ///&#8195;     1   : success.<br/>    
        ///&#8195;     900 : service process false.<br/>
        ///&#8195;     904 : key not allowed to use method.<br/>
        ///&#8195;     905 : cannot connect database.<br/>
        /// </returns>
        //[Route("CheckOnline")] //*Net 63-03-18 เปลี่ยนให้เหมือน API2Master
        [Route("IsOnline")]
        [HttpGet]
        public cmlResIsOnline GET_CHKoIsOnline()
        {
            //*Arm 63-01-23
            cMS oMsg;
            cIsOnline oOnline;
            cmlResIsOnline oResult;
            string tErrCode, tErrDesc, tDBName, tErrAPI;
            bool bChk;
            cDatabase oDatabase;
            cSP oSP;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oResult = new cmlResIsOnline();
                oMsg = new cMS();
                oOnline = new cIsOnline();
                oSP = new cSP();

                
                oDatabase = new cDatabase();
                tDBName = (oDatabase.C_CONoDatabase()).Database;

                oResult.rtResult = tDBName + " :Ready";
                oResult.rtCode = oMsg.tMS_RespCode1;
                oResult.rtDesc = oMsg.tMS_RespDesc1;
                return oResult;
            }
            catch (Exception)
            {
                oResult = new cmlResIsOnline();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oMsg = null;
                oResult = null;
                oDatabase = null;
                tDBName = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

    }
}