using System;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace MyLib.MyConnect.MSSQL
{
    public class MyExecuteData
    {
        private SqlConnection Conn;
        private SqlTransaction Tran;

        public bool UseTransction = false;

        /// <summary>
        /// Khởi tạo với 1 connection có sẵn trong webconfig
        /// </summary>
        public MyExecuteData()
        {
            Conn = MyConnect.GetConnection();
        }

        /// <summary>
        /// Khởi tạo với 1 connection khác
        /// </summary>
        /// <param name="KeyOnConfig">Key chứa chuỗi connect trong config</param>
        public MyExecuteData(string KeyOnConfig)
        {
            Conn = MyConnect.GetConnection(KeyOnConfig);
        }

        /// <summary>
        /// Khởi tạo đối tượng với nội dung của chuỗi connect hoặc là key trong webconfig
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="IsContent">= True: là nội dung của chuỗi connection, = False là key trong webconfig</param>
        public MyExecuteData(string ConnectionString, bool IsContent)
        {
            Conn = MyConnect.GetConnection(ConnectionString, IsContent);
        }
        
        [Obsolete("Sử dụng có thể bị lỗi SQL Injection")]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Query"></param>
        /// <returns></returns>
        public int ExecQuery(string Query)
        {
            int result = -1;
            SqlCommand Cmd = new SqlCommand();
           
            try
            {
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = Query;
                Conn.Open();
                result = Cmd.ExecuteNonQuery();

            }
            catch (SqlException ex)
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
        /// Sử dụng hàm này để loại bỏ SQL Injection
        /// </summary>
        /// <param name="Query"></param>
        /// <param name="arrParaNames"></param>
        /// <param name="arrValues"></param>
        /// <returns></returns>
        public int ExecQuery(string Query, string[] arrParaNames, string[] arrValues)
        {
            int result = -1;
            SqlCommand Cmd = new SqlCommand();

            try
            {
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.Text;

                Cmd.CommandText = Query;
                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    SqlParameter mPara = new SqlParameter(arrParaNames[i], arrValues[i]);
                    Cmd.Parameters.Add(mPara);
                }

                Conn.Open();
                result = Cmd.ExecuteNonQuery();

            }
            catch (SqlException ex)
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
            SqlCommand Cmd = new SqlCommand();
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

                if (UseTransction)
                {
                    Tran = Conn.BeginTransaction();
                    Cmd.Transaction = Tran;
                }

                result = Cmd.ExecuteNonQuery();

                if (UseTransction)
                    Tran.Commit();
            }
            catch (SqlException ex)
            {
                if (UseTransction)
                    Tran.Rollback();

                throw ex;
            }
            finally
            {
                Conn.Close();

            }

            return result;
        }

        public int ExecProcedure(string strProcedure, string[] arrParaNames, object[] arrValues, SqlDbType[] arrType)
        {
            int result = -1;
            SqlCommand Cmd = new SqlCommand();
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
                    //Cmd.Parameters.AddWithValue("@" + argNames[i], argTypes[i]).Value = argValues[i];//With VS2003 use method Add()
                }
                Conn.Open();
                result = Cmd.ExecuteNonQuery();

            }
            catch (SqlException ex)
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
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;

                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@" + arrParaNames[i], arrValues[i]);
                }

                SqlParameter p = Cmd.Parameters.Add("@" + strResult, SqlDbType.Int);
                p.Direction = ParameterDirection.Output;

                Conn.Open();
                result = Cmd.ExecuteNonQuery();
                strResult = Convert.ToString(p.Value);
            }
            catch (SqlException ex)
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

            SqlCommand Cmd = new SqlCommand();
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
            catch (SqlException ex)
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

            SqlCommand Cmd = new SqlCommand();
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
                if (UseTransction)
                {
                    Tran = Conn.BeginTransaction();
                    Cmd.Transaction = Tran;
                }
                SqlDataReader mReader = Cmd.ExecuteReader();

                if(UseTransction)
                    Tran.Commit();

                DataTable result = new DataTable();
                result.Load(mReader);
                return result;
            }
            catch (SqlException ex)
            {
                if(UseTransction)
                    Tran.Rollback();
                throw ex;
                // chuyen ve trang loi cua server
            }
            finally
            {

                Conn.Close();
            }

        }

        /// <summary>
        /// Added   : 3h PM, 08/09/2009
        /// By      : PCuong
        /// Exec procedure with names, values and types
        /// </summary>
        /// <param name="strProcedure"></param>
        /// <param name="argNames"></param>
        /// <param name="argValues"></param>
        /// <param name="argTypes"></param>
        /// <returns></returns>
        public DataTable ExecProcedure_Return(string strProcedure, string[] argNames, object[] argValues, SqlDbType[] argTypes)
        {

            SqlCommand Cmd = new SqlCommand();
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
                    //Cmd.Parameters.AddWithValue("@" + argNames[i], argTypes[i]).Value = argValues[i];//With VS2003 use method Add()
                }

                Conn.Open();
                SqlDataReader mReader = Cmd.ExecuteReader();
                DataTable result = new DataTable();
                result.Load(mReader);
                return result;
            }
            catch (SqlException ex)
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
