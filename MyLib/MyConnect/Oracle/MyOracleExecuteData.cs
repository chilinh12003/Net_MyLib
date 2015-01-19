using System;
using System.Data;
using System.Configuration;
using System.Data.OracleClient;

namespace MyLib.MyConnect.Oracle
{
    public class MyOracleExecuteData
    {
        private OracleConnection Conn;
        private OracleTransaction Tran;

        /// <summary>
        /// Khởi tạo với 1 connection có sẵn trong webconfig
        /// </summary>
        public MyOracleExecuteData()
        {
            Conn = MyOracleConnect.GetConnection();
        }

        /// <summary>
        /// Khởi tạo với 1 connection khác
        /// </summary>
        /// <param name="KeyOnConfig">Key chứa chuỗi connect trong config</param>
        public MyOracleExecuteData(string KeyOnConfig)
        {
            Conn = MyOracleConnect.GetConnection(KeyOnConfig);
        }

        /// <summary>
        /// Khởi tạo đối tượng với nội dung của chuỗi connect hoặc là key trong webconfig
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="IsContent">= True: là nội dung của chuỗi connection, = False là key trong webconfig</param>
        public MyOracleExecuteData(string ConnectionString, bool IsContent)
        {
            Conn = MyOracleConnect.GetConnection(ConnectionString, IsContent);
        }

        public int ExecQuery(string Query)
        {
            int result = -1;
            OracleCommand Cmd = new OracleCommand();
           
            try
            {
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = Query;
              
                Conn.Open();
                result = Cmd.ExecuteNonQuery();

            }
            catch (OracleException ex)
            {

                throw ex;
            }
            finally
            {
                Conn.Close();

            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strProcedure"></param>
        /// <param name="arrParaNames"></param>
        /// <param name="arrValues"></param>
        /// <returns></returns>
        public int ExecProcedure(string strProcedure, string[] arrParaNames, string[] arrValues)
        {
            int result = -1;
            OracleCommand Cmd = new OracleCommand();
            if (arrParaNames.Length != arrValues.Length)
                throw new ArgumentException("The Array Parameter Names and Array Parameter Values is not equal", "arrParaNames or arrValues");

            try
            {
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;

                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }
                Conn.Open();
                result = Cmd.ExecuteNonQuery();

            }
            catch (OracleException ex)
            {

                throw ex;
            }
            finally
            {
                Conn.Close();
            }

            return result;
        }

        public int ExecProcedure(string strProcedure, string[] arrParaNames, object[] arrValues, OracleType[] arrType)
        {
            int result = -1;
            OracleCommand Cmd = new OracleCommand();
            if (arrParaNames.Length != arrValues.Length)
                throw new ArgumentException("The Array Parameter Names and Array Parameter Values is not equal", "arrParaNames or arrValues");

            try
            {
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;

                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.Add("@" + arrParaNames[i], arrType[i]).Value = arrValues[i];
                }
                Conn.Open();
                result = Cmd.ExecuteNonQuery();

            }
            catch (OracleException ex)
            {

                throw ex;
            }
            finally
            {
                Conn.Close();

            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strProcedure"></param>
        /// <param name="arrParaNames"></param>
        /// <param name="arrValues"></param>
        /// <param name="strResult"></param>
        /// <returns></returns>
        public int ExecProcedure(string strProcedure, string[] arrParaNames, string[] arrValues, ref string strResult)
        {
            int result = -1;// Ket noi duoc voi Database khong?
            if (arrParaNames.Length != arrValues.Length)
                throw new ArgumentException("The Array Parameter Names and Array Parameter Values is not equal", "arrParaNames or arrValues");

            try
            {
                OracleCommand Cmd = new OracleCommand();
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;

                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@" + arrParaNames[i], arrValues[i]);
                }

                OracleParameter p = Cmd.Parameters.Add("@" + strResult, OracleType.Int16);
                p.Direction = ParameterDirection.Output;

                Conn.Open();
                result = Cmd.ExecuteNonQuery();
                strResult = Convert.ToString(p.Value);
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

            return result;
        }

        public int ExecProcedureTran(string strProcedure, string[] arrParaNames, string[] arrValues)
        {

            OracleCommand Cmd = new OracleCommand();
            if (arrParaNames.Length != arrValues.Length)
                throw new ArgumentException("The Array Parameter Names and Array Parameter Values is not equal", "arrParaNames or arrValues");

            try
            {
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;// mo connect

                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;

                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }

                Conn.Open();
                Tran = Conn.BeginTransaction();
                Cmd.Transaction = Tran;
                int result = Cmd.ExecuteNonQuery();
                Tran.Commit();
                return result;
            }
            catch (OracleException ex)
            {
                Tran.Rollback();
                throw ex;
                // chuyen ve trang loi cua server
            }
            finally
            {
                Conn.Close();
            }

        }

        public DataTable ExecProcedure_Return(string strProcedure, string[] arrParaNames, string[] arrValues)
        {

            OracleCommand Cmd = new OracleCommand();
            if (arrParaNames.Length != arrValues.Length)
                throw new ArgumentException("The Array Parameter Names and Array Parameter Values is not equal", "arrParaNames or arrValues");

            try
            {
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;// mo connect

                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;

                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }

                Conn.Open();
                OracleDataReader mReader = Cmd.ExecuteReader();
                DataTable result = new DataTable();
                result.Load(mReader);
                return result;
            }
            catch (OracleException ex)
            {
                throw ex;
                // chuyen ve trang loi cua server
            }
            finally
            {
                Conn.Close();
            }

        }

        /// <summary>
        /// Exec procedure with names, values and types
        /// </summary>
        /// <param name="strProcedure"></param>
        /// <param name="argNames"></param>
        /// <param name="argValues"></param>
        /// <param name="argTypes"></param>
        /// <returns></returns>
        public DataTable ExecProcedure_Return(string strProcedure, string[] argNames, object[] argValues, OracleType[] argTypes)
        {

            OracleCommand Cmd = new OracleCommand();
            if (argValues.Length != argNames.Length)
                throw new ArgumentException("The Array Parameter Names and Array Parameter Values is not equal", "arrParaNames or arrValues");

            try
            {
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;// mo connect

                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;

                for (int i = 0; i < argNames.Length; i++)
                {
                    Cmd.Parameters.Add("@" + argNames[i], argTypes[i]).Value = argValues[i];
                }

                Conn.Open();
                OracleDataReader mReader = Cmd.ExecuteReader();
                DataTable result = new DataTable();
                result.Load(mReader);
                return result;
            }
            catch (OracleException ex)
            {
                throw ex;
                // chuyen ve trang loi cua server
            }
            finally
            {
                Conn.Close();
            }

        }

    }
}
