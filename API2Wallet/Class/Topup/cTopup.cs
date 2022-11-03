using API2Wallet.Class.ResetExpired;
using API2Wallet.Class.Standard;
using API2Wallet.Models;
using API2Wallet.Models.WebService.Request.Topup;
using API2Wallet.Models.WebService.Response.Topup;
using System;
using System.Data;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using System.Linq;

namespace API2Wallet.Class.Topup
{
    /// <summary>
    /// 
    /// </summary>
    public class cTopup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="poPara"></param>
        /// <param name="ptErrCode"></param>
        /// <param name="ptErrDesc"></param>
        /// <param name="poResult"></param>
        /// <returns></returns>
        public bool C_DATbProcTopup(cmlReqTopup poPara,
                    out string ptErrCode, out string ptErrDesc, out cmlResTopup poResult)
        {
            cSP oFunc = new cSP();
            cMS oMsg = new cMS();
            cDatabase oDatabase;
            int nRowEff;
            cmlTFNMCard oCard;
            string tExpireTopup = "";
            string tNow = "";
            DateTime dExpireTopup = DateTime.Now;
            cResetExpired oResetExpire = new cResetExpired();
            bool bResultReset;
            StringBuilder oSql;
            decimal cValTopup;
            DataTable oTblTopup;
            SqlParameter[] aoSqlParam;
            try
            {
                oSql = new StringBuilder();
                oDatabase = new cDatabase();
                #region  Get ข้อมูลบัตร

                //Get ข้อมูลบัตร
                oCard = new cmlTFNMCard();
                // oCard = oFunc.SP_GEToCard(paoSysConfig,poPara.ptCrdCode,poPara.pnLngID);
                oCard = oFunc.SP_GEToCardByStored(poPara.ptCrdCode, poPara.pnLngID);
                #endregion

                // ตรวจสอบว่ามี รหัสบัตร หรือไม่
                if (oCard == null)
                {
                    poResult = new cmlResTopup();
                    ptErrCode = oMsg.tMS_RespCode800;
                    ptErrDesc = oMsg.tMS_RespDesc800;
                    return false;
                }
                else
                {
                    #region Verify
                    // ตรวจสอบวันหมดอายุรหัสบัตร
                    tNow = DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));
                    //tCrdExpireDate = oCard.FDCrdExpireDate.ToString("yyyy-MM-dd HH:mm:ss");
                    if (DateTime.Parse(tNow) > oCard.FDCrdExpireDate)
                    {
                        poResult = new cmlResTopup();
                        ptErrCode = oMsg.tMS_RespCode713;
                        ptErrDesc = oMsg.tMS_RespDesc713;
                        return false;
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
                                    //dExpireTopup = Convert.ToDateTime(oCard.FDCrdLastTopup).AddYears(Convert.ToInt32(oCard.FNCtyExpirePeriod));
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
                                //bResultReset = oResetExpire.C_SETbResetExpired(poPara.ptCrdCode, poPara.ptBchCode, poPara.ptTxnPosCode, paoSysConfig);
                                bResultReset = oResetExpire.C_SETbResetExpired(poPara.ptCrdCode, poPara.ptBchCode, poPara.ptTxnPosCode,poPara.ptDocNoRef);    //*Em 61-12-11  Pandora
                                if (bResultReset == false)
                                {
                                    poResult = new cmlResTopup();
                                    ptErrCode = oMsg.tMS_RespCode716;
                                    ptErrDesc = oMsg.tMS_RespDesc716;
                                    return false;
                                }
                            }
                        }
                    }

                    // ตรวจสอบรูปแบบการเติมเงิน 1 เติมเงิน Auto จาก TFNMCardType.FCCtyTopUpAuto
                    if (poPara.ptAuto == "1")
                    {
                        cValTopup = oCard.FCCtyTopupAuto;
                    }
                    else
                    {
                        cValTopup = poPara.pcTxnValue;
                    }
                    #endregion

                    #region Process
                    //oSql.Clear();
                    //oSql.AppendLine("BEGIN TRANSACTION ");
                    //oSql.AppendLine("  SAVE TRANSACTION Topup ");
                    //oSql.AppendLine("  BEGIN TRY ");

                    //// Insert transection Sale
                    //oSql.AppendLine("     INSERT INTO TFNTCrdTopUp WITH(ROWLOCK)");
                    //oSql.AppendLine("     (");
                    //oSql.AppendLine("	  FTBchCode,FTTxnDocType,FTCrdCode,");
                    //oSql.AppendLine("	  FDTxnDocDate,FTBchCodeRef,FCTxnValue,");
                    //oSql.AppendLine("	  FTTxnStaPrc,FTTxnPosCode,FCTxnCrdValue,");
                    //oSql.AppendLine("	  FTShpCode,FTTxnStaOffLine");
                    //oSql.AppendLine("     )");
                    //oSql.AppendLine("     VALUES");
                    //oSql.AppendLine("     (");
                    //oSql.AppendLine("	  '" + poPara.ptBchCode + "','1','" + poPara.ptCrdCode + "',");
                    //oSql.AppendLine("	  GETDATE(),'" + poPara.ptBchCode + "','" + cValTopup + "',");
                    //oSql.AppendLine("	  '1','" + poPara.ptTxnPosCode + "'," + poPara.pcAvailable + ",");
                    //oSql.AppendLine("     '" + poPara.ptShpCode + "','" + poPara.pnTxnOffline + "'");
                    //oSql.AppendLine("     )");

                    //// Update Master Card
                    //oSql.AppendLine("     UPDATE TFNMCard WITH (ROWLOCK) SET ");
                    //oSql.AppendLine("     FCCrdValue=(ISNULL(FCCrdValue,0) + " + cValTopup + "),");
                    //oSql.AppendLine("     FDCrdLastTopup=GETDATE() ");
                    //oSql.AppendLine("     WHERE FTCrdCode='" + poPara.ptCrdCode + "'");

                    //// Commit Command
                    //oSql.AppendLine("     COMMIT TRANSACTION Topup");
                    //oSql.AppendLine("  END TRY");

                    //oSql.AppendLine("  BEGIN CATCH");
                    //oSql.AppendLine("   ROLLBACK TRANSACTION Topup");
                    //oSql.AppendLine("  END CATCH");

                    try
                    {
                        //nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);
                        aoSqlParam = new SqlParameter[] {
                            new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 5){ Value = poPara.ptBchCode },
                            new SqlParameter ("@ptCrdCode", SqlDbType.VarChar, 20){ Value = poPara.ptCrdCode },
                            new SqlParameter ("@pcValTopup", SqlDbType.Decimal){ Value = cValTopup },
                            new SqlParameter ("@ptTxnPosCode", SqlDbType.VarChar, 3){ Value = poPara.ptTxnPosCode },
                            new SqlParameter ("@pcAvailable", SqlDbType.Decimal){ Value = poPara.pcAvailable },
                            new SqlParameter ("@ptShpCode", SqlDbType.VarChar, 5){ Value = poPara.ptShpCode },
                            new SqlParameter ("@pnTxnOffline", SqlDbType.Int){ Value = poPara.pnTxnOffline }
                        };
                        nRowEff = oDatabase.C_GETnExecuteSqlStored("STP_PRCnTopUp",aoSqlParam);
                        if (nRowEff == 0)
                        {
                            poResult = new cmlResTopup();
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
                                poResult = new cmlResTopup();
                                ptErrCode = oMsg.tMS_RespCode905;
                                ptErrDesc = oMsg.tMS_RespDesc905;
                                return false;
                        }
                    }
                    #endregion

                    #region Response
                    oCard = new cmlTFNMCard();
                    //  oCard = oFunc.SP_GEToCard(paoSysConfig, poPara.ptCrdCode, poPara.pnLngID);
                    oCard = oFunc.SP_GEToCardByStored(poPara.ptCrdCode, poPara.pnLngID);
                    if (oCard != null)
                    {
                        oSql.Clear();
                        oSql.AppendLine("SELECT TOP 1 FNTxnID FROM TFNTCrdTopup WITH (NOLOCK)");
                        oSql.AppendLine("WHERE FTBchCode='" + poPara.ptBchCode + "' AND FTTxnPosCode='" + poPara.ptTxnPosCode + "' AND FTCrdCode='" + poPara.ptCrdCode + "'");
                        oSql.AppendLine("ORDER BY FNTxnID DESC");
                        oTblTopup = oDatabase.C_GEToQuerySQLTbl(oSql.ToString());
                        if (poPara.pnTxnOffline == oCard.FNCrdTxnPrcAdj || poPara.pnTxnOffline == 0)
                        {
                            //oSql.Clear();
                            //oSql.AppendLine("SELECT TOP 1 FNTxnID FROM TFNTCrdTopup WITH (NOLOCK)");
                            //oSql.AppendLine("WHERE FTBchCode='" + poPara.ptBchCode + "' AND FTTxnPosCode='" + poPara.ptTxnPosCode + "' AND FTCrdCode='" + poPara.ptCrdCode + "'");
                            //oSql.AppendLine("ORDER BY FNTxnID DESC");
                            //  oTblTopup = oDatabase.C_DAToSqlQuery(oSql.ToString(), nCmdTme);
                            oTblTopup = oDatabase.C_GEToQuerySQLTbl(oSql.ToString());
                            if (oTblTopup == null)
                            {
                                poResult = new cmlResTopup();
                                ptErrCode = oMsg.tMS_RespCode900;
                                ptErrDesc = oMsg.tMS_RespDesc900;
                                return false;
                            }

                            poResult = new cmlResTopup();
                            poResult.rcTxnValue = oCard.FCCrdValue;
                            poResult.rcCrdDeposit = oCard.FCCrdDeposit;
                            poResult.rcCrdDepositPdt = oCard.FCCrdDepositPdt;
                            poResult.rcTxnValueAvb = oCard.cAvailable;
                            poResult.rtCrdName = oCard.FTCrdName;
                            poResult.rtCtyName = oCard.FTCtyName;
                            poResult.rdCrdExpireDate = oCard.FDCrdExpireDate;
                            poResult.rnTxnID = Convert.ToInt32(oTblTopup.Rows[0][0]);
                            poResult.rnTxnOffline = 0;
                            poResult.rtCode = oMsg.tMS_RespCode1;
                            poResult.rtDesc = oMsg.tMS_RespDesc1;
                            ptErrCode = "";
                            ptErrDesc = "";
                            return true;
                        }
                        else
                        {
                            //oSql.Clear();
                            //oSql.AppendLine("SELECT TOP 1 FNTxnID FROM TFNTCrdTopup WITH (NOLOCK)");
                            //oSql.AppendLine("WHERE FTBchCode='" + poPara.ptBchCode + "' AND FTTxnPosCode='" + poPara.ptTxnPosCode + "' AND FTCrdCode='" + poPara.ptCrdCode + "'");
                            //oSql.AppendLine("ORDER BY FNTxnID DESC");
                            //oTblTopup = oDatabase.C_DAToSqlQuery(oSql.ToString(), nCmdTme);

                            if (oTblTopup == null)
                            {
                                poResult = new cmlResTopup();
                                ptErrCode = oMsg.tMS_RespCode900;
                                ptErrDesc = oMsg.tMS_RespDesc900;
                                return false;
                            }

                            poResult = new cmlResTopup();
                            poResult.rcTxnValueAvb = poPara.pcAvailable + poPara.pcTxnValue;
                            poResult.rtCrdName = oCard.FTCrdName;
                            poResult.rtCtyName = oCard.FTCtyName;
                            poResult.rdCrdExpireDate = oCard.FDCrdExpireDate;
                            poResult.rnTxnID = Convert.ToInt32(oTblTopup.Rows[0][0]);
                            poResult.rnTxnOffline = poPara.pnTxnOffline;
                            poResult.rtCode = oMsg.tMS_RespCode1;
                            poResult.rtDesc = oMsg.tMS_RespDesc1;
                            ptErrCode = "";
                            ptErrDesc = "";
                            return true;
                        }
                    }
                    else
                    {
                        poResult = new cmlResTopup();
                        ptErrCode = oMsg.tMS_RespCode800;
                        ptErrDesc = oMsg.tMS_RespDesc800;
                        return false;
                    }
                    #endregion
                }
            }
            catch (Exception oEx)
            {
                poResult = new cmlResTopup();
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
                oTblTopup = null;
                oCard = null;
                tExpireTopup = null;
                tNow = null;
                oResetExpire = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
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
        public bool C_DATbProcCancelTopup(cmlReqCancelTopup poPara,
               out string ptErrCode, out string ptErrDesc, out cmlResCancelTopup poResult)
        {
            cSP oFunc = new cSP();
            cMS oMsg = new cMS();
            cDatabase oDatabase;
            StringBuilder oSql;
            int nRowEff;
            cmlTFNMCard oCard = new cmlTFNMCard();
            string tNow = "";
            DateTime dExpireTopup = DateTime.Now;
            cResetExpired oResetExpire = new cResetExpired();
            cmlTFNTCrdTopup oCrdTopup;
            DataTable oDbTbl;
            SqlParameter[] aoSqlParam;
            try
            {
                oDatabase = new cDatabase();
                oSql = new StringBuilder();

                #region  Get ข้อมูลบัตร
                //Get ข้อมูลบัตร
                oCard = new cmlTFNMCard();
                //  oCard = oFunc.SP_GEToCard(paoSysConfig, poPara.ptCrdCode, 0);
                oCard = oFunc.SP_GEToCardByStored(poPara.ptCrdCode, 0);
                #endregion

                //ตรวจสอบว่ามี รหัสบัตร หรือไม่
                if (oCard == null)
                {
                    poResult = new cmlResCancelTopup();
                    ptErrCode = oMsg.tMS_RespCode800;
                    ptErrDesc = oMsg.tMS_RespDesc800;
                    return false;
                }
                else
                {
                    #region Verify
                    // ตรวจสอบ TxnOffline(parameter) จะต้องเท่ากับ 0
                    if (poPara.pnTxnOffline != 0)
                    {
                        poResult = new cmlResCancelTopup();
                        ptErrCode = oMsg.tMS_RespCode803;
                        ptErrDesc = oMsg.tMS_RespDesc803;
                        return false;
                    }

                    // ตรวจสอบวันหมดอายุรหัสบัตร
                    tNow = DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));
                    if (DateTime.Parse(tNow) > oCard.FDCrdExpireDate)
                    {
                        poResult = new cmlResCancelTopup();
                        ptErrCode = oMsg.tMS_RespCode713;
                        ptErrDesc = oMsg.tMS_RespDesc713;
                        return false;
                    }
                    #endregion

                    #region Process
                    // หายอดเงิน จากเลขที่เอกสาร
                    oCrdTopup = new cmlTFNTCrdTopup();
                    oSql.Clear();
                    oSql.AppendLine("SELECT TOP 1 ISNULL(FCTxnValue,0) AS FCTxnValue");
                    oSql.AppendLine(",ISNULL(FTTxnStaCancel,0) AS FTTxnStaCancel,FTTxnPosCode ");
                    oSql.AppendLine("FROM TFNTCrdTopUp WITH(ROWLOCK)");
                    oSql.AppendLine("WHERE FTTxnDocType = '1'");
                    oSql.AppendLine("AND FTBchCode='" + poPara.ptBchCode + "'");
                    //oSql.AppendLine("AND FTTxnDocNoRef='" + poPara.ptTxnDocNoRef + "'");
                    oSql.AppendLine("AND FNTxnID=" + poPara.pnTxnID + "");
                    //  oCrdTopup = oDatabase.C_DAToSqlQuery<cmlTFNTCrdTopup>(oSql.ToString(), nCmdTme);
                    oDbTbl = new DataTable();
                    oDbTbl = oDatabase.C_GEToQuerySQLTbl(oSql.ToString());

                    if (oDbTbl.Rows.Count > 0)
                    {
                        var oItem = from DataRow oRow in oDbTbl.Rows
                                    select new cmlTFNTCrdTopup()
                                    {
                                        FCTxnValue = (decimal)oRow["FCTxnValue"],
                                        FTTxnStaCancel = (string)oRow["FTTxnStaCancel"],
                                        FTTxnPosCode = (string)oRow["FTTxnPosCode"],
                                    };
                        oCrdTopup = oItem.FirstOrDefault();
                    }

                    if (oCrdTopup == null) {
                        poResult = new cmlResCancelTopup();
                        ptErrCode = oMsg.tMS_RespDesc814; 
                        ptErrDesc = oMsg.tMS_RespDesc814;
                        return false;
                    }

                    if (oCrdTopup.FTTxnStaCancel == "1")
                    {
                        poResult = new cmlResCancelTopup();
                        ptErrCode = oMsg.tMS_RespCode816;
                        ptErrDesc = oMsg.tMS_RespDesc816;
                        return false;
                    }

                    //ตรวจสอบยอดเงินพร้อมใช้งานต้องมากกว่า ยอดเงินยกเลิก
                    if (oCrdTopup.FCTxnValue > oCard.cAvailable) {
                        poResult = new cmlResCancelTopup();
                        ptErrCode = oMsg.tMS_RespCode815;
                        ptErrDesc = oMsg.tMS_RespDesc815;
                        return false;
                    }

                    //oSql.Clear();
                    //oSql.AppendLine("BEGIN TRANSACTION ");
                    //oSql.AppendLine("  SAVE TRANSACTION Topup ");
                    //oSql.AppendLine("  BEGIN TRY ");

                    //// Insert transection Sale
                    //oSql.AppendLine("     INSERT INTO TFNTCrdTopUp WITH(ROWLOCK)");
                    //oSql.AppendLine("     (");
                    //oSql.AppendLine("	  FTBchCode,FTTxnDocType,FTCrdCode,");
                    //oSql.AppendLine("	  FDTxnDocDate,FTBchCodeRef,FCTxnValue,");
                    //oSql.AppendLine("	  FTTxnStaPrc,FTTxnPosCode,FCTxnCrdValue,FTTxnStaOffLine");
                    //oSql.AppendLine("     )");
                    //oSql.AppendLine("     VALUES");
                    //oSql.AppendLine("     (");
                    //oSql.AppendLine("	  '" + poPara.ptBchCode + "','2','" + poPara.ptCrdCode + "',");
                    //oSql.AppendLine("	  GETDATE(),'" + poPara.ptBchCode + "','" + oCrdTopup.FCTxnValue + "',");
                    //oSql.AppendLine("	  '1','" + oCrdTopup.FTTxnPosCode + "'," + poPara.pcAvailable+ ",'" + poPara.pnTxnOffline +"'");
                    //oSql.AppendLine("     )");

                    //// Update Master Card
                    //oSql.AppendLine("     UPDATE TFNMCard WITH (ROWLOCK) SET ");
                    //oSql.AppendLine("     FCCrdValue=(ISNULL(FCCrdValue,0) - " + oCrdTopup.FCTxnValue + "),");
                    //oSql.AppendLine("     FDCrdLastTopup=GETDATE()");
                    //oSql.AppendLine("     WHERE FTCrdCode='" + poPara.ptCrdCode + "'");
                    //oSql.AppendLine("     COMMIT TRANSACTION Topup");

                    //// Update Ref Id Canceled
                    //oSql.AppendLine("     UPDATE TFNTCrdTopUp WITH (ROWLOCK) SET ");
                    //oSql.AppendLine("     FTTxnStaCancel='1'");
                    //oSql.AppendLine("     WHERE FNTxnID=" + poPara.pnTxnID + "");
                    //oSql.AppendLine("  END TRY");

                    //oSql.AppendLine("  BEGIN CATCH");
                    //oSql.AppendLine("   ROLLBACK TRANSACTION Topup");
                    //oSql.AppendLine("  END CATCH");

                    try
                    {
                        //nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);
                        aoSqlParam = new SqlParameter[] {
                            new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 5){ Value = poPara.ptBchCode },
                            new SqlParameter ("@ptCrdCode", SqlDbType.VarChar, 20){ Value = poPara.ptCrdCode },
                            new SqlParameter ("@pcFCTxnValue", SqlDbType.Decimal){ Value = oCrdTopup.FCTxnValue },
                            new SqlParameter ("@ptFTTxnPosCode", SqlDbType.VarChar, 3){ Value = oCrdTopup.FTTxnPosCode },
                            new SqlParameter ("@pcAvailable", SqlDbType.Decimal){ Value = poPara.pcAvailable },
                            new SqlParameter ("@pnTxnOffline", SqlDbType.Int){ Value = poPara.pnTxnOffline },
                            new SqlParameter ("@pnTxnID", SqlDbType.Int){ Value = poPara.pnTxnID }
                        };
                        nRowEff = oDatabase.C_GETnExecuteSqlStored("STP_PRCnCancelTopup", aoSqlParam);
                        if (nRowEff == 0)
                        {
                            poResult = new cmlResCancelTopup();
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
                                poResult = new cmlResCancelTopup();
                                ptErrCode = oMsg.tMS_RespCode905;
                                ptErrDesc = oMsg.tMS_RespDesc905;
                                return false;
                        }
                    }
                    #endregion

                    #region Response
                    oCard = new cmlTFNMCard();
                    //  oCard = oFunc.SP_GEToCard(paoSysConfig, poPara.ptCrdCode, poPara.pnLngID);
                    oCard = oFunc.SP_GEToCardByStored(poPara.ptCrdCode, 0);
                    if (oCard == null) {
                        poResult = new cmlResCancelTopup();
                        ptErrCode = oMsg.tMS_RespCode800;
                        ptErrDesc = oMsg.tMS_RespDesc800;
                        return false;
                    } else {
                        poResult = new cmlResCancelTopup();
                        poResult.rcTxnValue = oCard.FCCrdValue;
                        poResult.rcCrdDeposit = oCard.FCCrdDeposit;
                        poResult.rcCrdDepositPdt = oCard.FCCrdDepositPdt;
                        poResult.rcTxnValueAvb = oCard.cAvailable;
                        poResult.rtCrdName = oCard.FTCrdName;
                        poResult.rtCtyName = oCard.FTCtyName;
                        poResult.rdCrdExpireDate = oCard.FDCrdExpireDate;
                        poResult.rnTxnOffline = 0;
                        poResult.rtCode = oMsg.tMS_RespCode1;
                        poResult.rtDesc = oMsg.tMS_RespDesc1;
                        ptErrCode = "";
                        ptErrDesc = "";
                        return true;
                    }
                    #endregion
                }
            }
            catch (Exception oEx)
            {
                poResult = new cmlResCancelTopup();
                ptErrCode = oMsg.tMS_RespCode900;
                ptErrDesc = oMsg.tMS_RespDesc900;
                return false;
            }
            finally
            {
                oCard = null;
                oResetExpire = null;
                oCrdTopup = null;
                oDbTbl = null;
                aoSqlParam = null;
                oFunc = null;
                oMsg = null;
                oDatabase = null;
                oSql = null;
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
        /// <param name="poErrPay"></param>
        /// <returns></returns>
        public bool C_DATbVerifyResetAvb(cmlReqResetAvi poPara,
                    out string ptErrCode, out string ptErrDesc, out cmlResResetAvb poErrPay)
        {
            cSP oFunc = new cSP();
            cMS oMsg = new cMS();
            cmlTFNMCard oCard = new cmlTFNMCard();
            string tNow = "";
            DateTime dExpireTopup = DateTime.Now;
            cResetExpired oResetExpire = new cResetExpired();
            string tExpireTopup = "";
            bool bResultReset;
            try
            {
                tNow = DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));
                //oSql = new StringBuilder();
                //oSql.Clear();
                //oSql.AppendLine("SELECT C.FTCrdCode,C.FDCrdExpireDate");
                //oSql.AppendLine(",ISNULL(C.FCCrdValue-T.FCCtyDeposit,0.00) AS FCCrdValue");
                //oSql.AppendLine(",ISNULL(T.FNCtyExpirePeriod,0) AS FNCtyExpirePeriod");
                //oSql.AppendLine(",ISNULL(T.FNCtyExpiredType,0) AS FNCtyExpiredType");
                //oSql.AppendLine(",C.FDCrdLastTopup");
                //oSql.AppendLine(",ISNULL(T.FTCtyStaAlwRet,0) AS FTCtyStaAlwRet ");
                //oSql.AppendLine("FROM TFNMCard C WITH(NOLOCK) ");
                //oSql.AppendLine("LEFT JOIN TFNMCardType T WITH(NOLOCK) ON (C.FTCtyCode=T.FTCtyCode) ");
                //oSql.AppendLine("WHERE FTCrdCode='" + poPara.ptCrdCode + "'");
                //oCard = oDatabase.C_DAToSqlQuery<cmlTFNMCard>(oSql.ToString(), nCmdTme);

                oCard = oFunc.SP_GEToCardByStored(poPara.ptCrdCode, 0);

                //ตรวจสอบว่ามี รหัสบัตร หรือไม่
                if (oCard == null)
                {
                    poErrPay = new cmlResResetAvb();
                    ptErrCode = oMsg.tMS_RespCode800;
                    ptErrDesc = oMsg.tMS_RespDesc800;
                    return false;
                }
                else
                {
                    // ตรวจสอบประเภทบัตรคืนได้หรือไม่
                    if (oCard.FTCtyStaAlwRet == "1")  // คืนได้
                    {
                        // ตรวจสอบวันหมดอายุรหัสบัตร
                      //  tCrdExpireDate = oCard.FDCrdExpireDate.ToString("yyyy-MM-dd HH:mm:ss");
                        if (DateTime.Parse(tNow) > oCard.FDCrdExpireDate)
                        {
                            poErrPay = new cmlResResetAvb();
                            ptErrCode = oMsg.tMS_RespCode713;
                            ptErrDesc = oMsg.tMS_RespDesc713;
                            return false;
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
                                        //dExpireTopup = Convert.ToDateTime(oCard.FDCrdLastTopup).AddYears(Convert.ToInt32(oCard.FNCtyExpirePeriod));
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
                                    //bResultReset = oResetExpire.C_SETbResetExpired(poPara.ptCrdCode, poPara.ptBchCode, poPara.ptTxnPosCode, paoSysConfig);
                                    bResultReset = oResetExpire.C_SETbResetExpired(poPara.ptCrdCode, poPara.ptBchCode, poPara.ptTxnPosCode, poPara.ptDocNoRef);  //*Em 61-12-11  Pandora
                                    if (bResultReset == false)
                                    {
                                        poErrPay = new cmlResResetAvb();
                                        ptErrCode = oMsg.tMS_RespCode716;
                                        ptErrDesc = oMsg.tMS_RespDesc716;
                                        return false;
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        // คืนไม่ได้ ResetExpire
                        tExpireTopup = dExpireTopup.ToString(@"yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));
                        if (DateTime.Parse(tNow) > DateTime.Parse(tExpireTopup))
                        {
                            // ยอดเงินหมดอายุ ResetExpire
                            //bResultReset = oResetExpire.C_SETbResetExpired(poPara.ptCrdCode, poPara.ptBchCode, poPara.ptTxnPosCode, paoSysConfig);
                            bResultReset = oResetExpire.C_SETbResetExpired(poPara.ptCrdCode, poPara.ptBchCode, poPara.ptTxnPosCode, poPara.ptDocNoRef);      //*Em 61-12-11  Pandora
                            if (bResultReset == false)
                            {
                                poErrPay = new cmlResResetAvb();
                                ptErrCode = oMsg.tMS_RespCode716;
                                ptErrDesc = oMsg.tMS_RespDesc716;
                                return false;
                            }
                        }
                        poErrPay = new cmlResResetAvb();
                        ptErrCode = oMsg.tMS_RespCode717;
                        ptErrDesc = oMsg.tMS_RespDesc717;
                        return false;
                    }
                }

                ptErrCode = "";
                ptErrDesc = "";
                poErrPay = null;
                return true;
            }
            catch (Exception oEx)
            {
                poErrPay = new cmlResResetAvb();
                ptErrCode = oMsg.tMS_RespCode900;
                ptErrDesc = oMsg.tMS_RespDesc900;
                return false;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

    }
}