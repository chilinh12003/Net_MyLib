
using System;
using System.Data;
using System.Configuration;
using System.Web;


namespace MyLib.MyUtility
{
    public class MyConfig
    {
        static log4net.ILog mLog = log4net.LogManager.GetLogger(typeof(MyConfig));
        /// <summary>
        /// Các loại dòng máy truy cập vào trang web/wap
        /// </summary>
        public enum DeviceType
        {
            /// <summary>
            /// Không xác định được
            /// </summary>
            Nothing = 0,
            /// <summary>
            /// True if the device runs Android (any version).
            /// </summary>
            is_android = 1,
            /// <summary>
            /// True if the device runs iOS (any version).
            /// </summary>
            is_ios = 2,
            /// <summary>
            ///True if the device runs Windows Phone 6.5 or higher. Note that this does not include Windows Mobile or Windows CE.
            /// </summary>
            is_windows_phone = 3,
            /// <summary>
            /// True if the requests is from a native app. This typically of requests from WebView components and RESTful API calls.
            /// </summary>
            is_app = 4,
            /// <summary>
            /// True if the requesting device has a full desktop experience. This capability is an alias for ux_full_desktop.
            /// </summary>
            is_full_desktop = 5,
            /// <summary>
            /// True if the requesting device's screen is of a high resolution (over 480 pixels in width and height)
            /// </summary>
            is_largescreen = 6,
            /// <summary>
            /// True if the device is mobile, like a phone, tablet, media player, portable game console, etc. This capability is an alias for is_wireless_device.
            /// </summary>
            is_mobile = 7,
            /// <summary>
            /// True if the request is from a robot, crawler, or some other automated HTTP client.
            /// </summary>
            is_robot = 8,
            /// <summary>
            /// True if the device is a smartphone. Internally, the matcher checks the operating system, screen width, pointing method and a few other capabilities.
            /// </summary>
            is_smartphone = 9,
            /// <summary>
            /// True if the primary pointing method is a touchscreen.
            /// </summary>
            is_touchscreen = 10,
            /// <summary>
            /// True if the requesting device should be served with WML markup.
            /// </summary>
            is_wml_preferred = 11,
            /// <summary>
            /// True if the requesting device should be served with XHTML-MP markup.
            /// </summary>
            is_xhtmlmp_preferred = 12,
            /// <summary>
            /// True if the requesting device should be served with HTML markup.
            /// </summary>
            is_html_preferred = 13,
            /// <summary>
            /// Returns the operating system name of the requesting device. This works for mobile and desktop devices. (ex: "Windows", "Mac OS X")
            /// </summary>
            advertised_device_os = 14,
            /// <summary>
            /// Returns the operating system version of the requesting device. This works for mobile and desktop devices. (ex: "XP", "10.2.1")
            /// </summary>
            advertised_device_os_version = 15,
            /// <summary>
            /// Returns the browser name of the requesting device. This works for mobile and desktop devices. (ex: "Internet Explorer", "Chrome")
            /// </summary>
            advertised_browser = 16,
            /// <summary>
            /// Returns the browser version of the requesting device. This works for mobile and desktop devices. (ex: "7", "29")
            /// </summary>
            advertised_browser_version = 17,
        }

        public enum Telco
        {
            /// <summary>
            /// Không thuộc mạng nào
            /// </summary>
            Nothing = 0,
            /// <summary>
            /// Thuộc mạng Viettel
            /// </summary>
            Viettel = 1,
            /// <summary>
            /// Thuộc mạng Vinaphone
            /// </summary>
            Vinaphone = 2,
            /// <summary>
            /// Thuộc mạng Mobifone
            /// </summary>
            Mobifone = 3,
            Beeline = 4,
            Sfone =6,
            VietNamMobile = 7
        }

        public enum ChannelType
        {
            SMS = 1, IVR = 2, WEB = 3, WAP = 4, USSD = 5, CLIENT = 6, API = 7, UNSUB = 7, CSKH = 8, MAXRETRY = 9, SUBNOTEXIST = 10, SYSTEM = 11,
        }

