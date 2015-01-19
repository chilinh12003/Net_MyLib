using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Reflection;
using MyLib.MyUtility;

namespace MyLib.MyBase.MyAjax
{
    public class MyAjaxBase
    {
        public MyLog mLog = new MyLog(typeof(MyAjaxBase));

        public string TemplatePath = "";
        public string TemplatePathRepat = "";
        public string TemplatePathRepatChild = "";

        public HttpContext MyContext;
        protected Dictionary<string, string> Parameters = null;
        protected Dictionary<string, MethodInfo> MethodList;

        public List<AjaxResult> ListAjaxResult = new List<AjaxResult>();

        public MyAjaxBase()
        {

        }

        public MyAjaxBase(HttpContext mContext)
        {
            MyContext = mContext;
            GetParameters();
            GetMethodList();
        }
        public bool RunMethod(string MethodName)
        {
            try
            {
                MethodInfo mMethodInfo;
                if (MethodList == null || MethodList.Count < 1)
                {
                    GetMethodList();
                }
                if (Parameters == null || Parameters.Count < 1)
                {
                    GetParameters();
                }
                if (!MethodList.TryGetValue(MethodName.ToLower(), out mMethodInfo))
                {
                    if (!MethodList.TryGetValue("notfound", out mMethodInfo))
                    {
                        return false;
                    }
                }
                try
                {
                    mMethodInfo.Invoke(this, null);
                }
                catch (TargetInvocationException e)
                {
                    throw e.InnerException;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy tất cả các method của đối tượng thuộc lớp này
        /// </summary>
        private void GetMethodList()
        {
            MethodList = new Dictionary<string, MethodInfo>();

            MethodInfo[] arr_MethodInfo = GetType().GetMethods();

            foreach (MethodInfo mMethod in arr_MethodInfo)
            {
                if (mMethod.GetParameters().Length == 0 || mMethod.DeclaringType == GetType())
                {
                    string name = mMethod.Name.ToLower();
                    if (MethodList.ContainsKey(name)) 
                        MethodList.Remove(name);

                    MethodList.Add(name, mMethod);
                }
            }
        }

        /// <summary>
        /// Lấy tất cả các Querystring mà trên Jvascript truyền xuống
        /// </summary>
        private void GetParameters()
        {
            if (Parameters != null) return;

            if (MyContext == null)
                return;

            //parameters
            Parameters = new Dictionary<string, string>();

            //For GET Method
            foreach (string key in MyContext.Request.QueryString.AllKeys)
            {
                string rs = MyContext.Request.QueryString[key];
                if (!string.IsNullOrEmpty(rs))
                    Parameters.Add(key, rs);
            }

            //For POST Method
            foreach (string key in MyContext.Request.Form.AllKeys)
            {
                string rs = MyContext.Request.Form[key];
                if (!string.IsNullOrEmpty(rs) && !Parameters.ContainsKey(key.ToLower()))
                    Parameters.Add(key, rs);
            }

        }

        /// <summary>
        /// Lấy thông tin từ Request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Object"></param>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        public bool GetParemeter<T>(ref T Object, string Parameter)
        {
            try
            {
                if (Parameters.ContainsKey(Parameter))
                {
                    Object = (T)Convert.ChangeType(Parameters[Parameter], typeof(T));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
