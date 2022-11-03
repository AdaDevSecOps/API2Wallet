using API2Wallet.Class;
using API2Wallet.Class.ResetExpired;
using API2Wallet.Class.Standard;
using API2Wallet.Class.Topup;
using API2Wallet.Models;
using API2Wallet.Models.WebService.Request.Topup;
using API2Wallet.Models.WebService.Response.Topup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data.Entity.Core;

namespace API2Wallet.Controllers
{
    /// <summary>
    /// Topup information.
    /// </summary>
    [Route(cCS.tCS_APIVer + "/Topup")]
    public class cTopupController : ControllerBase
    {

        /// <summary>
        /// เติมเงิน Topup
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns>
        ///     System process status.<br/>
        ///&#8195;     1   : success.<br/>
        ///&#8195;     701 : validate parameter model false.<br/>
        ///&#8195;     713 : Card Date expired.<br/>
        ///&#8195;     716 : ResetExpire card unsuccess.<br/>
        ///&#8195;     800 : data not found.<br/>
        ///&#8195;     802 : formate data incorrect..<br/>
        ///&#8195;     900 : service process false.<br/>
        ///&#8195;     904 : key not allowed to use method.<br/>
        ///&#8195;     905 : cannot connect database.<br/>
        ///&#8195;     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("Topup")]
        [HttpPost]
        public cmlResTopup POST_PUNoInsTopup([FromBody] cmlReqTopup poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cTopup oTopup;
            cmlResTopup oResult;
            string tFuncName, tModelErr, tErrCode, tErrDesc;
            bool bVerifyPara;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oResult = new cmlResTopup();
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
                    oTopup = new cTopup();
                    bVerifyPara = oTopup.C_DATbProcTopup(poPara, out tErrCode, out tErrDesc, out oResult);
                    if (bVerifyPara == true)
                    {
                        return oResult;
                    }
                    else
                    {
                        // Varify parameter value false.
                        oResult = new cmlResTopup();
                        oResult.rtCode = tErrCode;
                        oResult.rtDesc = tErrDesc;
                        return oResult;
                    }
                }
                else
                {
                    // Validate parameter model false.
                    oResult = new cmlResTopup();
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception oEx)
            {
                // Return error.
                oResult = new cmlResTopup();
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
                oTopup = null;
                oResult = null;
            }
        }

