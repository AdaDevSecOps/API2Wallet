using API2Wallet.Class.Standard;
using API2Wallet.Models.WebService.Request.Coupon;
using API2Wallet.Models.WebService.Response.Coupon;
using System;
using System.Globalization;
using System.Threading;
using System.Text;
using API2Wallet.Class;
using API2Wallet.Models.WebService.Response.Base;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace API2Wallet.Controllers
{
    /// <summary>
    /// Card History information
    /// </summary>
    [Route(cCS.tCS_APIVer + "/Coupon")]
    public class cCouponController : ControllerBase
    {
        /// <summary>
        ///  ตรวจสอบข้อมูลคูปองที่ใช้ได้
        /// </summary>
        /// <param name="poData"></param>
        /// <returns>
        ///    System process status<br/>
        ///    &#8195;     1   : success.<br/>
        ///    &#8195;     701 : validate parameter model false.<br/>
        ///    &#8195;     800 : data not found.<br/>
        ///    &#8195;     900 : service process false.<br/>
        ///    &#8195;     905 : cannot connect database.<br/>
        ///    &#8195;     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("CheckCoupon")]
        [HttpPost]
        public cmlResCoupon POST_CHKoCheckCoupon([FromBody] cmlReqCoupon poData)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cmlResCoupon oResResult;
            StringBuilder oSql;
            cDatabase oDB;
            string tFuncName, tModelErr;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();
                oResResult = new cmlResCoupon();
                

                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";

                if (oFunc.SP_CHKbParaModel(ref tModelErr, ModelState))
                {
                    oSql = new StringBuilder();
                    oDB = new cDatabase();

                    // Check Coupon
                    oSql.AppendLine("SELECT HD.FTBchCode AS rtBchCode, HD.FTCphDocNo AS rtCphDocNo, HD.FTCphDisType AS rtCphDisType, HD.FCCphDisValue AS rcCphDisValue,");
                    oSql.AppendLine("HD.FDCphDateStart AS rdCphDateStart, HD.FDCphDateStop AS rdCphDateStop, HD.FTCphTimeStart AS rtCphTimeStart, HD.FTCphTimeStop AS rtCphTimeStop, ");    //*Arm 63-01-08 
                    oSql.AppendLine("HDL.FTCpnName AS rtCpnName, HDL.FTCpnMsg1 AS rtCpnMsg1, HDL.FTCpnMsg2 AS rtCpnMsg2, HDL.FTCpnCond AS rtCpnCond,");
                    oSql.AppendLine("DT.FTCpdBarCpn AS rtCpdBarCpn, DT.FNCpdSeqNo AS rnCpdSeqNo,");
                    oSql.AppendLine("CPNTypeL.FTCptName AS rtCptName,CPNType.FTCptType AS rtCptType,");
                    oSql.AppendLine("(DT.FNCpdAlwMaxUse-ISNULL(DTHis.nQtyUse,0)) AS rnQtyLef, (DT.FNCpdAlwMaxUse-ISNULL(DTHis.nQtyUse,0)) AS rnQtyAvailable ");
                    oSql.AppendLine("FROM TFNTCouponHD HD WITH(NOLOCK) ");
                    oSql.AppendLine("LEFT JOIN TFNTCouponHD_L HDL WITH(NOLOCK) ON HD.FTCphDocNo = HDL.FTCphDocNo ");
                    oSql.AppendLine("INNER JOIN TFNTCouponDT DT WITH(NOLOCK) ON HD.FTBchCode = DT.FTBchCode AND HD.FTCphDocNo = DT.FTCphDocNo ");
                    oSql.AppendLine("INNER JOIN TFNMCouponType CPNType WITH(NOLOCK) ON HD.FTCptCode = CPNType.FTCptCode AND CPNType.FTCptType = '" + poData.ptCouponType + "' ");  //*Arm 63-01-08 
                    oSql.AppendLine("LEFT JOIN TFNMCouponType_L CPNTypeL WITH(NOLOCK) ON HD.FTCptCode = CPNTypeL.FTCptCode AND CPNTypeL.FNLngID ='" + poData.pnLangID + "' ");
                    //oSql.AppendLine("LEFT JOIN (SELECT FTCphDocNo, COUNT(*) AS nQtyUse FROM TFNTCouponDTHis WITH(NOLOCK) WHERE FTCpdBarCpn ='" + poData.ptCouponCode + "' AND FTBchCode ='" + poData.ptBranch + "' AND FTCpbStaBook !='3' GROUP BY FTCphDocNo) DTHis ON DT.FTCphDocNo = DTHis.FTCphDocNo ");
                    oSql.AppendLine("LEFT JOIN (SELECT FTCphDocNo, COUNT(*) AS nQtyUse FROM TFNTCouponDTHis WITH(NOLOCK) WHERE FTCpdBarCpn ='" + poData.ptBarCpn + "' AND FTCpbStaBook !='3' GROUP BY FTCphDocNo) DTHis ON DT.FTCphDocNo = DTHis.FTCphDocNo "); //*Arm 63-01-09
                    oSql.AppendLine("WHERE DT.FTCpdBarCpn = '" + poData.ptBarCpn + "'");
                    oSql.AppendLine("AND HDL.FNLngID = '" + poData.pnLangID + "'");
                    oSql.AppendLine("AND GETDATE() Between HD.FDCphDateStart AND HD.FDCphDateStop");
                    oSql.AppendLine("AND CONVERT(time(0), GETDATE(),108) Between HD.FTCphTimeStart AND HD.FTCphTimeStop");
                    oSql.AppendLine("AND HD.FTCphStaClosed = 1");   //สถานะใช้งาน
                    oSql.AppendLine("AND HD.FTCphStaDoc = 1 ");     //สถานะ เอกสาร  1:สมบูรณ์
                    oSql.AppendLine("AND HD.FTCphStaApv = 1 ");     //สถานะ 1:อนุมัติแล้ว
                    oSql.AppendLine("AND (ISNULL(HD.FTCphPplTo,'')='" + poData.ptPriceGroup + "' OR ISNULL(HD.FTCphPplTo,'')='' ) ");   //*Arm 63-01-09
                    oSql.AppendLine("AND (ISNULL(HD.FTCphBchTo,'')='" + poData.ptBranch + "' OR ISNULL(HD.FTCphBchTo,'')='' ) ");       //*Arm 63-01-09
                    oSql.AppendLine("AND (ISNULL(HD.FTCphMerTo,'')='" + poData.ptMerchant + "' OR ISNULL(HD.FTCphMerTo,'')='' ) ");     //*Arm 63-01-09
                    oSql.AppendLine("AND ((DT.FNCpdAlwMaxUse-ISNULL(DTHis.nQtyUse,0)) > 0"); //*Em 63-01-09
                    oSql.AppendLine("    OR DT.FNCpdAlwMaxUse = 0)");    //*Em 63-01-14
                    oResResult.raoCoupon = oDB.C_DATaSqlQuery<cmlResCouponList>(oSql.ToString());

                    if (oResResult.raoCoupon.Count > 0)
                    {
                        oResResult.rtCode = oMsg.tMS_RespCode1;
                        oResResult.rtDesc = oMsg.tMS_RespDesc1;
                        return oResResult;
                    }
                    else
                    {
                        oResResult = new cmlResCoupon();
                        oResResult.rtCode = oMsg.tMS_RespCode800;
                        oResResult.rtDesc = oMsg.tMS_RespDesc800;
                        return oResResult;
                    }
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
                oResResult = new cmlResCoupon();
                oResResult.rtCode = new cMS().tMS_RespCode900;
                oResResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                oDB = null;
                oSql = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                oResResult = null;
            }
        }

        /// <summary>
        ///  ตรวจสอบข้อมูลคูปองที่ใช้ได้
        /// </summary>
        /// <param name="poData"></param>
        /// <returns>
        ///    System process status<br/>
        ///    &#8195;     1   : success.<br/>
        ///    &#8195;     701 : validate parameter model false.<br/>
        ///    &#8195;     800 : data not found.<br/>
        ///    &#8195;     900 : service process false.<br/>
        ///    &#8195;     905 : cannot connect database.<br/>
        ///    &#8195;     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("CheckCouponHD")]
        [HttpPost]
        public cmlResCoupon POST_CHKoCheckCouponHD([FromBody] cmlReqCoupon poData)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cmlResCoupon oResResult;
            StringBuilder oSql;
            cDatabase oDB;
            string tFuncName, tModelErr;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();
                oResResult = new cmlResCoupon();


                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";

                if (oFunc.SP_CHKbParaModel(ref tModelErr, ModelState))
                {
                    oSql = new StringBuilder();
                    oDB = new cDatabase();

                    // Check Coupon
                    oSql.AppendLine($"SELECT HD.FTBchCode AS rtBchCode, HD.FTCphDocNo AS rtCphDocNo, HD.FTCphDisType AS rtCphDisType, HD.FCCphDisValue AS rcCphDisValue,");
                    oSql.AppendLine($"HD.FDCphDateStart AS rdCphDateStart, HD.FDCphDateStop AS rdCphDateStop, HD.FTCphTimeStart AS rtCphTimeStart, HD.FTCphTimeStop AS rtCphTimeStop,");
                    oSql.AppendLine($"HDL.FTCpnName AS rtCpnName, HDL.FTCpnMsg1 AS rtCpnMsg1, HDL.FTCpnMsg2 AS rtCpnMsg2, HDL.FTCpnCond AS rtCpnCond,");
                    oSql.AppendLine($"DT.FTCpdBarCpn AS rtCpdBarCpn, DT.FNCpdSeqNo AS rnCpdSeqNo,");
                    oSql.AppendLine($"CPNTL.FTCptName AS rtCptName,CPNT.FTCptType AS rtCptType,");
                    oSql.AppendLine($"(DT.FNCpdAlwMaxUse-ISNULL(DTHis.nQtyUse,0)) AS rnQtyLef,");
                    oSql.AppendLine($"(DT.FNCpdAlwMaxUse-ISNULL(DTHis.nQtyUse,0)) AS rnQtyAvailable ");
                    oSql.AppendLine($"FROM TFNTCouponHD HD WITH(NOLOCK) ");
                    oSql.AppendLine($"LEFT JOIN TFNTCouponHD_L HDL WITH(NOLOCK) ON HD.FTCphDocNo = HDL.FTCphDocNo AND HDL.FNLngID={poData.pnLangID}");
                    oSql.AppendLine($"INNER JOIN TFNTCouponDT DT WITH(NOLOCK) ON HD.FTBchCode = DT.FTBchCode AND HD.FTCphDocNo = DT.FTCphDocNo ");
                    oSql.AppendLine($"INNER JOIN TFNMCouponType CPNT WITH(NOLOCK) ON HD.FTCptCode = CPNT.FTCptCode AND CPNT.FTCptType = '{poData.ptCouponType}'");
                    oSql.AppendLine($"LEFT JOIN TFNMCouponType_L CPNTL WITH(NOLOCK) ON HD.FTCptCode = CPNTL.FTCptCode AND CPNTL.FNLngID ={poData.pnLangID}");
                    oSql.AppendLine($"LEFT JOIN ");
                    oSql.AppendLine($"(");
                    oSql.AppendLine($"SELECT FTCphDocNo, COUNT(*) AS nQtyUse ");
                    oSql.AppendLine($"FROM TFNTCouponDTHis WITH(NOLOCK) ");
                    oSql.AppendLine($"WHERE FTCpdBarCpn ='{poData.ptBarCpn}' ");
                    oSql.AppendLine($"AND FTCphDocNo = '{poData.ptCpnDocNo}'");
                    oSql.AppendLine($"AND FTCpbStaBook !='3' GROUP BY FTCphDocNo");
                    oSql.AppendLine($")");
                    oSql.AppendLine($" DTHis ON DT.FTCphDocNo = DTHis.FTCphDocNo");
                    oSql.AppendLine($"LEFT JOIN TFNTCouponDTCst DTCst WITH(NOLOCK) ");
                    oSql.AppendLine($"ON DTCst.FTBchCode=DT.FTBchCode AND DTCst.FTCpdBarCpn=DT.FTCpdBarCpn ");
                    oSql.AppendLine($"AND DTCst.FTCphDocNo=DT.FTCphDocNo AND DTCst.FNCpdSeqNo=DTCst.FNCpdSeqNo ");
                    oSql.AppendLine($"WHERE DT.FTCpdBarCpn = '{poData.ptBarCpn}'");
                    oSql.AppendLine($"AND HD.FTCphDocNo = '{poData.ptCpnDocNo}'");
                    oSql.AppendLine($"AND ((DT.FNCpdAlwMaxUse-ISNULL(DTHis.nQtyUse,0)) >= 0 OR ISNULL(HD.FTStaChkMember,'1')='2')");
                    //oSql.AppendLine($"AND ((ISNULL(HD.FTStaChkMember,'1')='2' AND DTCst.FTRefCst='{poData.ptCstCode}') OR");
                    oSql.AppendLine($"AND ((ISNULL(HD.FTStaChkMember,'1')='2' AND ISNULL(DTCst.FTCpdRefCst,'')='{poData.ptCstCode}') OR"); //*Arm 65-10-25
                    oSql.AppendLine($"     (ISNULL(HD.FTStaChkMember,'1')='1'))");


                    oResResult.raoCoupon = oDB.C_DATaSqlQuery<cmlResCouponList>(oSql.ToString());

                    if (oResResult.raoCoupon.Count > 0)
                    {
                        oResResult.rtCode = oMsg.tMS_RespCode1;
                        oResResult.rtDesc = oMsg.tMS_RespDesc1;
                        return oResResult;
                    }
                    else
                    {
                        oResResult = new cmlResCoupon();
                        oResResult.rtCode = oMsg.tMS_RespCode800;
                        oResResult.rtDesc = oMsg.tMS_RespDesc800;
                        return oResResult;
                    }
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
                oResResult = new cmlResCoupon();
                oResResult.rtCode = new cMS().tMS_RespCode900;
                oResResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                oDB = null;
                oSql = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                oResResult = null;
            }
        }

        /// <summary>
        ///  ยกเลิกการใช้ Coupon
        /// </summary>
        /// <param name="poData"></param>
        /// <returns>
        ///    System process status<br/>
        ///    &#8195;     1   : success.<br/>
        ///    &#8195;     701 : validate parameter model false.<br/>
        ///    &#8195;     800 : data not found.<br/>
        ///    &#8195;     900 : service process false.<br/>
        ///    &#8195;     905 : cannot connect database.<br/>
        ///    &#8195;     906 : this time not allowed to use method.<br/>
        /// </returns>
        [Route("CancelPayCoupon")]
        [HttpPost]
        public cmlResBese POST_CANoCancelPayCoupon([FromBody] cmlReqCancelPayCoupon poData)
        {
            cSP oFunc;
            cCS oCons;
            cMS oMsg;
            cmlResBese oResResult;
            StringBuilder oSql;
            cDatabase oDB;
            string tFuncName, tModelErr;

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                
                oFunc = new cSP();
                oCons = new cCS();
                oMsg = new cMS();
                oResResult = new cmlResBese();
               
                
                // Get method name.
                tFuncName = MethodBase.GetCurrentMethod().Name;

                // Validate parameter.
                tModelErr = "";

                if (oFunc.SP_CHKbParaModel(ref tModelErr, ModelState))
                {
                    oSql = new StringBuilder();
                    oDB = new cDatabase();

                    oSql.AppendLine("UPDATE TFNTCouponDTHis WITH(ROWLOCK) SET ");
                    oSql.AppendLine("FTCpbStaBook = '3',");
                    oSql.AppendLine("FDLastUpdOn = GETDATE(),");
                    oSql.AppendLine("FTLastUpdBy = 'API2Wallet'");
                    oSql.AppendLine("WHERE FTCpbFrmBch ='" + poData.ptCpbFrmBch + "' AND FTCpbFrmPos ='" + poData.ptCpbFrmPos + "' AND FTCpbFrmSalRef ='" + poData.ptCpbFrmSaleRef + "'");
                    oDB.C_DATnExecuteSql(oSql.ToString());

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
                oResResult = new cmlResBese();
                oResResult.rtCode = new cMS().tMS_RespCode900;
                oResResult.rtDesc = new cMS().tMS_RespDesc900;
                return oResResult;
            }
            finally
            {
                oFunc = null;
                oCons = null;
                oMsg = null;
                oDB = null;
                oSql = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
                oResResult = null;
            }
        }
    }
}