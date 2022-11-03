using API2Wallet.Class;
using API2Wallet.Class.Pay;
using API2Wallet.Class.Standard;
using API2Wallet.Models;
using API2Wallet.Models.WebService.Request.Pay;
using API2Wallet.Models.WebService.Response.Base;
using API2Wallet.Models.WebService.Response.Pay;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using log4net;
using System.Data.SqlClient;
using System.Data.Entity.Core;

namespace API2Wallet.Controllers
{
    /// <summary>
    /// Pay information.
    /// </summary>
    [Route(cCS.tCS_APIVer + "/Pay")]
    public class cPayController : ControllerBase
    {
        // log  //*[AnUBiS][][2019-04-29]
        private static readonly ILog oC_Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// ตัดจ่าย//PayTxn
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns>
        ///System process status.<br/>
        ///&#8195;     1   : success.<br/>
        ///&#8195;     701 : validate parameter model false.<br/>
        ///&#8195;     713 : Card Date expired.<br/>
        ///&#8195;     714 : The amount of money in the card is not enough.<br/>
        ///&#8195;     721 : Data sale not found.<br/>
        ///&#8195;     800 : data not found.<br/>
        ///&#8195;     802 : formate data incorrect.<br/>
        ///&#8195;     900 : service process false.<br/>
        ///&#8195;     904 : key not allowed to use method.<br/>
        ///&#8195;     905 : cannot connect database.<br/>
        ///&#8195;     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("PayTxn")]
        [HttpPost]
        public cmlResPayTxn POST_PUNoInsPayTxn([FromBody] cmlReqPayTxn poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cPay oPay;
            cmlResPayTxn oResult;
            string tFuncName, tModelErr,  tErrCode, tErrDesc;
            string tLogFmt, tLogInf;
            bool bVerifyPara;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                //*[AnUBiS][][2019-04-29] - log call web service.
                tLogFmt = "[{0}]  [{1}]  [{2}]  [{3}]  [{4}]  [{5}]  [{6}]";
                tLogInf = string.Format(
                    tLogFmt,
                    DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffff"),
                    "Payment transaction.",
                    "Branch: " + poPara.ptBchCode,
                    "Pos: " + poPara.ptTxnPosCode,
                    "Card: " + poPara.ptCrdCode,
                    "Bill: " + poPara.ptTxnDocNoRef,
                    "Request from client.");
                oC_Log.Info(tLogInf);

                oResult = new cmlResPayTxn();
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
                    oPay = new cPay();
                    bVerifyPara = oPay.C_DATbProcPaytxn(poPara, out tErrCode, out tErrDesc, out oResult);
                    if (bVerifyPara == true)
                    {
                        return oResult;
                    }
                    else
                    {
                        // Varify parameter value false.
                        oResult = new cmlResPayTxn();
                        oResult.rtCode = tErrCode;
                        oResult.rtDesc = tErrDesc;
                        return oResult;
                    }
                }
                else
                {
                    // Validate parameter model false.
                    oResult = new cmlResPayTxn();
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception oEx)
            {
                // Return error.
                oResult = new cmlResPayTxn();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                //*[AnUBiS][][2019-04-29] - log call web service.
                tLogFmt = "[{0}]  [{1}]  [{2}]  [{3}]  [{4}]  [{5}]  [{6}]";
                tLogInf = string.Format(
                    tLogFmt,
                    DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss.ffff"),
                    "Payment transaction.",
                    "Branch: " + poPara.ptBchCode,
                    "Pos: " + poPara.ptTxnPosCode,
                    "Card: " + poPara.ptCrdCode,
                    "Bill: " + poPara.ptTxnDocNoRef,
                    "Reply to the client.");
                oC_Log.Info(tLogInf);

                oFunc = null;
                oCons = null;
                oMsg = null;               
                oPay = null;              
                oResult = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// ยกเลิกการตัดจ่าย//CancelPayTxn
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns>
        ///     System process status.<br/>
        ///&#8195;     1   : success.<br/>
        ///&#8195;     701 : validate parameter model false.<br/>
        ///&#8195;     713 : Card Date expired.<br/>
        ///&#8195;     715 : Record is canceled.<br/>
        ///&#8195;     721 : Data sale not found.<br/>
        ///&#8195;     800 : data not found.<br/>
        ///&#8195;     802 : formate data incorrect..<br/>
        ///&#8195;     900 : service process false.<br/>
        ///&#8195;     904 : key not allowed to use method.<br/>
        ///&#8195;     905 : cannot connect database.<br/>
        ///&#8195;     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("CancelPayTxn")]
        [HttpPost]
        public cmlRescancelPayTxn POST_PUNoInsCancelPayTxn([FromBody] cmlReqCancelpayTxn poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cPay oPay;
            cmlRescancelPayTxn oResult;
            string tFuncName, tModelErr, tErrCode, tErrDesc;
            bool bVerifyPara;
            cmlTFNTCrdSale oCrdsale = new cmlTFNTCrdSale();
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oResult = new cmlRescancelPayTxn();
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
                    oPay = new cPay();
                    bVerifyPara = oPay.C_DATbProcCancelPaytxn(poPara, out tErrCode, out tErrDesc, out oResult);
                    if (bVerifyPara == true)
                    {
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
                else
                {
                    // Validate parameter model false.
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception)
            {
                // Return error.
                oResult = new cmlRescancelPayTxn();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;               
                oPay = null;
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        /// <summary>
        /// ตรวจสอบรายการการตัดจ่าย ยกเลิกตัดจ่าย
        /// </summary>
        /// <param name="poPara">Document information.</param>
        /// <returns>
        /// Transaction payment.<br/>
        /// System process status.<br/>
        ///&#8195;     1   : success.<br/>
        ///&#8195;     701 : validate parameter model false.<br/>
        ///&#8195;     800 : data not found.<br/>
        ///&#8195;     900 : service process false.<br/>
        ///&#8195;     905 : cannot connect database.<br/>
        /// </returns>
        [Route("CheckPayTxn")]
        [HttpPost]
        public cmlResResult<cmlResCheckPayTxn> POST_PAYoCheckPayTxn([FromBody] cmlReqCheckPayTxn poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDatabase;
            cmlResResult<cmlResCheckPayTxn> oResult;
            List<cmlTSysConfig> aoSysConfig;
            string tFuncName, tModelErr;
            int nConTme, nCmdTme;
            SqlParameter[] aoSqlParam;
            DataTable oDbTbl;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();
                oResult = new cmlResResult<cmlResCheckPayTxn>();

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";

                if (oFunc.SP_CHKbParaModel(ref tModelErr, ModelState))
                {
                    // Load configuration.
                    aoSysConfig = oFunc.SP_SYSaLoadConfiguration();

                    // Confuguration database.
                    nConTme = 0;
                    oFunc.SP_DATxGetConfigurationFromMem<int>(ref nConTme, cCS.nCS_ConTme, aoSysConfig, "002");
                    nCmdTme = 0;
                    oFunc.SP_DATxGetConfigurationFromMem<int>(ref nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "003");
                    oDatabase = new cDatabase(nConTme);

                    //oSql = new StringBuilder();
                    //oSql.AppendLine("SELECT");
                    //oSql.AppendLine("	TXN.FNTxnID AS rnTxnID, ");
                    //oSql.AppendLine("	TXN.FTTxnDocNoRef AS rtTxnDocNoRef, ");
                    //oSql.AppendLine("	TXN.FDTxnDocDate AS rdTxnDocDate, ");
                    //oSql.AppendLine("	TXN.FTCrdCode AS rtCrdCode, ");
                    //oSql.AppendLine("	CASE WHEN ISNULL(CRDL.FTCrdName,'') = '' ");
                    //oSql.AppendLine("		THEN (");
                    //oSql.AppendLine("				SELECT TOP 1 FTCrdName ");
                    //oSql.AppendLine("				FROM TFNMCard_L WITH(NOLOCK) ");
                    //oSql.AppendLine("				WHERE FTCrdCode = CRD.FTCrdCode ");
                    //oSql.AppendLine("				ORDER BY FNLngID ASC");
                    //oSql.AppendLine("			) ");
                    //oSql.AppendLine("		ELSE CRDL.FTCrdName ");
                    //oSql.AppendLine("	END AS rtCrdName,");
                    //oSql.AppendLine("	TXN.FCTxnCrdValue AS rcTxnCrdValue, ");
                    //oSql.AppendLine("	TXN.FCTxnValue AS rcTxnValue, ");
                    //oSql.AppendLine("	TXN.FNTxnIDRef AS rnTxnIDRef, ");
                    //oSql.AppendLine("	CRD.FTCrdHolderID AS rtCrdHolderID ");
                    //oSql.AppendLine("FROM TFNTCrdSale TXN WITH(NOLOCK)");
                    //oSql.AppendLine("	INNER JOIN TFNMCard CRD WITH(NOLOCK) ON TXN.FTCrdCode = CRD.FTCrdCode");
                    //oSql.AppendLine("	LEFT JOIN TFNMCard_L CRDL WITH(NOLOCK) ON CRD.FTCrdCode = CRDL.FTCrdCode ");
                    //oSql.AppendLine("	AND CRDL.FNLngID = " + poPara.pnLngID);
                    //oSql.AppendLine("WHERE TXN.FTTxnDocNoRef = '" + poPara.ptDocNo + "'");
                    //oSql.AppendLine("AND TXN.FTTxnPosCode = '" + poPara.ptPosCode + "'");
                    //oSql.AppendLine("AND TXN.FTBchCode = '" + poPara.ptBchCode + "'");
                    //oSql.AppendLine("AND TXN.FTTxnDocType = '" + poPara.ptDocType + "'");

                    //if (string.Equals(poPara.ptDocType , "3"))
                    //    oSql.AppendLine("AND ISNULL(TXN.FTTxnStaCancel, '') = ''");

                    //oSql.AppendLine("ORDER BY TXN.FDTxnDocDate ASC");

                    try
                    {
                        //  oResult.raoItems = oDatabase.C_DATaSqlQuery<cmlResCheckPayTxn>(oSql.ToString(), nCmdTme);
                        oDbTbl = new DataTable();
                        aoSqlParam = new SqlParameter[] {
                             new SqlParameter ("@ptDocNo", SqlDbType.NVarChar, 30){ Value = poPara.ptDocNo },
                             new SqlParameter ("@ptPosCode", SqlDbType.NVarChar, 3){ Value = poPara.ptPosCode },
                             new SqlParameter ("@ptBchCode", SqlDbType.NVarChar, 5){ Value = poPara.ptBchCode },
                             new SqlParameter ("@pnLngID", SqlDbType.Int){ Value = poPara.pnLngID },
                             new SqlParameter ("@ptDocType", SqlDbType.NVarChar, 1){ Value =  poPara.ptDocType },
                             new SqlParameter ("@pdSysDate", SqlDbType.DateTime) { Value = poPara.pdSysDate },  //*[AnUBiS][][2019-06-14] - add condition system date.
                             new SqlParameter ("@pnPrevious", SqlDbType.Int) {Value = poPara.pnPrevious}        //*[AnUBiS][][2019-06-14] - add condition previous system date.
                        };

                        oDbTbl = oDatabase.C_GEToQueryStoreDataTbl("STP_SEToCheckPayTxn", aoSqlParam);
                        if (oDbTbl.Rows.Count > 0)
                        {
                            var oItem = from DataRow oRow in oDbTbl.Rows
                                        select new cmlResCheckPayTxn()
                                        {
                                            rnTxnID = (long)oRow["rnTxnID"],
                                            rtTxnDocNoRef = (string)oRow["rtTxnDocNoRef"],
                                            rdTxnDocDate = oRow["rdTxnDocDate"] == DBNull.Value ? (DateTime?)null : (DateTime?)oRow["rdTxnDocDate"],
                                            rtCrdCode = (string)oRow["rtCrdCode"],
                                            rtCrdName = (string)oRow["rtCrdName"],
                                            rcTxnCrdValue = (decimal)oRow["rcTxnCrdValue"],
                                            rcTxnValue = (decimal)oRow["rcTxnValue"],
                                            rnTxnIDRef = (long)oRow["rnTxnIDRef"],
                                            rtCrdHolderID = (string)oRow["rtCrdHolderID"],
                                        };
                            oResult.raoItems = oItem.ToList();
                        }
                    }
                    catch (EntityException oEtyExn)
                    {
                        switch (oEtyExn.HResult)
                        {
                            case -2146232060:
                                // cannot connect database..
                                oResult.rtCode = oMsg.tMS_RespCode905;
                                oResult.rtDesc = oMsg.tMS_RespDesc905;
                                return oResult;
                        }
                    }

                    if (oResult.raoItems == null || oResult.raoItems.Count == 0)
                    {
                        // data not found.
                        oResult.rtCode = oMsg.tMS_RespCode800;
                        oResult.rtDesc = oMsg.tMS_RespDesc800;
                        return oResult;
                    }
                    else
                    {
                        // success.
                        oResult.rtCode = oMsg.tMS_RespCode1;
                        oResult.rtDesc = oMsg.tMS_RespDesc1;
                        return oResult;
                    }
                }
                else
                {
                    // validate parameter model false.
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception oExn)
            { 
                // return error.
                oResult = new cmlResResult<cmlResCheckPayTxn>();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                oDatabase = null;
                oResult = null;
                aoSysConfig = null;
                tFuncName = null;
                tModelErr = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

    }
}
