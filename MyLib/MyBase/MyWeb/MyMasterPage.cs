using System;
using System.Collections.Generic;
using System.Text;
using MyLib.MyUtility;
namespace MyLib.MyBase.MyWeb
{
    /// <summary>
    /// Lớp base của trang MasterPage
    /// </summary>
    public class MyMasterPage : System.Web.UI.MasterPage
    {
        public MyLog mLog = new MyLog(typeof(MyMasterPage));

        private string _ChildPageTitle = "Quản trị";
        /// <summary>
        /// Tên của trang con của trang masterpage
        /// </summary>
        public string ChildPageTitle
        {
            get { return _ChildPageTitle; }
            set { _ChildPageTitle = value; }
        }
        private bool _ShowSearchBox = true;
        /// <summary>
        /// Cho phép hiển thị hộp tìm kiếm hay không
        /// </summary>
        public bool ShowSearchBox
        {
            get { return _ShowSearchBox; }
            set { _ShowSearchBox = value; }
        }
        private bool _ShowToolBox = true;
        /// <summary>
        /// Cho phép hiển thị toolbox hay không
        /// </summary>
        public bool ShowToolBox
        {
            get { return _ShowToolBox; }
            set { _ShowToolBox = value; }
        }

        private string _TitleSearchBox = "Tìm kiếm thông tin:";
        /// <summary>
        /// Title của hộp tiềm kiếm
        /// </summary>
        public string TitleSearchBox
        {
            get { return _TitleSearchBox; }
            set { _TitleSearchBox = value; }
        }
    }
}
