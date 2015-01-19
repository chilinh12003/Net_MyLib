using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web;
using MyLib.MyUtility;
namespace MyLib.MyBase.MyWeb
{
    /// <summary>
    /// 
    /// </summary>
    public class MyASPX : Page
    {
        public MyLog mLog = new MyLog(typeof(MyASPX));

        public bool AllowLogVisit
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("AllowLogVisit")))
                    {
                        return true;
                    }
                    else
                    {
                        return bool.Parse(MyConfig.GetKeyInConfigFile("AllowLogVisit"));
                    }
                }
                catch
                {
                    return true;
                }
            }
        }

        protected override void OnInit(EventArgs e)
        {
            WriteLog();
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public string MemberID = string.Empty;

        public void WriteLog()
        {
            try
            {
                if (AllowLogVisit)
                {
                    string strFormat = "Request--->IP:{0} || MemberID:{1} || Link{2} || UserAgent:{3}";
                    string strContent = string.Format(strFormat, MyCurrent.GetRequestIP, MemberID, Request.Url.ToString(), Request.UserAgent);
                    mLog.Info(strContent);
                }
            }
            catch (Exception ex)
            {
                mLog.Error(ex);
            }
        }
    }
}
