using API2Wallet.Class.Standard;
using API2Wallet.Models.WebService.Request.Tranfer;
using API2Wallet.Models.WebService.Response.Tranfer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;

namespace API2Wallet.Class.Tranfer
{
    /// <summary>
    /// Information Class tranfer
    /// </summary>
    public class cTranfer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="poPara"></param>
        /// <returns></returns>
        public cmlResTnfCrd C_PUNoTnfCrd([FromBody] cmlReqTnfCrd poPara)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cmlResTnfCrd oResult;
            string tErrCode, tErrDesc;
            bool bProc;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oResult = new cmlResTnfCrd();
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();

                // Load configuration.
            //    aoSysConfig = oFunc.SP_SYSaLoadConfiguration();

                // Check range time use function.
             //   if (oFunc.SP_CHKbAllowRangeTime(aoSysConfig))
            //    {
                    bProc = C_DATbProcTnfCrd(poPara, out tErrCode, out tErrDesc, out oResult);
                    if (bProc == true)
                    {
                        return oResult;
                    }
                    else
                    {
                        oResult = new cmlResTnfCrd();
                        oResult.rtCode = tErrCode;
                        oResult.rtDesc = tErrDesc;
                        return oResult;
                    }
                //}
                //else
                //{
                //    // This time not allowed to use method.
                //    oResult = new cmlResTnfCrd();
                //    oResult.rtCode = oMsg.tMS_RespCode906;
                //    oResult.rtDesc = oMsg.tMS_RespDesc906;
                //    return oResult;
                //}

            }
            catch (Exception oEx)
            {
                // Return error.
                oResult = new cmlResTnfCrd();
                oResult.rtCode = new cMS().tMS_RespCode900;
                oResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                oResult = null;
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="poPara"></param>
        /// <param name="ptErrCode"></param>
        /// <param name="ptErrDesc"></param>
        /// <param name="poResult"></param>
        /// <returns></returns>
        public bool C_DATbProcTnfCrd(cmlReqTnfCrd poPara,
                     out string ptErrCode, out string ptErrDesc, out cmlResTnfCrd poResult)
        {
            cSP oFunc = new cSP();
            cMS oMsg = new cMS();
            cDatabase oDatabase;
            int nRowEff;
            SqlParameter[] aoSqlParam;
            try
            {
                #region Process
                ////oSql.Clear();
                ////oSql.AppendLine("BEGIN TRANSACTION ");
                ////oSql.AppendLine("  SAVE TRANSACTION TnfCrd ");
                ////oSql.AppendLine("  BEGIN TRY ");

                //////โอนเงินออก --ใช้ จากรหัสบัตร(parameter)
                ////oSql.AppendLine("     INSERT INTO TFNTCrdTopUp WITH(ROWLOCK)");
                ////oSql.AppendLine("     (");
                ////oSql.AppendLine("	  FTBchCode,FTTxnDocType,FTCrdCode,");
                ////oSql.AppendLine("	  FDTxnDocDate,FTBchCodeRef,FCTxnValue,");
                ////oSql.AppendLine("	  FTTxnStaPrc,FTTxnPosCode,FCTxnCrdValue");
                ////oSql.AppendLine("     ,FTTxnStaOffLine");    //*Em 61-12-06  Pandora
                ////oSql.AppendLine("     ,FTTxnDocNoRef");     //*[AnUBiS][][2019-02-13] - บันทึก doc no ref. เพิ่ม (Pandora)
                ////oSql.AppendLine("     )");
                ////oSql.AppendLine("     VALUES");
                ////oSql.AppendLine("     (");
                ////oSql.AppendLine("	  '" + poPara.ptBchCode + "','8','" + poPara.ptFrmCrdCode + "',");
                ////oSql.AppendLine("	  GETDATE(),'" + poPara.ptBchCode + "'," + poPara.pcFrmCrdValue + ",");
                ////oSql.AppendLine("	  '1',''," + poPara.pcFrmCrdValue + "");
                ////oSql.AppendLine("     ,'0'");    //*Em 61-12-06  Pandora
                ////oSql.AppendLine("     ,'" + poPara.ptDocNoRef + "'");   //*[AnUBiS][][2019-02-13] - บันทึก doc no ref. เพิ่ม (Pandora)
                ////oSql.AppendLine("     )");

                ////// Update Master Card จากรหัสบัตร(parameter)
                ////oSql.AppendLine("     UPDATE TFNMCard WITH (ROWLOCK) SET ");
                ////oSql.AppendLine("     FDCrdResetDate=GetDate()");
                ////oSql.AppendLine("     ,FDCrdLastTopup=NULL");
                ////oSql.AppendLine("     ,FCCrdValue=0");
                ////oSql.AppendLine("     ,FCCrdDeposit=0");
                ////oSql.AppendLine("     ,FCCrdDepositPdt=0");
                ////oSql.AppendLine("     ,FNCrdTxnOffline=0");
                ////oSql.AppendLine("     ,FNCrdTxnPrcAdj=0");
                ////oSql.AppendLine("     ,FTCrdStaActive='2'");  //*Em 62-01-09  Pandora
                ////oSql.AppendLine("     WHERE FTCrdCode='" + poPara.ptFrmCrdCode + "'");

                //////โอนเงินเข้า --ใช้ ถึงรหัสบัตร(parameter)
                ////oSql.AppendLine("     INSERT INTO TFNTCrdTopUp WITH(ROWLOCK)");
                ////oSql.AppendLine("     (");
                ////oSql.AppendLine("	  FTBchCode,FTTxnDocType,FTCrdCode,");
                ////oSql.AppendLine("	  FDTxnDocDate,FTBchCodeRef,FCTxnValue,");
                ////oSql.AppendLine("	  FTTxnStaPrc,FTTxnPosCode,FCTxnCrdValue");
                ////oSql.AppendLine("     ,FTTxnStaOffLine");    //*Em 61-12-06  Pandora
                ////oSql.AppendLine("     ,FTTxnDocNoRef");     //*[AnUBiS][][2019-02-13] - บันทึก doc no ref. เพิ่ม (Pandora)
                ////oSql.AppendLine("     )");
                ////oSql.AppendLine("     VALUES");
                ////oSql.AppendLine("     (");
                ////oSql.AppendLine("	  '" + poPara.ptBchCode + "','9','" + poPara.ptToCrdCode + "',");
                ////oSql.AppendLine("	  GETDATE(),'" + poPara.ptBchCode + "'," + poPara.pcFrmCrdValue + ",");
                ////oSql.AppendLine("	  '1',''," + poPara.pcToCrdValue + "");
                ////oSql.AppendLine("     ,'0'");    //*Em 61-12-06  Pandora
                ////oSql.AppendLine("     ,'" + poPara.ptDocNoRef + "'");   //*[AnUBiS][][2019-02-13] - บันทึก doc no ref. เพิ่ม (Pandora)
                ////oSql.AppendLine("     )");

                ////// Update Master Card ถึงรหัสบัตร(parameter)
                ////oSql.AppendLine("     UPDATE TFNMCard WITH (ROWLOCK) SET ");
                ////oSql.AppendLine("     FDCrdLastTopup=GetDate()");
                ////oSql.AppendLine("     ,FCCrdValue=" + poPara.pcFrmCrdValue + "");
                //////oSql.AppendLine("     ,FCCrdDeposit=" + poPara.pcCrdDeposit + "");
                //////oSql.AppendLine("     ,FCCrdDepositPdt=" + poPara.pcCrdDepositPdt + "");
                ////oSql.AppendLine("     ,FTCrdStaActive='1'");  //*Em 62-01-09  Pandora
                ////oSql.AppendLine("     WHERE FTCrdCode='" + poPara.ptToCrdCode + "'");

                ////oSql.AppendLine("     COMMIT TRANSACTION TnfCrd");
                ////oSql.AppendLine("  END TRY");

                ////oSql.AppendLine("  BEGIN CATCH");
                ////oSql.AppendLine("   ROLLBACK TRANSACTION TnfCrd");
                ////oSql.AppendLine("  END CATCH");

                try
                {
                    oDatabase = new cDatabase();
                    // nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);
                    aoSqlParam = new SqlParameter[] {
                        new SqlParameter ("@ptFrmCrdCode", SqlDbType.VarChar, 30){ Value = poPara.ptFrmCrdCode },
                        new SqlParameter ("@ptToCrdCode", SqlDbType.VarChar, 20){ Value = poPara.ptToCrdCode },
                        new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 5){ Value = poPara.ptBchCode },
                        new SqlParameter ("@pcFrmCrdValue", SqlDbType.Decimal){ Value = poPara.pcFrmCrdValue },
                        new SqlParameter ("@pcToCrdValue", SqlDbType.Decimal){ Value = poPara.pcToCrdValue },
                        new SqlParameter ("@ptDocNoRef", SqlDbType.VarChar, 20){ Value = poPara.ptDocNoRef }
                    };
                    nRowEff = oDatabase.C_GETnExecuteSqlStored("STP_PRCnTnfCrd", aoSqlParam);

                    if (nRowEff == 0)
                    {
                        poResult = new cmlResTnfCrd();
                        ptErrCode = oMsg.tMS_RespCode900;
                        ptErrDesc = oMsg.tMS_RespDesc900;
                        return false;
                    }
                }
                catch (EntityException oEtyExn)
                {
                    switch (oEtyExn.HResult)
                    {
                        case -2146232060:
                            // Cannot connect database..
                            poResult = new cmlResTnfCrd();
                            ptErrCode = oMsg.tMS_RespCode905;
                            ptErrDesc = oMsg.tMS_RespDesc905;
                            return false;
                    }
                }

                #endregion
                poResult = new cmlResTnfCrd();
                poResult.rtCode = oMsg.tMS_RespCode1; ;
                poResult.rtDesc = oMsg.tMS_RespDesc1;
                poResult.rtFrmCrdCode = poPara.ptFrmCrdCode;
                poResult.rtToCrdCode = poPara.ptToCrdCode;
                poResult.rtBchCode = poPara.ptBchCode;
                poResult.rtStatus = "1";
                ptErrCode = "";
                ptErrDesc = "";
                return true;
            }
            catch (Exception oEx)
            {
                poResult = new cmlResTnfCrd();
                ptErrCode = oMsg.tMS_RespCode900;
                ptErrDesc = oMsg.tMS_RespDesc900;
                return false;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oDatabase = null;
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
    }
}