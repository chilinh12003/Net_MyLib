using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Data.OracleClient;
using System.Data;

namespace MyLib.MyConnect.Oracle
{
   public class MyOracleConnect
    {
        /// <summary>
        /// Lấy chuỗi kết nối từ WebConfig và trả về đối tường OracleConnection
        /// </summary>
        /// <returns></returns>
        public static OracleConnection GetConnection()
        {
            try
            {
                string strConnect = MyConnectConfig.GetConfigConnectionString(MyConnectConfig.DBType.Oracle);

                OracleConnection myConnection = new OracleConnection(strConnect);
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();


                return myConnection;
            }
            catch (OracleException ex)
            {
                throw ex;
            }
        }

        public static OracleConnection GetConnection(string KeyOnConfig)
        {
            try
            {
                string strConnect = MyConnectConfig.GetConfigConnectionString(KeyOnConfig);

                OracleConnection myConnection = new OracleConnection(strConnect);

                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
                return myConnection;
            }
            catch (OracleException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strConnection">Nội dung chuỗi connection</param>
        /// <returns></returns>
        public static OracleConnection GetConnection(string ConnectionString, bool IsContent)
        {
            try
            {
                string strConnect = MyConnectConfig.GetConfigConnectionString(ConnectionString, IsContent);

                OracleConnection myConnection = new OracleConnection(strConnect);

                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
                return myConnection;
            }
            catch (OracleException ex)
            {
                throw ex;
            }
        }
        public static string ConnectionString
        {
            get
            {
                string strConnect = MyConnectConfig.GetConfigConnectionString(MyConnectConfig.DBType.Oracle);
                return strConnect;
            }
        }
    }
}
