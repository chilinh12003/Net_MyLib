using System;
using System.Data;
using System.Configuration;
using System.Data.OracleClient;

namespace MyLib.MyConnect.Oracle
{
    public class MyOracleGetData
    { 
        private OracleConnection Conn;

        public MyOracleGetData()
        {
            Conn = MyOracleConnect.GetConnection();
        }

        public MyOracleGetData(string KeyOnConfig)
        {
            Conn = MyOracleConnect.GetConnection(KeyOnConfig);
        }

        /// <summary>
        /// Khởi tạo đối tượng với nội dung của chuỗi connect hoặc là key trong webconfig
        /// </summary>
        /// <param name="ConnectionString"></param>
        /// <param name="IsContent">= True: là nội dung của chuỗi connection, = False là key trong webconfig</param>
        public MyOracleGetData(string ConnectionString, bool IsContent)
        {
            Conn = MyOracleConnect.GetConnection(ConnectionString, IsContent);
        }
        /// <summary> 
        /// Lẫy dữ liệu và trả về 1 DataSet
        /// </summary>
        /// <param name="strProcedure">Tên Store cần chạy</param>
        /// <returns></returns>
        public DataSet GetDataSet(string strProcedure)
        {
            OracleCommand Cmd = new OracleCommand();
            Cmd.CommandTimeout = Conn.ConnectionTimeout;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = strProcedure;
            DataSet Ds = new DataSet();
            OracleDataAdapter Da = new OracleDataAdapter();
            try
            {
                Conn.Open();
                Cmd.Connection = Conn;
                Da.SelectCommand = Cmd;
                Da.Fill(Ds);
            }
            catch (OracleException ex)
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
            OracleCommand Cmd = new OracleCommand();
            Cmd.CommandTimeout = Conn.ConnectionTimeout;
            Cmd.CommandType = CommandType.StoredProcedure;
            Cmd.CommandText = strProcedure;
            DataTable Dt = new DataTable();
            OracleDataAdapter Da = new OracleDataAdapter();
            try
            {
                Conn.Open();
                Cmd.Connection = Conn;
                Da.SelectCommand = Cmd;
                Da.Fill(Dt);

            }
            catch (OracleException ex)
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
        /// Lấy dữ liệu theo đâu query
        /// </summary>
        /// <param name="strQuery"></param>
        /// <returns></returns>
        public DataTable GetDataTable_Query(string strQuery)
        {
            OracleCommand Cmd = new OracleCommand();
            Cmd.CommandTimeout = Conn.ConnectionTimeout;
            Cmd.CommandType = CommandType.Text;
            Cmd.CommandText = strQuery;
            DataTable Dt = new DataTable();
            OracleDataAdapter Da = new OracleDataAdapter();
            try
            {
                Conn.Open();
                Cmd.Connection = Conn;
                Da.SelectCommand = Cmd;
                Da.Fill(Dt);
            }
            catch (OracleException ex)
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
                OracleCommand Cmd = new OracleCommand();
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
            catch (OracleException ex)
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
            OracleCommand Cmd = new OracleCommand();
            Cmd.CommandTimeout = Conn.ConnectionTimeout;
            Cmd.CommandType = CommandType.Text;
            Cmd.CommandText = strQuery;
           
            try
            {
                Conn.Open();
                Cmd.Connection = Conn;             
                return Cmd.ExecuteScalar();
            }          
            catch (OracleException ex)
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
                OracleCommand Cmd = new OracleCommand();
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Conn.Open();
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;
                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }
                OracleParameter p = Cmd.Parameters.Add("@" + strResult, OracleType.Int16);
                p.Direction = ParameterDirection.Output;

                result = Cmd.ExecuteNonQuery();
                strResult = Convert.ToString(p.Value); ;

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
                OracleCommand Cmd = new OracleCommand();
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Conn.Open();
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;
                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }
                OracleParameter[] p = new OracleParameter[arrParaNameOutput.Length];
                for (int j = 0; j < arrParaNameOutput.Length; j++)
                {
                    p[j] = new OracleParameter("@" + arrParaNameOutput[j], arrValueOutput[j]);
                    p[j].Direction = ParameterDirection.InputOutput;
                    Cmd.Parameters.Add(p[j]);
                }

                using (OracleDataAdapter adap = new OracleDataAdapter())
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
            catch (OracleException ex)
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
                OracleCommand Cmd = new OracleCommand();
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
        ///  lấy về 1 datatable từ Storeprocedure
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
                OracleCommand Cmd = new OracleCommand();
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

                using (OracleDataAdapter adap = new OracleDataAdapter())
                {
                    adap.SelectCommand = Cmd;
                    using (DataTable result = new DataTable())
                    {
                        adap.Fill(result); return result;
                    }
                }
            }
            catch (OracleException ex)
            {

                throw ex;

            }
            finally
            {
                Conn.Close();
            }

        }

        /// <summary>
        /// lấy về 1 dataset từ Storeprocedure
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
                OracleCommand Cmd = new OracleCommand();
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;

                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }

                using (OracleDataAdapter adap = new OracleDataAdapter())
                {
                    adap.SelectCommand = Cmd;
                    using (DataSet result = new DataSet())
                    {
                        adap.Fill(result); return result;
                    }
                }
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }

        public DataSet GetDataSet(string strProcedure, string[] arrParaNames, string[] arrValues, int Startrecord, int Maxrecord)
        {

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
                    Cmd.Parameters.AddWithValue("@" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }


                using (OracleDataAdapter adap = new OracleDataAdapter())
                {
                    adap.SelectCommand = Cmd;
                    using (DataSet result = new DataSet())
                    {
                        adap.Fill(result, Startrecord, Maxrecord, "tbresult"); return result;
                    }
                }
            }
            catch (OracleException ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
        }
       
    }
}