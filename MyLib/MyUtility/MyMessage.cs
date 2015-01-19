using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace MyLib.MyUtility
{

    public class MyMessage
    {
        public static string ReplaceSpecialKey(string Message)
        {
            Message = Message.Replace("'", "");
            Message = Message.Replace("\"", "");
            Message = Message.Replace("\n", "");
            Message = Message.Replace("\r", "");
            return Message;
        }

        /// <summary>
        /// Xuất thông báo lỗi không trực tiếp với Thông báo đưa vào, thống báo được hiện lên hay không tùy thuộc vào cookie
        /// </summary>
        /// <param name="ShowMessage">Lỗi cần thông báo</param>
        public static void ShowMessage(string message)
        {
            message = ReplaceSpecialKey(message);
            try
            {
                Page mPage = HttpContext.Current.Handler as Page;
                mPage.ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", "ShowMessage('" + message + "');", true);
            }
            catch
            {
            }
        }
        
        public static void ShowMessage(string message, string newUrl)
        {
            message = ReplaceSpecialKey(message);
            Page mPage = HttpContext.Current.Handler as Page;
            if (newUrl.Length > 1)
                mPage.ClientScript.RegisterStartupScript(typeof(Page), "ShowMessage", "alert('" + mPage.Server.HtmlEncode(message) + "'); location.href='" + newUrl + "';", true);
            else ShowMessage(message);
        }

        public static void ShowError(string message)
        {
            message = ReplaceSpecialKey(message);
            Page mPage = HttpContext.Current.Handler as Page;
            mPage.ClientScript.RegisterStartupScript(typeof(Page), "ShowError", "alert('" + message + "');", true);
        }
    }
}
