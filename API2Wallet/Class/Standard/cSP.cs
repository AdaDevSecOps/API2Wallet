using API2Wallet.Models;
using API2Wallet.Models.WebService.Response.SpotCheck;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Text;

namespace API2Wallet.Class.Standard
{
    /// <summary>
    /// 
    /// </summary>
    public class cSP
    {
        /// <summary>
        /// 
        /// </summary>
        public const int SP_nCmdTimeOut = 300;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptSql"></param>
        /// <param name="poException"></param>
        /// <returns></returns>
        public DataTable SP_GEToDataQuery(string ptSql, ref Exception poException)
        {
            poException = null;
            System.Data.SqlClient.SqlConnection oDbCon = new System.Data.SqlClient.SqlConnection();
            System.Data.SqlClient.SqlDataAdapter oDbAdt;
            DataTable oDbTbl = new DataTable();
            try
            {
                oDbCon.ConnectionString = SP_GETtConn();
                oDbCon.Open();
                oDbTbl = new DataTable();
                oDbAdt = new System.Data.SqlClient.SqlDataAdapter(ptSql, oDbCon);
                oDbAdt.SelectCommand.CommandTimeout = SP_nCmdTimeOut;
                oDbAdt.Fill(oDbTbl);

                return oDbTbl;
            }
            catch (Exception ex)
            {
                poException = ex;
                return oDbTbl;
            }
            finally
            {
                if (oDbCon.State == ConnectionState.Open)
                    oDbCon.Close();
                oDbCon = null/* TODO Change to default(_) if this is not a reference type */;
                oDbAdt = null/* TODO Change to default(_) if this is not a reference type */;
                oDbTbl = null/* TODO Change to default(_) if this is not a reference type */;
            }
        }

        /// <summary>
        /// Get Connection
        /// </summary>
        /// <returns>*CH 14-12-2016</returns>
        public string SP_GETtConn()
        {
            string tConn = "";
            try
            {
                //*Ton 64-05-24
                //tConn = new AdaFCEntities().Database.Connection.ConnectionString.ToString();
                tConn = cAppSettings.tConnectionString;
            }
            // Dim atConn As String() = ConfigurationManager.ConnectionStrings("AdaAccFCEntities").ConnectionString.Split("=")
            // If atConn.Count = 10 Then
            // tConn = Right(atConn(3), atConn(3).Length - 1)
            // tConn &= "=" & atConn(4)
            // tConn &= "=" & atConn(5)
            // tConn &= "=" & atConn(6)
            // tConn &= "=" & atConn(7)
            // tConn &= "=" & atConn(8)
            // tConn &= "=" & Left(atConn(9), atConn(9).Length - 1)
            // End If
            catch (Exception ex)
            {
            }
            return tConn;
        }

        /// <summary>
        ///     Validate model.
        /// </summary>
        /// 
        /// <param name="ptModelErr">ref Error message.</param>
        /// <param name="poModelState">Parameter model.</param>
        /// 
        /// <returns>
        ///     true : validate pass.<br/>
        ///     false : validate false.
        /// </returns>
        public bool SP_CHKbParaModel(ref string ptModelErr, ModelStateDictionary poModelState)
        {
            try
            {
                if (poModelState.IsValid)
                {
                    // Validate pass.
                    return true;
                }
                else
                {
                    // Validate false.
                    IEnumerable<string> atErrList = from oState in poModelState.Values
                                                    from oError in oState.Errors
                                                    where !string.IsNullOrEmpty(oError.ErrorMessage)
                                                    select oError.ErrorMessage;

                    ptModelErr = string.Join("|", atErrList);
                }
            }
            catch (Exception)
            {

            }

            return false;
        }

