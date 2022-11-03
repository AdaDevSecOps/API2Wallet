using API2Wallet.Class.Standard;
using API2Wallet.Models;
using API2Wallet.Models.WebService.Request.Pay;
using API2Wallet.Models.WebService.Response.Pay;
using System;
using System.Data;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;

namespace API2Wallet.Class.Pay
{
    /// <summary>
    /// 
    /// </summary>
    public class cPay
    {
       /// <summary>
       /// 
       /// </summary>
       /// <param name="poPara"></param>
       /// <param name="ptErrCode"></param>
       /// <param name="ptErrDesc"></param>
       /// <param name="poResult"></param>
       /// <returns></returns>
        public bool C_DATbProcPaytxn(cmlReqPayTxn  poPara,
               out string ptErrCode, out string ptErrDesc, out cmlResPayTxn poResult)
        {
            cSP oFunc = new cSP();
            cMS oMsg = new cMS();
            cDatabase oDatabase;
            StringBuilder oSql;
            int nRowEff;
            cmlTFNMCard oCard;
            string tNow;
            cmlTFNTCrdSale oCrdsale;
            int nFNTxnIDRef = 0;
            SqlParameter[] aoSqlParam;
            int nFNTxnID;   
            try
            {
                oDatabase = new cDatabase();
                oSql = new StringBuilder();
                oCard = new cmlTFNMCard();
                // oCard = oFunc.SP_GEToCard(paoSysConfig, poPara.ptCrdCode, poPara.pnLngID);
                oCard = oFunc.SP_GEToCardByStored(poPara.ptCrdCode, poPara.pnLngID);
                // ตรวจสอบว่ามี รหัสบัตร หรือไม่
                if (oCard == null)
                {
                    poResult = new cmlResPayTxn();
                    ptErrCode = oMsg.tMS_RespCode800;
                    ptErrDesc = oMsg.tMS_RespDesc800;
                    return false;
                } else {

                    #region  verify
                    // ตรวจสอบวันหมดอายุรหัสบัตร
                    tNow = DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));
                    if (DateTime.Parse(tNow) > oCard.FDCrdExpireDate)
                    {
                        poResult = new cmlResPayTxn();
                        ptErrCode = oMsg.tMS_RespCode713;
                        ptErrDesc = oMsg.tMS_RespDesc713;
                        return false;
                    }

                    if (poPara.pnTxnOffline == oCard.FNCrdTxnPrcAdj || poPara.pnTxnOffline == 0)
                    {
                        //2.1.1 ตรวจสอบยอดเงินใช้ได้
                        if (oCard.cAvailable < Convert.ToDecimal(poPara.pcTxnValue + poPara.pcCrdDepositPdt))
                        {
                            poResult = new cmlResPayTxn();
                            ptErrCode = oMsg.tMS_RespCode714;
                            ptErrDesc = oMsg.tMS_RespDesc714;
                            return false;
                        }
                    }
                    #endregion

                    #region กรณี Auto void ต้องยกเลิกสถานะคืนแล้ว FTTxnStaCancel ต้องไม่เท่ากับ 1
                    oCrdsale = new cmlTFNTCrdSale();
                    if (poPara.pnTxnRef != 0)
                    {
                        oSql.Clear();
                        oSql.AppendLine("SELECT ISNULL(FNTxnIDRef,0) AS FNTxnIDRef FROM TFNTCrdSale WITH (NOLOCK)");
                        oSql.AppendLine("WHERE FNTxnID=" + poPara.pnTxnRef + "");
                        // oCrdsale = oDatabase.C_DAToSqlQuery<cmlTFNTCrdSale>(oSql.ToString(), nCmdTme);

                        nFNTxnIDRef = oDatabase.C_GETnSQLScalarInt(oSql.ToString());

                        //if (oCrdsale != null)
                        //{
                        //    oSql.Clear();
                        //    oSql.AppendLine("UPDATE TFNTCrdSale WITH (ROWLOCK) SET ");
                        //    oSql.AppendLine("FTTxnStaCancel=null ");
                        //    oSql.AppendLine("WHERE FNTxnID=" + oCrdsale.FNTxnIDRef + "");
                        //    tSQLStaCancel = oSql.ToString();
                        //}
                    }

                    #endregion

                    #region Process
                    //oSql.Clear();
                    //oSql.AppendLine("BEGIN TRANSACTION ");
                    //oSql.AppendLine("  SAVE TRANSACTION SavePay ");
                    //oSql.AppendLine("  BEGIN TRY ");

                    //// Update StaCancel
                    //oSql.AppendLine(tSQLStaCancel);

                    //// Insert transection Sale
                    //oSql.AppendLine("     INSERT INTO TFNTCrdSale WITH(ROWLOCK)");
                    //oSql.AppendLine("     (");
                    //oSql.AppendLine("	  FTBchCode,FTCrdCode,FTTxnDocType,");
                    //oSql.AppendLine("	  FTTxnPosCode,FTTxnDocNoRef,FCTxnValue,");
                    //oSql.AppendLine("	  FTTxnstaPrc,FDTxnDocDate,FTBchCodeRef,");
                    //oSql.AppendLine("	  FCTxnDeposit,FCTxnCrdValue,FTShpCode,");
                    //if (poPara.pnTxnRef != 0) {   oSql.AppendLine("	FTTxnStaCancel,"); }
                    //oSql.AppendLine("	  FNTxnIDRef,FTTxnStaOffLine");
                    //oSql.AppendLine("     )");
                    //oSql.AppendLine("     VALUES");
                    //oSql.AppendLine("     (");
                    //oSql.AppendLine("	  '" + poPara.ptBchCode + "','" + poPara.ptCrdCode + "','3',");
                    //oSql.AppendLine("	  '" + poPara.ptTxnPosCode + "','" + poPara.ptTxnDocNoRef + "','" + poPara.pcTxnValue + "',");
                    //oSql.AppendLine("	  '1',GETDATE(),'" + poPara.ptBchCode + "',");
                    //oSql.AppendLine("	  '" + poPara.pcCrdDeposit + "'," + poPara.pcAvailable + ",'" + poPara.ptShpCode + "',");
                    //if (poPara.pnTxnRef != 0) {  oSql.AppendLine("	'1',"); }
                    //oSql.AppendLine("     '" + poPara.pnTxnRef + "'," + poPara.pnTxnOffline + "");
                    //oSql.AppendLine("     )");

                    //// Update Master Card
                    //oSql.AppendLine("     UPDATE TFNMCard WITH (ROWLOCK) SET ");
                    //oSql.AppendLine("     FCCrdValue=(ISNULL(FCCrdValue,0) - " + poPara.pcTxnValue + ") ");
                    //oSql.AppendLine("     WHERE FTCrdCode='" + poPara.ptCrdCode + "'");
                    //oSql.AppendLine("     COMMIT TRANSACTION SavePay");
                    //oSql.AppendLine("  END TRY");

                    //oSql.AppendLine("  BEGIN CATCH");
                    //oSql.AppendLine("   ROLLBACK TRANSACTION SavePay");
                    //oSql.AppendLine("  END CATCH");

                    try
                    {
                        //nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);

                        aoSqlParam = new SqlParameter[] {
                            new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 5){ Value = poPara.ptBchCode },
                            new SqlParameter ("@ptCrdCode", SqlDbType.VarChar, 20){ Value = poPara.ptCrdCode },
                            new SqlParameter ("@ptTxnPosCode", SqlDbType.VarChar, 3){ Value = poPara.ptTxnPosCode },
                            new SqlParameter ("@ptTxnDocNoRef", SqlDbType.VarChar, 30){ Value = poPara.ptTxnDocNoRef },
                            new SqlParameter ("@pcTxnValue", SqlDbType.Decimal){ Value = poPara.pcTxnValue },
                            new SqlParameter ("@pcCrdDeposit", SqlDbType.Decimal){ Value = poPara.pcCrdDeposit },
                            new SqlParameter ("@pcAvailable", SqlDbType.Decimal){ Value = poPara.pcAvailable },
                            new SqlParameter ("@ptShpCode", SqlDbType.VarChar, 3){ Value = poPara.ptShpCode },
                            new SqlParameter ("@pnTxnOffline", SqlDbType.Int){ Value = poPara.pnTxnOffline },
                            new SqlParameter ("@pnTxnRef", SqlDbType.Int){ Value = poPara.pnTxnRef },
                            new SqlParameter ("@pnFNTxnIDRef", SqlDbType.Int){ Value =  oCrdsale.FNTxnIDRef },
                        };
                        nRowEff = oDatabase.C_GETnExecuteSqlStored("STP_PRCnPaytxn", aoSqlParam);
                        if (nRowEff == 0)
                        {
                            poResult = new cmlResPayTxn();
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
                                poResult = new cmlResPayTxn();
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
                        oSql.AppendLine("SELECT TOP 1 ISNULL(FNTxnID,0) AS FNTxnID FROM TFNTCrdSale WITH (NOLOCK)");
                        oSql.AppendLine("WHERE FTBchCode='" + poPara.ptBchCode + "' AND FTTxnPosCode='" + poPara.ptTxnPosCode + "' AND FTCrdCode='" + poPara.ptCrdCode + "'");
                        oSql.AppendLine("ORDER BY FNTxnID DESC");
                        nFNTxnID = oDatabase.C_GETnSQLScalarInt(oSql.ToString());

                        if (nFNTxnID == 0)
                        {
                            poResult = new cmlResPayTxn();
                            ptErrCode = oMsg.tMS_RespCode900;
                            ptErrDesc = oMsg.tMS_RespDesc900;
                            return false;
                        }


                        // 4.2 TxnOffline(parameter) = TFNMCrd.FNCrdTxnPrcAjp หรือ TxnOffline(parameter) = 0
                        if (poPara.pnTxnOffline == oCard.FNCrdTxnPrcAdj || poPara.pnTxnOffline == 0)
                        {
                            //oSql.Clear();
                            //oSql.AppendLine("SELECT TOP 1 FNTxnID FROM TFNTCrdSale WITH (NOLOCK)");
                            //oSql.AppendLine("WHERE FTBchCode='" + poPara.ptBchCode + "' AND FTTxnPosCode='" + poPara.ptTxnPosCode + "' AND FTCrdCode='" + poPara.ptCrdCode + "'");
                            //oSql.AppendLine("ORDER BY FNTxnID DESC");
                            //oTblSale = oDatabase.C_DAToSqlQuery(oSql.ToString(), nCmdTme);

                            //if (oTblSale == null)
                            //{
                            //    poResult = new cmlResPayTxn();
                            //    ptErrCode = oMsg.tMS_RespCode900;
                            //    ptErrDesc = oMsg.tMS_RespDesc900;
                            //    return false;
                            //}

                            poResult = new cmlResPayTxn();
                            poResult.rcTxnValue = oCard.FCCrdValue;
                            poResult.rcCrdDeposit = oCard.FCCrdDeposit;
                            poResult.rcCrdDepositPdt = oCard.FCCrdDepositPdt;
                            poResult.rcTxnValueAvb = oCard.cAvailable;
                            poResult.rtCrdName = oCard.FTCrdName;
                            poResult.rtCtyName = oCard.FTCtyName;
                            poResult.rdCrdExpireDate = oCard.FDCrdExpireDate;
                            //  poResult.rnTxnID = Convert.ToInt32(oTblSale.Rows[0][0]);
                            poResult.rnTxnID = nFNTxnID;
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
                            //oSql.AppendLine("SELECT TOP 1 FNTxnID FROM TFNTCrdSale WITH (NOLOCK)");
                            //oSql.AppendLine("WHERE FTBchCode='" + poPara.ptBchCode + "' AND FTTxnPosCode='" + poPara.ptTxnPosCode + "' AND FTCrdCode='" + poPara.ptCrdCode + "'");
                            //oSql.AppendLine("ORDER BY FNTxnID DESC");
                            //oTblSale = oDatabase.C_DAToSqlQuery(oSql.ToString(), nCmdTme);

                            //if (oTblSale == null)
                            //{
                            //    poResult = new cmlResPayTxn();
                            //    ptErrCode = oMsg.tMS_RespCode721;
                            //    ptErrDesc = oMsg.tMS_RespDesc721;
                            //    return false;
                            //}

                            poResult = new cmlResPayTxn();
                            poResult.rcTxnValueAvb = poPara.pcAvailable - (poPara.pcTxnValue + poPara.pcCrdDepositPdt);
                            poResult.rtCrdName = oCard.FTCrdName;
                            poResult.rtCtyName = oCard.FTCtyName;
                            poResult.rdCrdExpireDate = oCard.FDCrdExpireDate;
                            // poResult.rnTxnID = Convert.ToInt32(oTblSale.Rows[0][0]);
                            poResult.rnTxnID = nFNTxnID;
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
                        poResult = new cmlResPayTxn();
                        ptErrCode = oMsg.tMS_RespCode800;
                        ptErrDesc = oMsg.tMS_RespDesc800;
                        return false;
                    }
                    #endregion
                }
            }
            catch (Exception oEx)
            {
                poResult = new cmlResPayTxn();
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
                oCard = null;
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
        public bool C_DATbProcCancelPaytxn(cmlReqCancelpayTxn poPara,
               out string ptErrCode, out string ptErrDesc, out cmlRescancelPayTxn poResult)
        {
            cSP oFunc = new cSP();
            cMS oMsg = new cMS();
            cDatabase oDatabase;
            StringBuilder oSql;
            int nRowEff;
            cmlTFNMCard oCard;
            string tNow = "";
            cmlTFNTCrdSale oCrdsale = new cmlTFNTCrdSale();
            string tFTTxnStaCancel = "";
            DataTable oTBlCrdsale = new DataTable();
            SqlParameter[] aoSqlParam;
            int nFNTxnID;
            try
            {
                oDatabase = new cDatabase();
                oSql = new StringBuilder();
                oCard = new cmlTFNMCard();
                //  oCard = oFunc.SP_GEToCard(paoSysConfig, poPara.ptCrdCode, poPara.pnLngID);
                oCard = oFunc.SP_GEToCardByStored(poPara.ptCrdCode, poPara.pnLngID);

                // ตรวจสอบว่ามี รหัสบัตร หรือไม่
                if (oCard == null)
                {
                    poResult = new cmlRescancelPayTxn();
                    ptErrCode = oMsg.tMS_RespCode800;
                    ptErrDesc = oMsg.tMS_RespDesc800;
                    return false;
                }
                else
                {
                    #region Verify
                    // ตรวจสอบวันหมดอายุรหัสบัตร
                    tNow = DateTime.Now.ToString(@"yyyy-MM-dd", new CultureInfo("en-US"));
                    //  tCrdExpireDate = oCard.FDCrdExpireDate.ToString("yyyy-MM-dd");
                    if (DateTime.Parse(tNow) > oCard.FDCrdExpireDate)
                    {
                        poResult = new cmlRescancelPayTxn();
                        ptErrCode = oMsg.tMS_RespCode713;
                        ptErrDesc = oMsg.tMS_RespDesc713;
                        return false;
                    }

                    // Get สถานะเคยยกเลิกแล้วหรือยัง
                    oSql.Clear();
                    oSql.AppendLine("SELECT ISNULL(FTTxnStaCancel,'0') AS FTTxnStaCancel FROM TFNTCrdSale WITH (NOLOCK)");
                    oSql.AppendLine("WHERE FNTxnID=" + poPara.pnTxnID + "");
                    // oCrdsale = oDatabase.C_DAToSqlQuery<cmlTFNTCrdSale>(oSql.ToString(), nCmdTme);
                    tFTTxnStaCancel = oDatabase.C_GETtSQLScalarString(oSql.ToString());

                    //if (oCrdsale.FTTxnStaCancel == "1")
                    //{
                    //    poResult = new cmlRescancelPayTxn();
                    //    ptErrCode = oMsg.tMS_RespCode715;
                    //    ptErrDesc = oMsg.tMS_RespDesc715;
                    //    return false;
                    //}

                    if (tFTTxnStaCancel == "1")
                    {
                        poResult = new cmlRescancelPayTxn();
                        ptErrCode = oMsg.tMS_RespCode715;
                        ptErrDesc = oMsg.tMS_RespDesc715;
                        return false;
                    }
                    #endregion

                    #region Get value Sale
                    oSql.Clear();
                    oSql.AppendLine("SELECT ISNULL(S.FCTxnValue,0.00) AS FCTxnValue,ISNULL(S.FCTxnDeposit,0.00) AS FCTxnDeposit FROM TFNTCrdSale S WITH(NOLOCK)");
                    oSql.AppendLine("WHERE S.FTTxnDocType='3'");
                   // oSql.AppendLine("AND TFNTCrdSale.FTBchCode='" + poPara.ptBchCode +"'");
                  //  oSql.AppendLine("AND TFNTCrdSale.FTTxnDocNoRef='" + poPara.ptTxnDocNoRef + "'");
                    oSql.AppendLine("AND S.FNTxnID=" + poPara.pnTxnID + "");
                    //   oCrdsale = oDatabase.C_DAToSqlQuery<cmlTFNTCrdSale>(oSql.ToString(), nCmdTme);
                    oTBlCrdsale = oDatabase.C_GEToQuerySQLTbl(oSql.ToString());
                    if (oTBlCrdsale.Rows.Count > 0)
                    {
                        var oItem = from DataRow oRow in oTBlCrdsale.Rows
                                    select new cmlTFNTCrdSale()
                                    {
                                        FCTxnValue = (decimal)oRow["FCTxnValue"],
                                        FCTxnDeposit = (decimal)oRow["FCTxnDeposit"],
                                    };
                        oCrdsale = oItem.FirstOrDefault();
                    }
                    

                    if (oCrdsale == null)
                    {
                        poResult = new cmlRescancelPayTxn();
                        ptErrCode = oMsg.tMS_RespCode900;
                        ptErrDesc = oMsg.tMS_RespDesc900;
                        return false;
                    }
                    #endregion

                    #region Process
                    //oSql.Clear();
                    //oSql.AppendLine("BEGIN TRANSACTION ");
                    //oSql.AppendLine("  SAVE TRANSACTION SavePay ");
                    //oSql.AppendLine("  BEGIN TRY ");

                    //// Insert transection Sale
                    //oSql.AppendLine("     INSERT INTO TFNTCrdSale WITH(ROWLOCK)");
                    //oSql.AppendLine("     (");
                    //oSql.AppendLine("	  FTBchCode,FTCrdCode,FTTxnDocType,");
                    //oSql.AppendLine("	  FTTxnDocNoRef,FCTxnValue,FNTxnIDRef,");
                    //oSql.AppendLine("	  FTTxnstaPrc,FDTxnDocDate,FTBchCodeRef");
                    //oSql.AppendLine("	  ,FTTxnPosCode,FCTxnDeposit");
                    ////oSql.AppendLine("	  ,FCTxnCrdValue,FTShpCode,FTTxnStaOffLine");
                    //oSql.AppendLine("	  ,FTShpCode,FTTxnStaOffLine"); //*Em 61-12-17  Pandora ยกเลิกการตัดจ่ายไม่ต้องใส่ยอดบัตรก่อนใช้
                    //oSql.AppendLine("	  ,FCTxnCrdValue"); //*[AnUBiS][][2019-02-27] - Pandora เพิ่มยอดบัตรในการยกเลิกการตัดจ่าย
                    //oSql.AppendLine("     )");

                    ////*[AnUBiS][][2019-02-27] - get card value and save to card sale.
                    //oSql.AppendLine("     SELECT");
                    //oSql.AppendLine("       '" + poPara.ptBchCode + "','" + poPara.ptCrdCode + "','4',");
                    //oSql.AppendLine("       '" + poPara.ptTxnDocNoRef + "','" + oCrdsale.FCTxnValue + "','" + poPara.pnTxnID + "',");
                    //oSql.AppendLine("       '1',GETDATE(),'" + poPara.ptBchCode + "'");
                    //oSql.AppendLine("       ,'" + poPara.ptPosCode + "','" + oCrdsale.FCTxnDeposit + "'");
                    //oSql.AppendLine("       ,'" + poPara.ptShpCode + "','" + poPara.pnTxnOffline + "'");  //*Em 61-12-17  Pandora ยกเลิกการตัดจ่ายไม่ต้องใส่ยอดบัตรก่อนใช้
                    //oSql.AppendLine("       ,FCCrdValue");
                    //oSql.AppendLine("     FROM TFNMCard WITH(NOLOCK)");
                    //oSql.AppendLine("     WHERE FTCrdCode = '" + poPara.ptCrdCode + "'");

                    ////*[AnUBiS][][2019-02-26] - comment code.
                    ///*
                    //oSql.AppendLine("     VALUES");
                    //oSql.AppendLine("     (");
                    //oSql.AppendLine("	  '" + poPara.ptBchCode + "','" + poPara.ptCrdCode + "','4',");
                    //oSql.AppendLine("	  '" + poPara.ptTxnDocNoRef + "','" + oCrdsale.FCTxnValue + "','" + poPara.pnTxnID + "',");
                    //oSql.AppendLine("	  '1',GETDATE(),'" + poPara.ptBchCode + "'");
                    //oSql.AppendLine("	  ,'" + oCrdsale.FTTxnPosCode + "','" + oCrdsale.FCTxnDeposit + "'");
                    ////oSql.AppendLine("	  ," + poPara.pcAvailable + ",'" + poPara.ptShpCode + "'," + poPara.pnTxnOffline + "");
                    //oSql.AppendLine("	  ,'" + poPara.ptShpCode + "'," + poPara.pnTxnOffline + "");  //*Em 61-12-17  Pandora ยกเลิกการตัดจ่ายไม่ต้องใส่ยอดบัตรก่อนใช้
                    //oSql.AppendLine("     )");
                    //*/

                    //// Update Master Card
                    //oSql.AppendLine("     UPDATE TFNMCard WITH (ROWLOCK) SET ");
                    //oSql.AppendLine("     FCCrdValue=(ISNULL(FCCrdValue,0) + '" + oCrdsale.FCTxnValue + "'),");
                    //oSql.AppendLine("     FCCrdDeposit=(ISNULL(FCCrdDeposit,0) - '" + oCrdsale.FCTxnDeposit + "')");
                    //oSql.AppendLine("     WHERE FTCrdCode='" + poPara.ptCrdCode + "'");

                    //// Update Cancel Pay
                    //oSql.AppendLine("     UPDATE TFNTCrdSale WITH (ROWLOCK) SET ");
                    //oSql.AppendLine("     FTTxnStaCancel='1'");
                    //oSql.AppendLine("     WHERE FNTxnID='" + poPara.pnTxnID + "'");

                    //oSql.AppendLine("     COMMIT TRANSACTION SavePay");
                    //oSql.AppendLine("  END TRY");

                    //oSql.AppendLine("  BEGIN CATCH");
                    //oSql.AppendLine("   ROLLBACK TRANSACTION SavePay");
                    //oSql.AppendLine("  END CATCH");
                    
                    try
                    {
                        // nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);

                        aoSqlParam = new SqlParameter[] {
                            new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 5){ Value = poPara.ptBchCode },
                            new SqlParameter ("@ptCrdCode", SqlDbType.VarChar, 20){ Value = poPara.ptCrdCode },
                            new SqlParameter ("@ptShpCode", SqlDbType.VarChar, 5){ Value = poPara.ptShpCode },
                            new SqlParameter ("@ptTxnDocNoRef", SqlDbType.VarChar, 30){ Value = poPara.ptTxnDocNoRef },
                            new SqlParameter ("@pcTxnValue", SqlDbType.Decimal){ Value = oCrdsale.FCTxnValue },
                            new SqlParameter ("@pnTxnID", SqlDbType.Int){ Value = poPara.pnTxnID },
                            new SqlParameter ("@ptPosCode", SqlDbType.VarChar, 3){ Value = poPara.ptPosCode },
                            new SqlParameter ("@pcTxnDeposit", SqlDbType.Decimal){ Value = oCrdsale.FCTxnDeposit },
                            new SqlParameter ("@pnTxnOffline", SqlDbType.Int){ Value = poPara.pnTxnOffline },
                        };

                        nRowEff = oDatabase.C_GETnExecuteSqlStored("STP_SETnCancelPayTxn", aoSqlParam);
                        if (nRowEff == 0)
                        {
                            poResult = new cmlRescancelPayTxn();
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
                                poResult = new cmlRescancelPayTxn();
                                ptErrCode = oMsg.tMS_RespCode905;
                                ptErrDesc = oMsg.tMS_RespDesc905;
                                return false;
                        }
                    }
                    #endregion

                    #region Response
                    oCard = new cmlTFNMCard();
                    //   oCard = oFunc.SP_GEToCard(paoSysConfig, poPara.ptCrdCode, poPara.pnLngID);
                    oCard = oFunc.SP_GEToCardByStored(poPara.ptCrdCode, poPara.pnLngID);
                    if (oCard == null)  {
                        poResult = new cmlRescancelPayTxn();
                        ptErrCode = oMsg.tMS_RespCode800;
                        ptErrDesc = oMsg.tMS_RespDesc800;
                        return false;
                    }

                    oSql.Clear();
                    oSql.AppendLine("SELECT TOP 1 ISNULL(FNTxnID,0) AS FNTxnID FROM TFNTCrdSale WITH (NOLOCK)");
                    oSql.AppendLine("WHERE FTBchCode='" + poPara.ptBchCode + "' AND FTShpCode='" + poPara.ptShpCode + "' ");
                    oSql.AppendLine("AND FTCrdCode='" + poPara.ptCrdCode + "' AND FTTxnDocType='4'");
                    oSql.AppendLine("ORDER BY FNTxnID DESC");
                    //   oTblSale = oDatabase.C_DAToSqlQuery(oSql.ToString(), nCmdTme);

                    nFNTxnID = oDatabase.C_GETnSQLScalarInt(oSql.ToString());


                    //if (oTblSale == null)
                    //{
                    //    poResult = new cmlRescancelPayTxn();
                    //    ptErrCode = oMsg.tMS_RespCode721;
                    //    ptErrDesc = oMsg.tMS_RespDesc721;
                    //    return false;
                    //}

                    if (nFNTxnID == 0)
                    {
                        poResult = new cmlRescancelPayTxn();
                        ptErrCode = oMsg.tMS_RespCode721;
                        ptErrDesc = oMsg.tMS_RespDesc721;
                        return false;
                    }


                    poResult = new cmlRescancelPayTxn();
                    poResult.rcTxnValue = oCard.FCCrdValue;
                    poResult.rcCrdDeposit = oCard.FCCrdDeposit;
                    poResult.rcCrdDepositPdt = oCard.FCCrdDepositPdt;
                    poResult.rcTxnValueAvb = oCard.cAvailable;
                    poResult.rtCrdName = oCard.FTCrdName;
                    poResult.rtCtyName = oCard.FTCtyName;
                    poResult.rdCrdExpireDate = oCard.FDCrdExpireDate;
                    poResult.rdCrdLastTopup = oCard.FDCrdLastTopup;
                    poResult.rnTxnOffline = 0;
                    // poResult.rnTxnID = Convert.ToInt32(oTblSale.Rows[0][0]);
                    poResult.rnTxnID = nFNTxnID;
                    poResult.rtCode = oMsg.tMS_RespCode1;
                    poResult.rtDesc = oMsg.tMS_RespDesc1;
                    ptErrCode = "";
                    ptErrDesc = "";
                    return true;
                    #endregion
                }
            }
            catch (Exception)
            {
                poResult = new cmlRescancelPayTxn();
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
                oCard = null;
                tNow = null;
                oCrdsale = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}