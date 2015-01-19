using System;
using System.Collections.Generic;
using System.Text;
using log4net;
namespace MyLib.MyUtility
{
    public class MyLog
    {
        public log4net.ILog Log;
        public MyLog(Type ClassName)
        {
            Log = log4net.LogManager.GetLogger(ClassName);
            
        }
        public void Debug(string Message)
        {
            Log.Debug(Message);
        }
        public void Debug(string Prefix, string Message)
        {
            Log.Debug(Prefix + " " + Message);
        }

        public void Info(string Message)
        {
            Log.Info(Message);
        }
        public void Info(string Prefix,string Message)
        {
            Log.Info(Prefix + " " + Message);
        }

        public void Warn(string Message)
        {
            Log.Warn(Message);
        }

        public void Error(Exception Ex)
        {
            if (MyCurrent.CurrentPage != null && MyCurrent.CurrentPage.Session != null)
            {
                Log.Error(MyCurrent.CurrentPage.Session.SessionID,Ex);
            }
            else
            {
                Log.Error(Ex);
            }
            
        }
        public void Error(string Message, Exception Ex)
        {
            if (MyCurrent.CurrentPage != null && MyCurrent.CurrentPage.Session != null)
            {
                Log.Error(MyCurrent.CurrentPage.Session.SessionID+"|"+Message, Ex);
            }
            else
            {
                Log.Error(Message, Ex);
            }
            
        }
        public void Error(string Message, bool ShowMessage, Exception Ex)
        {
            if (ShowMessage)
            {
                MyMessage.ShowError(Message);
            }

            if (MyCurrent.CurrentPage != null && MyCurrent.CurrentPage.Session != null)
            {
                Log.Error(MyCurrent.CurrentPage.Session.SessionID + "|" + Message, Ex);
            }
            else
            {
                Log.Error(Message, Ex);
            }
        }
        public void Fatal(Exception Ex)
        {
            Log.Fatal(Ex);
        }
        public void Fatal(string Message, Exception Ex)
        {
            Log.Fatal(Message, Ex);
        }
    }
}
