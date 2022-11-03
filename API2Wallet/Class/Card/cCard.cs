using API2Wallet.Class.ResetExpired;
using API2Wallet.Class.Standard;
using API2Wallet.Class.Tranfer;
using API2Wallet.Models;
using API2Wallet.Models.WebService.Request.Card;
using API2Wallet.Models.WebService.Request.ChangeCard;
using API2Wallet.Models.WebService.Request.Tranfer;
using API2Wallet.Models.WebService.Response.Card;
using API2Wallet.Models.WebService.Response.ChangeCard;
using API2Wallet.Models.WebService.Response.Tranfer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace API2Wallet.Class.Log
{
    /// <summary>
    /// 
    /// </summary>
    public class cChangeCard
    {
        /// <summary>
       /// 
       /// </summary>
       /// <param name="poPara"></param>
       /// <param name="ptErrCode"></param>
       /// <param name="ptErrDesc"></param>
       /// <param name="poResult"></param>
       /// <returns></returns>
        public bool C_DATbProcChangeCard(List<cmlReqChangeCard> poPara,
                    out string ptErrCode, out string ptErrDesc, out cmlResChangeCard poResult)
        {
            cSP oFunc = new cSP();
            cMS oMsg = new cMS();
            cDatabase oDatabase;
            StringBuilder oSql;
            cmlTFNMCard oCardOut = new cmlTFNMCard();
            cmlTFNMCard oCardIn = new cmlTFNMCard();
            DateTime dExpireTopup = DateTime.Now;
            cResetExpired oResetExpire = new cResetExpired();
            cTranfer oTranfer = new cTranfer();
            string tNow;
            string tFrmCrdTopupExp;
            string tToCrdTopupExp;
            bool bResetFrmCrdTopup = true;
            bool bResetToCrdTopup = true;
            List<cmlResaoChangeCard> aoChangeCrd = new List<cmlResaoChangeCard>();
            cmlResaoChangeCard oChangeCrd;
            cmlReqTnfCrd oReqTnfCrd;
            cmlResTnfCrd oResTnfCrd;
            SqlParameter[] aoSqlParam;
            try
            {
                oDatabase = new cDatabase();
                oSql = new StringBuilder();
                tNow = DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));
                for (int nRow = 0; nRow < poPara.Count; nRow++)
                {
                    //*Em 61-12-06  Pandora
                    #region สร้างบัตรใหม่ ด้วยข้อมูลบัตรเดิม
                    ////oSql = new StringBuilder();
                    ////oSql.AppendLine("IF NOT EXISTS(SELECT TOP 1 FTCrdCode FROM TFNMCard WITH(NOLOCK) WHERE FTCrdCode = '" + poPara[nRow].ptToCrdCode + "')");
                    ////oSql.AppendLine("BEGIN");
                    ////oSql.AppendLine("INSERT INTO TFNMCard (FTCrdCode, FDCrdStartDate, FDCrdExpireDate, FDCrdResetDate, FDCrdLastTopup, FTCtyCode, FCCrdValue, FCCrdDeposit, FCCrdDepositPdt, FTDptCode, ");
                    ////oSql.AppendLine("FTCrdHolderID, FTCrdRefID, FTCrdStaType, FTCrdStaShift, FTCrdStaActive, FNCrdTxnOffline, FNCrdTxnPrcAdj, ");
                    ////oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy)");
                    ////oSql.AppendLine("SELECT '" + poPara[nRow].ptToCrdCode + "' AS FTCrdCode, FDCrdStartDate, FDCrdExpireDate, FDCrdResetDate, FDCrdLastTopup, FTCtyCode, FCCrdValue, FCCrdDeposit, FCCrdDepositPdt, FTDptCode, ");
                    ////oSql.AppendLine("FTCrdHolderID, FTCrdRefID, FTCrdStaType, FTCrdStaShift, FTCrdStaActive, FNCrdTxnOffline, FNCrdTxnPrcAdj,");
                    ////oSql.AppendLine("FDLastUpdOn, FTLastUpdBy, FDCreateOn, FTCreateBy");
                    ////oSql.AppendLine("FROM TFNMCard WITH(NOLOCK)");
                    ////oSql.AppendLine("WHERE FTCrdCode='" + poPara[nRow].ptFrmCrdCode + "'");
                    ////oSql.AppendLine("");
                    ////oSql.AppendLine("INSERT INTO TFNMCard_L (FTCrdCode, FNLngID, FTCrdName, FTCrdRmk)");
                    ////oSql.AppendLine("SELECT '" + poPara[nRow].ptToCrdCode + "' AS FTCrdCode, FNLngID, FTCrdName, FTCrdRmk");
                    ////oSql.AppendLine("FROM TFNMCard_L WITH(NOLOCK)");
                    ////oSql.AppendLine("WHERE FTCrdCode='" + poPara[nRow].ptFrmCrdCode + "'");
                    ////oSql.AppendLine("");
                    //////oSql.AppendLine("UPDATE TFNMCard WITH(ROWLOCK)");
                    //////oSql.AppendLine("SET FCCrdValue = 0");  //ปรับยอดบัตรเดิมให้เป็น 0
                    //////oSql.AppendLine(",FTCrdStaActive = '3'"); //ยกเลิกบัตรเดิม
                    //////oSql.AppendLine(",FDLastUpdOn = GETDATE()");
                    //////oSql.AppendLine("WHERE FTCrdCode='" + poPara[nRow].ptFrmCrdCode + "'");
                    ////oSql.AppendLine("END");
                    ////oDatabase.C_DATnExecuteSql(oSql.ToString());


                    aoSqlParam = new SqlParameter[] {
                        new SqlParameter ("@ptToCrdCode", SqlDbType.NVarChar, 20) { Value = poPara[nRow].ptToCrdCode },
                        new SqlParameter ("@ptFrmCrdCode", SqlDbType.NVarChar, 20) { Value = poPara[nRow].ptFrmCrdCode },
                    };

                    oDatabase.C_GETnExecuteSqlStored("STP_DATnCreateCrdOldData",aoSqlParam);
                    #endregion
                    //+++++++++++++++++++++

                    #region เตรียมข้อมูลบัตร
                    oChangeCrd = new cmlResaoChangeCard();
                    //oSql.Clear();
                    //oSql.AppendLine("SELECT C.FTCrdCode,C.FDCrdExpireDate");
                    //oSql.AppendLine(",ISNULL(C.FCCrdValue,0.0000) AS FCCrdValue");
                    //oSql.AppendLine(",ISNULL(C.FCCrdDeposit,0) AS FCCrdDeposit");
                    //oSql.AppendLine(",ISNULL(C.FCCrdDepositPdt,0) AS FCCrdDepositPdt");
                    //oSql.AppendLine(",ISNULL(T.FNCtyExpirePeriod,0) AS FNCtyExpirePeriod");
                    //oSql.AppendLine(",ISNULL(T.FNCtyExpiredType,0) AS FNCtyExpiredType,C.FDCrdLastTopup,FTCrdStaLocate ");
                    //oSql.AppendLine(",C.FTCrdHolderID ");   //*Em 62-01-09  Pandora
                    //oSql.AppendLine("FROM TFNMCard C WITH(NOLOCK) LEFT JOIN TFNMCardType T ON (C.FTCtyCode=T.FTCtyCode) ");
                    //oSql.AppendLine("WHERE FTCrdCode='" + poPara[nRow].ptFrmCrdCode + "'");
                    //oCardOut = oDatabase.C_DAToSqlQuery<cmlTFNMCard>(oSql.ToString(), nCmdTme);

                    oCardOut = oFunc.SP_GEToCardByStored(poPara[nRow].ptFrmCrdCode);

                    //oSql.Clear();
                    //oSql.AppendLine("SELECT C.FTCrdCode,C.FDCrdExpireDate");
                    //oSql.AppendLine(",ISNULL(C.FCCrdValue,0.0000) AS FCCrdValue");
                    //oSql.AppendLine(",ISNULL(T.FNCtyExpirePeriod,0) AS FNCtyExpirePeriod");
                    //oSql.AppendLine(",ISNULL(T.FNCtyExpiredType,0) AS FNCtyExpiredType,C.FDCrdLastTopup,FTCrdStaLocate ");
                    //oSql.AppendLine(",C.FTCrdHolderID ");   //*Em 62-01-09  Pandora
                    //oSql.AppendLine("FROM TFNMCard C WITH(NOLOCK) LEFT JOIN TFNMCardType T ON (C.FTCtyCode=T.FTCtyCode) ");
                    //oSql.AppendLine("WHERE FTCrdCode='" + poPara[nRow].ptToCrdCode + "'");
                    //oCardIn = oDatabase.C_DAToSqlQuery<cmlTFNMCard>(oSql.ToString(), nCmdTme);
                    oCardIn = oFunc.SP_GEToCardByStored(poPara[nRow].ptToCrdCode);
                    #endregion

                    #region Verify
                    // ตรวจสอบวันหมดอายุรหัสบัตร จากรหัสบัตร
                    if (DateTime.Parse(tNow) > oCardOut.FDCrdExpireDate)
                    {
                        oChangeCrd.rtFrmCrdCode = poPara[nRow].ptFrmCrdCode;
                        oChangeCrd.rtToCrdCode = poPara[nRow].ptToCrdCode;
                        oChangeCrd.rtBchCode = poPara[nRow].ptBchCode;
                        oChangeCrd.rtStatus = "3";
                        aoChangeCrd.Add(oChangeCrd);
                        continue;
                    }

                    // ตรวจสอบวันหมดอายุรหัสบัตร ถึงรหัสบัตร
                    if (DateTime.Parse(tNow) > oCardIn.FDCrdExpireDate)
                    {
                        oChangeCrd.rtFrmCrdCode = poPara[nRow].ptFrmCrdCode;
                        oChangeCrd.rtToCrdCode = poPara[nRow].ptToCrdCode;
                        oChangeCrd.rtBchCode = poPara[nRow].ptBchCode;
                        oChangeCrd.rtStatus = "4";
                        aoChangeCrd.Add(oChangeCrd);
                        continue;
                    }

                    // ตรวจสอบจากรหัสบัตร เบิกใช้งานแล้วหรือยัง
                    //if (oCardOut.FTCrdStaLocate != "1")
                    //if (oCardOut.FTCrdStaShift != "1" && oCardOut.FTCrdStaType == "1")  //*Em 61-12-11  Pandora
                    if (oCardOut.FTCrdStaShift != "2" && oCardOut.FTCrdStaType == "1")  //*Em 62-01-12  Pandora StaShift 1:ยังไม่ได้เบิก 2:เบิกแล้ว
                    {
                        oChangeCrd.rtFrmCrdCode = poPara[nRow].ptFrmCrdCode;
                        oChangeCrd.rtToCrdCode = poPara[nRow].ptToCrdCode;
                        oChangeCrd.rtBchCode = poPara[nRow].ptBchCode;
                        oChangeCrd.rtStatus = "5";
                        aoChangeCrd.Add(oChangeCrd);
                        continue;
                    }

                    //// ตรวจสอบบัตรที่รับโอน เบิกใช้งานแล้วหรือยัง
                    //if (oCardIn.FTCrdStaLocate != "1")
                    //{
                    //    oChangeCrd.rtFrmCrdCode = poPara[nRow].ptFrmCrdCode;
                    //    oChangeCrd.rtToCrdCode = poPara[nRow].ptToCrdCode;
                    //    oChangeCrd.rtBchCode = poPara[nRow].ptBchCode;
                    //    oChangeCrd.rtStatus = "5";
                    //    aoChangeCrd.Add(oChangeCrd);
                    //    continue;
                    //}

                    // ตรวจสอบอายุเงิน ตรวจสอบอายุเงิน บัตร Form
                    // ตรวจสอบอายุเงิน ถ้า FNCtyExpirePeriod = 0 ไม่ต้องตรวจสอบ
                    if (oCardOut.FNCtyExpirePeriod != 0)
                    {
                        // ตรวจสอบกรณี ยังไม่เคยเติมเงิน ไม่ต้องตรวจสอบวัน Expire ของยอดเงิน
                        if (oCardOut.FDCrdLastTopup != null)
                        {
                            switch (oCardOut.FNCtyExpiredType)
                            {
                                case 1:
                                    dExpireTopup = Convert.ToDateTime(oCardOut.FDCrdLastTopup).AddHours(Convert.ToInt32(oCardOut.FNCtyExpirePeriod));
                                    break;
                                case 2:
                                    dExpireTopup = Convert.ToDateTime(oCardOut.FDCrdLastTopup).AddDays(Convert.ToInt32(oCardOut.FNCtyExpirePeriod));
                                    break;
                                case 3:
                                    dExpireTopup = Convert.ToDateTime(oCardOut.FDCrdLastTopup).AddMonths(Convert.ToInt32(oCardOut.FNCtyExpirePeriod));
                                    break;
                                case 4:
                                    //dExpireTopup = Convert.ToDateTime(oCardOut.FDCrdLastTopup).AddYears(Convert.ToInt32(oCardOut.FNCtyExpirePeriod));
                                    //*Em 61-12-10  Pandora
                                    if (oCardOut.FNCtyExpirePeriod == 9999 || (Convert.ToDateTime(oCardOut.FDCrdLastTopup).Year + oCardOut.FNCtyExpirePeriod) > 9999)
                                    {
                                        dExpireTopup = new DateTime((int)oCardOut.FNCtyExpirePeriod, Convert.ToDateTime(oCardOut.FDCrdLastTopup).Month, Convert.ToDateTime(oCardOut.FDCrdLastTopup).Day);
                                    }
                                    else
                                    {
                                        dExpireTopup = Convert.ToDateTime(oCardOut.FDCrdLastTopup).AddYears(Convert.ToInt32(oCardOut.FNCtyExpirePeriod));
                                    }
                                    //+++++++++++++++++++++
                                    break;
                            }

                            tFrmCrdTopupExp = dExpireTopup.ToString(@"yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));
                            if (DateTime.Parse(tNow) > DateTime.Parse(tFrmCrdTopupExp))
                            {
                                // ยอดเงินหมดอายุ ResetExpire
                                //bResetFrmCrdTopup = oResetExpire.C_SETbResetExpired(poPara[nRow].ptFrmCrdCode, poPara[nRow].ptBchCode, "", paoSysConfig);
                                bResetFrmCrdTopup = oResetExpire.C_SETbResetExpired(poPara[nRow].ptFrmCrdCode, poPara[nRow].ptBchCode, "", poPara[nRow].ptDocNoRef, null);  //*Em 61-12-11  Pandora
                                if (bResetFrmCrdTopup == false)
                                {
                                    oChangeCrd.rtFrmCrdCode = poPara[nRow].ptFrmCrdCode;
                                    oChangeCrd.rtToCrdCode = poPara[nRow].ptToCrdCode;
                                    oChangeCrd.rtBchCode = poPara[nRow].ptBchCode;
                                    oChangeCrd.rtStatus = "7";
                                    aoChangeCrd.Add(oChangeCrd);
                                    continue;
                                }
                            }
                        }
                    }

                    // ตรวจสอบอายุเงิน ตรวจสอบอายุเงิน บัตร To
                    // ตรวจสอบอายุเงิน ถ้า FNCtyExpirePeriod = 0 ไม่ต้องตรวจสอบ
                    if (oCardIn.FNCtyExpirePeriod != 0)
                    {
                        // ตรวจสอบกรณี ยังไม่เคยเติมเงิน ไม่ต้องตรวจสอบวัน Expire ของยอดเงิน
                        if (oCardIn.FDCrdLastTopup != null)
                        {
                            switch (oCardIn.FNCtyExpiredType)
                            {
                                case 1:
                                    dExpireTopup = Convert.ToDateTime(oCardOut.FDCrdLastTopup).AddHours(Convert.ToInt32(oCardIn.FNCtyExpirePeriod));
                                    break;
                                case 2:
                                    dExpireTopup = Convert.ToDateTime(oCardOut.FDCrdLastTopup).AddDays(Convert.ToInt32(oCardIn.FNCtyExpirePeriod));
                                    break;
                                case 3:
                                    dExpireTopup = Convert.ToDateTime(oCardOut.FDCrdLastTopup).AddMonths(Convert.ToInt32(oCardIn.FNCtyExpirePeriod));
                                    break;
                                case 4:
                                    //dExpireTopup = Convert.ToDateTime(oCardOut.FDCrdLastTopup).AddYears(Convert.ToInt32(oCardIn.FNCtyExpirePeriod));
                                    //*Em 61-12-10  Pandora
                                    if (oCardOut.FNCtyExpirePeriod == 9999 || (Convert.ToDateTime(oCardOut.FDCrdLastTopup).Year + oCardOut.FNCtyExpirePeriod) > 9999)
                                    {
                                        dExpireTopup = new DateTime((int)oCardOut.FNCtyExpirePeriod, Convert.ToDateTime(oCardOut.FDCrdLastTopup).Month, Convert.ToDateTime(oCardOut.FDCrdLastTopup).Day);
                                    }
                                    else
                                    {
                                        dExpireTopup = Convert.ToDateTime(oCardOut.FDCrdLastTopup).AddYears(Convert.ToInt32(oCardOut.FNCtyExpirePeriod));
                                    }
                                    //+++++++++++++++++++++
                                    break;
                            }

                            tToCrdTopupExp = dExpireTopup.ToString(@"yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));
                            if (DateTime.Parse(tNow) > DateTime.Parse(tToCrdTopupExp))
                            {
                                // ยอดเงินหมดอายุ ResetExpire
                                //bResetToCrdTopup = oResetExpire.C_SETbResetExpired(poPara[nRow].ptToCrdCode, poPara[nRow].ptBchCode, "", paoSysConfig);
                                bResetToCrdTopup = oResetExpire.C_SETbResetExpired(poPara[nRow].ptToCrdCode, poPara[nRow].ptBchCode, "", poPara[nRow].ptDocNoRef, null);    //*Em 61-12-11  Pandora
                                if (bResetToCrdTopup == false)
                                {
                                    oChangeCrd.rtFrmCrdCode = poPara[nRow].ptFrmCrdCode;
                                    oChangeCrd.rtToCrdCode = poPara[nRow].ptToCrdCode;
                                    oChangeCrd.rtBchCode = poPara[nRow].ptBchCode;
                                    oChangeCrd.rtStatus = "8";
                                    aoChangeCrd.Add(oChangeCrd);
                                    continue;
                                }
                            }

                            //*Em 62-01-09  Pandora 
                            //รหัสพนักงานของบัตรต้นทางและปลายทางไม่ตรงกัน
                            if (!string.Equals(oCardOut.FTCrdHolderID, oCardIn.FTCrdHolderID))
                            {
                                oChangeCrd.rtFrmCrdCode = poPara[nRow].ptFrmCrdCode;
                                oChangeCrd.rtToCrdCode = poPara[nRow].ptToCrdCode;
                                oChangeCrd.rtBchCode = poPara[nRow].ptBchCode;
                                oChangeCrd.rtStatus = "9";
                                aoChangeCrd.Add(oChangeCrd);
                                continue;
                            }

                            //มูลค่าคงเหลือของบัตรปลายทางไม่เท่ากับ 0
                            if (oCardIn.FCCrdValue != Convert.ToDecimal(0))
                            {
                                oChangeCrd.rtFrmCrdCode = poPara[nRow].ptFrmCrdCode;
                                oChangeCrd.rtToCrdCode = poPara[nRow].ptToCrdCode;
                                oChangeCrd.rtBchCode = poPara[nRow].ptBchCode;
                                oChangeCrd.rtStatus = "10";
                                aoChangeCrd.Add(oChangeCrd);
                                continue;
                            }
                            //+++++++++++++++++++++
                        }
                    }
                    #endregion

                    #region Process
                    oReqTnfCrd = new cmlReqTnfCrd();
                    oResTnfCrd = new cmlResTnfCrd();

                    oReqTnfCrd.ptFrmCrdCode = poPara[nRow].ptFrmCrdCode;
                    oReqTnfCrd.ptToCrdCode = poPara[nRow].ptToCrdCode;
                    oReqTnfCrd.ptBchCode = poPara[nRow].ptBchCode;
                    oReqTnfCrd.pcFrmCrdValue = oCardOut.FCCrdValue;
                    oReqTnfCrd.pcToCrdValue = oCardIn.FCCrdValue;
                    oReqTnfCrd.ptDocNoRef = poPara[nRow].ptDocNoRef;    //*[AnUBiS][][2019-02-13] - เพิ่มการบันทึก doc no ref. (Pandora)

                    oResTnfCrd = oTranfer.C_PUNoTnfCrd(oReqTnfCrd);
                    if (oResTnfCrd.rtCode != "1")
                    {
                        oChangeCrd.rtFrmCrdCode = poPara[nRow].ptFrmCrdCode;
                        oChangeCrd.rtToCrdCode = poPara[nRow].ptToCrdCode;
                        oChangeCrd.rtBchCode = poPara[nRow].ptBchCode;
                        oChangeCrd.rtStatus = "2";
                        aoChangeCrd.Add(oChangeCrd);
                        continue;
                    }

                    // สำเร็จ
                    oChangeCrd.rtFrmCrdCode = poPara[nRow].ptFrmCrdCode;
                    oChangeCrd.rtToCrdCode = poPara[nRow].ptToCrdCode;
                    oChangeCrd.rtBchCode = poPara[nRow].ptBchCode;
                    oChangeCrd.rtStatus = "1";
                    aoChangeCrd.Add(oChangeCrd);
                    #endregion
                }

                poResult = new cmlResChangeCard();
                poResult.rtCode = oMsg.tMS_RespCode1;
                poResult.rtDesc = oMsg.tMS_RespDesc1;
                poResult.raoChangeCard = aoChangeCrd;
                ptErrCode = "";
                ptErrDesc = "";
                return true;
            }
            catch (Exception oEx)
            {
                poResult = new cmlResChangeCard();
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
                oCardOut = null;
                oCardIn = null;
                oResetExpire = null;
                oTranfer = null;
                tNow = null;
                tFrmCrdTopupExp = null;
                tToCrdTopupExp = null;
                aoChangeCrd = null;
                oChangeCrd = null;
                oReqTnfCrd = null;
                oResTnfCrd = null;
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
        public bool C_DATbProcChangeSta(List<cmlReqChangeSta> poPara,
                    out string ptErrCode, out string ptErrDesc, out cmlResChangeSta poResult)
        {
            cSP oFunc = new cSP();
            cMS oMsg = new cMS();
            cDatabase oDatabase;
            StringBuilder oSql;
            int nRowEff;
            cmlTFNMCard oCard;
            DateTime dExpireTopup = DateTime.Now;
            string tNow;
            cResetExpired oResetExpire;
            bool bResetCrd = true;
            List<cmlResaoChangeSta> aoChangeSta = new List<cmlResaoChangeSta>();
            cmlResaoChangeSta oChangeSta;
            try
            {
                oSql = new StringBuilder();
                tNow = DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));
                oResetExpire = new cResetExpired();

                for (int nRow = 0; nRow < poPara.Count; nRow++)
                {
                    #region Get Card
                    //oCard = new cmlTFNMCard();
                    //oSql.Clear();
                    //oSql.AppendLine("SELECT C.FTCrdCode,C.FDCrdExpireDate");
                    //oSql.AppendLine(",ISNULL(C.FCCrdValue,0.0000) AS FCCrdValue");
                    //oSql.AppendLine(",ISNULL(T.FNCtyExpirePeriod,0) AS FNCtyExpirePeriod");
                    //oSql.AppendLine(",ISNULL(T.FNCtyExpiredType,0) AS FNCtyExpiredType,C.FDCrdLastTopup ");
                    //oSql.AppendLine("FROM TFNMCard C WITH(NOLOCK) LEFT JOIN TFNMCardType T ON (C.FTCtyCode=T.FTCtyCode) ");
                    //oSql.AppendLine("WHERE FTCrdCode='" + poPara[nRow].ptCrdCode + "'");
                    //oCard = oDatabase.C_DAToSqlQuery<cmlTFNMCard>(oSql.ToString(), nCmdTme);

                    oCard  = oFunc.SP_GEToCardByStored(poPara[nRow].ptCrdCode);
                    #endregion

                    oChangeSta = new cmlResaoChangeSta();
                    #region ตรวจสอบว่ามี รหัสบัตร หรือไม่
                    if (oCard == null)
                    {
                        oChangeSta.rtCrdCode = poPara[nRow].ptCrdCode;
                        oChangeSta.rtBchCode = poPara[nRow].ptBchCode;
                        oChangeSta.rtStatus = "2";
                        aoChangeSta.Add(oChangeSta);
                        continue;
                    }
                    #endregion

                    #region ยอดเงินหมดอายุ ResetExpire
                    //bResetCrd = oResetExpire.C_SETbResetExpired(poPara[nRow].ptCrdCode, poPara[nRow].ptBchCode, "", paoSysConfig);

                    //*[AnUBiS][][2019-02-21] - increment parameter card status.
                    bResetCrd = oResetExpire.C_SETbResetExpired(
                        poPara[nRow].ptCrdCode, poPara[nRow].ptBchCode, "", poPara[nRow].ptDocNoRef, poPara[nRow].ptCrdSta); //*Em 61-12-11  Pandora
                    if (bResetCrd == false)
                    {
                        oChangeSta.rtCrdCode = poPara[nRow].ptCrdCode;
                        oChangeSta.rtBchCode = poPara[nRow].ptBchCode;
                        oChangeSta.rtStatus ="3";
                        aoChangeSta.Add(oChangeSta);
                        continue;
                    }
                    #endregion

                    #region Process Update
                    oSql.Clear();
                    oSql.AppendLine("UPDATE TFNMCard WITH (ROWLOCK)SET FTCrdStaActive='" + poPara[nRow].ptCrdSta + "'");
                    oSql.AppendLine("WHERE FTCrdCode='" + poPara[nRow].ptCrdCode + "'");

                    try
                    {
                        oDatabase = new cDatabase();
                        nRowEff = oDatabase.C_GETnQuerySQL(oSql.ToString());
                        if (nRowEff == 0)
                        {
                            oChangeSta.rtCrdCode = poPara[nRow].ptCrdCode;
                            oChangeSta.rtBchCode = poPara[nRow].ptBchCode;
                            oChangeSta.rtStatus = "4";
                            aoChangeSta.Add(oChangeSta);
                            continue;
                        }
                    }
                    catch (EntityException oEtyExn)
                    {
                        switch (oEtyExn.HResult)
                        {
                            case -2146232060:
                                // Cannot connect database..
                                oChangeSta.rtCrdCode = poPara[nRow].ptCrdCode;
                                oChangeSta.rtBchCode = poPara[nRow].ptBchCode;
                                oChangeSta.rtStatus = "5";
                                aoChangeSta.Add(oChangeSta);
                                continue;
                        }
                    }

                    oChangeSta.rtCrdCode = poPara[nRow].ptCrdCode;
                    oChangeSta.rtBchCode = poPara[nRow].ptBchCode;
                    oChangeSta.rtStatus = "1";
                    aoChangeSta.Add(oChangeSta);
                    #endregion
                }

                poResult = new cmlResChangeSta();
                poResult.rtCode = oMsg.tMS_RespCode1;
                poResult.rtDesc = oMsg.tMS_RespDesc1;
                poResult.raoCancelCard = aoChangeSta;
                ptErrCode = "";
                ptErrDesc = "";
                return true;
            }
            catch (Exception oEx)
            {
                poResult = new cmlResChangeSta();
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
                oResetExpire = null;
                aoChangeSta = null;
                oChangeSta = null;
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
        public bool C_DATbProcCrdClrLst(List<cmlReqCardClearList> poPara,
                    out string ptErrCode, out string ptErrDesc, out cmlResCardClearList poResult)
        {
            cSP oFunc = new cSP();
            cMS oMsg = new cMS();
            DateTime dExpireTopup = DateTime.Now;
            string tNow;
            cResetExpired oResetExpire;
            List<cmlResaoCardClearList> raoCrdClrLst = new List<cmlResaoCardClearList>(); ;
            cmlResaoCardClearList roCrdClrLst;
            bool bResult;
            cmlTFNMCard oCard;
            try
            {
                tNow = DateTime.Now.ToString(@"yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));
                oResetExpire = new cResetExpired();
                for (int nRow = 0; nRow < poPara.Count; nRow++)
                {
                    roCrdClrLst = new cmlResaoCardClearList();
                    #region  Get ข้อมูลบัตร
                    //Get ข้อมูลบัตร
                    oCard = new cmlTFNMCard();
                    //  oCard = oFunc.SP_GEToCard(paoSysConfig, poPara[nRow].ptCrdCode);
                    oCard = oFunc.SP_GEToCardByStored(poPara[nRow].ptCrdCode);
                    #endregion

                    // ตรวจสอบว่ามี รหัสบัตร หรือไม่
                    if (oCard == null)
                    {
                        roCrdClrLst.rtCrdCode = poPara[nRow].ptCrdCode;
                        roCrdClrLst.rtStatus = "3";
                        raoCrdClrLst.Add(roCrdClrLst);
                        continue;
                    }

                    // ยอดเงินหมดอายุ ResetExpire
                    //bResult = oResetExpire.C_SETbResetExpired(poPara[nRow].ptCrdCode, poPara[nRow].ptBchCode, "", paoSysConfig);
                    bResult = oResetExpire.C_SETbResetExpired(poPara[nRow].ptCrdCode, poPara[nRow].ptBchCode, "", poPara[nRow].ptDocNoRef);   //*Em 61-12-11  Pandora
                    if (bResult == false)
                    {
                        roCrdClrLst.rtCrdCode = poPara[nRow].ptCrdCode;
                        roCrdClrLst.rtStatus = "2";
                        raoCrdClrLst.Add(roCrdClrLst);
                        continue;
                    }
                    else
                    {
                        roCrdClrLst.rtCrdCode = poPara[nRow].ptCrdCode;
                        roCrdClrLst.rtStatus = "1";
                        raoCrdClrLst.Add(roCrdClrLst);
                        continue;
                    }
                }

                poResult = new cmlResCardClearList();
                poResult.rtCode = oMsg.tMS_RespCode1;
                poResult.rtDesc = oMsg.tMS_RespDesc1;
                poResult.raoCardClearList = raoCrdClrLst;
                ptErrCode = "";
                ptErrDesc = "";
                return true;
            }
            catch (Exception oEx)
            {
                poResult = new cmlResCardClearList();
                ptErrCode = oMsg.tMS_RespCode900;
                ptErrDesc = oMsg.tMS_RespDesc900;
                return false;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                tNow = null;
                oResetExpire = null;
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
        public bool C_DATbProcCardNewList(List<cmlReqCardNewList> poPara,
                    out string ptErrCode, out string ptErrDesc, out cmlResCardNewList poResult)
        {
            cSP oFunc = new cSP();
            cMS oMsg = new cMS();
            DateTime dExpireTopup = DateTime.Now;
            List<cmlResaoCardNewList> raoCrdClrLst = new List<cmlResaoCardNewList>(); ;
            cmlResaoCardNewList roCrdClrLst;
            cmlTFNMCard oCard;
            int nRowEff;
            cDatabase oDatabase;
            StringBuilder oSql;
            DateTime dExpCrd = DateTime.Now;
            DateTime dNow = DateTime.Now;
            cmlTFNMCardType oCrdType;
            DataTable oTbl;
            int nStaINSOrUPD = 0;  // 0 Insert, 1 Update
            SqlParameter[] aoSqlParam;
            try
            {
                oDatabase = new cDatabase();
                oSql = new StringBuilder();

                for (int nRow = 0; nRow < poPara.Count; nRow++)
                {
                    nStaINSOrUPD = 0;
                    roCrdClrLst = new cmlResaoCardNewList();
                    #region  Get ข้อมูลบัตร
                    //Get ข้อมูลบัตร
                  //  oCard = new cmlTFNMCard();
                  //  oCard = oFunc.SP_GEToCard(paoSysConfig, poPara[nRow].ptCrdCode);
                    oCard = oFunc.SP_GEToCardByStored(poPara[nRow].ptCrdCode);
                    #endregion

                    // ตรวจสอบว่ามี รหัสบัตร หรือไม่
                    if (oCard == null)
                    {
                        nStaINSOrUPD = 0;
                        //คำนวณวันที่ Expire
                        oSql.Clear();
                        oSql.AppendLine("SELECT FNCtyExpiredType,FNCtyExpirePeriod,FTCtyCode,FCCtyDeposit");
                        oSql.AppendLine("FROM TFNMCardType WITH (NOLOCK) WHERE FTCtyCode='" + poPara[nRow].ptCtyCode + "'");
                        oCrdType = new cmlTFNMCardType();
                        // oCrdType = oDatabase.C_DAToSqlQuery<cmlTFNMCardType>(oSql.ToString(), nCmdTme);
                         oTbl = oDatabase.C_GEToQuerySQLTbl(oSql.ToString());

                        if (oTbl.Rows.Count > 0) {
                            var oItem = from DataRow oRow in oTbl.Rows
                                        select new cmlTFNMCardType()
                                        {
                                            FNCtyExpiredType = (int)oRow["FNCtyExpiredType"],
                                            FNCtyExpirePeriod = (Int64)oRow["FNCtyExpirePeriod"],
                                            FTCtyCode = (string)oRow["FTCtyCode"],
                                            FCCtyDeposit = (decimal)oRow["FCCtyDeposit"]
                                        };
                            oCrdType = oItem.FirstOrDefault();
                        }
                        
                        if (oCrdType == null)
                        {
                            roCrdClrLst = new cmlResaoCardNewList();
                            roCrdClrLst.rtCrdCode = poPara[nRow].ptCrdCode;
                            roCrdClrLst.rtCrdHolderID = poPara[nRow].ptCrdHolderID;
                            roCrdClrLst.rtStatus = "3";
                            raoCrdClrLst.Add(roCrdClrLst);
                            continue;
                        }
                        else
                        {
                            //switch (oCrdType.FNCtyExpiredType)
                            //{
                            //    case 1:
                            //        dExpCrd = dNow.AddHours(Convert.ToInt32(oCrdType.FNCtyExpirePeriod));
                            //        break;
                            //    case 2:
                            //        dExpCrd = dNow.AddDays(Convert.ToInt32(oCrdType.FNCtyExpirePeriod));
                            //        break;
                            //    case 3:
                            //        dExpCrd = dNow.AddMonths(Convert.ToInt32(oCrdType.FNCtyExpirePeriod));
                            //        break;
                            //    case 4:
                            //        //dExpCrd = dNow.AddYears(Convert.ToInt32(oCrdType.FNCtyExpirePeriod));
                            //        //*Em 61-12-10  Pandora
                            //        if (oCrdType.FNCtyExpirePeriod == 9999 || (dNow.Year + oCrdType.FNCtyExpirePeriod) > 9999)
                            //        {
                            //            dExpireTopup = new DateTime((int)oCrdType.FNCtyExpirePeriod, dNow.Month, dNow.Day);
                            //        }
                            //        else
                            //        {
                            //            dExpireTopup = Convert.ToDateTime(dNow).AddYears(Convert.ToInt32(oCrdType.FNCtyExpirePeriod));
                            //        }
                            //        //+++++++++++++++++++++
                            //        break;
                            //}
                        }

                        //tExpCrd = dExpCrd.ToString(@"yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));

                        //tExpCrd = "9999-12-31"; //*Em 61-12-18  Pandora
                        //// Case Insert
                        //oSql.Clear();
                        //oSql.AppendLine("BEGIN TRANSACTION ");
                        //oSql.AppendLine("  SAVE TRANSACTION Card ");
                        //oSql.AppendLine("  BEGIN TRY ");

                        //// DECLARE
                        //oSql.AppendLine("  DECLARE @pnLngID AS Integer --รหัสภาษา");
                        //oSql.AppendLine("  DECLARE @ptBchCode AS NVARCHAR(5)--รหัสสาขา");
                        //oSql.AppendLine("  DECLARE @ptCrdCode AS NVARCHAR(30)--รหัสบัตร");
                        //oSql.AppendLine("  DECLARE @ptCtyCode AS NVARCHAR(30)--รหัสประเภทบัตร");
                        //oSql.AppendLine("  DECLARE @ptCrdHolderID AS NVARCHAR(30)--รหัสผู้ถือบัตร");
                        //oSql.AppendLine("  DECLARE @ptDptCode AS NVARCHAR(30)--รหัสแผนก");
                        //oSql.AppendLine("  DECLARE @ptCrdName AS NVARCHAR(30)--ชื่อผู้ถือบัตร");
                        //oSql.AppendLine("  SET @pnLngID='" + poPara[nRow].pnLngID + "' ");
                        //oSql.AppendLine("  SET @ptBchCode='" + poPara[nRow].ptBchCode + "' ");
                        //oSql.AppendLine("  SET @ptCrdCode='" + poPara[nRow].ptCrdCode + "' ");
                        //oSql.AppendLine("  SET @ptCtyCode='" + poPara[nRow].ptCtyCode + "' ");
                        //oSql.AppendLine("  SET @ptCrdHolderID='" + poPara[nRow].ptCrdHolderID + "' ");
                        //oSql.AppendLine("  SET @ptDptCode='" + poPara[nRow].ptDptCode + "' ");
                        //oSql.AppendLine("  SET @ptCrdName='" + poPara[nRow].ptCrdName + "' ");

                        //// Insert TFNMCard
                        //oSql.AppendLine("  INSERT INTO TFNMCard WITH(ROWLOCK)");
                        //oSql.AppendLine("  (FTCrdCode,FDCrdStartDate,FDCrdExpireDate,FTCtyCode");
                        //oSql.AppendLine("  ,FCCrdDeposit,FTCrdHolderID,FTCrdRefID,FTCrdStaType");
                        //oSql.AppendLine("  ,FTDptCode,FTCrdStaShift,FTCrdStaActive,FDCreateOn,FTCreateBy)");
                        //oSql.AppendLine("  VALUES (@ptCrdCode,GETDATE(),'" + tExpCrd + "',@ptCtyCode");
                        //oSql.AppendLine("  ,'" + oCrdType.FCCtyDeposit +"',@ptCrdHolderID,'','2'");
                        ////oSql.AppendLine("  ,@ptDptCode,2,1,GETDATE(),'AdaPostStoreBack')");
                        //oSql.AppendLine("  ,@ptDptCode,1,1,GETDATE(),'AdaPostStoreBack')"); //StaShift 1:ยังไม่ได้เบิก 2:เบิกแล้ว       //*Em 62-01-12  Pandora

                        //// Insert TFNMCard_L
                        //oSql.AppendLine("  INSERT INTO TFNMCard_L WITH (ROWLOCK)");
                        //oSql.AppendLine("  (FTCrdCode,FNLngID,FTCrdName,FTCrdRmk)");
                        //oSql.AppendLine("  VALUES (@ptCrdCode,@pnLngID,@ptCrdName,'')");

                        //// Commit Command
                        //oSql.AppendLine("  COMMIT TRANSACTION Card");
                        //oSql.AppendLine("END TRY");

                        //oSql.AppendLine("BEGIN CATCH");
                        //oSql.AppendLine("  ROLLBACK TRANSACTION Card");
                        //oSql.AppendLine("END CATCH");
                    }
                    else
                    {
                        nStaINSOrUPD = 1;

                        //คำนวณวันที่ Expire
                        oSql.Clear();
                        oSql.AppendLine("SELECT FNCtyExpiredType,FNCtyExpirePeriod,FTCtyCode,FCCtyDeposit");
                        oSql.AppendLine("FROM TFNMCardType WITH (NOLOCK) WHERE FTCtyCode='" + poPara[nRow].ptCtyCode + "'");
                        oCrdType = new cmlTFNMCardType();
                        //oCrdType = oDatabase.C_DAToSqlQuery<cmlTFNMCardType>(oSql.ToString(), nCmdTme);
                        oTbl = oDatabase.C_GEToQuerySQLTbl(oSql.ToString());

                        if (oTbl.Rows.Count > 0)
                        {
                            var oItem = from DataRow oRow in oTbl.Rows
                                        select new cmlTFNMCardType()
                                        {
                                            FNCtyExpiredType = (int)oRow["FNCtyExpiredType"],
                                            FNCtyExpirePeriod = (Int64)oRow["FNCtyExpirePeriod"],
                                            FTCtyCode = (string)oRow["FTCtyCode"],
                                            FCCtyDeposit = (decimal)oRow["FCCtyDeposit"]
                                        };
                            oCrdType = oItem.FirstOrDefault();
                        }

                        if (oCrdType == null)
                        {
                            roCrdClrLst = new cmlResaoCardNewList();
                            roCrdClrLst.rtCrdCode = poPara[nRow].ptCrdCode;
                            roCrdClrLst.rtCrdHolderID = poPara[nRow].ptCrdHolderID;
                            roCrdClrLst.rtStatus = "3";
                            raoCrdClrLst.Add(roCrdClrLst);
                            continue;
                        }
                        else
                        {
                            //switch (oCrdType.FNCtyExpiredType)
                            //{
                            //    case 1:
                            //        dExpCrd = dNow.AddHours(Convert.ToInt32(oCrdType.FNCtyExpirePeriod));
                            //        break;
                            //    case 2:
                            //        dExpCrd = dNow.AddDays(Convert.ToInt32(oCrdType.FNCtyExpirePeriod));
                            //        break;
                            //    case 3:
                            //        dExpCrd = dNow.AddMonths(Convert.ToInt32(oCrdType.FNCtyExpirePeriod));
                            //        break;
                            //    case 4:
                            //        //dExpCrd = dNow.AddYears(Convert.ToInt32(oCrdType.FNCtyExpirePeriod));
                            //        //*Em 61-12-10  Pandora
                            //        if (oCard.FNCtyExpirePeriod == 9999 || (dNow.Year + oCard.FNCtyExpirePeriod) > 9999)
                            //        {
                            //            dExpireTopup = new DateTime((int)oCard.FNCtyExpirePeriod, dNow.Month, dNow.Day);
                            //        }
                            //        else
                            //        {
                            //            dExpireTopup = Convert.ToDateTime(dNow).AddYears(Convert.ToInt32(oCard.FNCtyExpirePeriod));
                            //        }
                            //        //+++++++++++++++++++++++
                            //        break;
                            //}
                        }

                        ////tExpCrd = dExpCrd.ToString(@"yyyy-MM-dd HH:mm:ss", new CultureInfo("en-US"));
                        //tExpCrd = "9999-12-31"; //*Em 61-12-18  Pandora
                        //// Case Update
                        //oSql.Clear();
                        //oSql.AppendLine("BEGIN TRANSACTION ");
                        //oSql.AppendLine("  SAVE TRANSACTION Card ");
                        //oSql.AppendLine("  BEGIN TRY ");

                        //// DECLARE
                        //oSql.AppendLine("  DECLARE @pnLngID AS Integer --รหัสภาษา");
                        //oSql.AppendLine("  DECLARE @ptBchCode AS NVARCHAR(5)--รหัสสาขา");
                        //oSql.AppendLine("  DECLARE @ptCrdCode AS NVARCHAR(30)--รหัสบัตร");
                        //oSql.AppendLine("  DECLARE @ptCtyCode AS NVARCHAR(30)--รหัสประเภทบัตร");
                        //oSql.AppendLine("  DECLARE @ptCrdHolderID AS NVARCHAR(30)--รหัสผู้ถือบัตร");
                        //oSql.AppendLine("  DECLARE @ptDptCode AS NVARCHAR(30)--รหัสแผนก");
                        //oSql.AppendLine("  DECLARE @ptCrdName AS NVARCHAR(30)--ชื่อผู้ถือบัตร");
                        //oSql.AppendLine("  SET @pnLngID='" + poPara[nRow].pnLngID + "' ");
                        //oSql.AppendLine("  SET @ptBchCode='" + poPara[nRow].ptBchCode + "' ");
                        //oSql.AppendLine("  SET @ptCrdCode='" + poPara[nRow].ptCrdCode + "' ");
                        //oSql.AppendLine("  SET @ptCtyCode='" + poPara[nRow].ptCtyCode + "' ");
                        //oSql.AppendLine("  SET @ptCrdHolderID='" + poPara[nRow].ptCrdHolderID + "' ");
                        //oSql.AppendLine("  SET @ptDptCode='" + poPara[nRow].ptDptCode + "' ");
                        //oSql.AppendLine("  SET @ptCrdName='" + poPara[nRow].ptCrdName + "' ");

                        //// Update TFNMCard
                        //oSql.AppendLine("  UPDATE TFNMCard WITH(ROWLOCK) SET");
                        //oSql.AppendLine("  FDCrdStartDate=GETDATE()");
                        //oSql.AppendLine("  ,FDCrdExpireDate='" + tExpCrd + "'");
                        //oSql.AppendLine("  ,FTCtyCode=@ptCtyCode");
                        //oSql.AppendLine("  ,FCCrdDeposit='" + oCrdType.FCCtyDeposit + "'");
                        //oSql.AppendLine("  ,FTCrdHolderID=@ptCrdHolderID");
                        //oSql.AppendLine("  ,FTCrdRefID=NULL");
                        //oSql.AppendLine("  ,FTCrdStaType='1'");
                        //oSql.AppendLine("  ,FTDptCode=@ptDptCode");
                        //oSql.AppendLine("  ,FDLastUpdOn=GETDATE()");
                        //oSql.AppendLine("  ,FTLastUpdBy='AdaPostStoreBack'");
                        //oSql.AppendLine("  WHERE FTCrdCode=@ptCrdCode");

                        //// Update TFNMCard_L
                        //oSql.AppendLine("  UPDATE TFNMCard_L WITH (ROWLOCK) SET FTCrdName=@ptCrdName,FTCrdRmk=''");
                        //oSql.AppendLine("  WHERE FNLngID = 1 AND FTCrdCode=@ptCrdCode");

                        //// Commit Command
                        //oSql.AppendLine("  COMMIT TRANSACTION Card");
                        //oSql.AppendLine("END TRY");

                        //oSql.AppendLine("BEGIN CATCH");
                        //oSql.AppendLine("  ROLLBACK TRANSACTION Card");
                        //oSql.AppendLine("END CATCH");
                    }

                    #region Processs Card
                    try
                    {
                        //nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);
                        // nRowEff = oDatabase.C_GETnQuerySQL(oSql.ToString());
                        aoSqlParam = new SqlParameter[] {
                            new SqlParameter ("@pnLngID", SqlDbType.Int){ Value = poPara[nRow].pnLngID },
                            new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 5){ Value = poPara[nRow].ptBchCode },
                            new SqlParameter ("@ptCrdCode", SqlDbType.VarChar,20){ Value = poPara[nRow].ptCrdCode },
                            new SqlParameter ("@ptCtyCode", SqlDbType.VarChar, 30){ Value = poPara[nRow].ptCtyCode },
                            new SqlParameter ("@ptCrdHolderID", SqlDbType.VarChar, 30){ Value = poPara[nRow].ptCrdHolderID },
                            new SqlParameter ("@ptDptCode", SqlDbType.VarChar, 30){ Value = poPara[nRow].ptDptCode },
                            new SqlParameter ("@ptCrdName", SqlDbType.VarChar,30){ Value = poPara[nRow].ptCrdName },
                            new SqlParameter ("@pcCtyDeposit", SqlDbType.Decimal){ Value = oCrdType.FCCtyDeposit }
                        };

                        if (nStaINSOrUPD == 0)
                        {
                            //Case Insert
                            nRowEff = oDatabase.C_GETnExecuteSqlStored("STP_PRCnCardNew", aoSqlParam);
                        }
                        else {
                            //Case Update
                            nRowEff = oDatabase.C_GETnExecuteSqlStored("STP_PRCnCardUPD", aoSqlParam);
                        }


                        if (nRowEff == 0)
                        {
                            roCrdClrLst = new cmlResaoCardNewList();
                            roCrdClrLst.rtCrdCode = poPara[nRow].ptCrdCode;
                            roCrdClrLst.rtCrdHolderID = poPara[nRow].ptCrdHolderID;
                            roCrdClrLst.rtStatus = "2";
                            raoCrdClrLst.Add(roCrdClrLst);
                            continue;
                        }
                    }
                    catch (EntityException oEtyExn)
                    {
                        switch (oEtyExn.HResult)
                        {
                            case -2146232060:
                                //// Cannot connect database..
                                roCrdClrLst = new cmlResaoCardNewList();
                                roCrdClrLst.rtCrdCode = poPara[nRow].ptCrdCode;
                                roCrdClrLst.rtCrdHolderID = poPara[nRow].ptCrdHolderID;
                                roCrdClrLst.rtStatus = "4";
                                raoCrdClrLst.Add(roCrdClrLst);
                                continue;
                        }
                    }
                    #endregion

                    roCrdClrLst = new cmlResaoCardNewList();
                    roCrdClrLst.rtCrdCode = poPara[nRow].ptCrdCode;
                    roCrdClrLst.rtCrdHolderID = poPara[nRow].ptCrdHolderID;
                    roCrdClrLst.rtStatus = "1";
                    raoCrdClrLst.Add(roCrdClrLst);
                }

                poResult = new cmlResCardNewList();
                poResult.rtCode = oMsg.tMS_RespCode1;
                poResult.rtDesc = oMsg.tMS_RespDesc1;
                poResult.raoCardNewList = raoCrdClrLst;
                ptErrCode = "";
                ptErrDesc = "";
                return true;
            }
            catch (Exception oEx)
            {
                poResult = new cmlResCardNewList();
                ptErrCode = oMsg.tMS_RespCode900;
                ptErrDesc = oMsg.tMS_RespDesc900;
                return false;
            }
            finally
            {
                oFunc = null;
                oMsg = null;
                oCard = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}