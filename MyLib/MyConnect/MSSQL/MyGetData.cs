
using System;
using System.Data;
using System.Data.SqlClient;

namespace MyLib.MyConnect.MSSQL
{

    public class MyGetData
    {
        private SqlConnection Conn;

        public MyGetData()
        {
            Conn = MyConnect.GetConnection();
        }

        public MyGetData(string KeyOnConfig)
        {
            Conn = MyConnect.GetConnection(KeyOnConfig);
        }

        /// <summary>
        /// Khởi tạo đối tượng với nội dung của chuỗi connect hoặc là key trong webconfig
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="IsContent">= True: là nội dung của chuỗi connection, = False là key trong webconfig</param>
        public MyGetData(string ConnectionString, bool IsContent)
        {
            Conn = MyConnect.GetConnection(ConnectionString, IsContent);
        }

        /// <summary> 
        /// Lẫy dữ liệu và trả về 1 DataSet
        /// </summary>
        /// <param name="strProcedure">Tên Store cần chạy</param>
        /// <returns></returns>
        public DataSet GetDataSet(string strProcedure)
        {
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandTimeout = Conn.ConnectionTimeout;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = strProcedure;
            DataSet Ds = new DataSet();
            SqlDataAdapter Da = new SqlDataAdapter();
            try
            {
                Conn.Open();
                Cmd.Connection = Conn;
                Da.SelectCommand = Cmd;
                Da.Fill(Ds);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return Ds;
        }

        /// <summary>
        /// Lẫy dữ liệu và trả về 1 DataTable
        /// </summary>
        /// <param name="strProcedure">ên Store cần chạy</param>
        /// <returns></returns>
        public DataTable GetDataTable(string strProcedure)
        {
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandTimeout = Conn.ConnectionTimeout;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = strProcedure;
            DataTable Dt = new DataTable();
            SqlDataAdapter Da = new SqlDataAdapter();
            try
            {
                Conn.Open();
                Cmd.Connection = Conn;
                Da.SelectCommand = Cmd;
                Da.Fill(Dt);

            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return Dt;
        }

        [Obsolete("Sử dụng có thể bị lỗi SQL Injection")]
        /// <summary>
        /// Lấy dữ liệu theo đâu query
        /// </summary>
        /// <param name="strQuery"></param>
        /// <returns></returns>
        public DataTable GetDataTable_Query(string strQuery)
        {
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandTimeout = Conn.ConnectionTimeout;
            Cmd.CommandType = CommandType.Text;
            Cmd.CommandText = strQuery;
            DataTable Dt = new DataTable();
            SqlDataAdapter Da = new SqlDataAdapter();
            try
            {
                Conn.Open();
                Cmd.Connection = Conn;
                Da.SelectCommand = Cmd;
                Da.Fill(Dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return Dt;
        }

        /// <summary>
        /// Sử dụng hàm này để loại bỏ SQL Injection
        /// </summary>
        /// <param name="strQuery"></param>
        /// <param name="arrParaNames"></param>
        /// <param name="arrValues"></param>
        /// <returns></returns>
        public DataTable GetDataTable_Query(string strQuery, string[] arrParaNames, string[] arrValues)
        {
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandTimeout = Conn.ConnectionTimeout;
            Cmd.CommandType = CommandType.Text;
            Cmd.CommandText = strQuery;
            for (int i = 0; i < arrParaNames.Length; i++)
            {
                SqlParameter mPara = new SqlParameter(arrParaNames[i], arrValues[i]);
                Cmd.Parameters.Add(mPara);
            }
            DataTable Dt = new DataTable();
            SqlDataAdapter Da = new SqlDataAdapter();
            try
            {
                Conn.Open();
                Cmd.Connection = Conn;
                Da.SelectCommand = Cmd;
                Da.Fill(Dt);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            return Dt;
        }

        public object GetExecuteScalar(string strProcedure, string[] arrParaNames, string[] arrValues)
        {
            if (arrParaNames.Length != arrValues.Length)
                throw new ArgumentException("The Array Parameter Names and Array Parameter Values is not equal", "arrParaNames or arrValues");

            try
            {
                SqlCommand Cmd = new SqlCommand();
                if (Conn.State != ConnectionState.Closed)
                {
                    Conn.Close();
                }
                Cmd.CommandTimeout = Conn.ConnectionTimeout; //
                Conn.Open();
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;
                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }

                return Cmd.ExecuteScalar();
            }
            catch (SqlException ex)
            {

                throw ex;

            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strQuery"></param>
        /// <returns></returns>
        public object GetExecuteScalar_Query(string strQuery)
        {
            SqlCommand Cmd = new SqlCommand();
            Cmd.CommandTimeout = Conn.ConnectionTimeout;
            Cmd.CommandType = CommandType.Text;
            Cmd.CommandText = strQuery;
           
            try
            {
                Conn.Open();
                Cmd.Connection = Conn;             
                return Cmd.ExecuteScalar();
            }          
            catch (SqlException ex)
            {

                throw ex;

            }
            finally
            {
                Conn.Close();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strProcedure"></param>
        /// <param name="arrParaNames"></param>
        /// <param name="arrValues"></param>
        /// <param name="strResult"></param>
        /// <returns></returns>
        public int Returnvalue(string strProcedure, string[] arrParaNames, string[] arrValues, ref string strResult)
        {
            int result = -1;
            if (arrParaNames.Length != arrValues.Length)
                throw new ArgumentException("The Array Parameter Names and Array Parameter Values is not equal", "arrParaNames or arrValues");

            try
            {
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Conn.Open();
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;
                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }
                SqlParameter p = Cmd.Parameters.Add("@" + strResult, SqlDbType.Int);
                p.Direction = ParameterDirection.Output;

                result = Cmd.ExecuteNonQuery();
                strResult = Convert.ToString(p.Value); ;

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
        /// <param name="arrParaNameOutput"></param>
        /// <param name="arrValueOutput"></param>
        /// <returns></returns>
        public DataTable GetDataTable(string strProcedure, string[] arrParaNames, string[] arrValues, string[] arrParaNameOutput, ref string[] arrValueOutput)
        {
            if ((arrParaNames.Length != arrValues.Length) || (arrParaNameOutput.Length != arrValueOutput.Length))
                throw new ArgumentException("The Array Parameter Names and Array Parameter Values is not equal", "arrParaNames or arrValues");

            DataTable result = new DataTable();
            try
            {
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Conn.Open();
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;
                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }
                SqlParameter[] p = new SqlParameter[arrParaNameOutput.Length];
                for (int j = 0; j < arrParaNameOutput.Length; j++)
                {
                    p[j] = new SqlParameter("@" + arrParaNameOutput[j], arrValueOutput[j]);
                    p[j].Direction = ParameterDirection.InputOutput;
                    Cmd.Parameters.Add(p[j]);
                }

                using (SqlDataAdapter adap = new SqlDataAdapter())
                {
                    adap.SelectCommand = Cmd;
                    adap.Fill(result);
                }

                /////////////
                for (int j = 0; j < arrParaNameOutput.Length; j++)
                {
                    arrValueOutput[j] = p[j].Value.ToString();
                }
                return result;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }

        }

        /// <summary>
        /// Trả về giá trị của cell[0][0] của Table lấy được
        /// </summary>
        /// <param name="strProcedure"></param>
        /// <param name="arrParaNames"></param>
        /// <param name="arrValues"></param>
        /// <returns></returns>
        public string Returnvalue(string strProcedure, string[] arrParaNames, string[] arrValues)
        {
            string result;
            if (arrParaNames.Length != arrValues.Length)
                throw new ArgumentException("The Array Parameter Names and Array Parameter Values is not equal", "arrParaNames or arrValues");

            try
            {
                SqlCommand Cmd = new SqlCommand();
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Conn.Open();
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;
                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }
                result = Cmd.ExecuteScalar().ToString();

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
        public DataTable GetDataTable(string strProcedure, string[] arrParaNames, string[] arrValues)
        {
            if (arrParaNames.Length != arrValues.Length)
                throw new ArgumentException("The Array Parameter Names and Array Parameter Values is not equal", "arrParaNames or arrValues");

            try
            {
                SqlCommand Cmd = new SqlCommand();
                if (Conn.State != ConnectionState.Closed)
                {
                    Conn.Close();
                }
                Cmd.CommandTimeout = Conn.ConnectionTimeout; //
                Conn.Open();
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;
                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }

                using (SqlDataAdapter adap = new SqlDataAdapter())
                {
                    adap.SelectCommand = Cmd;
                    using (DataTable result = new DataTable())
                    {
                        adap.Fill(result); return result;
                    }
                }
            }
            catch (SqlException ex)
            {

                throw ex;

            }
            finally
            {
                Conn.Close();
            }
            //return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strProcedure"></param>
        /// <param name="arrParaNames"></param>
        /// <param name="arrValues"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string strProcedure, string[] arrParaNames, string[] arrValues)
        {

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
                    Cmd.Parameters.AddWithValue("@" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }

                using (SqlDataAdapter adap = new SqlDataAdapter())
                {
                    adap.SelectCommand = Cmd;
                    using (DataSet result = new DataSet())
                    {
                        adap.Fill(result); return result;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            //return null;
        }

        public DataSet GetDataSet(string strProcedure, string[] arrParaNames, string[] arrValues, int Startrecord, int Maxrecord)
        {

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
                    Cmd.Parameters.AddWithValue("@" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }


                using (SqlDataAdapter adap = new SqlDataAdapter())
                {
                    adap.SelectCommand = Cmd;
                    using (DataSet result = new DataSet())
                    {
                        adap.Fill(result, Startrecord, Maxrecord, "tbresult"); return result;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            //return null;
        }
       
    }
}