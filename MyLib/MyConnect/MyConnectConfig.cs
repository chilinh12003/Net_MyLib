using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using MyLib.MyUtility;
namespace MyLib.MyConnect
{
    public class MyConnectConfig
    {
        static string PasswordEnCode = "CHIlInH123";
        public enum DBType
        {
            Nothing = 0,
            MSSQL = 1,
            MySQL = 2,
            Oracle = 3
        }
        public static string GetConfigConnectionString(DBType mDBType)
        {
            //Kiểm tra License
            if (!MyCheck.CheckLicense())
                throw new Exception("Errors in the process connection!");

            string ConnectionKey = mDBType.ToString() + "_ConnectionKey";

            string strConnect = string.Empty;
            if (ConfigurationManager.ConnectionStrings[ConnectionKey] != null)
                strConnect = ConfigurationManager.ConnectionStrings[ConnectionKey].ConnectionString;
            else
                strConnect = System.Configuration.ConfigurationManager.AppSettings[ConnectionKey];

            strConnect = MySecurity.AESDecrypt_Simple(strConnect, PasswordEnCode);

            return strConnect;
        }

        public static string GetConfigConnectionString(string KeyOnConfig)
        {
            //Kiểm tra License
            if (!MyCheck.CheckLicense())
                throw new Exception("Errors in the process connection!");

            string strConnect = string.Empty;
            if (ConfigurationManager.ConnectionStrings[KeyOnConfig] != null)
                strConnect = ConfigurationManager.ConnectionStrings[KeyOnConfig].ConnectionString;
            else
                strConnect = System.Configuration.ConfigurationManager.AppSettings[KeyOnConfig];

            strConnect = MySecurity.AESDecrypt_Simple(strConnect, PasswordEnCode);

            return strConnect;
        }

        public static string GetConfigConnectionString(string ConnectionString, bool IsContent)
        {
            string strConnect = string.Empty;

            if (IsContent)
            {
                strConnect = ConnectionString;
            }
            else
            {
                if (ConfigurationManager.ConnectionStrings[ConnectionString] != null)
                    strConnect = ConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString;
                else
                    strConnect = System.Configuration.ConfigurationManager.AppSettings[ConnectionString];
            }
            strConnect = MySecurity.AESDecrypt_Simple(strConnect, PasswordEnCode);

            return strConnect;
        }
    }
}
