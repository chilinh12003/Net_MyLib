using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
namespace MyLib.MyUtility.UploadFile
{
    public class MyUploadAudio
    {
        //Mặc định upload là upload cho nhạc chuông
        public bool IsRingtones = true;

        private HttpPostedFile mPostedFile;

        /// <summary>
        /// Chiều dài thời gian của audio
        /// </summary>
        public double FileLength = 0;

        /// <summary>
        /// Kích thước của file
        /// </summary>
        public double FileSize = 0;

        /// <summary>
        /// BitRate của file
        /// </summary>
        public double FileBitrate = 0;

        /// <summary>
        /// Đuôi file
        /// </summary>
        public string FileExtension = string.Empty;

        /// <summary>
        /// Tên của file sau khi upload
        /// </summary>
        public string FileName = string.Empty;

        /// <summary>
        /// Đường dẫn vật lý của file sau khi upload
        /// </summary>
        public string PhysicalUploadedPath = string.Empty;

        /// <summary>
        /// Kích thước lớn nhất cho phép của file cần upload(mặc định là 3MB-3702KB)
        /// </summary>
        public double UploadMaxSize = 3702;

        /// <summary>
        /// Danh sách định dạng file cho phép
        /// </summary>
        public string UploadExtension = ".mp3|.wma|.mid|.wav|.amr|.mmf";

        /// <summary>
        /// Đường dẫn của file sau khi được upload
        /// </summary>
        public string UploadedPath = string.Empty;

        /// <summary>
        /// Cho phép tạo file nghe thử hay không
        /// </summary>
        public bool AllowCreateSampleFile = false;

        /// <summary>
        /// Thông báo khi upload
        /// </summary>
        public string Message = string.Empty;


        string _UploadPath = string.Empty;
        /// <summary>
        /// Đường dẫn upload lên server
        /// </summary>
        public string UploadPath
        {
            get { return _UploadPath; }
            set { _UploadPath = value; }
        }

        public MyUploadAudio(bool b_IsRingtones, HttpPostedFile mFile)
        {
            try
            {
                this.mPostedFile = mFile;
                this.IsRingtones = b_IsRingtones;

                this.UploadExtension = MyConfig.GetKeyInConfigFile("AudioUploadExtension");
                double.TryParse(MyConfig.GetKeyInConfigFile("AudioMaxSize"), out this.UploadMaxSize);

                //Lấy đường dẫn upload trong file config
                if (IsRingtones)
                {
                    this.UploadPath = MyConfig.GetKeyInConfigFile("RingtonesUploadPath");

                    if (!string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("RingtonesUploadExtension")))
                    {
                        this.UploadExtension = MyConfig.GetKeyInConfigFile("RingtonesUploadExtension");
                    }
                    if (!string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("RingtonesMaxSize")))
                    {
                        double.TryParse(MyConfig.GetKeyInConfigFile("RingtonesMaxSize"), out this.UploadMaxSize);
                    }
                }
                else
                {
                    this.UploadPath = MyConfig.GetKeyInConfigFile("RingBackUploadPath");

                    if (!string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("RingBackUploadExtension")))
                    {
                        this.UploadExtension = MyConfig.GetKeyInConfigFile("RingBackUploadExtension");
                    }
                    if (!string.IsNullOrEmpty(MyConfig.GetKeyInConfigFile("RingBackMaxSize")))
                    {
                        double.TryParse(MyConfig.GetKeyInConfigFile("RingBackMaxSize"), out this.UploadMaxSize);
                    }
                }

               

               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MyUploadAudio(bool b_IsRingtones, string str_UploadPath, HttpPostedFile mFile)
        {
            this.mPostedFile = mFile;
            this.IsRingtones = b_IsRingtones;
            this.UploadPath = str_UploadPath;
            this.UploadExtension = MyConfig.GetKeyInConfigFile("AudioUploadExtension");

            this.UploadMaxSize = double.Parse(MyConfig.GetKeyInConfigFile("AudioMaxSize"));
        }

        /// <summary>
        /// Tạo filename hợp lệ, không có dấu việt nam, không có khoảng trắng...
        /// </summary>
        /// <param name="str_FileName"></param>
        /// <returns></returns>
        private string ValidFileName(string str_FileName)
        {
            string[] chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "-", "(", ")", ":", "|", "[", "]" };
            str_FileName = MyText.RemoveSignVietnameseString(str_FileName);
            for (int i = 0; i < chars.Length; i++)
            {
                if (str_FileName.Contains(chars[i]))
                {
                    str_FileName = str_FileName.Replace(chars[i], "");
                }
            }
            str_FileName = str_FileName.Replace(" ", "_");
            return str_FileName;
        }


        /// <summary>
        /// Kiểm tra định dạng file
        /// </summary>
        /// <param name="str_Extension"></param>
        /// <returns></returns>
        private bool CheckExtension(string str_Extension)
        {
            try
            {
                if (UploadExtension.Length < 1)
                    return true;

                string str_Temp = UploadExtension.ToLower();
                str_Extension = str_Extension.ToLower();

                str_Temp = str_Temp.Replace(str_Extension, "");
                if (str_Temp.Length < UploadExtension.Length)
                    return true;
                else
                {
                    Message = "Định dạng file không hợp lệ, định dạng hợp lệ là: " + UploadExtension;
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool CheckSize()
        {
            if (mPostedFile.ContentLength/1024 > UploadMaxSize)
            {
                Message = "Dung lượng file quá lớn, dung lượng file lớn nhất cho phép là: " + UploadMaxSize.ToString();
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Upload()
        {
            try
            {
                if (!CheckSize())
                    return false;

                FileExtension = Path.GetExtension(mPostedFile.FileName);

                if (!CheckExtension(FileExtension))
                    return false;

                FileName = ValidFileName(Path.GetFileNameWithoutExtension(mPostedFile.FileName)) + FileExtension;

                FileSize = mPostedFile.ContentLength / 1024;

                if (!Directory.Exists(HttpContext.Current.Server.MapPath(UploadPath)))
                {
                    Directory.CreateDirectory(HttpContext.Current.Server.MapPath(UploadPath));
                }
                PhysicalUploadedPath = HttpContext.Current.Server.MapPath(UploadPath + FileName);

                mPostedFile.SaveAs(PhysicalUploadedPath);

                Message += "Upload file thành công";

                UploadedPath = UploadPath + FileName;

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }
    }
}
