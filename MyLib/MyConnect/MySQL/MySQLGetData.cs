using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace MyLib.MyConnect.MySQL
{
    public class MySQLGetData
    {
        private MySqlConnection Conn;

        public MySQLGetData()
        {
           Conn = MySQLConnect.GetConnection();
        }

        public MySQLGetData(string str_key)
        {
            Conn = MySQLConnect.GetConnection(str_key);
        }

        [Obsolete("Sử dụng có thể bị lỗi SQL Injection")]
        /// <summary>
        /// Lấy dữ liệu bằng cách truyền vào câu lệnh Query
        /// </summary>
        /// <param name="str_Query">Câu lệnh cần thức thi để lấy dữ liệu</param>
        /// <returns></returns>
        public DataTable GetDataTableByQuery(string str_Query)
        {
            try
            {
                MySqlCommand Cmd = new MySqlCommand();

                if (Conn.State != ConnectionState.Closed)
                {
                    Conn.Close();
                }

                Cmd.CommandTimeout = Conn.ConnectionTimeout; //
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = str_Query;

                using (MySqlDataAdapter adap = new MySqlDataAdapter())
                {
                    adap.SelectCommand = Cmd;
                    using (DataTable result = new DataTable())
                    {
                        adap.Fill(result); return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Conn.State != ConnectionState.Closed)
                {
                    Conn.Close();
                }
            }
        }

        /// <summary>
        /// Sử dụng hàm này để loại bỏ injection
        /// </summary>
        /// <param name="str_Query"></param>
        /// <param name="arrParaNames"></param>
        /// <param name="arrValues"></param>
        /// <returns></returns>
        public DataTable GetDataTableByQuery(string str_Query, string[] arrParaNames, string[] arrValues)
        {
            try
            {
                MySqlCommand Cmd = new MySqlCommand();

                if (Conn.State != ConnectionState.Closed)
                {
                    Conn.Close();
                }

                Cmd.CommandTimeout = Conn.ConnectionTimeout; //
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = str_Query;

                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    MySqlParameter mPara = new MySqlParameter(arrParaNames[i], arrValues[i]);
                    Cmd.Parameters.Add(mPara);
                }

                using (MySqlDataAdapter adap = new MySqlDataAdapter())
                {
                    adap.SelectCommand = Cmd;
                    using (DataTable result = new DataTable())
                    {
                        adap.Fill(result); return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Conn.State != ConnectionState.Closed)
                {
                    Conn.Close();
                }
            }
        }

        public DataTable GetDataTable(string strProcedure)
        {
            try
            {
                MySqlCommand Cmd = new MySqlCommand();

                if (Conn.State != ConnectionState.Closed)
                {
                    Conn.Close();
                }

                Cmd.CommandTimeout = Conn.ConnectionTimeout; //
                
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;

                using (MySqlDataAdapter adap = new MySqlDataAdapter())
                {
                    adap.SelectCommand = Cmd;
                    using (DataTable result = new DataTable())
                    {
                        adap.Fill(result); return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Conn.State != ConnectionState.Closed)
                {
                    Conn.Close();
                }
            }
        }

        public DataTable GetDataTable(string strProcedure, string[] arrParaNames, string[] arrValues)
        {
            if (arrParaNames.Length != arrValues.Length)
                throw new ArgumentException("The Array Parameter Names and Array Parameter Values is not equal", "arrParaNames or arrValues");

            try
            {
                MySqlCommand Cmd = new MySqlCommand();
                if (Conn.State != ConnectionState.Closed)
                {
                    Conn.Close();
                }
                Cmd.CommandTimeout = Conn.ConnectionTimeout; //
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;
                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@p" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }

                using (MySqlDataAdapter adap = new MySqlDataAdapter())
                {
                    adap.SelectCommand = Cmd;
                    using (DataTable result = new DataTable())
                    {
                        adap.Fill(result); return result;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;

            }
            finally
            {
                if (Conn.State != ConnectionState.Closed)
                {
                    Conn.Close();
                }
            }
            //return null;

        }


        public DataTable GetDataTable(string strProcedure, string[] arrParaNames, object[] arrValues, MySqlDbType[] arrType)
        {
            if (arrParaNames.Length != arrValues.Length)
                throw new ArgumentException("The Array Parameter Names and Array Parameter Values is not equal", "arrParaNames or arrValues");

            try
            {
                MySqlCommand Cmd = new MySqlCommand();
                if (Conn.State != ConnectionState.Closed)
                {
                    Conn.Close();
                }
                Cmd.CommandTimeout = Conn.ConnectionTimeout; //
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;
                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@p" + arrParaNames[i], arrType[i]).Value = arrValues[i];
                    //Cmd.Parameters.AddWithValue("@p" + argNames[i], argTypes[i]).Value = argValues[i];//With VS2003 use method Add()
                }

                using (MySqlDataAdapter adap = new MySqlDataAdapter())
                {
                    adap.SelectCommand = Cmd;
                    using (DataTable result = new DataTable())
                    {
                        adap.Fill(result); return result;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;

            }
            finally
            {
                if (Conn.State != ConnectionState.Closed)
                {
                    Conn.Close();
                }
            }
            //return null;

        }

        public object GetExecuteScalar(string str_Query)
        {
            try
            {
                MySqlCommand Cmd = new MySqlCommand();

                if (Conn.State != ConnectionState.Closed)
                {
                    Conn.Close();
                }

                Cmd.CommandTimeout = Conn.ConnectionTimeout; //
                Conn.Open();
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = str_Query;

                return Cmd.ExecuteScalar();
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
            finally
            {
                if (Conn.State != ConnectionState.Closed)
                {
                    Conn.Close();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strProcedure"></param>
        /// <param name="arrParaNames"></param>
        /// <param name="arrValues"></param>
        /// <param name="arrType"></param>
        /// <returns></returns>
        public object GetExecuteScalar(string strProcedure, string[] arrParaNames, object[] arrValues, MySqlDbType[] arrType)
        {
            if (arrParaNames.Length != arrValues.Length)
                throw new ArgumentException("The Array Parameter Names and Array Parameter Values is not equal", "arrParaNames or arrValues");

            try
            {
                MySqlCommand Cmd = new MySqlCommand();
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
                    Cmd.Parameters.AddWithValue("@p" + arrParaNames[i], arrType[i]).Value = arrValues[i];
                    //Cmd.Parameters.AddWithValue("@p" + argNames[i], argTypes[i]).Value = argValues[i];//With VS2003 use method Add()
                }

                return Cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {

                throw ex;

            }
            finally
            {
                Conn.Close();
            }
        }

        public object GetExecuteScalar(string strProcedure, string[] arrParaNames, string[] arrValues)
        {
            if (arrParaNames.Length != arrValues.Length)
                throw new ArgumentException("The Array Parameter Names and Array Parameter Values is not equal", "arrParaNames or arrValues");

            try
            {
                MySqlCommand Cmd = new MySqlCommand();
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
                    Cmd.Parameters.AddWithValue("@p" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }

                return Cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {

                throw ex;

            }
            finally
            {
                Conn.Close();
            }
        }

        public int Returnvalue(string strProcedure, string[] arrParaNames, string[] arrValues, ref string strResult)
        {
            int result = -1;
            if (arrParaNames.Length != arrValues.Length)
                throw new ArgumentException("The Array Parameter Names and Array Parameter Values is not equal", "arrParaNames or arrValues");

            try
            {
                MySqlCommand Cmd = new MySqlCommand();
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Conn.Open();
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;
                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@p" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }
                MySqlParameter p = Cmd.Parameters.AddWithValue("@p" + strResult, SqlDbType.Int);
                p.Direction = ParameterDirection.Output;

                result = Cmd.ExecuteNonQuery();
                strResult = Convert.ToString(p.Value); ;

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
                MySqlCommand Cmd = new MySqlCommand();
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Conn.Open();
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;
                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@p" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }
                MySqlParameter[] p = new MySqlParameter[arrParaNameOutput.Length];
                for (int j = 0; j < arrParaNameOutput.Length; j++)
                {
                    p[j] = new MySqlParameter("@p" + arrParaNameOutput[j], arrValueOutput[j]);
                    p[j].Direction = ParameterDirection.InputOutput;
                    Cmd.Parameters.Add(p[j]);
                }

                using (MySqlDataAdapter adap = new MySqlDataAdapter())
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
            catch (Exception ex)
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
                MySqlCommand Cmd = new MySqlCommand();
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Conn.Open();
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;
                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@p" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }
                result = Cmd.ExecuteScalar().ToString();

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
                MySqlCommand Cmd = new MySqlCommand();
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;

                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@p" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }

                using (MySqlDataAdapter adap = new MySqlDataAdapter())
                {
                    adap.SelectCommand = Cmd;
                    using (DataSet result = new DataSet())
                    {
                        adap.Fill(result); return result;
                    }
                }
            }
            catch (Exception ex)
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
                MySqlCommand Cmd = new MySqlCommand();
                Cmd.CommandTimeout = Conn.ConnectionTimeout;
                Cmd.Connection = Conn;
                Cmd.CommandType = CommandType.StoredProcedure;
                Cmd.CommandText = strProcedure;

                for (int i = 0; i < arrParaNames.Length; i++)
                {
                    Cmd.Parameters.AddWithValue("@p" + arrParaNames[i], arrValues[i]);//With VS2003 use method Add()
                }


                using (MySqlDataAdapter adap = new MySqlDataAdapter())
                {
                    adap.SelectCommand = Cmd;
                    using (DataSet result = new DataSet())
                    {
                        adap.Fill(result, Startrecord, Maxrecord, "tbresult"); return result;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Conn.Close();
            }
            //return null;
        }

        #region GET XML DATA
        /// <summary>
        /// Lấy dữ liệu từ 1 file XML
        /// </summary>
        /// <param name="FileName">Đường dẫn tuyệt đối tới File XML</param>
        /// <returns></returns>
        public DataSet GetXMLData(string FileName)
        {
            try
            {
                System.Xml.XmlTextReader mReader = new System.Xml.XmlTextReader(FileName);
                DataSet mDataSet = new DataSet();
                mDataSet.ReadXml(mReader);
                mReader.Close();
                return mDataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
