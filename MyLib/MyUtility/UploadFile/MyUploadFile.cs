using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Web;
namespace MyLib.MyUtility.UploadFile
{
    public class MyUploadFile
    {
        public HttpPostedFile mPostedFile;

        /// <summary>
        /// Kích thước của file
        /// </summary>
        public double FileSize = 0;

        /// <summary>
        /// Đuôi file
        /// </summary>
        public string FileExtension = string.Empty;

        /// <summary>
        /// Cho phép tạo file name tự động
        /// </summary>
        public bool AutoGeneralFileName = false;

        /// <summary>
        /// Tên của file sau khi upload bao gồm cả đuôi file VD: abd.mp3
        /// </summary>
        public string FileName = string.Empty;

        /// <summary>
        /// Tên file không có extension sau khi upload VD: abd
        /// </summary>
        public string FileNameWithoutExtension = string.Empty;

        /// <summary>
        /// Phần chuỗi được thêm vào tên của file sau khi upload xong
        /// </summary>
        public string AddToFileName = string.Empty;

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
        public string UploadExtension = "";

        /// <summary>
        /// Đường dẫn của file sau khi được upload
        /// </summary>
        public string UploadedPath = string.Empty;

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Prefix">Tiền tố, như game, Ringtone...</param>
        public void GetConfigFile(string Prefix)
        {
            try
            {
               
                this.UploadPath = MyConfig.GetKeyInConfigFile("UF_Path");
                this.UploadExtension = MyConfig.GetKeyInConfigFile("UF_Extension");
                string UI_MaxSize = MyConfig.GetKeyInConfigFile("UI_MaxSize");
                string UI_GeneralFN = MyConfig.GetKeyInConfigFile("UI_GeneralFN");

                if (!string.IsNullOrEmpty(UI_MaxSize))
                {
                    double.TryParse(UI_MaxSize, out this.UploadMaxSize);
                }
                if (!string.IsNullOrEmpty(UI_GeneralFN))
                {
                    bool.TryParse(UI_GeneralFN, out this.AutoGeneralFileName);
                }

                if (!string.IsNullOrEmpty(Prefix))
                {
                    this.UploadPath = MyConfig.GetKeyInConfigFile(Prefix + "UF_Path");

                    this.UploadExtension = MyConfig.GetKeyInConfigFile(Prefix + "UF_Extension");

                    UI_MaxSize = MyConfig.GetKeyInConfigFile(Prefix + "UI_MaxSize");
                    UI_GeneralFN = MyConfig.GetKeyInConfigFile(Prefix + "UI_GeneralFN");

                    if (!string.IsNullOrEmpty(UI_MaxSize))
                    {
                        double.TryParse(UI_MaxSize, out this.UploadMaxSize);
                    }
                    if (!string.IsNullOrEmpty(UI_GeneralFN))
                    {
                        bool.TryParse(UI_GeneralFN, out this.AutoGeneralFileName);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Khởi tạo
        /// </summary>
        public MyUploadFile(string Prefix)
        {
            GetConfigFile(Prefix);
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="Prefix">Tiền tố, như game, Ringtone...</param>
       /// <param name="mFile"></param>
        public MyUploadFile(string Prefix, HttpPostedFile mFile)
        {
            try
            {
                this.mPostedFile = mFile;

                GetConfigFile(Prefix);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       

        /// <summary>
        /// Tạo filename hợp lệ, không có dấu việt nam, không có khoảng trắng...
        /// </summary>
        /// <param name="str_FileName"></param>
        /// <returns></returns>
        private string ValidFileName(string str_FileName)
        {
            string[] chars = new string[] { ",", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "-", "(", ")", ":", "|", "[", "]","~","`","<",">" };
            str_FileName = MyText.RemoveSignVietnameseString(str_FileName);
            for (int i = 0; i < chars.Length; i++)
            {
                if (str_FileName.Contains(chars[i]))
                {
                    str_FileName = str_FileName.Replace(chars[i], "");
                }
            }
            str_FileName = str_FileName.Replace("  ", " ").Replace(" ", "_");
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
            if (mPostedFile.ContentLength / 1024 > UploadMaxSize)
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

                FileNameWithoutExtension = Path.GetFileNameWithoutExtension(mPostedFile.FileName);
                FileNameWithoutExtension = ValidFileName(FileNameWithoutExtension);

                if (AutoGeneralFileName)
                {
                    string chars = "12346789abcdefghjkmnpqrtuvwxyz";
                    // create random generator
                    Random rnd = new Random();
                    do
                    {
                        // create name
                        FileNameWithoutExtension = string.Empty;
                        while (FileNameWithoutExtension.Length < 10)
                        {
                            FileNameWithoutExtension += chars.Substring(rnd.Next(chars.Length), 1);
                        }
                        // add extension

                        if (!string.IsNullOrEmpty(AddToFileName))
                        {
                            FileNameWithoutExtension += AddToFileName;
                        }
                        FileName = FileNameWithoutExtension + FileExtension;
                        PhysicalUploadedPath = HttpContext.Current.Server.MapPath(UploadPath + FileName);
                        // check against files in the folder
                    } while (File.Exists(PhysicalUploadedPath));

                    //FileNameWithoutExtension = DateTime.Now.ToString("yyyyMMddhhmmssfff");
                }
                else
                {
                    if (!string.IsNullOrEmpty(AddToFileName))
                    {
                        FileNameWithoutExtension += AddToFileName;
                    }
                    FileName = FileNameWithoutExtension + FileExtension;
                    PhysicalUploadedPath = HttpContext.Current.Server.MapPath(UploadPath + FileName);
                }

                FileSize = mPostedFile.ContentLength / 1024;

                //Kiểm tra tồn tại của thư mục
                string str_Directory = Path.GetDirectoryName(PhysicalUploadedPath);

                if (!Directory.Exists(str_Directory))
                {
                    Directory.CreateDirectory(str_Directory);
                }
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
