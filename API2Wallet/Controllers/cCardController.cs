using API2Wallet.Class.Log;
using API2Wallet.Class.Standard;
using API2Wallet.Models.WebService.Request.Card;
using API2Wallet.Models.WebService.Request.ChangeCard;
using API2Wallet.Models.WebService.Response.Card;
using API2Wallet.Models.WebService.Response.ChangeCard;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;

namespace API2Wallet.Controllers
{
    /// <summary>
    /// Change Card information.
    /// </summary>
    [Route(cCS.tCS_APIVer + "/Card")]
    public class cCardController : ControllerBase
    {
        /// <summary>
        ///  เปลี่ยนบัตรเป็นชุด//ChangeCardLis
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns>
        ///    System process status<br/>
        ///    &#8195;     1   : success.<br/>
        ///    &#8195;     701 : validate parameter model false.<br/>
        ///    &#8195;     713 : Card Date expired..<br/>
        ///    &#8195;     716 : ResetExpire card unsuccess."..<br/>
        ///    &#8195;     800 : data not found.<br/>
        ///    &#8195;     802 : formate data incorrect..<br/>
        ///    &#8195;     900 : service process false.<br/>
        ///    &#8195;     904 : key not allowed to use method.<br/>
        ///    &#8195;     905 : cannot connect database.<br/>
        ///    &#8195;     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("ChangeCardLis")]
        [HttpPost]
        public cmlResChangeCard POST_PUNoChangeCard([FromBody] List<cmlReqChangeCard> poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cChangeCard oChangeCard;
          //  List<cmlTSysConfig> aoSysConfig;
            cmlResChangeCard oResult;
            string tFuncName, tModelErr,  tErrCode, tErrDesc;
            bool bVerifyPara;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oResult = new cmlResChangeCard();
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
                    oChangeCard = new cChangeCard();
                    bVerifyPara = oChangeCard.C_DATbProcChangeCard(poPara, out tErrCode, out tErrDesc, out oResult);
                    if (bVerifyPara == true)
                    {
                        return oResult;
                    }
                    else
                    {
                        // Varify parameter value false.
                        oResult = new cmlResChangeCard();
                        oResult.rtCode = tErrCode;
                        oResult.rtDesc = tErrDesc;
                        return oResult;
                    }
                }
                else
                {
                    // Validate parameter model false.
                    oResult = new cmlResChangeCard();
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception oEx)
            {
                // Return error.
                oResult = new cmlResChangeCard();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                GC.WaitForPendingFinalizers();
                GC.Collect();
                oChangeCard= null;
                oResult = null;
            }
        }

        /// <summary>
        ///  ยกเลิกบัตร Cancel Card
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns>
        ///   System process status.<br/>
        ///   &#8195;    1   : success.<br/>
        ///   &#8195;    701 : validate parameter model false.<br/>
        ///   &#8195;    713 : Card Date expired.<br/>
        ///   &#8195;    716 : ResetExpire card unsuccess.<br/>
        ///   &#8195;    800 : data not found.<br/>
        ///   &#8195;    802 : formate data incorrect..<br/>
        ///   &#8195;    900 : service process false.<br/>
        ///   &#8195;    904 : key not allowed to use method.<br/>
        ///   &#8195;    905 : cannot connect database.<br/>
        ///   &#8195;    906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("ChangSta")]
        [HttpPost]
        public cmlResChangeSta POST_PUNoCancelCard([FromBody] List<cmlReqChangeSta> poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cChangeCard oChangeCard;
            //List<cmlTSysConfig> aoSysConfig;
            cmlResChangeSta oResult;
            string tFuncName, tModelErr, tErrCode, tErrDesc;
            bool bProcChangeSta;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResChangeSta();
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
                    oChangeCard = new cChangeCard();
                    bProcChangeSta = oChangeCard.C_DATbProcChangeSta(poPara, out tErrCode, out tErrDesc, out oResult);
                    if (bProcChangeSta == true)
                    {
                        return oResult;
                    }
                    else
                    {
                        // Varify parameter value false.
                        oResult = new cmlResChangeSta();
                        oResult.rtCode = tErrCode;
                        oResult.rtDesc = tErrDesc;
                        return oResult;
                    }
                }
                else
                {
                    // Validate parameter model false.
                    oResult = new cmlResChangeSta();
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception oEx)
            {
                // Return error.
                oResult = new cmlResChangeSta();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                GC.WaitForPendingFinalizers();
                GC.Collect();
                oChangeCard = null;
                oResult = null;
            }
        }

        /// <summary>
        /// ล้างบัตร//Card Clear List
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns>
        ///   System process status.<br/>
        ///   &#8195;    1   : success.<br/>
        ///   &#8195;    701 : validate parameter model false.<br/>
        ///   &#8195;    900 : service process false.<br/>
        ///   &#8195;    904 : key not allowed to use method.<br/>
        ///   &#8195;    905 : cannot connect database.<br/>
        ///   &#8195;    906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("CardClearList")]
        [HttpPost]
        public cmlResCardClearList POST_PUNoCardClearList([FromBody] List<cmlReqCardClearList> poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cChangeCard oChangeCard;
          //  List<cmlTSysConfig> aoSysConfig;
            cmlResCardClearList oResult;
            string tFuncName, tModelErr, tErrCode, tErrDesc;
            bool bProcChangeSta;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResCardClearList();
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
                    oChangeCard = new cChangeCard();
                    bProcChangeSta = oChangeCard.C_DATbProcCrdClrLst(poPara, out tErrCode, out tErrDesc, out oResult);
                    if (bProcChangeSta == true)
                    {
                        return oResult;
                    }
                    else
                    {
                        oResult = new cmlResCardClearList();
                        oResult.rtCode = tErrCode;
                        oResult.rtDesc = tErrDesc;
                        return oResult;
                    }
                }
                else
                {
                    // Validate parameter model false.
                    oResult = new cmlResCardClearList();
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception oEx)
            {
                // Return error.
                oResult = new cmlResCardClearList();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                GC.WaitForPendingFinalizers();
                GC.Collect();
                oChangeCard = null;
                oResult = null;
            }
        }

        /// <summary>
        /// เพิ่มบัตรใหม่//CardNewList
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns></returns>
        [Route("CardNewList")]
        [HttpPost]
        public cmlResCardNewList POST_PUNoCardNewList([FromBody] List<cmlReqCardNewList> poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cChangeCard oChangeCard;
            cmlResCardNewList oResult;
            string tFuncName, tModelErr, tErrCode, tErrDesc;
            bool bProcChangeSta;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResCardNewList();
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
                    oChangeCard = new cChangeCard();
                    bProcChangeSta = oChangeCard.C_DATbProcCardNewList(poPara, out tErrCode, out tErrDesc, out oResult);
                    if (bProcChangeSta == true)
                    {
                        return oResult;
                    }
                    else
                    {
                        oResult = new cmlResCardNewList();
                        oResult.rtCode = tErrCode;
                        oResult.rtDesc = tErrDesc;
                        return oResult;
                    }
                }
                else
                {
                    // Validate parameter model false.
                    oResult = new cmlResCardNewList();
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception oEx)
            {
                // Return error.
                oResult = new cmlResCardNewList();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                GC.WaitForPendingFinalizers();
                GC.Collect();
                oChangeCard = null;
                oResult = null;
            }
        }
    }
}
