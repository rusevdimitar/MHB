using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace MHB.DAL
{
    public class DataBaseConnector : Database
    {
        public DataBaseConnector(string connectionString)
            : base(connectionString, System.Data.SqlClient.SqlClientFactory.Instance)
        {
        }

        protected override void DeriveParameters(DbCommand discoveryCommand)
        {
        }

        public static T GetSingleValue<T>(string qry, SqlConnection connection, params IDataParameter[] parameters)
        {
            if (connection == null)
                throw new ArgumentNullException("DatabaseConnector.GetSingleValue<T>: required parameter connection is null!");

            return DataBaseConnector.GetSingleValueInternal<T>(qry, connection, parameters);
        }

        public static T GetSingleValue<T>(string qry, string connectionString, params IDataParameter[] parameters)
        {
            if (connectionString == null)
                throw new ArgumentNullException("DatabaseConnector.GetSingleValue<T>: required parameter connectionString is null!");

            SqlConnection connection = new SqlConnection(connectionString);

            return DataBaseConnector.GetSingleValueInternal<T>(qry, connection, parameters);
        }

        private static T GetSingleValueInternal<T>(string qry, SqlConnection connection, IDataParameter[] parameters)
        {
            object result = default(T);

            try
            {
                if (connection.State != ConnectionState.Open) connection.Open();

                using (SqlCommand cmd = new SqlCommand(qry, connection))
                {
                    cmd.Parameters.AddRange(parameters);
                    result = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("GetSingleValue<T>:{0} qry:{1}", ex.Message, qry), ex);
            }

            return (T)Convert.ChangeType(result, typeof(T));
            //return (T)result;
        }

        [Obsolete("Please use GetSingleValue<T>")]
        public static object GetSingleValue(string qry, string connectionString, params IDataParameter[] parameters)
        {
            DataBaseConnector myDb = new DataBaseConnector(connectionString);

            object result = new object();

            try
            {
                using (DbCommand cmd = myDb.GetSqlStringCommand(qry))
                {
                    using (DbConnection cn = myDb.CreateConnection())
                    {
                        cn.Open();
                        cmd.Parameters.Clear();
                        cmd.Connection = cn;
                        cmd.Parameters.AddRange(parameters);
                        result = myDb.ExecuteScalar(cmd);
                        cn.Close();
                    }

                    if (result == DBNull.Value || result == null)
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("GetSingleValue:{0} qry:{1}", ex.Message, qry), ex);
            }
        }

        [Obsolete("Please use GetSingleValue<T>")]
        public static object GetSingleValue(string qry, SqlConnection connection, params IDataParameter[] parameters)
        {
            object result = new object();

            try
            {
                if (connection.State != ConnectionState.Open) connection.Open();

                using (SqlCommand cmd = new SqlCommand(qry, connection))
                {
                    cmd.Parameters.AddRange(parameters);
                    result = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("GetSingleValue:{0} qry:{1}", ex.Message, qry), ex);
            }

            return result;
        }

        public static IDataReader GetDataReader(string qry, string connectionString, params IDataParameter[] parameters)
        {
            DataBaseConnector myDb = new DataBaseConnector(connectionString);

            try
            {
                IDataReader reader;

                using (DbCommand cmd = myDb.GetSqlStringCommand(qry))
                {
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.AddRange(parameters);
                    reader = myDb.ExecuteReader(cmd);
                }

                return reader;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("GetDataReader:{0} qry:{1}", ex.Message, qry), ex);
            }
            finally
            {
                string[] methodNames = new StackTrace().GetFrames().Select(x => x.GetMethod()).Where(x => x.Module.Name.Contains("MHB")).Select(x => x.Name).ToArray();
                Trace.WriteLine(string.Format("\n********GetDataReader: {0}\n", string.Join(">", methodNames.Reverse())));
            }
        }

        public static IDataReader GetDataReader(string qry, SqlConnection connection, params IDataParameter[] parameters)
        {
            IDataReader reader = null;

            try
            {
                if (connection.State != ConnectionState.Open) connection.Open();

                using (SqlCommand cmd = new SqlCommand(qry, connection))
                {
                    cmd.CommandTimeout = 180;
                    cmd.Parameters.AddRange(parameters);
                    reader = cmd.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("GetDataReader:{0} qry:{1}", ex.Message, qry), ex);
            }

            return reader;
        }

        public static int ExecuteQuery(string qry, string connectionString, params IDataParameter[] parameters)
        {
            DataBaseConnector myDb = new DataBaseConnector(connectionString);
            int rowsAffected = 0;

            try
            {
                using (DbConnection cn = myDb.CreateConnection())
                {
                    if (cn.State != ConnectionState.Open)
                    {
                        cn.Open();
                    }

                    using (DbTransaction trans = cn.BeginTransaction())
                    {
                        using (DbCommand cmd = myDb.GetSqlStringCommand(qry))
                        {
                            cmd.Parameters.AddRange(parameters);
                            rowsAffected = myDb.ExecuteNonQuery(cmd, trans);
                            trans.Commit();
                        }
                    }

                    if (cn.State != ConnectionState.Closed)
                    {
                        cn.Close();
                    }
                }

                return rowsAffected;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("ExecuteQuery:{0} qry:{1}", ex.Message, qry), ex);
            }
        }

        public static int ExecuteQuery(string qry, SqlConnection connection, params IDataParameter[] parameters)
        {
            int rowsAffected = 0;

            try
            {
                if (connection.State != ConnectionState.Open) connection.Open();

                using (SqlCommand cmd = new SqlCommand(qry, connection))
                {
                    cmd.Parameters.AddRange(parameters);
                    rowsAffected = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("ExecuteQuery:{0} qry:{1}", ex.Message, qry), ex);
            }

            return rowsAffected;
        }

        public static DataTable GetDataTable(string qry, string connectionString, params IDataParameter[] parameters)
        {
            DataBaseConnector myDb = new DataBaseConnector(connectionString);

            try
            {
                DataTable table = new DataTable();
                using (DbCommand cmd = myDb.GetSqlStringCommand(qry))
                {
                    cmd.Parameters.AddRange(parameters);

                    using (DbDataAdapter adapter = myDb.GetDataAdapter())
                    {
                        adapter.SelectCommand = cmd;
                        adapter.SelectCommand.Connection = myDb.CreateConnection();
                        adapter.SelectCommand.Connection.Open();
                        adapter.Fill(table);
                        adapter.SelectCommand.Connection.Close();
                    }
                }

                return table;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("GetDataTable:{0} qry:{1}", ex.Message, qry), ex);
            }
        }
    }
}