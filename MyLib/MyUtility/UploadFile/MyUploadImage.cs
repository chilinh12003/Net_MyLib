using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Collections.Generic;
namespace MyLib.MyUtility.UploadFile
{
    public class MyUploadImage
    {
        
        /// <summary>
        /// Đường dẫn tương đối đến thư mục cần upload trên server
        /// </summary>
        public string ServerUploadPath = string.Empty;
       
        /// <summary>
        /// 
        /// </summary>
        public HttpPostedFile mPostedFile;

        /// <summary>
        /// Thông báo trả về khi thực hiện upload
        /// </summary>
        public string Message = string.Empty;

        /// <summary>
        /// Cho phép tự động tạo file thumbnail (với Width, Height là thuộc tính Width_Thumb, Height_Thumb)
        /// </summary>
        public bool AllowCreateThumbnail = false;

        /// <summary>
        /// Cho phép tự động tạo tên file
        /// </summary>
        public bool AutoGeneralFileName = false;

        /// <summary>
        /// hậu tố cho đôi file thumbnail
        /// </summary>
        public string SuffixThumbnailName = "_thumb";

        /// <summary>
        /// Kích thước lớn nhất cho phép đối với file upload
        /// </summary>
        public double MaxSizeAllow = 0;

        /// <summary>
        /// Chuỗi danh sách tên đuôi file cho phép upload lên (mặc định là tất cả)
        /// <para>VD: .png|.gif|.jpg|.jpeg</para>
        /// </summary>
        public string ExtensionAllow = string.Empty;

        /// <summary>
        /// Kích thướng của file sau khi file đã được upload lên
        /// </summary>
        public double UploadedFileSize = 0;

        /// <summary>
        /// Đuôi của file đã được upload lên
        /// </summary>
        public string UploadedExtension = string.Empty;

        /// <summary>
        /// Tên của file(VD:File_ABC.png) sau khi đã upload
        /// </summary>
        public string UploadedFileName = string.Empty;

        /// <summary>
        /// Tên file không có extension sau khi upload VD: File_ABC
        /// </summary>
        public string UploadedFileNameWithoutExtension = string.Empty;

        /// <summary>
        /// Phần chuỗi được thêm vào tên của file sau khi upload xong
        /// </summary>
        public string AddToFileName = string.Empty;


        /// <summary>
        /// Đường dẫn tương đối của file đã được upload
        /// <para>VD: ../Uploads/News/File_ABC.png</para>
        /// </summary>
        public string UploadedPathFile = string.Empty;

        /// <summary>
        /// Đường dẫn vật lý (tuyệt đối) của file sau khi upload lên
        /// <para>VD: D:\MyProject\Uploads\News\File_ABC.png</para>
        /// </summary>
        public string UploadedPhysicalPathFile = string.Empty;

        /// <summary>
        /// Tên của file thumbnail(VD:File_ABC_Thumb.png) sau khi đã upload
        /// </summary>
        public string UploadedFileName_Thumb = string.Empty;

        /// <summary>
        /// Đường dẫn tương đối của file thumbail đã được upload
        /// <para>VD: ../Uploads/News/File_ABC_Thumb.png</para>
        /// </summary>
        public  string UploadedPathFile_Thumb = string.Empty;


        /// <summary>
        /// Nếu tạo nhiều file Thumb thì List này chứa các danh sách file Thumb
        /// </summary>
        public List<string> List_UploadedPathFile_Thumb = new List<string>();

        /// <summary>
        /// Đường dẫn vật lý (tuyệt đối) của file sau khi upload lên
        /// <para>VD: D:\MyProject\Uploads\News\File_ABC_Thumb.png</para>
        /// </summary>
        public string UploadedPhysicalPathFile_Thumb = string.Empty;

        /// <summary>
        /// Chiếu rộng (tính bằng pixel) của file Ảnh thumbnail cần resize khi upload
        /// <para>Mặc định là 90px</para>
        /// </summary>
        public int Width_Thumb = 90;

        /// <summary>
        /// Chiếu cao (tính bằng pixel) của file Ảnh thumbnail cần resize khi upload
        /// <para>Mặc định là 90px</para>
        /// </summary>
        public int Height_Thumb = 60;

        /// <summary>
        /// Danh sách kích thước tạo thumbnail
        /// </summary>
        public string Image_WH_Thumb = "50x50";

        /// <summary>
        /// Cho biết đây là upload cho web hay là cho wap, false là upload cho wap
        /// </summary>
        public bool UploadForWeb = false;


