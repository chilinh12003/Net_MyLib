using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;

using MyLib.MyUtility;

namespace MyLib.MyBase.MyWeb
{
    /// <summary>
    /// Là lớp base để cho các trang web (ashx) kế thừa
    /// </summary>
    public class MyASHX : IHttpHandler, IRequiresSessionState
    {
        public MyLog mLog = new MyLog(typeof(MyASHX));

        public HttpContext MyContext
        {
            get;
            set;
        }

        public HttpSessionState Session
        {
            get
            {
                return MyContext.Session;
            }
        }

        public HttpRequest Request
        {
            get
            {
                return MyContext.Request;
            }
        }

        public HttpResponse Response
        {
            get { return MyContext.Response; }
        }

        public HttpServerUtility Server
        {
            get { return MyContext.Server; }
        }

        public string ContentType = "text/html";

        public string MemberID
        {
            get
            {
                if (Session != null && Session["MemberID"] != null)
                    return Session["MemberID"].ToString();
                else
                    return string.Empty;
            }
            set
            {
                if (Session != null)
                    Session["MemberID"] = value;
            }
        }
        public string PageCode = string.Empty;

        /// <summary>
        /// Mã của trang, dung để phần quyền cho trang
        /// </summary>
        public string PageFullName
        {
            get
            {
                return this.GetType().FullName;
            }
        }
      
        /// <summary>
        /// Cho phép log mọi truy cập của User đến trang này
        /// </summary>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                MyContext = context;
                
                WriteHTML();
                MyContext.Response.ContentType = ContentType;

                //Write log
                WriteLog();
            }
            catch (Exception ex)
            {
                mLog.Error(ex);
            }
        }

        /// <summary>
        /// Wirte text
        /// </summary>
        /// <param name="strContent"></param>
        public void Write(string strContent)
        {
            Response.Write(strContent);
        }

        /// <summary>
        /// Hàm để các lớp kế thừa lớp này override
        /// </summary>
        public virtual void WriteHTML()
        {
            MyContext.Response.Write("");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public string PreviusURL
        {
            get
            {
                if (Request.UrlReferrer != null)
                    return Request.UrlReferrer.ToString();

                return string.Empty;
            }
        }

        /// <summary>
        /// Ghi log truy cập của người dùng
        /// </summary>
        public void WriteLog()
        {
            try
            {
                if (AllowLogVisit)
                {
                    string strFormat = "Request ---> IP:{0} || UserID:{1} || Link{2} || UserAgent:{3} || PreviusURL:{4}";
                    string strContent = string.Format(strFormat, MyCurrent.GetRequestIP, MemberID, Request.Url.ToString(), Request.UserAgent, PreviusURL);
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
