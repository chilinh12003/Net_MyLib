using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Data.SqlClient;
using MyLib.MyUtility;
namespace MyLib.MyConnect.MSSQL
{
    public class MyConnect
    {
        /// <summary>
        /// Lấy chuỗi kết nối từ WebConfig và trả về đối tường SqlConnection
        /// </summary>
        /// <returns></returns>
        public static SqlConnection GetConnection()
        {
            try
            {
                string strConnect = MyConnectConfig.GetConfigConnectionString(MyConnectConfig.DBType.MSSQL);

                SqlConnection myConnection = new SqlConnection(strConnect);
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
                
                return myConnection;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static SqlConnection GetConnection(string KeyOnConfig)
        {
            try
            {
                string strConnect = MyConnectConfig.GetConfigConnectionString(KeyOnConfig);

                SqlConnection myConnection = new SqlConnection(strConnect);

                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
                return myConnection;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strConnection">Nội dung chuỗi connection</param>
        /// <returns></returns>
        public static SqlConnection GetConnection(string ConnectionString, bool IsContent)
        {
            try
            {
                //Kiểm tra License
                if (!MyCheck.CheckLicense())
                    throw new Exception("Errors in the process connection!");

                string strConnect = MyConnectConfig.GetConfigConnectionString(ConnectionString, IsContent);               

                SqlConnection myConnection = new SqlConnection(strConnect);

                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
                return myConnection;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        public static string ConnectionString
        {
            get
            {
                string strConnect = MyConnectConfig.GetConfigConnectionString(MyConnectConfig.DBType.MSSQL);
                return strConnect;
            }
        }
    }
}
