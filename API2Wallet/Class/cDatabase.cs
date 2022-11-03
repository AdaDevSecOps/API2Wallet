using API2Wallet.Class.Standard;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace API2Wallet.Class
{
    /// <summary>
    /// Class Database
    /// </summary>
    public class cDatabase:IDisposable
    {
        SqlConnection oC_Con;
        /// <summary>
        ///     Constructor
        /// </summary>
        /// 
        /// <param name="pnConTme">Connection time out.</param>
        /// <param name="pnCmdTme">Execute command time out.</param>
        /// 
        public cDatabase(int pnConTme = cCS.nCS_ConTme)
        {
            try
            {
                oC_Con = new SqlConnection(cAppSettings.tConnectionString);
                oC_Con.Open();
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     Execute sql command insert, update, delete etc.
        /// </summary>
        /// 
        /// <param name="ptSqlCmd">Sql command.</param>
        /// <param name="pnConTme">Connect database time out.</param>
        /// <param name="pnCmdTme">Execute command time out.</param>
        /// 
        /// <returns>
        ///     Row effect of command.
        /// </returns>
        public int C_DATnExecuteSql(string ptSqlCmd, int pnCmdTme = cCS.nCS_CmdTme)
        {
            int nRowEff = 0;

            try
            {
                nRowEff = oC_Con.Execute(ptSqlCmd,null,null,pnCmdTme);
            }
            catch (Exception)
            {
                throw;
            }

            return nRowEff;
        }

        /// <summary>
        ///     Query sql command.
        /// </summary>
        /// 
        /// <typeparam name="T">Type return.</typeparam>
        /// <param name="ptSqlCmd">Sql command.</param>
        /// <param name="pnConTme">Connect database time out.</param>
        /// <param name="pnCmdTme">Execute command time out.</param>
        /// 
        /// <returns>
        ///     Result of sql command in list of class model.
        /// </returns>
        public List<T> C_DATaSqlQuery<T>(string ptSqlCmd, int pnCmdTme = cCS.nCS_CmdTme)
        {
            try
            {
                List<T> aoResult = new List<T>();
                aoResult = oC_Con.Query<T>(ptSqlCmd, null, null, true, pnCmdTme).ToList();
                return aoResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     Query sql command.
        /// </summary>
        /// 
        /// <typeparam name="T">Type return.</typeparam>
        /// <param name="ptSqlCmd">Sql command.</param>
        /// <param name="pnConTme">Connect database time out.</param>
        /// <param name="pnCmdTme">Execute command time out.</param>
        /// 
        /// <returns>
        ///     Result of sql command in class model.
        /// </returns>
        public T C_DAToSqlQuery<T>(string ptSqlCmd, int pnCmdTme = cCS.nCS_CmdTme)
        {
            try
            {
                T oResult = default(T);
                oResult = oC_Con.Query<T>(ptSqlCmd, null, null, true, pnCmdTme).FirstOrDefault();
                return oResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///     Query sql command.
        /// </summary>
        /// 
        /// <param name="ptSqlCmd">Sql command.</param>
        /// <param name="pnConTme">Connect database time out.</param>
        /// <param name="pnCmdTme">Execute command time out.</param>
        /// <param name="ptTblName">Table name.</param>
        /// 
        /// <returns>
        ///     Result of sql command in DataTable.
        /// </returns>
        public DataTable C_DAToSqlQuery(string ptSqlCmd, int pnCmdTme = cCS.nCS_CmdTme, string ptTblName = "TableTemp")
        {
            DataTable oDbTblResult = new DataTable(ptTblName);

            try
            {
                IDataReader oDR = oC_Con.ExecuteReader(ptSqlCmd,null,null,pnCmdTme);
                oDbTblResult.Load(oDR);
            }
            catch (Exception)
            {
                throw;
            }

            return oDbTblResult;
        }

        /// <summary>
        ///     Bulk copy table.
        /// </summary>
        /// 
        /// <param name="poDbTblData">Data.</param>
        /// <param name="pnConTme">Connect database time out.</param>
        /// <param name="pnBcpTme">Bulk copy time out.</param>
        /// 
        /// <returns>
        ///     true : Bulk copy success.<br/>
        ///     false : Bulk copy false.
        /// </returns>
        public bool C_DAToBulkCopyTable(DataTable poDbTblData, int pnBcpTme = cCS.nCS_BcpTme)
        {
            try
            {
                using (SqlBulkCopy oSqlBcp = new SqlBulkCopy(oC_Con))
                {
                    oSqlBcp.BulkCopyTimeout = pnBcpTme;
                    oSqlBcp.DestinationTableName = poDbTblData.TableName;
                    oSqlBcp.WriteToServer(poDbTblData);

                    oSqlBcp.Close();
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        ///  Interface IDisposable.
        /// </summary>
        public void Dispose()
        {
            if(oC_Con.State == ConnectionState.Open)
                oC_Con.Close();
            oC_Con.Dispose();
        }

        /// <summary>
        /// Get name DB
        /// </summary>
        /// <returns></returns>
        public string C_GETtDBName()
        {
            string tResult;
            try
            {
                tResult = oC_Con.Database;
                return tResult;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Query sql return Datatable
        /// </summary>
        /// <param name="ptSql"></param>
        /// <returns></returns>
        public DataTable C_GEToQuerySQLTbl(string ptSql)
        {
            //*Ton 64-05-24
            //SqlConnection oConn = new SqlConnection();
            SqlConnection oConn = oC_Con;
            SqlCommand oCmd = new SqlCommand();
            SqlDataAdapter oDbAdt = new SqlDataAdapter();
            DataTable oDbTbl = new DataTable();
            try
            {
                try
                {
                    if (oConn.State == ConnectionState.Open) {
                        oConn.Close();
                        //*Ton 64-05-24
                        //oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    } else {
                        //*Ton 64-05-24
                        //oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    }
                } catch {
                }
                oCmd = new SqlCommand(ptSql, oConn);
                oDbAdt = new SqlDataAdapter(oCmd);
                oDbAdt.Fill(oDbTbl);
            }
            catch (Exception) { }
            finally
            {
                oConn.Close();
                oConn = null;
                oCmd = null;
                oDbAdt = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return oDbTbl;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptSql"></param>
        /// <returns></returns>
        public int C_GETnQuerySQL(string ptSql)
        {
            //SqlConnection oConn = new SqlConnection();
            SqlConnection oConn = oC_Con;
            SqlCommand oCmd = new SqlCommand();
            SqlDataAdapter oDbAdt = new SqlDataAdapter();
            DataTable oDbTbl = new DataTable();
            int nResult = 0;
            try
            {
                try
                {
                    if (oConn.State == ConnectionState.Open)
                    {
                        oConn.Close();
                        //oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    }
                    else
                    {
                        //oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    }
                }
                catch { }
                oCmd = new SqlCommand(ptSql, oConn);
                nResult = oCmd.ExecuteNonQuery();
            }
            catch (Exception) { }
            finally
            {
                oConn.Close();
                oConn = null;
                oCmd = null;
                oDbAdt = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return nResult;
        }

        /// <summary>
        /// SQl Query ExecuteScalar String
        /// </summary>
        /// <param name="ptSql"></param>
        /// <returns></returns>
        public string C_GETtSQLScalarString(string ptSql)
        {
            //SqlConnection oConn = new SqlConnection();
            SqlConnection oConn = oC_Con;
            SqlCommand oCmd = new SqlCommand();
            SqlDataAdapter oDbAdt = new SqlDataAdapter();
            DataTable oDbTbl = new DataTable();
            string tResult = "";
            try
            {
                try
                {
                    if (oConn.State == ConnectionState.Open) {
                        oConn.Close();
                        //oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    } else {
                        //oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    }
                }
                catch { }
                oCmd = new SqlCommand(ptSql, oConn);
                tResult = oCmd.ExecuteScalar().ToString();
            }
            catch (Exception) { }
            finally
            {
                oConn.Close();
                oConn = null;
                oCmd = null;
                oDbAdt = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return tResult;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptSql"></param>
        /// <returns></returns>
        public int C_GETnSQLScalarInt(string ptSql)
        {
            //SqlConnection oConn = new SqlConnection();
            SqlConnection oConn = oC_Con;
            SqlCommand oCmd = new SqlCommand();
            SqlDataAdapter oDbAdt = new SqlDataAdapter();
            DataTable oDbTbl = new DataTable();
            int nResult = 0;
            try
            {
                try
                {
                    if (oConn.State == ConnectionState.Open)
                    {
                        oConn.Close();
                        //oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    }
                    else
                    {
                        //oConn.ConnectionString = ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString;
                        oConn.Open();
                    }
                }
                catch { }
                oCmd = new SqlCommand(ptSql, oConn);
                var oResult = oCmd.ExecuteScalar();
                nResult = oResult != null ? Convert.ToInt32(oResult) : 0;
            }
            catch (Exception oEx) { }
            finally
            {
                oConn.Close();
                oConn = null;
                oCmd = null;
                oDbAdt = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            return nResult;
        }

        /// <summary>
        /// Query stored procedure 
        /// </summary>
        /// <param name="ptStoreName"></param>
        /// <param name="paPara"></param>
        /// <returns>DataTable</returns>
        public DataTable C_GEToQueryStoreDataTbl(string ptStoreName,SqlParameter[] paPara)
        {
            DataTable oDbTbl = new DataTable();
            try
            {
                //using (SqlConnection oDbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString))
                using (SqlConnection oDbConn = new SqlConnection(cAppSettings.tConnectionString))
                {
                    oDbConn.Open();

                    using (SqlCommand oDbCmd = new SqlCommand(ptStoreName, oDbConn))
                    {
                        oDbCmd.CommandType = CommandType.StoredProcedure;
                        oDbCmd.CommandTimeout = 60;
                        oDbCmd.Parameters.AddRange(paPara);
                        using (SqlDataAdapter oDbAdt = new SqlDataAdapter(oDbCmd))
                        {
                            oDbAdt.Fill(oDbTbl);
                        }
                    }
                    oDbConn.Close();
                    oDbConn.Dispose();
                }
                return oDbTbl;
            }
            catch (Exception oEx)
            {
                return null;
            }
            finally
            {
                oDbTbl = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptStoreName"></param>
        /// <param name="paPara"></param>
        /// <param name="pnResult"></param>
        /// <returns></returns>
        public int C_GETnExecuteSqlStored(string ptStoreName,SqlParameter[] paPara)
        {
            int nResult = 0;
            try
            {
                //using (SqlConnection oDbConn = new SqlConnection(ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString))
                using (SqlConnection oDbConn = new SqlConnection(cAppSettings.tConnectionString))
                {
                    oDbConn.Open();
                    using (SqlCommand oDbCmd = new SqlCommand(ptStoreName, oDbConn))
                    {
                        oDbCmd.CommandType = CommandType.StoredProcedure;
                        oDbCmd.CommandTimeout = 60;
                        oDbCmd.Parameters.AddRange(paPara);
                        nResult = oDbCmd.ExecuteNonQuery();
                    }
                    oDbConn.Close();
                    oDbConn.Dispose();
                }
            }
            catch (Exception oEx) { }
            finally { }
            return nResult;
        }

        public SqlConnection C_CONoDatabase()
        {
            //*Arm 63-01-23
            SqlConnection oConn = null;
            string tConnString;
            try
            {
                //oConn = new SqlConnection(ConfigurationManager.ConnectionStrings["tConnDB"].ConnectionString.ToString());
                oConn = new SqlConnection(cAppSettings.tConnectionString);
                oConn.Open();
            }
            catch (Exception oEx) { throw oEx; }

            return oConn;
        }
    }
}