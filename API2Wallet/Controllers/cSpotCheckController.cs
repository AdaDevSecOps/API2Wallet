using API2Wallet.Class.SpotCheck;
using API2Wallet.Class.Standard;
using API2Wallet.Models.WebService.Request.SpotCheck;
using API2Wallet.Models.WebService.Response.SpotCheck;
using log4net;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace API2Wallet.Controllers
{
    /// <summary>
    /// SpotCheck information.
    /// </summary>
    [Route(cCS.tCS_APIVer + "/SpotCheck")]
    public class cSpotCheckController : ControllerBase
    {
        // log  //*[AnUBiS][][2019-04-29]
        private static readonly ILog oC_Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //*Em 62-05-29  Pandora
        /// <summary>
        /// ตรวจสอบยอดคงเหลือ
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns>
        ///     System process status.<br/>
        ///&#8195;     1   : success.<br/>
        ///&#8195;     701 : validate parameter model false.<br/>
        ///&#8195;     800 : data not found.<br/>
        ///&#8195;     802 : formate data incorrect..<br/>
        ///&#8195;     900 : service process false.<br/>
        ///&#8195;     904 : key not allowed to use method.<br/>
        ///&#8195;     905 : cannot connect database.<br/>
        ///&#8195;     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Check")]
        [HttpPost]
        public cmlResSpotChk POST_GEToSportCheck([FromBody] cmlReqSpotChk poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cSpotCheck oSpotCheck;
            cmlResSpotChk oResult;
            string tFuncName, tModelErr, tErrCode, tErrDesc;
            string tLogFmt, tLogInf;
            bool bVerifyPara;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                //*[AnUBiS][][2019-04-29] - log call web service.
                tLogFmt = "[{0}]  [{1}]  [{2}]  [{3}]  [{4}]";
                tLogInf = string.Format(
                    tLogFmt,
                    DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffff"),
                    "Spot check.",
                    "Branch: " + poPara.ptBchCode,
                    "Card: " + poPara.ptCrdCode,
                    "Request from client.");
                oC_Log.Info(tLogInf);

                oResult = new cmlResSpotChk();
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(ref tModelErr, ModelState))
                {
                    // Varify parameter value.
                    oSpotCheck = new cSpotCheck();
                    bVerifyPara = oSpotCheck.C_DATbProcSpotCheck(poPara, out tErrCode, out tErrDesc, out oResult);
                    if (bVerifyPara == true)
                    {
                        return oResult;
                    }
                    else
                    {
                        // Varify parameter value false.
                        oResult = new cmlResSpotChk();
                        oResult.rtCode = tErrCode;
                        oResult.rtDesc = tErrDesc;
                        return oResult;
                    }
                }
                else
                {
                    // Validate parameter model false.
                    oResult = new cmlResSpotChk();
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception oEx)
            {
                oResult = new cmlResSpotChk();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                //*[AnUBiS][][2019-04-29] - log call web service.
                tLogFmt = "[{0}]  [{1}]  [{2}]  [{3}]  [{4}]";
                tLogInf = string.Format(
                    tLogFmt,
                    DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffff"),
                    "Spot check.",
                    "Branch: " + poPara.ptBchCode,
                    "Card: " + poPara.ptCrdCode,
                    "Reply to the client.");
                oC_Log.Info(tLogInf);

                oFunc = null;
                oCons = null;
                oMsg = null;
                oResult = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}

