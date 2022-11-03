using API2Wallet.Class.Standard;
using API2Wallet.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace API2Wallet.Class.ResetExpired
{
    /// <summary>
    /// 
    /// </summary>
    public class cResetExpired
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptCrdCode"></param>
        /// <param name="ptBchCode"></param>
        /// <param name="ptPosCode"></param>
        /// <param name="ptDocNoRef"></param>
        /// <param name="ptCrdSta"></param>
        /// <returns></returns>
        //public bool C_SETbResetExpired(string ptCrdCode,string ptBchCode,string ptPosCode, List<cmlTSysConfig> paoSysConfig)
        public bool C_SETbResetExpired(
            string ptCrdCode, string ptBchCode, string ptPosCode, string ptDocNoRef, string ptCrdSta = "")
        {
            bool bResult = true;
            cDatabase oDatabase;
            StringBuilder oSql = new StringBuilder();
            int nRowEff;
            cSP oFunc = new cSP();
            cmlTFNMCard oCard = new cmlTFNMCard();
            SqlParameter[] aoSqlParam;
            try
            {
                

                oDatabase = new cDatabase();
                // Get value Card 
                oCard = oFunc.SP_GEToCardByStored(ptCrdCode);

                //oSql.Clear();
                //oSql.AppendLine("SELECT TOP 1 ISNULL(FCCrdValue,0) AS FCCrdValue");
                //oSql.AppendLine(" FROM TFNMCard WITH (NOLOCK) WHERE FTCrdCode='" + ptCrdCode + "'");
                //oCard = oDatabase.C_DAToSqlQuery<cmlTFNMCard>(oSql.ToString(), nCmdTme);

                // Process

                //oSql.Clear();
                //oSql.AppendLine("BEGIN TRANSACTION ");
                //oSql.AppendLine("  SAVE TRANSACTION SavePay ");
                //oSql.AppendLine("  BEGIN TRY ");

                //// Insert transection Sale
                //oSql.AppendLine("     INSERT INTO TFNTCrdTopUp WITH(ROWLOCK)");
                //oSql.AppendLine("     (");
                //oSql.AppendLine("	  FTBchCode,FTTxnDocType,FTCrdCode,");
                //oSql.AppendLine("	  FDTxnDocDate,FTBchCodeRef,FCTxnValue,");
                //oSql.AppendLine("	  FTTxnStaPrc,FTTxnPosCode,FCTxnCrdValue");
                //oSql.AppendLine("     ,FTTxnStaOffLine");    //*Em 61-12-10  Pandora
                //oSql.AppendLine("     ,FTTxnDocNoRef");    //*Em 61-12-11  Pandora
                //oSql.AppendLine("     )");
                //oSql.AppendLine("     VALUES");
                //oSql.AppendLine("     (");
                //oSql.AppendLine("	  '" + ptBchCode + "','10','" + ptCrdCode + "',");
                //oSql.AppendLine("	  GETDATE(),'" + ptBchCode + "','" + oCard.FCCrdValue + "',");
                //oSql.AppendLine("	  '1','" + ptPosCode + "','" + oCard.FCCrdValue + "'");
                //oSql.AppendLine("     ,'0'");    //*Em 61-12-10  Pandora
                //oSql.AppendLine("     ,'"+ ptDocNoRef +"'");    //*Em 61-12-11  Pandora
                //oSql.AppendLine("     )");

                //// Update Master Card
                //oSql.AppendLine("     UPDATE TFNMCard WITH (ROWLOCK) SET ");

                ////*[AnUBiS][][2019-02-21] - check status card.
                //if(string.IsNullOrEmpty(ptCrdSta) || string.Equals(ptCrdSta, "3"))
                //{
                //    oSql.AppendLine("     FCCrdValue=0,");
                //}

                //oSql.AppendLine("     FCCrdDeposit=0,");
                //oSql.AppendLine("     FCCrdDepositPdt=0,");
                //oSql.AppendLine("     FDCrdLastTopup=NULL,");
                //oSql.AppendLine("     FDCrdResetDate=GETDATE()");
                //oSql.AppendLine("     WHERE FTCrdCode='" + ptCrdCode + "'");
                //oSql.AppendLine("     COMMIT TRANSACTION SavePay");
                //oSql.AppendLine("  END TRY");

                //oSql.AppendLine("  BEGIN CATCH");
                //oSql.AppendLine("   ROLLBACK TRANSACTION SavePay");
                //oSql.AppendLine("  END CATCH");
                //nRowEff = oDatabase.C_DATnExecuteSql(oSql.ToString(), nCmdTme);

                aoSqlParam = new SqlParameter[] {
                        new SqlParameter ("@ptBchCode", SqlDbType.VarChar, 5){ Value = ptBchCode },
                        new SqlParameter ("@ptCrdCode", SqlDbType.VarChar, 20){ Value = ptCrdCode },
                        new SqlParameter ("@ptPosCode", SqlDbType.VarChar, 3){ Value = ptPosCode },
                        new SqlParameter ("@FCCrdValue", SqlDbType.Decimal){ Value = oCard.FCCrdValue },
                        new SqlParameter ("@ptDocNoRef", SqlDbType.VarChar){ Value = ptDocNoRef },
                        new SqlParameter ("@ptCrdSta", SqlDbType.VarChar, 1){ Value = ptCrdSta },
                        new SqlParameter ("@FNResult", SqlDbType.Int) { Direction = ParameterDirection.Output }
               };
                nRowEff = oDatabase.C_GETnExecuteSqlStored("STP_SETnResetExpired",aoSqlParam);

                if (nRowEff == 0)
                {
                    bResult = false;
                }
                return bResult;
            }
            catch (Exception oEx)
            {
                return false;
            }
            finally
            {
                oDatabase = null;
                oSql = null;
                oFunc = null;
                oCard = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }


    }
}
