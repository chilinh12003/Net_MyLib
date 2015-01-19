using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
namespace MyLib.MyUtility
{
    /// <summary>
    /// Lấy các đối tượng hiện tại
    /// </summary>
    public class MyCurrent
    {

        /// <summary>
        /// Lấy trang hiện tại
        /// </summary>
        public static HttpContext CurrentPage
        {
            get{
                if (HttpContext.Current != null)
                    return HttpContext.Current;
                else
                    return HttpContext.Current;
            }
        }


        /// <summary>
        /// Lấy tên domain của trang web hiện hành
        /// </summary>
        /// <returns></returns>
        public static string GetDomainName()
        {
            return CurrentPage.Request.Url.Host;
        }

        /// <summary>
        /// Lấy link trang hiện tại
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentLink()
        {
            return CurrentPage.Request.Url.AbsoluteUri;
        }

        /// <summary>
        /// Lấy IP của khác hàng đang duyệt web 
        /// </summary>
        public static string GetRequestIP
        {
            get
            {
                if (CurrentPage.Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                {
                    return CurrentPage.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
                }
                if (CurrentPage.Request.ServerVariables["REMOTE_ADDR"] != null)
                {
                    return CurrentPage.Request.ServerVariables["REMOTE_ADDR"].ToString();
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// Lấy IP của host
        /// </summary>
        /// <returns></returns>
        public static string GetHostIP()
        {
            string myHost = System.Net.Dns.GetHostName();
            System.Net.IPAddress[] CurrentIP = System.Net.Dns.GetHostEntry(myHost).AddressList;

            for (int i = 0; i < CurrentIP.Length; i++)
            {
                if (CurrentIP[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return CurrentIP[i].ToString();
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Thiết lập cate cho một đối tượng.
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="CacheTime">Số giây mà cache này tồn tại</param>
        /// <returns></returns>
        public static bool SetCache(string Key, object Value, double CacheTime)
        {
            try
            {
                if (MyCurrent.CurrentPage == null || MyCurrent.CurrentPage.Cache == null)
                    return false;

                MyCurrent.CurrentPage.Cache.Add(Key, Value, null, DateTime.Now.AddSeconds(CacheTime), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Kểm tra sự tồn tại của Cache
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static bool CheckExistsCache(string CacheKey)
        {
            try
            {
                if (MyCurrent.CurrentPage == null || MyCurrent.CurrentPage.Cache == null)
                    return false;

                if (MyCurrent.CurrentPage.Cache[CacheKey] != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetURLPort()
        {
            return CurrentPage.Request.Url.Port.ToString();
        }


    }
}
