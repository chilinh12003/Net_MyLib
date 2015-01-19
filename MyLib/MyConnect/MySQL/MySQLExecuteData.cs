using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MyLib.MyConnect.MySQL
{
    public class MySQLExecuteData
    {
        private MySqlConnection Conn;
        private MySqlTransaction Tran;
       
        public MySQLExecuteData()
        {
            Conn = MySQLConnect.GetConnection();
        }

        public MySQLExecuteData(string str_key)
        {
            Conn = MySQLConnect.GetConnection(str_key);
        }

         [Obsolete("Sử dụng có thể bị lỗi SQL Injection")]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_Query"></param>
        /// <returns></returns>
        public int ExecQuery(string str_Query )
        {
            MySqlCommand Cmd = new MySqlCommand();

            try
            {
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;// mo connect

                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = str_Query;

                Conn.Open();
                int result = Cmd.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
                //chuyen ve trang loi cua server
            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// Sử dụng hàm này để loại bỏ lỗi Injection
        /// </summary>
        /// <param name="str_Query"><code>
        /// String Query =  INSERT INTO sms_receive_queue(user_id,service_id) 
        ///                 VALUE (@USER_ID, @SERVICE_ID)
        /// </code></param>
        /// <param name="arrParaNames">
        /// <code>string[] arr_para = { "@USER_ID", "@SERVICE_ID"}</code>
        /// </param>
        /// <param name="arrValues">
        /// <example>
        /// string[] arr_value = { USER_ID, SERVICE_ID}
        /// </example>
        /// </param>
        /// <returns></returns>
        public int ExecQuery(string str_Query, string[] arrParaNames, string[] arrValues)
        {
            MySqlCommand Cmd = new MySqlCommand();

            try
            {
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;// mo connect

                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = str_Query;
                
                for(int i =0;i < arrParaNames.Length; i++)
                {
                    MySqlParameter mPara = new MySqlParameter(arrParaNames[i],arrValues[i]);
                    
                    Cmd.Parameters.Add(mPara);
                }

                Conn.Open();
                int result = Cmd.ExecuteNonQuery();
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
                //chuyen ve trang loi cua server
            }
            finally
            {
                Conn.Close();
            }
        }

        [Obsolete("Sử dụng có thể bị lỗi SQL Injection")]
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_Query"></param>
        /// <returns></returns>
        public int ExecQueryTran(string str_Query)
        {
            MySqlCommand Cmd = new MySqlCommand();
           
            try
            {
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;// mo connect

                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = str_Query;

                Conn.Open();
                Tran = Conn.BeginTransaction();
                Cmd.Transaction = Tran;
                int result = Cmd.ExecuteNonQuery();
                Tran.Commit();
                return result;
            }
            catch (Exception ex)
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


        /// <summary>
        /// Sử dụng hàm này để loại bỏ SQL Injection
        /// </summary>
        /// <param name="str_Query"></param>
        /// <param name="arrParaNames"></param>
        /// <param name="arrValues"></param>
        /// <returns></returns>
        public int ExecQueryTran(string str_Query, string[] arrParaNames, string[] arrValues)
        {
            MySqlCommand Cmd = new MySqlCommand();

            try
            {
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;// mo connect

                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = str_Query;
                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    MySqlParameter mPara = new MySqlParameter(arrParaNames[i], arrValues[i]);
                    Cmd.Parameters.Add(mPara);
                }
                Conn.Open();
                Tran = Conn.BeginTransaction();
                Cmd.Transaction = Tran;
                int result = Cmd.ExecuteNonQuery();
                Tran.Commit();
                return result;
            }
            catch (Exception ex)
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

        public int ExecProcedure(string strProcedure, string[] arrParaNames, string[] arrValues)
        {
            int result = -1;
            MySqlCommand Cmd = new MySqlCommand();
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
                    Cmd.Parameters.AddWithValue("@p" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }
                Conn.Open();
                result = Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                Conn.Close();

            }

            return result;
        }

        public int ExecProcedure(string strProcedure, string[] arrParaNames, object[] arrValues, MySqlDbType[] arrType)
        {
            int result = -1;
            MySqlCommand Cmd = new MySqlCommand();
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
                    Cmd.Parameters.AddWithValue("@p" + arrParaNames[i], arrType[i]).Value = arrValues[i];
                    //Cmd.Parameters.AddWithValue("@p" + argNames[i], argTypes[i]).Value = argValues[i];//With VS2003 use method Add()
                }
                Conn.Open();
                result = Cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();

            }

            return result;
        }

        public int ExecProcedure(string strProcedure, string[] arrParaNames, string[] arrValues, ref string strResult)
        {
            int result = -1;// Ket noi duoc voi Database khong?
            if (arrParaNames.Length != arrValues.Length)
                throw new ArgumentException("The Array Parameter Names and Array Parameter Values is not equal", "arrParaNames or arrValues");

            try
            {
                MySqlCommand Cmd = new MySqlCommand();
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;

                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@p" + arrParaNames[i], arrValues[i]);
                }

                MySqlParameter p = Cmd.Parameters.AddWithValue("@p" + strResult, SqlDbType.Int);
                p.Direction = ParameterDirection.Output;

                Conn.Open();
                result = Cmd.ExecuteNonQuery();
                strResult = Convert.ToString(p.Value);
            }
            catch (Exception ex)
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

            MySqlCommand Cmd = new MySqlCommand();
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
                    Cmd.Parameters.AddWithValue("@p" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }

                Conn.Open();
                Tran = Conn.BeginTransaction();
                Cmd.Transaction = Tran;
                int result = Cmd.ExecuteNonQuery();
                Tran.Commit();
                return result;
            }
            catch (Exception ex)
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

            MySqlCommand Cmd = new MySqlCommand();
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
                    Cmd.Parameters.AddWithValue("@p" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }

                Conn.Open();
                MySqlDataReader mReader = Cmd.ExecuteReader();
                DataTable result = new DataTable();
                result.Load(mReader);
                return result;
            }
            catch (Exception ex)
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