        #region Cấu hình về đấu số của các mạng
        public static string VTNumber 
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("VTNumber")))
                {
                    return ",163,164,165,166,167,168,169,97,98,96,162";
                }
                else
                {
                    return GetKeyInConfigFile("VTNumber");
                }
               
            }
        }
        public static string VMSNumber
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("VMSNumber")))
                {
                    return ",90,93,121,122,126,128,120";
                }
                else
                {
                    return GetKeyInConfigFile("VMSNumber");
                }

            }
        }
        public static string VNPNumber
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("VNPNumber")))
                {
                    return ",124,123,125,127,129,91,94";
                }
                else
                {
                    return GetKeyInConfigFile("VNPNumber");
                }

            }
        }
        public static string BEENumber
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("BEENumber")))
                {
                    return ",199,99";
                }
                else
                {
                    return GetKeyInConfigFile("BEENumber");
                }

            }
        }
       

        /// <summary>
        /// List các đầu số của mạng Sfone
        /// </summary>
        public static string SFNumber
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("SFNumber")))
                {
                    return ",95";
                }
                else
                {
                    return GetKeyInConfigFile("SFNumber");
                }
            }
        }

        /// <summary>
        /// List các đầu số của mạng Vietnammobile
        /// </summary>
        public static string VNMNumber
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("VNMNumber")))
                {
                    return ",92,188,186";
                }
                else
                {
                    return GetKeyInConfigFile("VNMNumber");
                }
            }
        }

        #endregion
        /// <summary>
        /// Cho biết ứng dụng đang chạy trên WebForm hay WindowsForm
        /// </summary>
        public static bool IsWeb
        {
            get
            {
                try
                {
                    //Lấy trong webconfig
                    string str_IsWeb = GetKeyInConfigFile("IsWeb");

                    if (string.IsNullOrEmpty(str_IsWeb))
                    {
                        if (HttpContext.Current != null)
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        return bool.Parse(str_IsWeb);
                    }

                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Lấy Giá trị theo Key trong file Config
        /// </summary>
        /// <param name="key">Key cần lấy giá trị</param>
        /// <returns></returns>
        public static string GetKeyInConfigFile(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key) && ConfigurationManager.AppSettings[key] != null)
                {
                    
                    return ConfigurationManager.AppSettings[key].ToString();
                }
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Lưu lại thông tin vào file config
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SaveKeyInConfigFile(string key, string value)
        {
            try
            {
                ConfigurationManager.AppSettings.Remove(key);
                ConfigurationManager.AppSettings.Add(key,value);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy đưỡng dẫn của file LogError
        /// </summary>
        public static string LogPathError
        {
            get
            {
                string str_Path = GetKeyInConfigFile("LogPathError");

                if (string.IsNullOrEmpty(str_Path))
                {
                    if (IsWeb)
                        str_Path= @"~\LogFile\LogError_";
                    else
                        str_Path = @"\LogFile\LogError_";
                }
                string FileName = string.Empty;

                if (IsWeb)
                {
                    if (HttpContext.Current != null)
                    {
                        FileName = HttpContext.Current.Server.MapPath(str_Path + DateTime.Now.ToString("ddMMyyyy"));
                    }
                }
                else
                {
                    FileName = AppDomain.CurrentDomain.BaseDirectory + str_Path + DateTime.Now.ToString("ddMMyyyy");
                }

                string FolderPath = System.IO.Path.GetDirectoryName(FileName);
                //Kiểm tra sự tồn tại của folder, nếu ko tồn tại thì tạo folder
                if (!System.IO.Directory.Exists(FolderPath))
                {
                    System.IO.Directory.CreateDirectory(FolderPath);
                }

                return FileName;
               
            }
        }

        /// <summary>
        /// Lấy đường dẫn của file LogPathData
        /// </summary>
        public static string LogPathData
        {
            get
            {
                string str_Path = GetKeyInConfigFile("LogPathData");

                if (string.IsNullOrEmpty(str_Path))
                {
                    if (IsWeb)
                        str_Path = @"~\LogFile\LogData_";
                    else
                        str_Path = @"\LogFile\LogData_";
                }
                string FileName = string.Empty;

                if (IsWeb)
                {
                    if (HttpContext.Current != null)
                    {
                        FileName = HttpContext.Current.Server.MapPath(str_Path + DateTime.Now.ToString("ddMMyyyy"));
                    }
                }
                else
                {
                    FileName = AppDomain.CurrentDomain.BaseDirectory + str_Path + DateTime.Now.ToString("ddMMyyyy");
                }

                string FolderPath = System.IO.Path.GetDirectoryName(FileName);
                //Kiểm tra sự tồn tại của folder, nếu ko tồn tại thì tạo folder
                if (!System.IO.Directory.Exists(FolderPath))
                {
                    System.IO.Directory.CreateDirectory(FolderPath);
                }

                return FileName;

            }
        }

        /// <summary>
        /// Lấy đường dẫn của file LogPathData nhưng không có ngày tháng trong filename
        /// </summary>
        public static string LogPathDataNotDate
        {
            get
            {
                string str_Path = GetKeyInConfigFile("LogPathDataNotDate");

                if (string.IsNullOrEmpty(str_Path))
                {
                    if (IsWeb)
                        str_Path = @"~\LogFile\LogData_";
                    else
                        str_Path = @"\LogFile\LogData_";
                }
                string FileName = string.Empty;

                if (IsWeb)
                {
                    if (HttpContext.Current != null)
                    {
                        FileName = HttpContext.Current.Server.MapPath(str_Path );
                    }
                }
                else
                {
                    FileName = AppDomain.CurrentDomain.BaseDirectory + str_Path;
                }

                string FolderPath = System.IO.Path.GetDirectoryName(FileName);
                //Kiểm tra sự tồn tại của folder, nếu ko tồn tại thì tạo folder
                if (!System.IO.Directory.Exists(FolderPath))
                {
                    System.IO.Directory.CreateDirectory(FolderPath);
                }

                return FileName;

            }
        }

        public static string LogExtensionFile
        {
            get
            {
                string str_Value = GetKeyInConfigFile("LogExtensionFile");

                if (string.IsNullOrEmpty(str_Value))
                {
                    str_Value= ".vai";
                }
                return str_Value;
            }
        }

        /// <summary>
        /// Định dạng này tháng theo kiểu Ngay/Thang/Nam - Gio:Phut:Giay
        /// </summary>
        public static string LongDateFormat
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("LongDateFormat")))
                {
                    return "dd/MM/yyyy - HH:mm:ss";
                }
                else
                {
                    return GetKeyInConfigFile("LongDateFormat");
                }
            }
        }

        /// <summary>
        /// Định dạng này tháng theo kiểu Ngay/Thang/Nam &lt;br/> Gio:Phut:Giay
        /// </summary>
        public static string LongDateFormatBR
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("LongDateFormatBR")))
                {
                    return "dd/MM/yyyy <br/> HH:mm:ss";
                }
                else
                {
                    return GetKeyInConfigFile("LongDateFormatBR");
                }
            }
        }
        
        /// <summary>
        /// Định dạng này tháng theo kiểu Ngay/Thang - Gio:Phut dành cho trang ngoài (tin tức...)
        /// </summary>
        public static string ViewDateFormat
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("ViewDateFormat")))
                {
                    return "dd/MM - HH:mm";
                }
                else
                {
                    return GetKeyInConfigFile("ViewDateFormat");
                }
            }
        }

        /// <summary>
        /// Định dạng cho số double
        /// </summary>
        public static string DoubleFormat
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("DoubleFormat")))
                {
                    return "N0";
                }
                else
                {
                    return GetKeyInConfigFile("DoubleFormat");
                }
            }
        }
        /// <summary>
        /// Định dạng cho số decimal
        /// </summary>
        public static string DecimalFormat
        {
            get
            {

                if (string.IsNullOrEmpty(GetKeyInConfigFile("DecimalFormat")))
                {
                    return "N0";
                }
                else
                {
                    return GetKeyInConfigFile("DecimalFormat");
                }
            }
        }

        public static string IntFormat
        {
            get
            {

                if (string.IsNullOrEmpty(GetKeyInConfigFile("IntFormat")))
                {
                    return "N0";
                }
                else
                {
                    return GetKeyInConfigFile("IntFormat");
                }
            }
        }

        public static string ShortDateFormat
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("ShortDateFormat")))
                {
                    return "dd/MM/yyyy";
                }
                else
                {
                    return GetKeyInConfigFile("ShortDateFormat");
                }
            }
        }

        public static string DateFormat_InsertToDB
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("DateFormat_InsertToDB")))
                {
                    return "yyyy-MM-dd HH:mm:ss.fff";
                }
                else
                {
                    return GetKeyInConfigFile("DateFormat_InsertToDB");
                }
            }
        }

        public static int DefaultPageSize
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetKeyInConfigFile("DefaultPageSize")))
                    {
                        return 10;
                    }
                    else
                    {
                        return int.Parse(GetKeyInConfigFile("DefaultPageSize"));
                    }
                }
                catch
                {
                    return 10;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool AllowSendEmailSecurity
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(GetKeyInConfigFile("AllowSendEmailSecurity")))
                    {
                        return true;
                    }
                    else
                    {
                        return bool.Parse(GetKeyInConfigFile("AllowSendEmailSecurity"));
                    }
                }
                catch
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Đường dẫn của trang khi chưa đăng nhập
        /// </summary>
        public static string URLNotLoginFail
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("URLNotLoginFail")))
                {
                    return string.Empty;
                }
                else
                {
                    return GetKeyInConfigFile("URLNotLoginFail");
                }
            }
        }

        /// <summary>
        /// Link sau khi đăng nhập thành công
        /// </summary>
        public static string URLLoginSuccess
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("URLLoginSuccess")))
                {
                    return string.Empty;
                }
                else
                {
                    return GetKeyInConfigFile("URLLoginSuccess");
                }
            }
        }

        /// <summary>
        /// Trang thông báo lỗi, thông báo
        /// </summary>
        public static string URLNotice
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("URLNotice")))
                {
                    return string.Empty;
                }
                else
                {
                    return GetKeyInConfigFile("URLNotice");
                }
            }
        }

        /// <summary>
        /// Trang login
        /// </summary>
        public static string URLLogin
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("URLLogin")))
                {
                    return string.Empty;
                }
                else
                {
                    return GetKeyInConfigFile("URLLogin");
                }
            }
        }

        /// <summary>
        /// Ngày tháng nhỏ nhất
        /// </summary>
        public static DateTime MinDateTime
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("MinDateTime")))
                {
                    return DateTime.MinValue;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            set { MinDateTime = value; }
        }

        /// <summary>
        /// Ngày tháng lớn nhất
        /// </summary>
        public static DateTime MaxDateTime
        {
            get
            {
                if (string.IsNullOrEmpty(GetKeyInConfigFile("MaxDateTime")))
                {
                    return DateTime.MaxValue;
                }
                else
                {
                    return DateTime.MaxValue;
                }
            }
            set { MaxDateTime = value; }
        }

        /// <summary>
        /// Ngày bắt đầu của tháng
        /// </summary>
        /// <returns></returns>
        public static DateTime StartDayOfMonth
        {
            get
            {
                DateTime FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                return FromDate;
            }
        }

        /// <summary>
        /// Ngày kết thúc của tháng
        /// </summary>
        /// <returns></returns>
        public static DateTime EndDayOfMonth
        {
            get
            {
                DateTime ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                return ToDate;
            }
        }

        /// <summary>
        /// Lấy 1 ngày nào đó trong 1 tuần của ngày input vào.
        /// </summary>
        /// <param name="InputDate">Ngày của Tuần cần lấy</param>
        /// <param name="DayName">Thứ cần lấy</param>
        /// <returns></returns>
        public static DateTime GetDayInWeek(DateTime InputDate, DayOfWeek DayName)
        {
            try
            {
                DateTime FindDate = InputDate.Date;

                int NowDayOfWeek = Convert.ToInt32(InputDate.DayOfWeek);
                FindDate = FindDate.AddDays(0 - NowDayOfWeek);

                while (FindDate.DayOfWeek != DayName)
                    FindDate = FindDate.AddDays(1);

                return FindDate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Đơn vị tiền tệ hiển thị
        /// </summary>
        public static string MoneyUnit
        {
            get
            {
                if (string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("MoneyUnit")))
                {
                    return "vnđ";
                }
                else
                {
                    return MyConfig.GetKeyInConfigFile("MoneyUnit");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool AllowCreateThumbnailGifFile
        {
            get { return true; }
        }

        /// <summary>
        /// Link của host chứa resource
        /// </summary>
        public static string ResourceLink
        {
            get
            {
                if (string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("ResourceLink")))
                {
                    return string.Empty;
                }
                else
                {
                    return MyConfig.GetKeyInConfigFile("ResourceLink");
                }
            }
        }

        /// <summary>
        /// Domain của website
        /// </summary>
        public static string Domain
        {
            get
            {
                if (string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("Domain")))
                {
                    return string.Empty;
                }
                else
                {
                    return MyConfig.GetKeyInConfigFile("Domain");
                }
            }
        }

        /// <summary>
        /// Tên biến trong template để Replace Domain VD" {DNS}
        /// </summary>
        public static string DomainParameter
        {
            get
            {
                if (string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("DomainParameter")))
                {
                    return "{DNS}";
                }
                else
                {
                    return MyConfig.GetKeyInConfigFile("DomainParameter");
                }
            }
        }

        /// <summary>
        /// Tài khoản gửi email lỗi
        /// </summary>
        public static string AccountSendEmail
        {
            get
            {
                if (string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("AccountSendEmail")))
                {
                    return string.Empty;
                }
                else
                {
                    return MyConfig.GetKeyInConfigFile("AccountSendEmail");
                }
            }
        }

        /// <summary>
        /// Mật khẩu của tài khoản gửi email lỗi
        /// </summary>
        public static string PasswordSendEmail
        {
            get
            {
                if (string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("PasswordSendEmail")))
                {
                    return string.Empty;
                }
                else
                {
                    return MyConfig.GetKeyInConfigFile("PasswordSendEmail");
                }
            }
        }

        /// <summary>
        /// Danh sách email nhận thông báo lỗi của hệ thống
        /// </summary>
        public static string ListReceiveEmail
        {
            get
            {
                if (string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("ListReceiveEmail")))
                {
                    return string.Empty;
                }
                else
                {
                    return MyConfig.GetKeyInConfigFile("ListReceiveEmail");
                }
            }
        }

        /// <summary>
        /// Cho phép hệ thống gửi email error
        /// </summary>
        public static bool AllowSendEmailError
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("AllowSendEmailError")))
                    {
                        return false;
                    }
                    else
                    {
                        return bool.Parse(MyConfig.GetKeyInConfigFile("AllowSendEmailError"));
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Cho phép gửi email lỗi bằng tài khoản email của ICom, giá trị mặc định = true
        /// </summary>
        public static bool SendEmailErrorByIComAccount
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("SendEmailErrorByIComAccount")))
                    {
                        return true;
                    }
                    else
                    {
                        return bool.Parse(MyConfig.GetKeyInConfigFile("SendEmailErrorByIComAccount"));
                    }
                }
                catch
                {
                    return true;
                }
            }
        }

        /// <summary>
        /// Tên của ứng dụng
        /// </summary>
        public static string ApplicationName
        {
            get
            {
                if (string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("ApplicationName")))
                {
                    return string.Empty;
                }
                else
                {
                    return MyConfig.GetKeyInConfigFile("ApplicationName");
                }
            }
        }

        /// <summary>
        /// Hình dành mặc định nếu ko có hình ảnh
        /// </summary>
        public static string DefaultImagePath
        {
            get
            {
                if (string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("DefaultImagePath")))
                {
                    return string.Empty;
                }
                else
                {
                    return MyConfig.GetKeyInConfigFile("DefaultImagePath");
                }
            }
        }

        public static string SpecialKey = "ChIlInH";

       }
}
