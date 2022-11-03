using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;
using API2Wallet.Class;
using API2Wallet.Class.Standard;
using API2Wallet.Models;
using API2Wallet.Models.WebService.Request.CardHistory;
using API2Wallet.Models.WebService.Response.CardHistory;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace API2Wallet.Controllers
{
    /// <summary>
    /// Card History information
    /// </summary>
    [Route(cCS.tCS_APIVer + "/CrdHistoryCheck")]
    public class cCheckCrdHistoryController : ControllerBase
    {
        /// <summary>
        ///  ประวัติการทำรายการบัตร Card History
        /// </summary>
        /// <param name="poPrm"></param>
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
        [Route("CrdHistory")]
        [HttpPost]
        public cmlResCardHistory POST_PUNoCrdHistory([FromBody] cmlReqCardHistory poPrm)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cDatabase oDatabase;
            StringBuilder oSql;
            cmlResCardHistory oResResult;
            int nConTme, nCmdTme, nStaOffine;
            string tFuncName, tModelErr;
            cmlTFNMCard oCard;
            int nDayPrv;
            DataTable oTblCrdHis;
            SqlParameter[] aoSqlParam;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();
                oResResult = new cmlResCardHistory();

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";

                if (oFunc.SP_CHKbParaModel(ref tModelErr, ModelState))
                {

                    oDatabase = new cDatabase();
                    oSql = new StringBuilder();
                    oCard = new cmlTFNMCard();
                    #region  Get ข้อมูลบัตร
                    //Get ข้อมูลบัตร
                    oCard = new cmlTFNMCard();
                    //  oCard = oFunc.SP_GEToCard(aoSysConfig, poPrm.ptCrdCode);
                    oCard = oFunc.SP_GEToCardByStored(poPrm.ptCrdCode);
                    #endregion

                    if (oCard == null)
                    {
                        oResResult.rtCode = oMsg.tMS_RespCode800;
                        oResResult.rtDesc = oMsg.tMS_RespDesc800;
                        return oResResult;
                    }

                    if (poPrm.pnTxnOffline == oCard.FNCrdTxnPrcAdj){
                        nStaOffine = 1;
                    } else {
                        nStaOffine = 0;
                    }

                    /* *[AnUBiS][][2018-12-03] - command code.
                    oSql.Clear();
                    oSql.AppendLine(" DECLARE @FNQty AS Integer");
                    oSql.AppendLine(" DECLARE @FTCrdCode AS VARCHAR(50) ");
                    oSql.AppendLine(" DECLARE @FTBchCode AS VARCHAR(50) ");
                    oSql.AppendLine(" DECLARE @FDTxnDocDate AS VARCHAR(10) ");
                    oSql.AppendLine(" SET @FNQty = '" + poPrm.pnQty + "' ");
                    oSql.AppendLine(" SET @FTCrdCode = '" + poPrm.ptCrdCode + "' ");
                    // oSql.AppendLine(" SET @FTBchCode = '" + poPrm.ptBchCode + "' ");
                    oSql.AppendLine(" SET @FDTxnDocDate = '" + poPrm.ptTxnDocDate + "' ");
                    oSql.AppendLine(" SELECT TOP(@FNQty) FTTxnDocType AS rtType,FDTxnDocDate AS rdDocDate ");
                    oSql.AppendLine(" ,FTBchCodeRef AS rtBchRef,FTTxnDocNoRef AS rtDocRef ");
                    oSql.AppendLine(" ,CONVERT(NUMERIC(10,2), FCTxnValue) AS rcTxnValue," + nStaOffine + " AS rnStaOffline");
                    oSql.AppendLine(" ,FTShpName AS rtShopName,ISNULL(FCTxnDeposit,0) AS rcTxnDeposit");
                    oSql.AppendLine(" FROM ");
                    oSql.AppendLine(" (SELECT T.FTCrdCode,T.FTBchCode,T.FTTxnDocType,T.FDTxnDocDate,T.FTBchCodeRef,T.FCTxnDeposit");
                    oSql.AppendLine(" ,T.FTTxnDocNoRef,T.FCTxnValue,L.FTShpName");
                    oSql.AppendLine(" FROM TFNTCrdTopup T LEFT JOIN TCNMShop_L L ON(T.FTShpCode=L.FTShpCode)");
                    oSql.AppendLine(" UNION ");
                    oSql.AppendLine(" SELECT S.FTCrdCode,S.FTBchCode,S.FTTxnDocType,S.FDTxnDocDate,S.FTBchCodeRef,S.FCTxnDeposit");
                    oSql.AppendLine(" ,S.FTTxnDocNoRef,S.FCTxnValue,L.FTShpName");
                    oSql.AppendLine(" FROM TFNTCrdSale S LEFT JOIN TCNMShop_L L ON(S.FTShpCode=L.FTShpCode)) AS CrdHistory");
                    oSql.AppendLine(" WHERE FTCrdCode=@FTCrdCode");
                    // oSql.AppendLine(" AND FTBchCode=@FTBchCode");

                    if (poPrm.ptTxnDocDate != null) {
                        oSql.AppendLine(" AND CONVERT(nvarchar(10), FDTxnDocDate, 121)=@FDTxnDocDate");
                    }
                       
                    if (poPrm.ptSort == "0") {
                        oSql.AppendLine("ORDER BY FDTxnDocDate ASC");
                    } else  {
                        oSql.AppendLine("ORDER BY FDTxnDocDate DESC");
                    }
                    */

                    // nDayPrv = Convert.ToInt32( new cDatabase().C_DAToSqlQuery<string>("SELECT FTSysStaUsrValue FROM TSysConfig with(nolock) WHERE FTSysCode = 'SetReadPrvTrn'", nCmdTme));   //*Em 62-01-23  Pandora
                    nDayPrv = Convert.ToInt32(oDatabase.C_GETtSQLScalarString("SELECT FTSysStaUsrValue FROM TSysConfig with(nolock) WHERE FTSysCode = 'SetReadPrvTrn'"));

                    //*[AnUBiS][][2018-12-03] - ปรับ query ให้อ่านง่าย
                    //oSql.Clear();
                    //oSql.AppendLine("DECLARE @nQty AS INTEGER");
                    //oSql.AppendLine("DECLARE @tCrdCode AS VARCHAR(50)");
                    //oSql.AppendLine("DECLARE @tTxnDocDate AS VARCHAR(10)");
                    //oSql.AppendLine("SET @nQty = " + poPrm.pnQty);
                    //oSql.AppendLine("SET @tCrdCode = '" + poPrm.ptCrdCode + "' ");
                    //oSql.AppendLine("SET @tTxnDocDate = '" + poPrm.ptTxnDocDate + "' ");
                    //oSql.AppendLine("");
                    //oSql.AppendLine("SELECT TOP(@nQty) FTTxnDocType AS rtType, FDTxnDocDate AS rdDocDate");
                    //oSql.AppendLine("	, FTBchCodeRef AS rtBchRef,FTTxnDocNoRef AS rtDocRef");
                    //oSql.AppendLine("	, CONVERT(NUMERIC(10,2), FCTxnValue) AS rcTxnValue," + nStaOffine + " AS rnStaOffline");
                    //oSql.AppendLine("	, FTShpName AS rtShopName, ISNULL(FCTxnDeposit,0) AS rcTxnDeposit");
                    ////oSql.AppendLine("	, ISNULL(FCTxnCrdValue,0) AS rcTxnCrdValue");
                    //oSql.AppendLine("	, FCTxnCrdValue AS rcTxnCrdValue"); //*Em 61-12-17  Pandora  ต้องการให้ Return ค่า null ได้ เพราะรายการคืนจะเป็นค่า Null
                    //oSql.AppendLine("FROM");
                    //oSql.AppendLine("(");
                    //oSql.AppendLine("	SELECT CTP.FTCrdCode, CTP.FTBchCode, CTP.FTTxnDocType");
                    //oSql.AppendLine("		, CTP.FDTxnDocDate, CTP.FTBchCodeRef, CTP.FCTxnDeposit");
                    //oSql.AppendLine("		, CTP.FTTxnDocNoRef, CTP.FCTxnValue, SHPL.FTShpName");
                    //oSql.AppendLine("		, CTP.FCTxnCrdValue");
                    //oSql.AppendLine("	FROM TFNTCrdTopup CTP WITH(NOLOCK)");
                    //oSql.AppendLine("		LEFT JOIN TCNMShop_L SHPL WITH(NOLOCK) ON CTP.FTShpCode = SHPL.FTShpCode");
                    //oSql.AppendLine("   WHERE CTP.FTCrdCode = @tCrdCode");
                    //oSql.AppendLine("");
                    //oSql.AppendLine("	UNION");
                    //oSql.AppendLine("");
                    //oSql.AppendLine("	SELECT CSL.FTCrdCode, CSL.FTBchCode, CSL.FTTxnDocType");
                    //oSql.AppendLine("		, CSL.FDTxnDocDate, CSL.FTBchCodeRef, CSL.FCTxnDeposit");
                    //oSql.AppendLine("		, CSL.FTTxnDocNoRef, CSL.FCTxnValue, SHPL.FTShpName");
                    //oSql.AppendLine("		, CSL.FCTxnCrdValue");
                    //oSql.AppendLine("	FROM TFNTCrdSale CSL WITH(NOLOCK)");
                    //oSql.AppendLine("		LEFT JOIN TCNMShop_L SHPL WITH(NOLOCK) ON CSL.FTShpCode = SHPL.FTShpCode");
                    //oSql.AppendLine("   WHERE CSL.FTCrdCode = @tCrdCode");
                    ////*Em 61-12-10  Pandora
                    //oSql.AppendLine("");
                    //oSql.AppendLine("	UNION");
                    //oSql.AppendLine("");
                    //oSql.AppendLine("	SELECT COF.FTCrdCode, COF.FTBchCode, COF.FTTxnDocType");
                    //oSql.AppendLine("		, COF.FDTxnDocDate, COF.FTBchCodeRef, COF.FCTxnDeposit");
                    //oSql.AppendLine("		, COF.FTTxnDocNoRef, COF.FCTxnValue, SHPL.FTShpName");
                    //oSql.AppendLine("		, COF.FCTxnCrdValue");
                    //oSql.AppendLine("	FROM TFNTCrdOffline COF WITH(NOLOCK)");
                    //oSql.AppendLine("		LEFT JOIN TCNMShop_L SHPL WITH(NOLOCK) ON COF.FTShpCode = SHPL.FTShpCode");
                    //oSql.AppendLine("   WHERE COF.FTCrdCode = @tCrdCode");
                    //oSql.AppendLine("");
                    //oSql.AppendLine("	UNION");
                    //oSql.AppendLine("");
                    //oSql.AppendLine("	SELECT CHS.FTCrdCode, CHS.FTBchCode, CHS.FTTxnDocType");
                    //oSql.AppendLine("		, CHS.FDTxnDocDate, CHS.FTBchCodeRef, CHS.FCTxnDeposit");
                    //oSql.AppendLine("		, CHS.FTTxnDocNoRef, CHS.FCTxnValue, SHPL.FTShpName");
                    //oSql.AppendLine("		, CHS.FCTxnCrdValue");
                    //oSql.AppendLine("	FROM TFNTCrdHis CHS WITH(NOLOCK)");
                    //oSql.AppendLine("		LEFT JOIN TCNMShop_L SHPL WITH(NOLOCK) ON CHS.FTShpCode = SHPL.FTShpCode");
                    //oSql.AppendLine("   WHERE CHS.FTCrdCode = @tCrdCode");
                    //oSql.AppendLine("");
                    //oSql.AppendLine("	UNION");
                    //oSql.AppendLine("");
                    //oSql.AppendLine("	SELECT CHB.FTCrdCode, CHB.FTBchCode, CHB.FTTxnDocType");
                    //oSql.AppendLine("		, CHB.FDTxnDocDate, CHB.FTBchCodeRef, CHB.FCTxnDeposit");
                    //oSql.AppendLine("		, CHB.FTTxnDocNoRef, CHB.FCTxnValue, SHPL.FTShpName");
                    //oSql.AppendLine("		, CHB.FCTxnCrdValue");
                    //oSql.AppendLine("	FROM TFNTCrdHisBch CHB WITH(NOLOCK)");
                    //oSql.AppendLine("		LEFT JOIN TCNMShop_L SHPL WITH(NOLOCK) ON CHB.FTShpCode = SHPL.FTShpCode");
                    //oSql.AppendLine("   WHERE CHB.FTCrdCode = @tCrdCode");
                    ////++++++++++++++++++++++
                    //oSql.AppendLine(") AS CrdHistory");
                    ////oSql.AppendLine("WHERE FTCrdCode = @tCrdCode");

                    //if (poPrm.ptTxnDocDate != null)
                    //{
                    //    //oSql.AppendLine("AND CONVERT(VARCHAR(10), FDTxnDocDate, 121) = @tTxnDocDate");
                    //    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDTxnDocDate, 121) = @tTxnDocDate");    //*Em 61-12-10  Pandora
                    //}
                    //else
                    //{
                    //    oSql.AppendLine("WHERE CONVERT(VARCHAR(10), FDTxnDocDate, 121) >= CONVERT(VARCHAR(10),DATEADD(DAY,-"+ nDayPrv +",GETDATE()),121)");    //*Em 62-01-23  Pandora
                    //}

                    //if (string.Equals(poPrm.ptSort,"0"))
                    //{
                    //    oSql.AppendLine("ORDER BY FDTxnDocDate ASC");
                    //}
                    //else
                    //{
                    //    oSql.AppendLine("ORDER BY FDTxnDocDate DESC");
                    //}

                    //Execute Add to list
                    // oDatabase = new cDatabase(nConTme);
                    //  oResResult.raoCrdHistory = oDatabase.C_DATaSqlQuery<cmlResCardHistoryList>(oSql.ToString());

                    if (poPrm.ptTxnDocDate == null) { poPrm.ptTxnDocDate = ""; }

                    oTblCrdHis = new DataTable();
                    aoSqlParam = new SqlParameter[] {
                            new SqlParameter ("@ptCrdCode", SqlDbType.VarChar, 20){ Value = poPrm.ptCrdCode },
                            new SqlParameter ("@ptTxnDocDate", SqlDbType.VarChar, 10){ Value = poPrm.ptTxnDocDate },
                            new SqlParameter ("@pnQty", SqlDbType.Int){ Value = poPrm.pnQty },
                            new SqlParameter ("@pnStaOffine", SqlDbType.Int){ Value = nStaOffine },
                            new SqlParameter ("@pnDayPrv", SqlDbType.Int){ Value = nDayPrv },
                            new SqlParameter ("@ptSort", SqlDbType.VarChar, 1){ Value = poPrm.ptSort }
                        };
                    oTblCrdHis = oDatabase.C_GEToQueryStoreDataTbl("STP_SEToCrdHistory", aoSqlParam);
                    if (oTblCrdHis.Rows.Count > 0)
                    {
                        var oItem = from DataRow oRow in oTblCrdHis.Rows
                                    select new cmlResCardHistoryList()
                                    {
                                        rtType = (string)oRow["rtType"],
                                        rdDocDate = oRow["rdDocDate"] == DBNull.Value ? (DateTime?)null : (DateTime?)oRow["rdDocDate"],
                                        rtBchRef = oRow["rtBchRef"] == DBNull.Value ? "" : (string)oRow["rtBchRef"],
                                        rtDocRef = oRow["rtDocRef"] == DBNull.Value ? "" : (string)oRow["rtDocRef"],
                                        rcTxnValue = (decimal)oRow["rcTxnValue"],
                                        rnStaOffline = (int)oRow["rnStaOffline"],
                                        rtShopName = oRow["rtShopName"] == DBNull.Value ? "" : (string)oRow["rtShopName"],
                                        rcTxnDeposit = (decimal)oRow["rcTxnDeposit"],
                                        rcTxnCrdValue = oRow["rcTxnCrdValue"] == DBNull.Value ? (decimal?)null : (decimal?)oRow["rcTxnCrdValue"],
                                    };
                        oResResult.raoCrdHistory = oItem.ToList();
                    }


                    oResResult.rtCode = oMsg.tMS_RespCode1;
                    oResResult.rtDesc = oMsg.tMS_RespDesc1;
                    return oResResult;
                }
                else
                {
                    // Validate parameter model false.
                    oResResult.rtCode = oMsg.tMS_RespCode701;
                    oResResult.rtDesc = oMsg.tMS_RespDesc701 + tModelErr;
                    return oResResult;
                }
            }
            catch (Exception oEx)
            {
                oResResult = new cmlResCardHistory();
                oResResult.rtCode = new cMS().tMS_RespCode900;
                oResResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                oDatabase = null;
                oSql = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                oResResult = null;
                oResResult = null;
                oCard = null;
            }
        }
    }
}
