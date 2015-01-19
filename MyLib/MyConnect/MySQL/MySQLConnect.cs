using System;
using System.Data;
using System.Configuration;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Web;
namespace MyLib.MyConnect.MySQL
{
    public class MySQLConnect
    {
        /// <summary>
        /// Lấy chuỗi kết nối từ WebConfig và trả về đối tường SqlConnection
        /// </summary>
        /// <returns></returns>
        public static MySqlConnection GetConnection()
        {
            try
            {
                string strConnect = MyConnectConfig.GetConfigConnectionString(MyConnectConfig.DBType.MySQL);

                MySqlConnection myConnection = new MySqlConnection(strConnect);
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
               
                return myConnection;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }

        public static string ConnectionString
        {
            get
            {
                string strConnect = MyConnectConfig.GetConfigConnectionString(MyConnectConfig.DBType.MySQL);

                return strConnect;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_key">Tên của key của chuỗi kết nối trong file web.config</param>
        /// <returns></returns>
        public static MySqlConnection GetConnection(string KeyOnConfig)
        {
            try
            {
                string strConnect = MyConnectConfig.GetConfigConnectionString(KeyOnConfig);

                MySqlConnection myConnection = new MySqlConnection(strConnect);
                if (myConnection.State == ConnectionState.Open)
                    myConnection.Close();
                return myConnection;
            }
            catch (MySqlException ex)
            {
                throw ex;
            }
        }
    }
}