        /// <summary>
        /// Cho biết ảnh sẽ được tạo thumb theo size của Width hay của Height
        /// <para>0: không fix size</para>
        /// <para>1: fix theo width</para>
        /// <para>2: fix theo height</para>
        /// </summary>
        public int FixWidthOrHeight = 0;

        /// <summary>
        /// Cho phép tạo thêm folder theo năm và tháng VD: 122012
        /// </summary>
        public bool AutoCreateFolderByMonth = false;

        /// <summary>
        /// Lấy thông tin từ file Config
        /// </summary>
        public void GetConfig(string Prefix)
        {
            //Lấy cấu hình mặc định.
            bool.TryParse(MyConfig.GetKeyInConfigFile("UI_Thumb"), out this.AllowCreateThumbnail);
            bool.TryParse(MyConfig.GetKeyInConfigFile("UI_GeneralFN"), out this.AutoGeneralFileName);
            this.ServerUploadPath = MyConfig.GetKeyInConfigFile("UI_Path");
            this.ExtensionAllow = MyConfig.GetKeyInConfigFile("UI_Extension");
            double.TryParse(MyConfig.GetKeyInConfigFile("UI_MaxSize"), out this.MaxSizeAllow);
            Image_WH_Thumb = MyConfig.GetKeyInConfigFile("UI_WH_Thumb");
            SuffixThumbnailName = MyConfig.GetKeyInConfigFile("UI_SuffixThumbnailName");
            int.TryParse(MyConfig.GetKeyInConfigFile("UI_FixWidthOrHeight"), out this.FixWidthOrHeight);
            bool.TryParse(MyConfig.GetKeyInConfigFile("UI_CreateFolderByMonth"), out this.AutoCreateFolderByMonth);

            if(!string.IsNullOrEmpty(Prefix))
            {
                string UI_Thumb = MyConfig.GetKeyInConfigFile(Prefix + "UI_Thumb");
                string UI_GeneralFN = MyConfig.GetKeyInConfigFile(Prefix + "UI_GeneralFN");
                string UI_Path = MyConfig.GetKeyInConfigFile(Prefix + "UI_Path");
                string UI_Extension = MyConfig.GetKeyInConfigFile(Prefix + "UI_Extension");
                string UI_MaxSize = MyConfig.GetKeyInConfigFile(Prefix + "UI_MaxSize");
                string UI_WH_Thumb = MyConfig.GetKeyInConfigFile(Prefix + "UI_WH_Thumb");
                string UI_FixWidthOrHeight = MyConfig.GetKeyInConfigFile(Prefix + "UI_FixWidthOrHeight");
                string UI_CreateFolderByMonth = MyConfig.GetKeyInConfigFile(Prefix + "UI_CreateFolderByMonth");
                SuffixThumbnailName = MyConfig.GetKeyInConfigFile(Prefix + "UI_SuffixThumbnailName");
                
                if (!string.IsNullOrEmpty(UI_FixWidthOrHeight))
                {
                    int.TryParse(UI_FixWidthOrHeight, out this.FixWidthOrHeight);
                }

                if (!string.IsNullOrEmpty(UI_Thumb))
                {
                    bool.TryParse(UI_Thumb, out this.AllowCreateThumbnail);
                }
                if (!string.IsNullOrEmpty(UI_GeneralFN))
                {
                    bool.TryParse(UI_GeneralFN, out this.AutoGeneralFileName);
                }
                if (!string.IsNullOrEmpty(UI_Path))
                {
                    this.ServerUploadPath = UI_Path;
                }
                if (!string.IsNullOrEmpty(UI_Extension))
                {
                    this.ExtensionAllow = UI_Extension;
                }
                if (!string.IsNullOrEmpty(UI_MaxSize))
                {
                    double.TryParse(UI_MaxSize, out this.MaxSizeAllow);
                }
                if (!string.IsNullOrEmpty(UI_WH_Thumb))
                {
                    Image_WH_Thumb = UI_WH_Thumb;
                }
                if (!string.IsNullOrEmpty(UI_CreateFolderByMonth))
                {
                    bool.TryParse(UI_CreateFolderByMonth, out this.AutoCreateFolderByMonth);
                }
            }

            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Prefix"></param>
        public MyUploadImage(string Prefix)
        {
            GetConfig(Prefix);
            if (AutoCreateFolderByMonth)
                ServerUploadPath += DateTime.Now.ToString("MMyyyy") + "/";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Prefix"></param>
        /// <param name="mFile"></param>
        public MyUploadImage(HttpPostedFile mFile,string Prefix)
        {
            mPostedFile = mFile;
            GetConfig(Prefix);

            if (AutoCreateFolderByMonth)
                ServerUploadPath += DateTime.Now.ToString("MMyyyy") + "/";
        }

        /// <summary>
        /// Kiểm tra size upload có được cho phép hay không
        /// </summary>
        /// <returns></returns>
        private bool CheckSize()
        {
            if (MaxSizeAllow <= 0)
                return true;

            if (mPostedFile.ContentLength / 1024 > MaxSizeAllow)
            {
                Message += "Dung lượng của File Upload quá lớn so với quy định. (Dung lượng <= " + MaxSizeAllow.ToString() + "(KB))" + "\n";
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Kiểm tra đuôi file
        /// </summary>
        /// <returns></returns>
        private bool CheckExtension(string mExtension)
        {
            try
            {
                if (string.IsNullOrEmpty(ExtensionAllow.Trim()))
                    return true;

                string str_Temp = ExtensionAllow.ToLower();
                mExtension = mExtension.ToLower();

                str_Temp = str_Temp.Replace(mExtension, "");
                if (str_Temp.Length < ExtensionAllow.Length)
                    return true;
                else
                {
                    Message = "Định dạng file không hợp lệ, định dạng hợp lệ là: " + ExtensionAllow + "\n";
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Xóa 1 file theo đường dẫn vật lý tuyệt đối truyền vào
        /// </summary>
        /// <param name="PhysicalPathFileName">Đường dẫn vật lý tuyệt đối của file cần xóa</param>
        /// <returns></returns>
        public bool DeleteFile(string PhysicalPathFileName)
        {
            try
            {
                FileInfo mFileInfo = new FileInfo(PhysicalPathFileName);
                if (mFileInfo.Exists)
                {
                    File.Delete(PhysicalPathFileName);

                    Message += "Xóa file " + PhysicalPathFileName + " thành công.\n";
                    return true;
                }
                else
                {
                    Message += "File " + PhysicalPathFileName + " không tồn tại.\n";
                    return false;
                }
            }
            catch (IOException ex)
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
            str_FileName = MyText.RemoveSignVietnameseString(str_FileName);
            str_FileName = MyText.RemoveSpecialChar(str_FileName);
            return str_FileName;
        }

        public void ResetInfo()
        {
            List_UploadedPathFile_Thumb.Clear();
            UploadedExtension = string.Empty;
            UploadedFileName = string.Empty;
            UploadedFileName_Thumb = string.Empty;
            UploadedFileNameWithoutExtension = string.Empty;
            UploadedFileSize = 0;
            UploadedPathFile = string.Empty;
            UploadedPathFile_Thumb = string.Empty;
            UploadedPhysicalPathFile = string.Empty;
            UploadedPhysicalPathFile_Thumb = string.Empty;
        }
        /// <summary>
        /// Upload file
        /// </summary>
        /// <returns></returns>
        public bool Upload()
        {
            try
            {
                //xóa bỏ các thuộc tính đã upload trược đó
                ResetInfo();

                //Chưa chọn file thì không thể upload
                if (string.IsNullOrEmpty(mPostedFile.FileName))
                {
                    Message += "Xin hãy chọn file cần upload!"+"\n";
                    return false;
                }

                UploadedExtension = Path.GetExtension(mPostedFile.FileName);

                //Kiểm tra Đuôi file và kích thước file cho phép
                if (!CheckExtension(UploadedExtension) || !CheckSize())
                    return false;

                UploadedFileNameWithoutExtension = Path.GetFileNameWithoutExtension(mPostedFile.FileName);
                UploadedFileNameWithoutExtension = ValidFileName(UploadedFileNameWithoutExtension);

                string str_SavePath = string.Empty;
                string str_Directory = string.Empty;

                //Nếu cho phép tạo tên file
                if (AutoGeneralFileName)
                {
                    #region MyRegion
                    string chars = "12346789abcdefghjkmnpqrtuvwxyz";
                    // create random generator
                    Random rnd = new Random();
                    do
                    {
                        // create name
                        UploadedFileNameWithoutExtension = string.Empty;

                        while (UploadedFileNameWithoutExtension.Length < 10)
                        {
                            UploadedFileNameWithoutExtension += chars.Substring(rnd.Next(chars.Length), 1);
                        }
                        // add extension

                        UploadedFileName = UploadedFileNameWithoutExtension + UploadedExtension;

                        //Lấy đường dẫn cần lưu file lên server
                        str_SavePath = HttpContext.Current.Server.MapPath(ServerUploadPath + UploadedFileName);


                    } while (File.Exists(str_SavePath));

                    //UploadedFileNameWithoutExtension = DateTime.Now.ToString("yyyyMMddhhmmssfff"); 
                    #endregion
                }
                else
                {
                    UploadedFileName = UploadedFileNameWithoutExtension + UploadedExtension;

                    //Lấy đường dẫn cần lưu file lên server
                    str_SavePath = HttpContext.Current.Server.MapPath(ServerUploadPath + UploadedFileName);
                }

                if (!string.IsNullOrEmpty(AddToFileName))
                {
                    UploadedFileNameWithoutExtension += AddToFileName;
                }              
                                
                //Kiểm tra tồn tại của thư mục
                str_Directory = Path.GetDirectoryName(str_SavePath);
                if(!Directory.Exists(str_Directory))
                {
                    Directory.CreateDirectory(str_Directory);
                }

                //Lưu file lên server
                mPostedFile.SaveAs(str_SavePath);

                Message += "Lưu file thành công" + "\n";

                //Lấy thông tin

                UploadedFileSize = mPostedFile.ContentLength / 1024;
                UploadedPhysicalPathFile = str_SavePath;
                UploadedPathFile = ServerUploadPath + UploadedFileName;

                UploadedPathFile = UploadedPathFile.Replace("~", "");

                ////Nếu trong trường hợp file là file gif thì không tạo thumbnail vì khi tao thumb thì sẽ ko còn là ảnh động
                //if (UploadedExtension.ToLower() == ".gif")
                //    AllowCreateThumbnail = false;

                if (AllowCreateThumbnail)
                {
                    CreateThumbnail(str_SavePath);
                }
                return true;
            }
            catch (IOException ex)
            {
                UploadedPhysicalPathFile = string.Empty;
                UploadedPhysicalPathFile_Thumb = string.Empty;
                throw ex;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_PhysicalServerPathFile"></param>
        /// <returns></returns>
        public bool CreateThumbnail(string str_PhysicalServerPathFile)
        {
            try
            {
                string[] arr_size = Image_WH_Thumb.ToLower().Split('|');
                string[] arr_name = SuffixThumbnailName.ToLower().Split('|');

                for (int i = 0; i < arr_size.Length; i++)
                {
                    string[] arr_wh = arr_size[i].Split('x');
                    if (arr_wh.Length != 2)
                        continue;
                    string thumbName = "_thumb";
                    if (arr_name.Length == arr_size.Length || arr_name.Length == i + 1)
                        thumbName = arr_name[i];

                    else if (arr_name.Length == 1)
                    {
                        thumbName = arr_name[0];
                    }
                    else
                    {
                        thumbName = SuffixThumbnailName;
                    }

                    int.TryParse(arr_wh[0], out Width_Thumb);
                    int.TryParse(arr_wh[1], out Height_Thumb);
                   
                    if (FixWidthOrHeight == 1) //fix theo width
                    {
                        CreateThumbnail(str_PhysicalServerPathFile, Width_Thumb, true, thumbName);
                    }
                    else if (FixWidthOrHeight == 2) //fix theo height
                    {
                        CreateThumbnail(str_PhysicalServerPathFile, Height_Thumb, false, thumbName);
                    }
                    else
                    {
                        CreateThumbnail(str_PhysicalServerPathFile, Width_Thumb, Height_Thumb, thumbName);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_PhysicalServerPathFile"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="addFileName"></param>
        /// <returns></returns>
        public bool CreateThumbnail(string str_PhysicalServerPathFile, int width, int height, string addFileName)
        {
            try
            {
                string str_Extension = Path.GetExtension(str_PhysicalServerPathFile);

                //Tạo tên file cho file thumbnail
                string str_SaveFileName_Thumb = str_PhysicalServerPathFile.Replace(str_Extension, "") + addFileName + str_Extension;

                System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(str_PhysicalServerPathFile);
               
                System.Drawing.Image.GetThumbnailImageAbort dummyCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);

                System.Drawing.Image thumbNailImg = fullSizeImg.GetThumbnailImage(width, height, dummyCallBack, IntPtr.Zero);

                //a holder for the result 
                Bitmap result = new Bitmap(width, height);

                //use a graphics object to draw the resized image into the bitmap 
                using (Graphics graphics = Graphics.FromImage(result))
                {
                    //set the resize quality modes to high quality 
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.AssumeLinear;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    //draw the image into the target bitmap 
                    graphics.DrawImage(thumbNailImg, 0, 0, result.Width, result.Height);
                }

                //Lưu file
                thumbNailImg.Save(str_SaveFileName_Thumb);

                //Lấy thông tin
                UploadedFileName_Thumb = Path.GetFileName(str_SaveFileName_Thumb);
                UploadedPhysicalPathFile_Thumb = str_SaveFileName_Thumb;

                UploadedPathFile_Thumb = ServerUploadPath + UploadedFileName_Thumb;
                UploadedPathFile_Thumb = UploadedPathFile_Thumb.Replace("~", "");

                List_UploadedPathFile_Thumb.Add(UploadedPathFile_Thumb);

                fullSizeImg.Dispose();
                thumbNailImg.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// tạo ảnh Thumbnail cố định width hoạc height
        /// </summary>
        /// <param name="str_PhysicalServerPathFile"></param>
        /// <param name="IsFixWidth">True nếu fix chiều rộng; False nếu fix chiều cao</param>
        /// <param name="FileName">Tên file</param>
        /// <param name="IsFileName">True nếu muốn tạo tên file là FileName</param>
        /// <returns></returns>
        public bool CreateThumbnail(string str_PhysicalServerPathFile,bool IsFixWidth,string FileName,bool IsFileName)
        {
            try
            {
                string[] arr_size = Image_WH_Thumb.ToLower().Split('|');
                string sFileName;
                int WH;
                for (int i = 0; i < arr_size.Length; i++)
                {
                    string[] arr_wh = arr_size[i].Split('x');
                    if (arr_wh.Length != 2)
                        continue;

                    int.TryParse(arr_wh[0], out Width_Thumb);
                    int.TryParse(arr_wh[1], out Height_Thumb);
                    if (IsFileName)
                    {
                        sFileName = FileName;
                    }
                    else
                    {
                        sFileName = arr_size[i];
                    }

                    if (IsFixWidth)
                    {
                        WH= Width_Thumb;
                    }
                    else
                    {
                        WH = Height_Thumb;
                    }
                    CreateThumbnail(str_PhysicalServerPathFile, WH, IsFixWidth, sFileName);
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Tạo thumbnail với chiều rộng cố định
        /// </summary>
        /// <param name="str_PhysicalServerPathFile"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="addFileName"></param>
        /// <returns></returns>
        public bool CreateThumbnail(string str_PhysicalServerPathFile, int WH, bool IsWidth, string addFileName)
        {
            try
            {
                int Height;
                int Widht;
                string str_Extension = Path.GetExtension(str_PhysicalServerPathFile);

                //Tạo tên file cho file thumbnail
                string str_SaveFileName_Thumb = str_PhysicalServerPathFile.Replace(str_Extension, "") + addFileName + str_Extension;


                System.Drawing.Image fullSizeImg = System.Drawing.Image.FromFile(str_PhysicalServerPathFile);

                if (IsWidth)
                {
                     Widht=WH;
                     Height = fullSizeImg.Height * Widht / fullSizeImg.Width;
                }
                else
                {
                    Height=WH;
                    Widht = fullSizeImg.Width * Height / fullSizeImg.Height;
                }

                System.Drawing.Image.GetThumbnailImageAbort dummyCallBack = new System.Drawing.Image.GetThumbnailImageAbort(ThumbnailCallback);

                System.Drawing.Image thumbNailImg = fullSizeImg.GetThumbnailImage(Widht, Height, dummyCallBack, IntPtr.Zero);

                //a holder for the result 
                Bitmap result = new Bitmap(Widht, Height);

                //use a graphics object to draw the resized image into the bitmap 
                using (Graphics graphics = Graphics.FromImage(result))
                {
                    //set the resize quality modes to high quality 
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.AssumeLinear;
                    graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.Low;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    //draw the image into the target bitmap 
                    graphics.DrawImage(thumbNailImg, 0, 0, result.Width, result.Height);
                }

                //Lưu file
                thumbNailImg.Save(str_SaveFileName_Thumb);

                //Lấy thông tin
                UploadedFileName_Thumb = Path.GetFileName(str_SaveFileName_Thumb);
                UploadedPhysicalPathFile_Thumb = str_SaveFileName_Thumb;

                UploadedPathFile_Thumb = ServerUploadPath + UploadedFileName_Thumb;
                UploadedPathFile_Thumb = UploadedPathFile_Thumb.Replace("~", "");
                List_UploadedPathFile_Thumb.Add(UploadedPathFile_Thumb);
                fullSizeImg.Dispose();
                thumbNailImg.Dispose();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ThumbnailCallback()
        {
            return false;
        }
             
    }
}