        /// <summary>
        /// ยกเลิกเติมเงิน CancelTopup
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns>
        ///     System process status.<br/>
        ///&#8195;     1   : success.<br/>
        ///&#8195;     701 : validate parameter model false.<br/>
        ///&#8195;     713 : Card Date expired.<br/>
        ///&#8195;     716 : ResetExpire card unsuccess.<br/>
        ///&#8195;     800 : data not found.<br/>
        ///&#8195;     802 : formate data incorrect.<br/>
        ///&#8195;     814 : Not found data TFNTCrdTopUp.<br/>
        ///&#8195;     815 : Not enough balance to cancel.<br/>
        ///&#8195;     816 : Record is canceled.<br/>
        ///&#8195;     900 : service process false.<br/>
        ///&#8195;     904 : key not allowed to use method.<br/>
        ///&#8195;     905 : cannot connect database.<br/>
        ///&#8195;     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("CancelTopup")]
        [HttpPost]
        public cmlResCancelTopup POST_PUNoInsCancelTopup([FromBody] cmlReqCancelTopup poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cTopup oTopup;
            cmlResCancelTopup oResult;
            string tFuncName, tModelErr, tErrCode, tErrDesc;
            bool bVerifyPara;
            cmlTFNTCrdTopup oCrdTopup = new cmlTFNTCrdTopup();
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResCancelTopup();
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
                    oTopup = new cTopup();
                    bVerifyPara = oTopup.C_DATbProcCancelTopup(poPara, out tErrCode, out tErrDesc, out oResult);
                    if (bVerifyPara == true)
                    {
                        return oResult;
                    }
                    else
                    {
                        // Varify parameter value false.
                        oResult = new cmlResCancelTopup();
                        oResult.rtCode = tErrCode;
                        oResult.rtDesc = tErrDesc;
                        return oResult;
                    }
                }
                else
                {
                    // Validate parameter model false.
                    oResult = new cmlResCancelTopup();
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception oEx)
            {
                // Return error.
                oResult = new cmlResCancelTopup();
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
                oTopup = null;
                oResult = null;
               
            }
        }

        /// <summary>
        /// แลกคืน//ResetAvailable
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns>
        ///     System process status.<br/>
        ///&#8195;     1   : success.<br/>
        ///&#8195;     701 : validate parameter model false.<br/>
        ///&#8195;     713 : Card Date expired<br/>
        ///&#8195;     716 : ResetExpire card unsuccess.<br/>
        ///&#8195;     800 : data not found.<br/>
        ///&#8195;     802 : formate data incorrect..<br/>
        ///&#8195;     900 : service process false.<br/>
        ///&#8195;     904 : key not allowed to use method.<br/>
        ///&#8195;     905 : cannot connect database.<br/>
        ///&#8195;     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("ResetAvailable")]
        [HttpPost]
        public cmlResResetAvb POST_PUNoResetAvailableTopup([FromBody] cmlReqResetAvi poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            cTopup oTopup;
            cmlResResetAvb oResTopupErr;
            cmlResResetAvb oResult;
            cmlResResetAvb oResTopup;
            int nRowEff;
            string tFuncName, tModelErr, tErrCode, tErrDesc;
            bool bVerifyPara;
            cmlTFNMCard oCard = new cmlTFNMCard();
            SqlParameter[] aoSqlParam;
            DataTable oDbTbl;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResult = new cmlResResetAvb();
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
                    oTopup = new cTopup();
                    bVerifyPara = oTopup.C_DATbVerifyResetAvb(poPara, out tErrCode, out tErrDesc, out oResTopupErr);
                    if (bVerifyPara == true)
                    {

                        oSql = new StringBuilder();

                        // Get value TFNMCard
                        //oSql.Clear();
                        //oSql.AppendLine("SELECT ISNULL(FCCrdValue,0) AS FCCrdValue");
                        //oSql.AppendLine(",ISNULL(FCCrdDeposit,0) AS FCCrdDeposit"); 
                        //oSql.AppendLine(" FROM TFNMCard WITH (NOLOCK)");
                        //oSql.AppendLine("WHERE FTCrdCode='" + poPara.ptCrdCode + "'");
                        //   oCard = oDatabase.C_DAToSqlQuery<cmlTFNMCard>(oSql.ToString(), nCmdTme);
                        oCard = oFunc.SP_GEToCardByStored(poPara.ptCrdCode, 0);

                        //  cTotalRtn = Convert.ToDecimal(oCard.FCCrdValue - oCard.FCCrdDeposit);

                       // oSql.Clear();
                       // oSql.AppendLine("BEGIN TRANSACTION ");
                       // oSql.AppendLine("  SAVE TRANSACTION Topup ");
                       // oSql.AppendLine("  BEGIN TRY ");

                       // // Insert transection Sale
                       // oSql.AppendLine("     INSERT INTO TFNTCrdTopUp WITH(ROWLOCK)");
                       // oSql.AppendLine("     (");
                       // oSql.AppendLine("	  FTBchCode,FTTxnDocType,FTCrdCode,");
                       // oSql.AppendLine("	  FDTxnDocDate,FTBchCodeRef,FCTxnValue,");
                       // oSql.AppendLine("	  FTTxnStaPrc,FTTxnPosCode");
                       // oSql.AppendLine("     ,FTTxnStaOffLine");    //*Em 61-12-10  Pandora
                       // oSql.AppendLine("     )");
                       // oSql.AppendLine("     VALUES");
                       // oSql.AppendLine("     (");
                       // oSql.AppendLine("	  '" + poPara.ptBchCode + "','5','" + poPara.ptCrdCode + "',");
                       // oSql.AppendLine("	  GETDATE(),'" + poPara.ptBchCode + "','" + oCard.FCCrdValue + "',");
                       // oSql.AppendLine("	  '1','" + poPara.ptTxnPosCode + "'");
                       // oSql.AppendLine("     ,'0'");    //*Em 61-12-10  Pandora
                       // oSql.AppendLine("     )");

                       // // Update Master Card
                       // oSql.AppendLine("     UPDATE TFNMCard WITH (ROWLOCK) SET ");
                       // oSql.AppendLine("     FCCrdValue='0',");
                       // oSql.AppendLine("     FCCrdDeposit='0',");
                       // oSql.AppendLine("     FDCrdResetDate=GETDATE() ");
                       //// oSql.AppendLine("     FDDateUpd=CONVERT(VARCHAR(10), GETDATE(), 121),");
                       // //oSql.AppendLine("     FTTimeUpd=CONVERT(VARCHAR(8),GETDATE(),114),");
                       // //oSql.AppendLine("     FTWhoUpd='AdaWellet'");
                       // oSql.AppendLine("     WHERE FTCrdCode='" + poPara.ptCrdCode + "'");
                       // oSql.AppendLine("     COMMIT TRANSACTION Topup");
                       // oSql.AppendLine("  END TRY");

                       // oSql.AppendLine("  BEGIN CATCH");
                       // oSql.AppendLine("   ROLLBACK TRANSACTION Topup");
                       // oSql.AppendLine("  END CATCH");

                        try
                        {
                            // Confuguration database.
                            //nConTme = 0;
                            //oFunc.SP_DATxGetConfigurationFromMem<int>(ref nConTme, cCS.nCS_ConTme, aoSysConfig, "002");
                            //nCmdTme = 0;
                            //oFunc.SP_DATxGetConfigurationFromMem<int>(ref nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "003");
                            //oDatabase = new cDatabase(nConTme);
                            //nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);

                            oDatabase = new cDatabase();
                            aoSqlParam = new SqlParameter[] {
                                new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 5){ Value = poPara.ptBchCode },
                                new SqlParameter ("@ptCrdCode", SqlDbType.VarChar, 20){ Value = poPara.ptCrdCode },
                                new SqlParameter ("@pcFCCrdValue", SqlDbType.Decimal){ Value = oCard.FCCrdValue },
                                new SqlParameter ("@ptTxnPosCode", SqlDbType.VarChar, 3){ Value = poPara.ptTxnPosCode }
                            };

                            nRowEff = oDatabase.C_GETnExecuteSqlStored("STP_PRCnAvailableTopup", aoSqlParam);
                            //if (nRowEff == 0)
                            if (nRowEff == 0)
                            {
                                oResult.rtCode = oMsg.tMS_RespCode900;
                                oResult.rtDesc = oMsg.tMS_RespDesc900;
                                return oResult;
                            }
                        }
                        catch (EntityException oEtyExn)
                        {
                            switch (oEtyExn.HResult)
                            {
                                case -2146232060:
                                    // Cannot connect database..
                                    oResult.rtCode = oMsg.tMS_RespCode905;
                                    oResult.rtDesc = oMsg.tMS_RespDesc905;
                                    return oResult;
                            }
                        }

                        oSql = new StringBuilder();
                        oSql.Clear();
                        oSql.AppendLine("SELECT TOP 1 ");
                        oSql.AppendLine("ISNULL(C.FCCrdValue,0) AS rcTxnValue");
                        oSql.AppendLine(",ISNULL(C.FCCrdDeposit,0.00) AS rcCrdDeposit");
                        oSql.AppendLine(",ISNULL(C.FCCrdDepositPdt,0.00) AS rcCrdDepositPdt");
                        oSql.AppendLine(",ISNULL(ISNULL(C.FCCrdValue,0.00) - ISNULL(C.FCCrdDepositPdt,0.00),0.00) AS rcTxnValueRtn");
                        //oSql.AppendLine(",T.FCCtyDeposit AS 'rcCtyDeposit'");
                        //oSql.AppendLine(",(C.FCCrdValue-T.FCCtyDeposit) AS 'rcTxnValueAvb'");
                        oSql.AppendLine(",TFNMCardType_L.FTCtyName AS rtCtyName");
                        oSql.AppendLine(",C.FDCrdExpireDate AS 'rdCrdExpireDate'");
                        oSql.AppendLine(",L.FTCrdName AS 'rtCrdName'");
                        oSql.AppendLine("FROM TFNMCard C WITH (NOLOCK)");
                        oSql.AppendLine("INNER JOIN TFNMCardType T WITH (NOLOCK) ON (C.FTCtyCode=T.FTCtyCode)");
                        oSql.AppendLine("INNER JOIN TFNMCard_L L WITH (NOLOCK) ON (C.FTCrdCode=L.FTCrdCode)");
                        oSql.AppendLine("INNER JOIN TFNMCardType_L WITH (NOLOCK) ON T.FTCtyCode= TFNMCardType_L.FTCtyCode");
                        oSql.AppendLine("WHERE C.FTCrdCode='" + poPara.ptCrdCode + "'");

                        //nConTme = 0;
                        //oFunc.SP_DATxGetConfigurationFromMem<int>(ref nConTme, cCS.nCS_ConTme, aoSysConfig, "002");
                        //nCmdTme = 0;
                        //oFunc.SP_DATxGetConfigurationFromMem<int>(ref nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "003");
                        //oDatabase = new cDatabase(nConTme);
                        //oResTopup = oDatabase.C_DAToSqlQuery<cmlResResetAvb>(oSql.ToString(), nCmdTme);

                        oDatabase = new cDatabase();
                        oDbTbl = new DataTable();
                        oResTopup = new cmlResResetAvb();
                        oDbTbl = oDatabase.C_GEToQuerySQLTbl(oSql.ToString());
                        if (oDbTbl.Rows.Count > 0)
                        {
                            var oItem = from DataRow oRow in oDbTbl.Rows
                                        select new cmlResResetAvb()
                                        {
                                            rcTxnValue = (decimal)oRow["rcTxnValue"],
                                            rcCrdDeposit = (decimal)oRow["rcCrdDeposit"],
                                            rcCrdDepositPdt = (decimal)oRow["rcCrdDepositPdt"],
                                            rcTxnValueRtn = (decimal)oRow["rcTxnValueRtn"],
                                            rtCtyName = (string)oRow["rtCtyName"],
                                            rtCrdName = (string)oRow["rtCrdName"],
                                            rdCrdExpireDate = oRow["rdCrdExpireDate"] == DBNull.Value ? (DateTime?)null : (DateTime?)oRow["rdCrdExpireDate"],
                                        };
                            oResTopup = oItem.FirstOrDefault();
                        }

                        oResult.rtCode = oMsg.tMS_RespCode1;
                        oResult.rtDesc = oMsg.tMS_RespDesc1;
                        oResult.rcTxnValue = oResTopup.rcTxnValue;
                        oResult.rcCrdDeposit = oResTopup.rcCrdDeposit;
                        oResult.rcCrdDepositPdt = oResTopup.rcCrdDepositPdt;
                        oResult.rcTxnValueRtn = oResTopup.rcTxnValueRtn;
                        oResult.rtCtyName = oResTopup.rtCtyName;
                        oResult.rtCrdName = oResTopup.rtCrdName;
                        oResult.rdCrdExpireDate = oResTopup.rdCrdExpireDate;
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
            catch (Exception oEx)
            {
                // Return error.
                oResult = new cmlResResetAvb();
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
                oSql = null;
                oResTopupErr = null;
                oTopup = null;
                oResult = null;
                oResTopup = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// เบิกบัตร//ApvOpenCardManual
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns>
        ///     System process status.<br/>
        ///&#8195;     1   : success.<br/>
        ///&#8195;     701 : validate parameter model false.<br/>
        ///&#8195;     802 : formate data incorrect..<br/>
        ///&#8195;     900 : service process false.<br/>
        ///&#8195;     904 : key not allowed to use method.<br/>
        ///&#8195;     905 : cannot connect database.<br/>
        ///&#8195;     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("ApvOpenCard")]
        [HttpPost]
        public cmlResaoApvOpenCard POST_PUNoApvOpenCard([FromBody] List<cmlReqApvReturnCard> poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            cmlResaoApvOpenCard oResult;
            int nRowEff;
            string tFuncName, tModelErr;
            cmlTFNMCard oCard = new cmlTFNMCard();
            cmlResApvOpenCard oApvOpenCard = new cmlResApvOpenCard();
            List<cmlResApvOpenCard> aoApvOpenCard = new List<cmlResApvOpenCard>();
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(ref tModelErr, ModelState))
                {
                    oResult = new cmlResaoApvOpenCard();
                    oDatabase = new cDatabase();
                    oSql = new StringBuilder();

                    for (int nRow = 0; nRow < poPara.Count; nRow++)
                    {
                        oApvOpenCard = new cmlResApvOpenCard();
                        //oSql.Clear();
                        //oSql.AppendLine("SELECT ISNULL(C.FTCrdStaLocate,2) AS FTCrdStaLocate,C.FTCrdStaType ");
                        //oSql.AppendLine("FROM TFNMCard C WITH(NOLOCK) ");
                        //oSql.AppendLine("LEFT JOIN TFNMCardType T WITH(NOLOCK) ON (C.FTCtyCode=T.FTCtyCode) ");
                        //oSql.AppendLine("WHERE FTCrdCode='" + poPara[nRow].ptCrdCode + "'");

                        //oCard = new cmlTFNMCard();
                        //oCard = oDatabase.C_DAToSqlQuery<cmlTFNMCard>(oSql.ToString(), nCmdTme);

                        oCard = new cmlTFNMCard();
                        oCard = oFunc.SP_GEToCardByStored(poPara[nRow].ptCrdCode, 0);

                        if (oCard == null)
                        {
                            // ไม่พบข้อมุลบัตร
                            oApvOpenCard.rtBchCode = poPara[nRow].ptBchCode;
                            oApvOpenCard.rtCrdCode = poPara[nRow].ptCrdCode;
                            oApvOpenCard.rtStatus = "4";
                            aoApvOpenCard.Add(oApvOpenCard);
                            continue;
                        }

                        if (oCard.FTCrdStaType != "1") {
                            // ประเภทบัตรไม่ใช่แบบปกติ
                            oApvOpenCard.rtBchCode = poPara[nRow].ptBchCode;
                            oApvOpenCard.rtCrdCode = poPara[nRow].ptCrdCode;
                            oApvOpenCard.rtStatus = "5";
                            aoApvOpenCard.Add(oApvOpenCard);
                            continue;
                        }

                        //if (oCard.FTCrdStaLocate == "1")
                        //if (oCard.FTCrdStaShift == "1" && oCard.FTCrdStaType == "1")    //*Em 61-12-12  Pandora
                        if (oCard.FTCrdStaShift == "2" && oCard.FTCrdStaType == "1")    //*Em 62-01-12  Pandora StaShift 1:ยังไม่ได้เบิก 2:เบิกแล้ว
                        {
                            oApvOpenCard.rtBchCode = poPara[nRow].ptBchCode;
                            oApvOpenCard.rtCrdCode = poPara[nRow].ptCrdCode;
                            oApvOpenCard.rtStatus = "3";
                            aoApvOpenCard.Add(oApvOpenCard);
                            continue;
                        }

                        //Update สถานะบัตร
                        oSql.Clear();
                        //oSql.AppendLine("UPDATE TFNMCard WITH (ROWLOCK) SET FTCrdStaLocate='1' ");
                        oSql.AppendLine("UPDATE TFNMCard WITH (ROWLOCK) SET FTCrdStaShift='2' ");   //*Em 62-01-14  Pandora StaShift 1:ยังไม่ได้เบิก 2:เบิกแล้ว
                        //oSql.AppendLine("     FDDateUpd=CONVERT(VARCHAR(10), GETDATE(), 121),");
                        //oSql.AppendLine("     FTTimeUpd=CONVERT(VARCHAR(8),GETDATE(),114),");
                        //oSql.AppendLine("     FTWhoUpd='AdaWellet'");
                        oSql.AppendLine("WHERE FTCrdCode='" + poPara[nRow].ptCrdCode + "'");
                        try
                        {
                            // Confuguration database.
                            //nConTme = 0;
                            //oFunc.SP_DATxGetConfigurationFromMem<int>(ref nConTme, cCS.nCS_ConTme, aoSysConfig, "002");
                            //nCmdTme = 0;
                            //oFunc.SP_DATxGetConfigurationFromMem<int>(ref nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "003");
                            //oDatabase = new cDatabase(nConTme);
                            //nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);
                            nRowEff = oDatabase.C_GETnQuerySQL(oSql.ToString());
                            if (nRowEff == 0)
                            {
                                oApvOpenCard.rtBchCode = poPara[nRow].ptBchCode;
                                oApvOpenCard.rtCrdCode = poPara[nRow].ptCrdCode;
                                oApvOpenCard.rtStatus = "2";
                                aoApvOpenCard.Add(oApvOpenCard);
                            }
                            else
                            { 
                                oApvOpenCard.rtBchCode = poPara[nRow].ptBchCode;
                                oApvOpenCard.rtCrdCode = poPara[nRow].ptCrdCode;
                                oApvOpenCard.rtStatus = "1";
                                aoApvOpenCard.Add(oApvOpenCard);
                            }
                        }
                        catch (EntityException oEtyExn)
                        {
                            switch (oEtyExn.HResult)
                            {
                                case -2146232060:
                                    // Cannot connect database..
                                    oResult = new cmlResaoApvOpenCard();
                                    oResult.rtCode = oMsg.tMS_RespCode905;
                                    oResult.rtDesc = oMsg.tMS_RespDesc905;
                                    return oResult;
                            }
                        }
                    }

                    // จบการทำงาน
                    oResult = new cmlResaoApvOpenCard();
                    oResult.rtCode = oMsg.tMS_RespCode1;
                    oResult.rtDesc = oMsg.tMS_RespDesc1;
                    oResult.roApvOpenCard = aoApvOpenCard;
                    return oResult;
                }
                else
                {
                    // Validate parameter model false.
                    oResult = new cmlResaoApvOpenCard();
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception oEx)
            {
                // Return error.
                oResult = new cmlResaoApvOpenCard();
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
                oSql = null;
                oResult = null;
                oCard = null;
                oApvOpenCard = null;
                aoApvOpenCard = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// คืนบัตร//ApvReturnCard
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns>
        ///     System process status.<br/>
        ///&#8195;     1   : success.<br/>
        ///&#8195;     701 : validate parameter model false.<br/>
        ///&#8195;     802 : formate data incorrect..<br/>
        ///&#8195;     900 : service process false.<br/>
        ///&#8195;     904 : key not allowed to use method.<br/>
        ///&#8195;     905 : cannot connect database.<br/>
        ///&#8195;     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("ApvReturnCard")]
        [HttpPost]
        public cmlResaoApvReturnCard POST_PUNoApvReturnCard([FromBody] List<cmlReqApvReturnCard> poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            cmlResaoApvReturnCard oResult;
            int nRowEff;
            string tFuncName, tModelErr;
            cmlTFNMCard oCard = new cmlTFNMCard();
            cmlResApvReturnCard oApvOpenCard = new cmlResApvReturnCard();
            List<cmlResApvReturnCard> aoApvOpenCard = new List<cmlResApvReturnCard>();
            bool bResetExp = true;
            cResetExpired oResetExpired = new cResetExpired();
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(ref tModelErr, ModelState))
                {
                    oResult = new cmlResaoApvReturnCard();
                    oDatabase = new cDatabase();
                    oSql = new StringBuilder();

                    for (int nRow = 0; nRow < poPara.Count; nRow++)
                    {
                        oApvOpenCard = new cmlResApvReturnCard();
                        //oSql.Clear();
                        //oSql.AppendLine("SELECT ISNULL(C.FTCrdStaShift,1) AS FTCrdStaShift,C.FTCrdStaType,ISNULL(C.FCCrdValue,0) AS FCCrdValue");
                        //oSql.AppendLine("FROM TFNMCard C WITH(NOLOCK) LEFT JOIN TFNMCardType T ON (C.FTCtyCode=T.FTCtyCode) ");
                        //oSql.AppendLine("WHERE FTCrdCode='" + poPara[nRow].ptCrdCode + "'");

                        //oCard = new cmlTFNMCard();
                        //oCard = oDatabase.C_DAToSqlQuery<cmlTFNMCard>(oSql.ToString(), nCmdTme);
                        oCard = new cmlTFNMCard();
                        oCard = oFunc.SP_GEToCardByStored(poPara[nRow].ptCrdCode, 0);

                        if (oCard == null)
                        {
                            // ไม่พบข้อมุลบัตร
                            oApvOpenCard.rtBchCode = poPara[nRow].ptBchCode;
                            oApvOpenCard.rtCrdCode = poPara[nRow].ptCrdCode;
                            oApvOpenCard.rtStatus = "4";
                            aoApvOpenCard.Add(oApvOpenCard);
                            continue;
                        }

                        if (oCard.FTCrdStaType != "1")
                        {
                            // ประเภทบัตรไม่ใช่แบบปกติ
                            oApvOpenCard.rtBchCode = poPara[nRow].ptBchCode;
                            oApvOpenCard.rtCrdCode = poPara[nRow].ptCrdCode;
                            oApvOpenCard.rtStatus = "5";
                            aoApvOpenCard.Add(oApvOpenCard);
                            continue;
                        }

                        //if (oCard.FTCrdStaShift != "2")
                        //if (oCard.FTCrdStaShift != "2" && oCard.FTCrdStaType == "1")    //*Em 61-12-12  Pandora
                        if (oCard.FTCrdStaShift != "2" && oCard.FTCrdStaType == "1")    //*Em 62-01-12  Pandora StaShift 1:ยังไม่ได้เบิก 2:เบิกแล้ว
                        {
                            // ประเภทบัตรไม่ใช่แบบปกติ
                            oApvOpenCard.rtBchCode = poPara[nRow].ptBchCode;
                            oApvOpenCard.rtCrdCode = poPara[nRow].ptCrdCode;
                            oApvOpenCard.rtStatus = "6";
                            aoApvOpenCard.Add(oApvOpenCard);
                            continue;
                        }

                        // ยอดเงินคงเหลือมากกว่า 0
                        if (oCard.FCCrdValue > 0)
                        {
                            bResetExp = true;  // Reset 
                            //bResetExp = oResetExpired.C_SETbResetExpired(poPara[nRow].ptCrdCode, poPara[nRow].ptBchCode, "", aoSysConfig);
                            bResetExp = oResetExpired.C_SETbResetExpired(poPara[nRow].ptCrdCode, poPara[nRow].ptBchCode, "", poPara[nRow].ptDocNoRef);  //*Em 61-12-12  Pandora
                            if (bResetExp == false) {
                                //ไม่สามารถ ResetExpired ได้
                                oApvOpenCard.rtBchCode = poPara[nRow].ptBchCode;
                                oApvOpenCard.rtCrdCode = poPara[nRow].ptCrdCode;
                                oApvOpenCard.rtStatus = "3";
                                aoApvOpenCard.Add(oApvOpenCard);
                                continue;
                            }
                        }

                        //Update สถานะบัตร
                        oSql.Clear();
                        //oSql.AppendLine("UPDATE TFNMCard WITH (ROWLOCK) SET FTCrdStaLocate='2'");
                        oSql.AppendLine("UPDATE TFNMCard WITH (ROWLOCK) SET FTCrdStaShift='1' ");   //*Em 62-01-14  Pandora StaShift 1:ยังไม่ได้เบิก 2:เบิกแล้ว
                        //oSql.AppendLine("     FDDateUpd=CONVERT(VARCHAR(10), GETDATE(), 121),");
                        //oSql.AppendLine("     FTTimeUpd=CONVERT(VARCHAR(8),GETDATE(),114),");
                        //oSql.AppendLine("     FTWhoUpd='AdaWellet'");
                        oSql.AppendLine("WHERE FTCrdCode='" + poPara[nRow].ptCrdCode + "'");
                        try
                        {
                            // Confuguration database.
                            //nConTme = 0;
                            //oFunc.SP_DATxGetConfigurationFromMem<int>(ref nConTme, cCS.nCS_ConTme, aoSysConfig, "002");
                            //nCmdTme = 0;
                            //oFunc.SP_DATxGetConfigurationFromMem<int>(ref nCmdTme, cCS.nCS_CmdTme, aoSysConfig, "003");
                            //oDatabase = new cDatabase(nConTme);
                            //nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);

                            nRowEff = oDatabase.C_GETnQuerySQL(oSql.ToString());
                            if (nRowEff == 0)
                            {
                                oApvOpenCard.rtBchCode = poPara[nRow].ptBchCode;
                                oApvOpenCard.rtCrdCode = poPara[nRow].ptCrdCode;
                                oApvOpenCard.rtStatus = "2";
                                aoApvOpenCard.Add(oApvOpenCard);
                            }
                            else
                            {
                                oApvOpenCard.rtBchCode = poPara[nRow].ptBchCode;
                                oApvOpenCard.rtCrdCode = poPara[nRow].ptCrdCode;
                                oApvOpenCard.rtStatus = "1";
                                aoApvOpenCard.Add(oApvOpenCard);
                            }
                        }
                        catch (EntityException oEtyExn)
                        {
                            switch (oEtyExn.HResult)
                            {
                                case -2146232060:
                                    // Cannot connect database..
                                    oResult = new cmlResaoApvReturnCard();
                                    oResult.rtCode = oMsg.tMS_RespCode905;
                                    oResult.rtDesc = oMsg.tMS_RespDesc905;
                                    return oResult;
                            }
                        }
                    }

                    // จบการทำงาน
                    oResult = new cmlResaoApvReturnCard();
                    oResult.rtCode = oMsg.tMS_RespCode1;
                    oResult.rtDesc = oMsg.tMS_RespDesc1;
                    oResult.roApvOpenCard = aoApvOpenCard;
                    return oResult;
                }
                else
                {
                    // Validate parameter model false.
                    oResult = new cmlResaoApvReturnCard();
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception oEx)
            {
                // Return error.
                oResult = new cmlResaoApvReturnCard();
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
                oSql = null;
                oResult = null;
                oCard = null;
                oApvOpenCard = null;
                aoApvOpenCard = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// ยกเลิกเติมเงินเป็นชุด//ReturnTopupList
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns>
        ///     System process status.<br/>
        ///&#8195;     1   : success.<br/>
        ///&#8195;     701 : validate parameter model false.<br/>
        ///&#8195;     802 : formate data incorrect..<br/>
        ///&#8195;     900 : service process false.<br/>
        ///&#8195;     904 : key not allowed to use method.<br/>
        ///&#8195;     905 : cannot connect database.<br/>
        ///&#8195;     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("ReturnTopupList")]
        [HttpPost]
        public cmlResaoReturnTopupList POST_PUNoReturnTopupList([FromBody] List<cmlReqReturnTopupList> poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            cmlResaoReturnTopupList oResult;
            int nRowEff;
            string tFuncName, tModelErr;
            cmlTFNMCard oCard = new cmlTFNMCard();
            cmlResReturnTopupList oReturnTopupList = new cmlResReturnTopupList();
            List<cmlResReturnTopupList> aoReturnTopupList = new List<cmlResReturnTopupList>();
            cResetExpired oResetExpired;
            SqlParameter[] aoSqlParam;
            decimal cValTopup = 0;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oResetExpired = new cResetExpired();
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();
                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(ref tModelErr, ModelState))
                {
                    oResult = new cmlResaoReturnTopupList();
                    oDatabase = new cDatabase();
                    oSql = new StringBuilder();

                    for (int nRow = 0; nRow < poPara.Count; nRow++)
                    {
                        oReturnTopupList= new cmlResReturnTopupList();

                        #region  Get ข้อมูลบัตร
                        //Get ข้อมูลบัตร
                        oCard = new cmlTFNMCard();
                        //    oCard = oFunc.SP_GEToCard(aoSysConfig, poPara[nRow].ptCrdCode);
                        oCard = oFunc.SP_GEToCardByStored(poPara[nRow].ptCrdCode, 0);
                        #endregion

                        if (oCard == null)
                        {
                            // ไม่พบข้อมุลบัตร
                            oReturnTopupList.rtBchCode = poPara[nRow].ptBchCode;
                            oReturnTopupList.rtCrdCode = poPara[nRow].ptCrdCode;
                            oReturnTopupList.rtStatus = "4";
                            aoReturnTopupList.Add(oReturnTopupList);
                            continue;
                        }

                        //if (oCard.FTCrdStaLocate != "1")
                        //if (oCard.FTCrdStaShift != "1" && oCard.FTCrdStaType == "1")    //*Em 61-12-11  Pandora
                        if (oCard.FTCrdStaShift != "2" && oCard.FTCrdStaType == "1")    //*Em 62-01-12  Pandora StaShift 1:ยังไม่ได้เบิก 2:เบิกแล้ว
                        {
                            oReturnTopupList.rtBchCode = poPara[nRow].ptBchCode;
                            oReturnTopupList.rtCrdCode = poPara[nRow].ptCrdCode;
                            oReturnTopupList.rtStatus = "3";
                            aoReturnTopupList.Add(oReturnTopupList);
                            continue;
                        }

                        //bResultReset = oResetExpired.C_SETbResetExpired(poPara[nRow].ptCrdCode, poPara[nRow].ptBchCode, "", aoSysConfig);
                        //if (bResultReset == false)
                        //{
                        //    oReturnTopupList.rtBchCode = poPara[nRow].ptBchCode;
                        //    oReturnTopupList.rtCrdCode = poPara[nRow].ptCrdCode;
                        //    oReturnTopupList.rtStatus = "5";
                        //    aoReturnTopupList.Add(oReturnTopupList);
                        //    continue;
                        //}

                        ////*Em 61-12-11  Pandora
                        //if (oCard.FCCrdValue != 0)
                        //{
                        //    bResultReset = oResetExpired.C_SETbResetExpired(poPara[nRow].ptCrdCode, poPara[nRow].ptBchCode, "", poPara[nRow].ptDocNoRef ,aoSysConfig);
                        //    if (bResultReset == false)
                        //    {
                        //        oReturnTopupList.rtBchCode = poPara[nRow].ptBchCode;
                        //        oReturnTopupList.rtCrdCode = poPara[nRow].ptCrdCode;
                        //        oReturnTopupList.rtStatus = "5";
                        //        aoReturnTopupList.Add(oReturnTopupList);
                        //        continue;
                        //    }
                        //}
                        ////++++++++++++++++++++++

                        ////Update สถานะบัตร
                        //oSql.Clear();
                        ////oSql.AppendLine("UPDATE TFNMCard WITH (ROWLOCK) SET FTCrdStaLocate='2' ");
                        ////oSql.AppendLine("UPDATE TFNMCard WITH (ROWLOCK) SET FTCrdStaShift='2' "); // 1:เบิกแล้ว 2:ยังไม่ได้เบิก
                        //oSql.AppendLine("UPDATE TFNMCard WITH (ROWLOCK) SET FTCrdStaShift='1' "); // 1:ยังไม่ได้เบิก 2:เบิกแล้ว       //*Em 62-01-12  Pandora
                        //oSql.AppendLine("WHERE FTCrdCode='" + poPara[nRow].ptCrdCode + "'");

                        ////*Em 62-01-16  Pandora
                        //oSql.Clear();
                        //oSql.AppendLine("BEGIN TRANSACTION ");
                        //oSql.AppendLine("  SAVE TRANSACTION Topup ");
                        //oSql.AppendLine("  BEGIN TRY ");


                        //[pong][08-06-2019][commect code]
                        //if (poPara[nRow].ptAuto == "0")
                        //{
                        //    // เติมเงินแบบ Manual
                        //    // Insert transection Sale
                        //    oSql.AppendLine("     INSERT INTO TFNTCrdTopUp WITH(ROWLOCK)");
                        //    oSql.AppendLine("     (");
                        //    oSql.AppendLine("	  FTBchCode,FTTxnDocType,FTCrdCode,");
                        //    oSql.AppendLine("	  FDTxnDocDate,FTBchCodeRef,FCTxnValue,");
                        //    oSql.AppendLine("	  FTTxnStaPrc,FTTxnPosCode,FCTxnCrdValue,FTShpCode");
                        //    oSql.AppendLine("     ,FTTxnStaOffLine");    //*Em 61-12-04  Pandora
                        //    oSql.AppendLine("     ,FTTxnDocNoRef");    //*Em 61-12-10  Pandora
                        //    oSql.AppendLine("     )");
                        //    oSql.AppendLine("     VALUES");
                        //    oSql.AppendLine("     (");
                        //    oSql.AppendLine("	  '" + poPara[nRow].ptBchCode + "','2','" + poPara[nRow].ptCrdCode + "',");
                        //    oSql.AppendLine("	  GETDATE(),'" + poPara[nRow].ptBchCode + "','" + poPara[nRow].pcTxnValue + "',");
                        //    oSql.AppendLine("	  '1','" + poPara[nRow].ptTxnPosCode + "'," + oCard.FCCrdValue + ",'" + poPara[nRow].ptShpCode + "'");
                        //    oSql.AppendLine("     ,'0'");    //*Em 61-12-04  Pandora
                        //    oSql.AppendLine("     ,'" + poPara[nRow].ptDocNoRef + "'");    //*Em 61-12-10  Pandora
                        //    oSql.AppendLine("     )");

                        //    // Update Master Card
                        //    oSql.AppendLine("     UPDATE TFNMCard WITH (ROWLOCK) SET ");
                        //    oSql.AppendLine("     FCCrdValue=(ISNULL(FCCrdValue,0) - " + poPara[nRow].pcTxnValue + "),");
                        //    oSql.AppendLine("     FDCrdLastTopup=GETDATE() ");
                        //    oSql.AppendLine("     WHERE FTCrdCode='" + poPara[nRow].ptCrdCode + "'");
                        //    oSql.AppendLine("     COMMIT TRANSACTION Topup");
                        //    oSql.AppendLine("  END TRY");

                        //}
                        //else if (poPara[nRow].ptAuto == "1")
                        //{
                        //    // เติมเงินแบบ Auto
                        //    // Insert transection Sale
                        //    oSql.AppendLine("     INSERT INTO TFNTCrdTopUp WITH(ROWLOCK)");
                        //    oSql.AppendLine("     (");
                        //    oSql.AppendLine("	  FTBchCode,FTTxnDocType,FTCrdCode,");
                        //    oSql.AppendLine("	  FDTxnDocDate,FTBchCodeRef,FCTxnValue,");
                        //    oSql.AppendLine("	  FTTxnStaPrc,FTTxnPosCode,FCTxnCrdValue,FTShpCode");
                        //    oSql.AppendLine("     ,FTTxnStaOffLine");    //*Em 61-12-04  Pandora
                        //    oSql.AppendLine("     ,FTTxnDocNoRef");    //*Em 61-12-10  Pandora
                        //    oSql.AppendLine("     )");
                        //    oSql.AppendLine("     VALUES");
                        //    oSql.AppendLine("     (");
                        //    oSql.AppendLine("	  '" + poPara[nRow].ptBchCode + "','2','" + poPara[nRow].ptCrdCode + "',");
                        //    oSql.AppendLine("	  GETDATE(),'" + poPara[nRow].ptBchCode + "','" + oCard.FCCtyTopupAuto + "',");
                        //    oSql.AppendLine("	  '1','" + poPara[nRow].ptTxnPosCode + "'," + oCard.FCCrdValue + ",'" + poPara[nRow].ptShpCode + "'");
                        //    oSql.AppendLine("     ,'0'");    //*Em 61-12-04  Pandora
                        //    oSql.AppendLine("     ,'" + poPara[nRow].ptDocNoRef + "'");    //*Em 61-12-10  Pandora
                        //    oSql.AppendLine("     )");

                        //    // Update Master Card
                        //    oSql.AppendLine("     UPDATE TFNMCard WITH (ROWLOCK) SET ");
                        //    oSql.AppendLine("     FCCrdValue=(ISNULL(FCCrdValue,0) - " + oCard.FCCtyTopupAuto + "),");
                        //    oSql.AppendLine("     FDCrdLastTopup=GETDATE() ");
                        //    oSql.AppendLine("     WHERE FTCrdCode='" + poPara[nRow].ptCrdCode + "'");
                        //    oSql.AppendLine("     COMMIT TRANSACTION Topup");
                        //    oSql.AppendLine("  END TRY");

                        //}

                        //oSql.AppendLine("  BEGIN CATCH");
                        //oSql.AppendLine("   ROLLBACK TRANSACTION Topup");
                        //oSql.AppendLine("  END CATCH");
                        ////++++++++++++++++++++++

                        if (poPara[nRow].ptAuto == "0")
                        {
                            // เติมเงินแบบ Manual
                            cValTopup = poPara[nRow].pcTxnValue;
                        }
                        else if (poPara[nRow].ptAuto == "1") { 
                            // เติมเงินแบบ Auto
                            cValTopup = oCard.FCCtyTopupAuto;
                        }

                        try
                            {
                            // Confuguration database.
                            // nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);

                            aoSqlParam = new SqlParameter[] {
                                new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 5){ Value = poPara[nRow].ptBchCode },
                                new SqlParameter ("@ptCrdCode", SqlDbType.VarChar, 20){ Value = poPara[nRow].ptCrdCode },
                                new SqlParameter ("@pcTxnValue", SqlDbType.Decimal){ Value = cValTopup },
                                new SqlParameter ("@ptTxnPosCode", SqlDbType.VarChar, 3){ Value = poPara[nRow].ptTxnPosCode },
                                new SqlParameter ("@pcCrdValue", SqlDbType.Decimal){ Value = oCard.FCCrdValue },
                                new SqlParameter ("@ptShpCode", SqlDbType.VarChar, 5){ Value = poPara[nRow].ptShpCode },
                                new SqlParameter ("@ptDocNoRef", SqlDbType.VarChar, 30){ Value = poPara[nRow].ptDocNoRef }
                            };
                            nRowEff = oDatabase.C_GETnExecuteSqlStored("STP_PRCnReturnTopupList", aoSqlParam);

                            if (nRowEff == 0)
                            {
                                oReturnTopupList.rtBchCode = poPara[nRow].ptBchCode;
                                oReturnTopupList.rtCrdCode = poPara[nRow].ptCrdCode;
                                oReturnTopupList.rtStatus = "2";
                                aoReturnTopupList.Add(oReturnTopupList);
                            }
                            else
                            {
                                oReturnTopupList.rtBchCode = poPara[nRow].ptBchCode;
                                oReturnTopupList.rtCrdCode = poPara[nRow].ptCrdCode;
                                oReturnTopupList.rtStatus = "1";
                                aoReturnTopupList.Add(oReturnTopupList);
                            }
                        }
                        catch (EntityException oEtyExn)
                        {
                            switch (oEtyExn.HResult)
                            {
                                case -2146232060:
                                    // Cannot connect database..
                                    oResult = new cmlResaoReturnTopupList();
                                    oResult.rtCode = oMsg.tMS_RespCode905;
                                    oResult.rtDesc = oMsg.tMS_RespDesc905;
                                    return oResult;
                            }
                        }
                    }

                    // จบการทำงาน
                    oResult = new cmlResaoReturnTopupList();
                    oResult.rtCode = oMsg.tMS_RespCode1;
                    oResult.rtDesc = oMsg.tMS_RespDesc1;
                    oResult.roReturnTopupList = aoReturnTopupList;
                    return oResult;
                }
                else
                {
                    // Validate parameter model false.
                    oResult = new cmlResaoReturnTopupList();
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception oEx)
            {
                // Return error.
                oResult = new cmlResaoReturnTopupList();
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
                oSql = null;
                oResult = null;
                oCard = null;
                oReturnTopupList = null;
                aoReturnTopupList = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        ///  เติมเงินเป็นชุด // Topup List
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns>
        ///     System process status.<br/>
        ///&#8195;     1   : success.<br/>
        ///&#8195;     701 : validate parameter model false.<br/>
        ///&#8195;     802 : formate data incorrect..<br/>
        ///&#8195;     900 : service process false.<br/>
        ///&#8195;     904 : key not allowed to use method.<br/>
        ///&#8195;     905 : cannot connect database.<br/>
        ///&#8195;     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("TopupList")]
        [HttpPost]
        public cmlResaoTopupList POST_PUNoTopupList([FromBody] List<cmlReqTopupList> poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            cmlResaoTopupList oResult;
            int nRowEff;
            string tFuncName, tModelErr;
            cmlTFNMCard oCard = new cmlTFNMCard();
            cmlResTopupList oReturnTopupList = new cmlResTopupList();
            List<cmlResTopupList> aoReturnTopupList = new List<cmlResTopupList>();
            string tNow;
            cResetExpired oResetExpired = new cResetExpired();
            DateTime dExpireTopup = DateTime.Now;
            string tExpireTopup;
            bool bResultReset = true;
            cResetExpired oResetExpire = new cResetExpired();
            decimal cTxnValue = 0;
            SqlParameter[] aoSqlParam;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");

                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";
                if (oFunc.SP_CHKbParaModel(ref tModelErr, ModelState))
                {
                    oDatabase = new cDatabase();
                    oSql = new StringBuilder();
                    for (int nRow = 0; nRow < poPara.Count; nRow++)
                    {
                        oReturnTopupList = new cmlResTopupList();
                       
                        #region  Get ข้อมูลบัตร
                        //Get ข้อมูลบัตร
                        oCard = new cmlTFNMCard();
                        //oCard = oFunc.SP_GEToCard(aoSysConfig, poPara[nRow].ptCrdCode);
                        oCard = oFunc.SP_GEToCardByStored(poPara[nRow].ptCrdCode, 0);
                        #endregion

                        if (oCard == null)
                        {
                            // ไม่พบข้อมุลบัตร
                            oReturnTopupList.rtBchCode = poPara[nRow].ptBchCode;
                            oReturnTopupList.rtCrdCode = poPara[nRow].ptCrdCode;
                            oReturnTopupList.rtStatus = "4";
                            aoReturnTopupList.Add(oReturnTopupList);
                            continue;
                        }

                        ////if (oCard.FTCrdStaLocate != "1")
                        //if (oCard.FTCrdStaLocate != "1" && oCard.FTCrdStaType == "1")   //*Em 61-12-10  Pandora
                        //{
                        //    // สถานะบัตรยังไม่ถูกเปิดใช้งาน
                        //    oReturnTopupList.rtBchCode = poPara[nRow].ptBchCode;
                        //    oReturnTopupList.rtCrdCode = poPara[nRow].ptCrdCode;
                        //    oReturnTopupList.rtStatus = "6";
                        //    aoReturnTopupList.Add(oReturnTopupList);
                        //    continue;
                        //}

                        // ตรวจสอบวันหมดอายุรหัสบัตร
                        tNow = DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));
                      //  tCrdExpireDate = oCard.FDCrdExpireDate.ToString("yyyy-MM-dd HH:mm:ss");
                        if ((DateTime.Parse(tNow) > oCard.FDCrdExpireDate)) {
                            oReturnTopupList.rtBchCode = poPara[nRow].ptBchCode;
                            oReturnTopupList.rtCrdCode = poPara[nRow].ptCrdCode;
                            oReturnTopupList.rtStatus = "5";
                            aoReturnTopupList.Add(oReturnTopupList);
                            continue; 
                        }

                        // ตรวจสอบอายุเงิน ถ้า FNCtyExpirePeriod = 0 ไม่ต้องตรวจสอบ
                        if (oCard.FNCtyExpirePeriod != 0)
                        {

                            // ตรวจสอบกรณี ยังไม่เคยเติมเงิน ไม่ต้องตรวจสอบวัน Expire ของยอดเงิน
                            if (oCard.FDCrdLastTopup != null)
                            {
                                switch (oCard.FNCtyExpiredType)
                                {
                                    case 1:
                                        dExpireTopup = Convert.ToDateTime(oCard.FDCrdLastTopup).AddHours(Convert.ToInt32(oCard.FNCtyExpirePeriod));
                                        break;
                                    case 2:
                                        dExpireTopup = Convert.ToDateTime(oCard.FDCrdLastTopup).AddDays(Convert.ToInt32(oCard.FNCtyExpirePeriod));
                                        break;
                                    case 3:
                                        dExpireTopup = Convert.ToDateTime(oCard.FDCrdLastTopup).AddMonths(Convert.ToInt32(oCard.FNCtyExpirePeriod));
                                        break;
                                    case 4:
                                        //*Em 61-12-10  Pandora
                                        if (oCard.FNCtyExpirePeriod == 9999 || (Convert.ToDateTime(oCard.FDCrdLastTopup).Year + oCard.FNCtyExpirePeriod) > 9999)
                                        {
                                            dExpireTopup = new DateTime((int)oCard.FNCtyExpirePeriod, Convert.ToDateTime(oCard.FDCrdLastTopup).Month, Convert.ToDateTime(oCard.FDCrdLastTopup).Day);
                                        }
                                        else
                                        {
                                            dExpireTopup = Convert.ToDateTime(oCard.FDCrdLastTopup).AddYears(Convert.ToInt32(oCard.FNCtyExpirePeriod));
                                        }
                                        //+++++++++++++++++++++
                                        break;
                                }

                                tExpireTopup = dExpireTopup.ToString(@"yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));

                                if (DateTime.Parse(tNow) > DateTime.Parse(tExpireTopup))
                                {
                                    // ยอดเงินหมดอายุ ResetExpire
                                    //bResultReset = oResetExpire.C_SETbResetExpired(poPara[nRow].ptCrdCode, poPara[nRow].ptBchCode,"", aoSysConfig);
                                    bResultReset = oResetExpire.C_SETbResetExpired(poPara[nRow].ptCrdCode, poPara[nRow].ptBchCode, "", poPara[nRow].ptDocNoRef);   //*Em 61-12-12  Pandora
                                    if (bResultReset == false)
                                    {
                                        oReturnTopupList.rtBchCode = poPara[nRow].ptBchCode;
                                        oReturnTopupList.rtCrdCode = poPara[nRow].ptCrdCode;
                                        oReturnTopupList.rtStatus = "3";
                                        aoReturnTopupList.Add(oReturnTopupList);
                                        continue;
                                    }
                                }
                            }

                        }

                        //oSql.Clear();
                        //oSql.AppendLine("BEGIN TRANSACTION ");
                        //oSql.AppendLine("  SAVE TRANSACTION Topup ");
                        //oSql.AppendLine("  BEGIN TRY ");

                        //if (poPara[nRow].ptAuto == "0")
                        //{
                        //    // เติมเงินแบบ Manual
                        //    // Insert transection Sale
                        //    oSql.AppendLine("     INSERT INTO TFNTCrdTopUp WITH(ROWLOCK)");
                        //    oSql.AppendLine("     (");
                        //    oSql.AppendLine("	  FTBchCode,FTTxnDocType,FTCrdCode,");
                        //    oSql.AppendLine("	  FDTxnDocDate,FTBchCodeRef,FCTxnValue,");
                        //    oSql.AppendLine("	  FTTxnStaPrc,FTTxnPosCode,FCTxnCrdValue,FTShpCode");
                        //    oSql.AppendLine("     ,FTTxnStaOffLine");    //*Em 61-12-04  Pandora
                        //    oSql.AppendLine("     ,FTTxnDocNoRef");    //*Em 61-12-10  Pandora
                        //    oSql.AppendLine("     )");
                        //    oSql.AppendLine("     VALUES");
                        //    oSql.AppendLine("     (");
                        //    oSql.AppendLine("	  '" + poPara[nRow].ptBchCode + "','1','" + poPara[nRow].ptCrdCode + "',");
                        //    oSql.AppendLine("	  GETDATE(),'" + poPara[nRow].ptBchCode + "','" + poPara[nRow].pcTxnValue + "',");
                        //    oSql.AppendLine("	  '1','" + poPara[nRow].ptTxnPosCode + "'," + oCard.FCCrdValue+ ",'" + poPara[nRow].ptShpCode + "'");
                        //    oSql.AppendLine("     ,'0'");    //*Em 61-12-04  Pandora
                        //    oSql.AppendLine("     ,'"+ poPara[nRow].ptDocNoRef +"'");    //*Em 61-12-10  Pandora
                        //    oSql.AppendLine("     )");

                        //    // Update Master Card
                        //    oSql.AppendLine("     UPDATE TFNMCard WITH (ROWLOCK) SET ");
                        //    oSql.AppendLine("     FCCrdValue=(ISNULL(FCCrdValue,0) + " + poPara[nRow].pcTxnValue + "),");
                        //    oSql.AppendLine("     FDCrdLastTopup=GETDATE() ");
                        //    oSql.AppendLine("     WHERE FTCrdCode='" + poPara[nRow].ptCrdCode + "'");
                        //    oSql.AppendLine("     COMMIT TRANSACTION Topup");
                        //    oSql.AppendLine("  END TRY");

                        //}
                        //else if(poPara[nRow].ptAuto == "1")
                        //{
                        //    // เติมเงินแบบ Auto
                        //    // Insert transection Sale
                        //    oSql.AppendLine("     INSERT INTO TFNTCrdTopUp WITH(ROWLOCK)");
                        //    oSql.AppendLine("     (");
                        //    oSql.AppendLine("	  FTBchCode,FTTxnDocType,FTCrdCode,");
                        //    oSql.AppendLine("	  FDTxnDocDate,FTBchCodeRef,FCTxnValue,");
                        //    oSql.AppendLine("	  FTTxnStaPrc,FTTxnPosCode,FCTxnCrdValue,FTShpCode");
                        //    oSql.AppendLine("     ,FTTxnStaOffLine");    //*Em 61-12-04  Pandora
                        //    oSql.AppendLine("     ,FTTxnDocNoRef");    //*Em 61-12-10  Pandora
                        //    oSql.AppendLine("     )");
                        //    oSql.AppendLine("     VALUES");
                        //    oSql.AppendLine("     (");
                        //    oSql.AppendLine("	  '" + poPara[nRow].ptBchCode + "','1','" + poPara[nRow].ptCrdCode + "',");
                        //    oSql.AppendLine("	  GETDATE(),'" + poPara[nRow].ptBchCode + "','" + oCard.FCCtyTopupAuto + "',");
                        //    oSql.AppendLine("	  '1','" + poPara[nRow].ptTxnPosCode + "'," + oCard.FCCrdValue + ",'" + poPara[nRow].ptShpCode + "'");
                        //    oSql.AppendLine("     ,'0'");    //*Em 61-12-04  Pandora
                        //    oSql.AppendLine("     ,'" + poPara[nRow].ptDocNoRef + "'");    //*Em 61-12-10  Pandora
                        //    oSql.AppendLine("     )");

                        //    // Update Master Card
                        //    oSql.AppendLine("     UPDATE TFNMCard WITH (ROWLOCK) SET ");
                        //    oSql.AppendLine("     FCCrdValue=(ISNULL(FCCrdValue,0) + " + oCard.FCCtyTopupAuto + "),");
                        //    oSql.AppendLine("     FDCrdLastTopup=GETDATE() ");
                        //    oSql.AppendLine("     WHERE FTCrdCode='" + poPara[nRow].ptCrdCode + "'");
                        //    oSql.AppendLine("     COMMIT TRANSACTION Topup");
                        //    oSql.AppendLine("  END TRY");

                        //}

                        //oSql.AppendLine("  BEGIN CATCH");
                        //oSql.AppendLine("   ROLLBACK TRANSACTION Topup");
                        //oSql.AppendLine("  END CATCH");

                        if (poPara[nRow].ptAuto == "0")
                        {
                            // เติมเงินแบบ Manual
                            cTxnValue = poPara[nRow].pcTxnValue;
                        }
                        else if (poPara[nRow].ptAuto == "1")
                        {
                            // เติมเงินแบบ Auto
                            cTxnValue = oCard.FCCtyTopupAuto;
                        }

                        try
                        {
                            // nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);

                            aoSqlParam = new SqlParameter[] {
                                new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 5){ Value = poPara[nRow].ptBchCode },
                                new SqlParameter ("@ptCrdCode", SqlDbType.VarChar, 20){ Value = poPara[nRow].ptCrdCode },
                                new SqlParameter ("@pcTxnValue", SqlDbType.Decimal){ Value = cTxnValue },
                                new SqlParameter ("@ptTxnPosCode", SqlDbType.VarChar, 3){ Value = poPara[nRow].ptTxnPosCode },
                                new SqlParameter ("@pcCrdValue", SqlDbType.Decimal){ Value = oCard.FCCrdValue },
                                new SqlParameter ("@ptShpCode", SqlDbType.VarChar, 5){ Value = poPara[nRow].ptShpCode },
                                new SqlParameter ("@ptDocNoRef", SqlDbType.VarChar, 30){ Value = poPara[nRow].ptDocNoRef }
                            };
                            nRowEff = oDatabase.C_GETnExecuteSqlStored("STP_PRCnTopupList", aoSqlParam);

                            if (nRowEff == 0)
                            {
                                oReturnTopupList.rtBchCode = poPara[nRow].ptBchCode;
                                oReturnTopupList.rtCrdCode = poPara[nRow].ptCrdCode;
                                oReturnTopupList.rtStatus = "2";
                                aoReturnTopupList.Add(oReturnTopupList);
                            }
                            else
                            {
                                oReturnTopupList.rtBchCode = poPara[nRow].ptBchCode;
                                oReturnTopupList.rtCrdCode = poPara[nRow].ptCrdCode;
                                oReturnTopupList.rtStatus = "1";
                                aoReturnTopupList.Add(oReturnTopupList);
                            }
                        }
                        catch (EntityException oEtyExn)
                        {
                            switch (oEtyExn.HResult)
                            {
                                case -2146232060:
                                    // Cannot connect database..
                                    oResult = new cmlResaoTopupList();
                                    oResult.rtCode = oMsg.tMS_RespCode905;
                                    oResult.rtDesc = oMsg.tMS_RespDesc905;
                                    return oResult;
                            }
                        }
                    }

                    // จบการทำงาน
                    oResult = new cmlResaoTopupList();
                    oResult.rtCode = oMsg.tMS_RespCode1;
                    oResult.rtDesc = oMsg.tMS_RespDesc1;
                    oResult.roTopupList = aoReturnTopupList;
                    return oResult;
                }
                else
                {
                    // Validate parameter model false.
                    oResult = new cmlResaoTopupList();
                    oResult.rtCode = oMsg.tMS_RespCode701;
                    oResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResult;
                }
            }
            catch (Exception oEx)
            {
                // Return error.
                oResult = new cmlResaoTopupList();
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
                oSql = null;
                oResult = null;
                oCard = null;
                oReturnTopupList = null;
                aoReturnTopupList = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}