        /// <summary>
        ///     Load configuration.
        /// </summary>
        /// 
        /// <returns>
        ///     Configuration.
        /// </returns>
        public List<cmlTSysConfig> SP_SYSaLoadConfiguration()
        {
            cDatabase oDatabase;
            List<cmlTSysConfig> aoSysConfig;
            StringBuilder oSql = new StringBuilder();
            var oCache = MemoryCache.Default;
            var oCM_cachePolicty = new CacheItemPolicy();
            try
            {

                //[Pong][2019-05-22][Comment code]
                //oCM_cachePolicty.AbsoluteExpiration = DateTime.Now.AddHours(1);
                //if (oCache.Get("cache_Config") == null)
                //{
                //    oSql.Clear();
                //    oSql.AppendLine("SELECT FTSysCode,FTSysSeq,FTSysStaUsrValue,FTSysStaUsrRef,FTSysKey ");
                //    oSql.AppendLine("FROM TSysConfig WITH(NOLOCK) ");
                //    oSql.AppendLine("WHERE FTSysCode='PAPI2Wallet' ");
                //    oSql.AppendLine("ORDER BY FTSysSeq");
                //    oDatabase = new cDatabase();
                //    aoSysConfig = oDatabase.C_DATaSqlQuery<cmlTSysConfig>(oSql.ToString());
                //    oCache.Set("cache_Config", aoSysConfig, oCM_cachePolicty);
                //}
                //aoSysConfig = (List<cmlTSysConfig>)oCache.Get("cache_Config");

                aoSysConfig = new List<cmlTSysConfig>();
                //aoSysConfig = (List<cmlTSysConfig>)oCache.Get("cache_Config");
                //oCM_cachePolicty.AbsoluteExpiration = DateTime.Now.AddHours(1);
                //if (aoSysConfig == null || aoSysConfig.Count == 0)
                //{
                //    oSql.Clear();
                //    oSql.AppendLine("SELECT FTSysCode,FTSysSeq,FTSysStaUsrValue,FTSysStaUsrRef,FTSysKey ");
                //    oSql.AppendLine("FROM TSysConfig WITH(NOLOCK) ");
                //    oSql.AppendLine("WHERE FTSysCode='PAPI2Wallet' ");
                //    oSql.AppendLine("ORDER BY FTSysSeq");
                //    oDatabase = new cDatabase();
                //    aoSysConfig = oDatabase.C_DATaSqlQuery<cmlTSysConfig>(oSql.ToString());
                //    oCache.Set("cache_Config", aoSysConfig, oCM_cachePolicty);
                //}
                //aoSysConfig = (List<cmlTSysConfig>)oCache.Get("cache_Config");

                return aoSysConfig;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                oDatabase = null;
                oCache = null;
                oSql = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            
        }

        /// <summary>
        ///     Get configuration from memory.
        /// </summary>
        /// 
        /// <typeparam name="T">Type data user value.</typeparam>
        /// <param name="poUsrValue">ref Data user value.</param>
        /// <param name="poDefValue">Default data user value.</param>
        /// <param name="paoSysConfig">Valiable TSysConfig in memory.</param>
        /// <param name="ptSysSeq">Sequence.</param>
        public void SP_DATxGetConfigurationFromMem<T>(ref T poUsrValue, T poDefValue, List<cmlTSysConfig> paoSysConfig, string ptSysSeq)
        {
            try
            {
                // UsrValue.
                try
                {
                    poUsrValue = (T)Convert.ChangeType(paoSysConfig.Where(
                        oItem => string.Equals(oItem.FTSysSeq, ptSysSeq)).Select(oItem => oItem.FTSysStaUsrValue).FirstOrDefault(), typeof(T));
                }
                catch (Exception)
                {
                    poUsrValue = poDefValue;
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        ///     Check allow to user function in range time.
        /// </summary>
        /// 
        /// <param name="paoSysConfig">Configuration.</param>
        /// 
        /// <returns>
        ///     true : Allow to use.<br/>
        ///     false : Not allow to use.
        /// </returns>
        public bool SP_CHKbAllowRangeTime(List<cmlTSysConfig> paoSysConfig)
        {
            TimeSpan oTmeStart, oTmeEnd, oTmeNow;
            string tTmeStart, tTmeEnd, tTmeNow;

            try
            {
                //oTmeNow = DateTime.Now.TimeOfDay;
                //tTmeNow = oTmeNow.ToString("hh\\:mm");
                //tTmeStart = "";
                //tTmeEnd = "";

                //SP_DATxGetConfigurationFromMem<string, string>(ref tTmeStart, tTmeNow, ref tTmeEnd, tTmeNow, paoSysConfig, "002");

                //oTmeStart = TimeSpan.Parse(tTmeStart);
                //oTmeEnd = TimeSpan.Parse(tTmeEnd);

                //// Check time use function.
                //if ((oTmeNow >= oTmeStart) && (oTmeNow <= oTmeEnd))
                //{
                //    return true;
                //}

                // Default return True ไปก่อน
                return true;
            }
            catch (Exception)
            {

            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            return false;
        }

        /// <summary>
        ///     Check key API.
        /// </summary>
        /// 
        /// <param name="ptKeyApi">ref Key API.</param>
        /// <param name="ptFuncName">Function name.</param>
        /// <param name="poHttpContext">HttpContext.</param>
        /// <param name="paoSysConfig">Valiable TSysConfig in memory.</param>
        /// 
        /// <returns>
        ///     true : Verify key API pass.
        ///     false : Verify key API false.
        /// </returns>
        public bool SP_CHKbKeyApi(string ptFuncName, HttpContext poHttpContext)
        {
            //*Ton 64-05-24
            //NameValueCollection oReqHeaders;
            IHeaderDictionary oReqHeaders;

            string tXKeyApi;
            StringBuilder oSql = new StringBuilder();
            cmlTCNMAgency oAgency = new cmlTCNMAgency();
            cDatabase oDatabase;
            try
            {
                oDatabase = new cDatabase();
                //*Ton 64-05-24
                //oReqHeaders = poHttpContext.Request.Headers;
                //tXKeyApi = oReqHeaders.Get("X-Api-Key");
                oReqHeaders = poHttpContext.Request.Headers;
                tXKeyApi = oReqHeaders["X-Api-Key"];

                oSql.Clear();
                oSql.AppendLine("SELECT TOP 1 FTAgnKeyAPI FROM TCNMAgency ");
                oSql.AppendLine("WHERE FTAgnKeyAPI='" + tXKeyApi + "' AND FTAgnStaActive='1'");
                oAgency = oDatabase.C_DAToSqlQuery<cmlTCNMAgency>(oSql.ToString());

                if (oAgency != null) 
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception oEx)
            {
                return false;
            }
            finally
            {
                oAgency = null;
                oDatabase = null;
                oReqHeaders = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

       /// <summary>
       /// Get ข้อมูลบัตร
       /// </summary>
       /// <param name="paoSysConfig"></param>
       /// <param name="ptCrdCode"></param>
       /// <param name="pnLngID"></param>
       /// <returns></returns>
        public cmlTFNMCard SP_GEToCard(List<cmlTSysConfig> paoSysConfig, string ptCrdCode,int pnLngID = 0)
        {
            cDatabase oDatabase;
            int nConTme, nCmdTme;
            cSP oFunc = new cSP();
            StringBuilder oSql;
            List<cmlTFNMCard> aoCard;
            try
            {
                oSql = new StringBuilder();
                //nConTme = 0;
                //oFunc.SP_DATxGetConfigurationFromMem<int>(ref nConTme, cCS.nCS_ConTme, paoSysConfig, "002");
                //nCmdTme = 0;
                //oFunc.SP_DATxGetConfigurationFromMem<int>(ref nCmdTme, cCS.nCS_CmdTme, paoSysConfig, "003");

                nConTme = cCS.nCS_ConTme;
                nCmdTme = cCS.nCS_CmdTme;
                oDatabase = new cDatabase(nConTme);

                aoCard = new List<cmlTFNMCard>();
                oSql.Clear();
                //oSql.AppendLine("SELECT ");
                oSql.AppendLine("SELECT TOP 1");    //*Em 62-05-29  Pandora
                oSql.AppendLine("ISNULL(M.FCCrdValue,0.00) AS FCCrdValue");
                oSql.AppendLine(",ISNULL(M.FNCrdTxnPrcAdj,0) AS FNCrdTxnPrcAdj");
                oSql.AppendLine(",ISNULL(M.FCCrdDeposit,0.00) AS FCCrdDeposit");
                oSql.AppendLine(",ISNULL(M.FCCrdDepositPdt,0.00) AS FCCrdDepositPdt");
                oSql.AppendLine(",ISNULL((ISNULL(M.FCCrdValue,0.00) - (ISNULL(M.FCCrdDeposit,0.00)+ISNULL(M.FCCrdDepositPdt,0.00))),0.00) AS cAvailable");
                //oSql.AppendLine(",L.FTCrdName");
                //oSql.AppendLine(",TFNMCardType_L.FTCtyName");
                oSql.AppendLine(",ISNULL(L.FTCrdName,(SELECT TOP 1 FTCrdName FROM TFNMCard_L WITH(NOLOCK) WHERE FTCrdCode = M.FTCrdCode)) AS FTCrdName");   //*Em 62-05-29  Pandora
                oSql.AppendLine(",ISNULL(TFNMCardType_L.FTCtyName,(SELECT TOP 1 FTCtyName FROM TFNMCardType_L WITH(NOLOCK) WHERE FTCtyCode = T.FTCtyCode)) AS FTCtyName");  //*Em 62-05-29  Pandora
                oSql.AppendLine(",M.FDCrdExpireDate");
                oSql.AppendLine(",M.FDCrdLastTopup");
                oSql.AppendLine(",ISNULL(T.FCCtyTopUpAuto,0.00) AS FCCtyTopUpAuto");
                oSql.AppendLine(",T.FNCtyExpiredType");
                oSql.AppendLine(",T.FNCtyExpirePeriod");
                //oSql.AppendLine(",L.FNLngID");
                oSql.AppendLine(",ISNULL(L.FNLngID," + pnLngID +") AS FNLngID");   //*Em 62-06-04  Pandora
                //oSql.AppendLine(",M.FTCrdStaLocate");
                oSql.AppendLine(",M.FTCrdStaShift");   //*Em 61-12-11 Pandora
                oSql.AppendLine(",M.FTCrdHolderID");    //*Em 61-12-13  Pandora
                oSql.AppendLine(",M.FTCrdStaActive");    //*[AnUBiS][][2019-02-21] - Pandora
                oSql.AppendLine("FROM TFNMCard M WITH (NOLOCK)");
                //oSql.AppendLine("LEFT JOIN TFNMCard_L L WITH (NOLOCK) ON M.FTCrdCode = L.FTCrdCode");
                oSql.AppendLine("LEFT JOIN TFNMCard_L L WITH (NOLOCK) ON M.FTCrdCode = L.FTCrdCode AND L.FNLngID = " + pnLngID);    //*Em 62-05-29  Pandora
                oSql.AppendLine("LEFT JOIN TFNMCardType  T WITH (NOLOCK) ON M.FTCtyCode= T.FTCtyCode");
                //oSql.AppendLine("LEFT JOIN TFNMCardType_L WITH (NOLOCK) ON T.FTCtyCode= TFNMCardType_L.FTCtyCode  AND L.FNLngID = TFNMCardType_L.FNLngID");
                oSql.AppendLine("LEFT JOIN TFNMCardType_L WITH (NOLOCK) ON T.FTCtyCode= TFNMCardType_L.FTCtyCode  AND TFNMCardType_L.FNLngID = " + pnLngID);    //*Em 62-05-29  Pandora
                oSql.AppendLine("WHERE M.FTCrdCode='" + ptCrdCode + "'");
                //oSql.AppendLine("AND  L.FNLngID=" + pnLngID + "");
                //oSql.AppendLine("AND TFNMCardType_L.FNLngID=" + pnLngID + "");
                aoCard = oDatabase.C_DATaSqlQuery<cmlTFNMCard>(oSql.ToString(), nCmdTme);

                //if (pnLngID != 0)
                //{
                //    //var oCard = from oCrd in aoCard
                //    //            where oCrd.FNLngID == pnLngID
                //    //            select oCrd;
                //    //if (oCard != null)
                //    //{
                //    //    return oCard.FirstOrDefault();
                //    //}

                //    //*Em 61-12-14  Pandora
                //    if (aoCard.Count == 1)
                //    {
                //        return aoCard.FirstOrDefault();
                //    }
                //    //else
                //    //{
                //    //    var oCard = from oCrd in aoCard
                //    //                where oCrd.FNLngID == pnLngID
                //    //                select oCrd;
                //    //    if (oCard != null)
                //    //    {
                //    //        return oCard.FirstOrDefault();
                //    //    }
                //    //}
                //    //+++++++++++++++++++++
                //}
               
                return aoCard.FirstOrDefault();
            }
            catch (Exception oExn)
            {
                return null;
            }
            finally
            {
                oDatabase = null;
                oFunc = null;
                oSql = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pnTxnOffline"></param>
        /// <param name="ptCrdCode"></param>
        /// <param name="pnLngID"></param>
        /// <param name="pcAvailable"></param>
        /// <returns></returns>
        public cmlResSpotChk SP_GEToValueCard(int pnTxnOffline,string ptCrdCode,int pnLngID, decimal pcAvailable)
        {
            cmlTFNMCard oCard;
            cmlResSpotChk oResult;
            cMS oMsg;
            try
            {
                oResult = new cmlResSpotChk();
                oCard = new cmlTFNMCard();
                oMsg = new cMS();
                oCard = SP_GEToCardByStored(ptCrdCode,pnLngID);
                if (oCard != null)
                {
                    if (pnTxnOffline == oCard.FNCrdTxnPrcAdj || pnTxnOffline == 0)
                    {
                        oResult.rcTxnValue = oCard.FCCrdValue;
                        oResult.rcCrdDeposit = oCard.FCCrdDeposit;
                        oResult.rcCrdDepositPdt = oCard.FCCrdDepositPdt;
                        oResult.rcTxnValueAvb = oCard.cAvailable;
                        //oResult.rtCrdName = oCard.FTCrdName;
                        //oResult.rtCtyName = oCard.FTCtyName;
                      //  oResult.rdCrdExpireDate = oCard.FDCrdExpireDate;
                      //  oResult.rdCrdLastTopup = oCard.FDCrdLastTopup;
                        oResult.rnTxnOffline = 0;
                        //oResult.rnCtyExpirePeriod = Convert.ToInt32(oCard.FNCtyExpirePeriod);
                        //oResult.rnCtyExpiredType = (int)oCard.FNCtyExpiredType;
                        //oResult.rtCode = oMsg.tMS_RespCode1;
                        //oResult.rtDesc = oMsg.tMS_RespDesc1;
                        //oResult.rtCrdHolderID = oCard.FTCrdHolderID;    //*Em 61-12-13  Pandora
                    }
                    else
                    {
                        oResult.rcTxnValueAvb = pcAvailable;
                        //oResult.rtCrdName = oCard.FTCrdName;
                        //oResult.rtCtyName = oCard.FTCtyName;
                      //  oResult.rdCrdExpireDate = oCard.FDCrdExpireDate;
                     //  oResult.rdCrdLastTopup = oCard.FDCrdLastTopup;
                        oResult.rnTxnOffline = pnTxnOffline;
                        //oResult.rnCtyExpirePeriod = Convert.ToInt32(oCard.FNCtyExpirePeriod);
                        //oResult.rnCtyExpiredType = (int)oCard.FNCtyExpiredType;
                        //oResult.rtCode = oMsg.tMS_RespCode1;
                        //oResult.rtDesc = oMsg.tMS_RespDesc1;
                        //oResult.rtCrdHolderID = oCard.FTCrdHolderID;    //*Em 61-12-13  Pandora
                    }

                    oResult.rtCrdName = oCard.FTCrdName;
                    oResult.rtCtyName = oCard.FTCtyName;
                    oResult.rdCrdExpireDate = oCard.FDCrdExpireDate;
                    oResult.rdCrdLastTopup = oCard.FDCrdLastTopup;
                    oResult.rnCtyExpirePeriod = Convert.ToInt32(oCard.FNCtyExpirePeriod);
                    oResult.rnCtyExpiredType = (int)oCard.FNCtyExpiredType;
                    oResult.rtCode = oMsg.tMS_RespCode1;
                    oResult.rtDesc = oMsg.tMS_RespDesc1;
                    oResult.rtCrdHolderID = oCard.FTCrdHolderID;    //*Em 61-12-13  Pandora
                    oResult.rtCrdStaActive = oCard.FTCrdStaActive;  //*[AnUBiS][][2019-02-21] - Pandora.

                    return oResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception oExn)
            {
                return null;
            }
            finally
            {
                oCard = null;
                oResult = null;
                oMsg = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paoSysConfig"></param>
        /// <param name="ptCrdCode"></param>
        /// <param name="pnLngID"></param>
        /// <returns></returns>
        public cmlTFNMCard SP_GEToCardNew(string ptCrdCode, int pnLngID = 0)
        {
            cDatabase oDatabase;
            cSP oFunc = new cSP();
            StringBuilder oSql;
            List<cmlTFNMCard> aoCard;
            DataTable oTbl;
            try
            {
                oSql = new StringBuilder();
                oDatabase = new cDatabase();
                oTbl = new DataTable();
                aoCard = new List<cmlTFNMCard>();
                oSql.Clear();
                oSql.AppendLine("SELECT ");
                oSql.AppendLine("ISNULL(M.FCCrdValue,0.00) AS FCCrdValue");
                oSql.AppendLine(",ISNULL(M.FNCrdTxnPrcAdj,0) AS FNCrdTxnPrcAdj");
                oSql.AppendLine(",ISNULL(M.FCCrdDeposit,0.00) AS FCCrdDeposit");
                oSql.AppendLine(",ISNULL(M.FCCrdDepositPdt,0.00) AS FCCrdDepositPdt");
                oSql.AppendLine(",ISNULL((ISNULL(M.FCCrdValue,0.00) - (ISNULL(M.FCCrdDeposit,0.00)+ISNULL(M.FCCrdDepositPdt,0.00))),0.00) AS cAvailable");
                oSql.AppendLine(",L.FTCrdName");
                oSql.AppendLine(",TFNMCardType_L.FTCtyName");
                oSql.AppendLine(",M.FDCrdExpireDate");
                oSql.AppendLine(",M.FDCrdLastTopup");
                oSql.AppendLine(",ISNULL(T.FCCtyTopUpAuto,0.00) AS FCCtyTopUpAuto");
                oSql.AppendLine(",T.FNCtyExpiredType");
                oSql.AppendLine(",T.FNCtyExpirePeriod");
                oSql.AppendLine(",L.FNLngID");
                oSql.AppendLine(",M.FTCrdStaShift");  
                oSql.AppendLine(",M.FTCrdHolderID");  
                oSql.AppendLine(",M.FTCrdStaActive");  
                oSql.AppendLine("FROM TFNMCard M WITH (NOLOCK)");
                oSql.AppendLine("LEFT JOIN TFNMCard_L L WITH (NOLOCK) ON M.FTCrdCode = L.FTCrdCode");
                oSql.AppendLine("LEFT JOIN TFNMCardType  T WITH (NOLOCK) ON M.FTCtyCode= T.FTCtyCode");
                oSql.AppendLine("LEFT JOIN TFNMCardType_L WITH (NOLOCK) ON T.FTCtyCode= TFNMCardType_L.FTCtyCode  AND L.FNLngID = TFNMCardType_L.FNLngID");
                oSql.AppendLine("WHERE M.FTCrdCode='" + ptCrdCode + "'");
                //oSql.AppendLine("AND  L.FNLngID=" + pnLngID + "");
                //oSql.AppendLine("AND TFNMCardType_L.FNLngID=" + pnLngID + "");
                // aoCard = oDatabase.C_DATaSqlQuery<cmlTFNMCard>(oSql.ToString(), nCmdTme);
                oTbl = oDatabase.C_GEToQuerySQLTbl(oSql.ToString());

                var oItem = from DataRow oRow in oTbl.Rows
                            select new cmlTFNMCard()
                            {
                                FCCrdValue = Convert.ToDecimal(oRow["FCCrdValue"]),
                                FNCrdTxnPrcAdj = Convert.ToInt32(oRow["FNCrdTxnPrcAdj"]),
                                FCCrdDeposit = Convert.ToDecimal(oRow["FCCrdDeposit"]),
                                FCCrdDepositPdt = Convert.ToDecimal(oRow["FCCrdDepositPdt"]),
                                cAvailable = Convert.ToDecimal(oRow["cAvailable"]),
                                FTCrdName = oRow["FTCrdName"].ToString(),
                                FTCtyName = oRow["FTCtyName"].ToString(),
                                FDCrdExpireDate = (DateTime?)(oRow["FDCrdExpireDate"]),
                                FDCrdLastTopup = (DateTime?)(oRow["FDCrdLastTopup"]),
                                FCCtyTopupAuto = Convert.ToDecimal(oRow["FCCtyTopUpAuto"]),
                                FNCtyExpiredType = Convert.ToInt32(oRow["FNCtyExpiredType"]),
                                FNCtyExpirePeriod = Convert.ToInt32(oRow["FNCtyExpirePeriod"]),
                                FNLngID = Convert.ToInt32(oRow["FNLngID"]),
                                FTCrdStaShift = oRow["FTCrdStaShift"].ToString(),
                                FTCrdHolderID = oRow["FTCrdHolderID"].ToString(),
                                FTCrdStaActive = oRow["FTCrdStaActive"].ToString(),
                            };
                aoCard = oItem.ToList();

                if (pnLngID != 0)
                {
                    if (aoCard.Count == 1)
                    {
                        return aoCard.FirstOrDefault();
                    }
                    else
                    {
                        var oCard = from oCrd in aoCard where oCrd.FNLngID == pnLngID select oCrd;
                        if (oCard != null)
                        {
                            return oCard.FirstOrDefault();
                        }
                    }
                }

                return aoCard.FirstOrDefault();
            }
            catch (Exception oExn)
            {
                return null;
            }
            finally
            {
                oDatabase = null;
                oFunc = null;
                oSql = null;
                 oDatabase = null;
                aoCard = null;
                oTbl = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptCrdCode"></param>
        /// <param name="pnLngID"></param>
        /// <returns></returns>
        public cmlTFNMCard SP_GEToCardByStored(string ptCrdCode, int pnLngID = 0)
        {
            cDatabase oDatabase;
            cSP oFunc = new cSP();
            List<cmlTFNMCard> aoCard;
            DataTable oDbTbl = new DataTable();
            SqlParameter[] aoSqlParam;
            try
            {
                oDatabase = new cDatabase();
                aoCard = new List<cmlTFNMCard>();
                aoSqlParam = new SqlParameter[] {
                    new SqlParameter ("@ptCrdCode", SqlDbType.NVarChar, 30){ Value = ptCrdCode }
                };

                oDbTbl = oDatabase.C_GEToQueryStoreDataTbl("STP_GEToCard", aoSqlParam);
                if (oDbTbl.Rows.Count > 0) {
                    var oItem = from DataRow oRow in oDbTbl.Rows
                                select new cmlTFNMCard()
                                {
                                    FCCrdValue = Convert.ToDecimal(oRow["FCCrdValue"]),
                                    FNCrdTxnPrcAdj = Convert.ToInt32(oRow["FNCrdTxnPrcAdj"]),
                                    FCCrdDeposit = Convert.ToDecimal(oRow["FCCrdDeposit"]),
                                    FCCrdDepositPdt = Convert.ToDecimal(oRow["FCCrdDepositPdt"]),
                                    cAvailable = Convert.ToDecimal(oRow["cAvailable"]),
                                    FTCrdName = oRow["FTCrdName"].ToString(),
                                    FTCtyName = oRow["FTCtyName"].ToString(),
                                    FDCrdExpireDate = oRow["FDCrdExpireDate"] == DBNull.Value ? (DateTime?)null : (DateTime?)oRow["FDCrdExpireDate"],
                                    FDCrdLastTopup = oRow["FDCrdLastTopup"] == DBNull.Value ? (DateTime?)null : (DateTime?)oRow["FDCrdLastTopup"],
                                    FNCtyExpiredType = Convert.ToInt32(oRow["FNCtyExpiredType"]),
                                   // FCCtyTopupAuto = Convert.ToDecimal(oRow["FCCtyTopUpAuto"]),
                                    FNCtyExpirePeriod = Convert.ToInt32(oRow["FNCtyExpirePeriod"]),
                                    FNLngID = Convert.ToInt32(oRow["FNLngID"]),
                                    FTCrdStaShift = oRow["FTCrdStaShift"].ToString(),
                                    FTCrdHolderID = oRow["FTCrdHolderID"].ToString(),
                                    FTCrdStaActive = oRow["FTCrdStaActive"].ToString(),
                                    FTCtyStaAlwRet = oRow["FTCtyStaAlwRet"].ToString(),
                                    FTCrdStaLocate = oRow["FTCrdStaLocate"].ToString(),
                                    FTCrdStaType = oRow["FTCrdStaType"].ToString(),
                                };
                    aoCard = oItem.ToList();
                    if (pnLngID != 0)
                    {
                        if (aoCard.Count == 1)
                        {
                            return aoCard.FirstOrDefault();
                        } else {
                            var oCard = from oCrd in aoCard where oCrd.FNLngID == pnLngID select oCrd;
                            if (oCard != null) { return oCard.FirstOrDefault(); }
                        }
                    }
                }
                return aoCard.FirstOrDefault();
            }
            catch (Exception oExn)
            {
                return null;
            }
            finally
            {
                oFunc = null;
                aoCard = null;
                oDatabase = null;
                oDbTbl = null;
                aoSqlParam = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
    }
}