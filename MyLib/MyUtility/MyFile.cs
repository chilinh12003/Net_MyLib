using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Text;
using System.IO;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Collections.Specialized;
namespace MyLib.MyUtility
{
    /// <summary>
    /// Chứa các hàm, thông tin xử lý file
    /// </summary>
    public class MyFile
    {
        /// <summary>
        /// Đọc nội dung của 1 file theo 1 đường dần tuyệt đối
        /// </summary>
        /// <param name="str_FullPath">Đường dẫn tuyệt đối của file</param>
        /// <returns></returns>
        public static string ReadFile(string str_FullPath)
        {
            string str_Content = string.Empty;

            System.IO.FileStream viewStream = null;

            //Kiểm tra sự tồn tại của file
            if (!File.Exists(str_FullPath))
            {
                //Thử lấy đường dẫn full rồi kiểm tra lại
                str_FullPath = GetFullPathFile(str_FullPath);
                if (!File.Exists(str_FullPath))
                {
                    return string.Empty;
                }
            }

            try
            {
                viewStream = new System.IO.FileStream(str_FullPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);

                System.IO.StreamReader reader = new System.IO.StreamReader(viewStream);
               
                str_Content = reader.ReadToEnd();

                //Release resources
                reader.Dispose();

                return str_Content;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (viewStream != null)
                {
                    viewStream.Close();
                    viewStream.Dispose();
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str_FullPath"></param>
        /// <returns></returns>
        public static StringCollection  ReadFile_Collection(string str_FullPath)
        {
            string str_Content = string.Empty;

            System.IO.FileStream viewStream = null;

            //Kiểm tra sự tồn tại của file
            if (!File.Exists(str_FullPath))
            {
                //Thử lấy đường dẫn full rồi kiểm tra lại
                str_FullPath = GetFullPathFile(str_FullPath);
                if (!File.Exists(str_FullPath))
                {
                    return new StringCollection();
                }
            }

            try
            {
                viewStream = new System.IO.FileStream(str_FullPath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);

                System.IO.StreamReader reader = new System.IO.StreamReader(viewStream);
                string Line = string.Empty;

                StringCollection mLsist = new StringCollection();
                while ((Line = reader.ReadLine()) != null)
                {
                    mLsist.Add(Line);
                }
                //Release resources
                reader.Dispose();

                return mLsist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (viewStream != null)
                {
                    viewStream.Close();
                    viewStream.Dispose();
                }
            }
        }


        /// <summary>
        /// Lấy đường dẫn tuyệt đối theo 1 đường dẫn tương đối truyền vào
        /// </summary>
        /// <param name="str_Path">Đường dẫn tương đối của file</param>
        /// <returns></returns>
        public static string GetFullPathFile(string str_Path)
        {
            try
            {
                if (MyConfig.IsWeb)
                {
                    if (System.Web.HttpContext.Current != null)
                    {
                        return System.Web.HttpContext.Current.Server.MapPath(str_Path);
                    }
                    else
                        return string.Empty;
                }
                else
                {
                    return AppDomain.CurrentDomain.BaseDirectory + str_Path;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Kiểm tra sự tồn tại của file và chỉnh sửa filePath thành fullPath
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public static bool CheckExistFile(ref string FilePath)
        {
            try
            {
                if (File.Exists(FilePath))
                    return true;
                else
                {
                    FilePath = GetFullPathFile(FilePath);

                    if (File.Exists(FilePath))
                        return true;
                    else
                    {
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Trả về 1 link theo Myconfig.ReourceLink (key =ReourceLink trong webconfig)
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static string GetFullResourceLink(string Path)
        {
            try
            {
                //return Path;
                if (!string.IsNullOrEmpty(MyConfig.ResourceLink))
                {
                    return MyConfig.ResourceLink + Path.Replace(@"..", "").Replace(@"~", "").Replace(MyConfig.ResourceLink, "");
                }
                else
                {
                    return Path;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Đọc nội dung của 1 trang web
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static string ReadContentFromURL(string URL)
        {
            System.Net.WebResponse response = null;
            System.IO.StreamReader reader = null;
            try
            {
                string str_Result = string.Empty;

                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(URL);

                request.Method = "GET";
                request.Timeout = 36000;

                response = request.GetResponse();
                reader = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8);

                str_Result = reader.ReadToEnd();

                return str_Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (response != null)
                    response.Close();
            }
        }

        public static string ReadContentFromURL_NotEncodingUTF8(string URL)
        {
            System.Net.WebResponse response = null;
            System.IO.StreamReader reader = null;
            try
            {
                string str_Result = string.Empty;

                System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(URL);

                request.Method = "GET";
                request.Timeout = 36000;

                response = request.GetResponse();

                var stream = response.GetResponseStream();

                try
                {
                    if (response.Headers["Content-Encoding"] != null
                        && response.Headers["Content-Encoding"].Contains("gzip"))
                    {
                        stream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress);
                    }

                    using (StreamReader reader_1 = new StreamReader(stream))
                    {
                        str_Result = reader_1.ReadToEnd();
                    }
                }
                finally
                {
                    if (stream != null)
                        stream.Dispose();
                }

                return str_Result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (reader != null)
                    reader.Close();

                if (response != null)
                    response.Close();
            }
        }


        /// <summary>
        /// Lấy dung lượng của file tính bằng(KB)
        /// </summary>
        /// <param name="FilePath">Đường dẫn tương đối tới file VD:~/u/fsdfdf.mp3</param>
        /// <returns></returns>
        public static long GetFileSize(string FilePath)
        {
            try
            {
                FileInfo finfo = new FileInfo(GetFullPathFile(FilePath));
                long FileInKB = finfo.Length / 1024;
                return FileInKB;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Đưa dữ liệu ra file Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dt">bảng chứa dữ liệu cần đưa ra</param>
        /// <param name="FileName">Tên file excel</param>
        /// <param name="Columns">tên nhãn các cột sẽ xuất</param>
        /// <returns></returns>
        public static bool ExportToExcel(System.Web.UI.Page page, DataTable dt, String FileName, string[] Columns)
        {
            try
            {
                //Get the data from database into datatable
                int tmp = 0;
                if (dt.Columns.Count > 0)
                {
                    while (tmp < Columns.Length)
                    {
                        dt.Columns[tmp].ColumnName = Columns[tmp];
                        tmp++;
                    }
                }
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();
                page.Response.Clear();
                page.Response.Buffer = true;
                page.Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xls");
                page.Response.ContentEncoding = Encoding.UTF8;
                page.Response.Charset = "";
                page.Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);
                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                page.Response.Write(style);
                string str_write = sw.ToString();
                //mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                page.Response.Output.Write(str_write);
                page.Response.Flush();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                page.Response.End();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Đưa dữ liệu ra file Excel
        /// </summary>
        /// <param name="page"></param>
        /// <param name="dt">bảng chứa dữ liệu cần đưa ra</param>
        /// <param name="FileName">Tên file excel</param>
        /// <param name="ColumnsDB">tên nhãn các cột sẽ xuất trong bảng dt</param>
        /// <param name="Columns">tên nhãn các cột sẽ xuất</param>
        /// <returns></returns>
        public static bool ExportToExcel(System.Web.UI.Page page, DataTable dt, String FileName, string[] ColumnsDB, string[] Columns)
        {
            try
            {
                //Get the data from database into datatable
                int tmp = 0;
                string sListColumn = "";
                bool flag;
                if (dt != null && dt.Columns.Count > 0)
                {
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        //Đổi tên column trong dt bằng tên sẽ hiển thị trong file excel
                        flag = false;
                        for (tmp = 0; tmp < ColumnsDB.Length; tmp++)
                        {
                            if (ColumnsDB[tmp].ToUpper() == dt.Columns[i].ColumnName.ToUpper())
                            {
                                dt.Columns[i].ColumnName = Columns[tmp].ToString();
                                flag = true;
                                break;
                            }
                        }
                        //lưu index các cột không cần cho report vào sListColumn
                        if (flag == false)
                        {
                            sListColumn = sListColumn + i.ToString() + ",";
                        }
                    }
                    //remove bỏ dấu phẩy cuối cùng
                    sListColumn = sListColumn.Substring(0, sListColumn.Length - 1);
                    string[] sListColumnDelete = sListColumn.Split(',');

                    //xóa bỏ những cột không cần đưa lên file excel
                    if (sListColumnDelete.Length > 0)
                    {
                        for (int i = sListColumnDelete.Length - 1; i >= 0; i--)
                        {
                            dt.Columns.Remove(dt.Columns[int.Parse(sListColumnDelete[i])]);
                        }
                    }
                }
                // đưa dữ liệu ra file excel
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();
                page.Response.Clear();
                page.Response.Buffer = true;
                page.Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".xls");
                page.Response.ContentEncoding = Encoding.UTF8;
                page.Response.Charset = "";
                page.Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);
                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                page.Response.Write(style);
                string str_write = sw.ToString();
                //mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                page.Response.Output.Write(str_write);
                page.Response.Flush();
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                page.Response.End();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Đọc dữ liệu từ file Excel
        /// </summary>
        /// <param name="sFile">Đường dẫn tới file Excel</param>
        /// <returns>dataset chứa nội dung file excel</returns>
        public static DataSet ReadExcelFiles(string sFile)
        {
            DataSet ds = new DataSet("Excel");
            if (!File.Exists(sFile))
            {
                return null;
            }
            OleDbConnection con = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFile + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"");
            try
            {
                con.Open();
                DataTable TBL;
                DataTable SheetTable = new DataTable();
                SheetTable = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                for (int Sheet = 0; Sheet < SheetTable.Rows.Count; Sheet++)
                {
                    TBL = new DataTable();
                    OleDbCommand com = new OleDbCommand("SELECT DISTINCT * FROM [" + SheetTable.Rows[Sheet][2].ToString() + "]", con);
                    OleDbDataAdapter dap = new OleDbDataAdapter(com);
                    dap.Fill(TBL);
                    TBL.TableName = "Table" + Sheet.ToString();
                    TBL.AcceptChanges();
                    ds.Tables.Add(TBL);
                }
                ds.AcceptChanges();
                return ds;
            }
            catch (Exception ex)
            {                
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        
        /// <summary>
        /// Nén các file cần nén có trong thư mục sFilePath và lưu các file nén này vào thư mục StorPath
        /// </summary>
        /// <param name="sFilePath">Đường dẫn tới file cần nén</param>
        /// <param name="StorPath">Đường dấn tới thư mục chưa file nén</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool ZipFile(string sFilePath, string StorPath)
        {
            byte[] byteSource = null;
            byte[] byteCompressed = null;
            try
            {
                string sFileName = sFilePath.Substring(sFilePath.LastIndexOf("\\") + 1);

                byteSource = System.IO.File.ReadAllBytes(sFilePath);
                byteCompressed = CompressByte(byteSource);
                System.IO.File.WriteAllBytes(StorPath + "\\" + sFileName + ".zip", byteCompressed);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Giải nén file .ZIP trong thư mục sFilePath và lưu file giải nén vào StorPath
        /// </summary>
        /// <param name="sFilePath">Đường dẫn tới thư mục chưa file .ZIP</param>
        /// <param name="StorPath">Đường dẫn tới thư mục chưa file giải nén</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public bool UnZipFile(string sFilePath, string StorPath)
        {
            byte[] byteDecompressed = null;
            try
            {
                string sFileName = sFilePath.Substring(sFilePath.LastIndexOf("\\") + 1);
                sFileName = sFileName.Remove(sFileName.LastIndexOf("."));
                byteDecompressed = DecompressByte(System.IO.File.ReadAllBytes(sFilePath));
                System.IO.File.WriteAllBytes(StorPath + "\\" + sFileName, byteDecompressed);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Nén mảng byte truyền vào
        /// </summary>
        /// <param name="byteSource"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private byte[] CompressByte(byte[] byteSource)
        {
            try
            {
                // Create and GZipStream objects and memory stream object to store compressed stream
                MemoryStream objMemStream = new MemoryStream();
                GZipStream objGZipStream = new GZipStream(objMemStream, CompressionMode.Compress, true);

                objGZipStream.Write(byteSource, 0, byteSource.Length);
                objGZipStream.Dispose();
                objMemStream.Position = 0;

                // Write compressed memory stream into byte array
                byte[] buffer = new byte[objMemStream.Length + 1];
                objMemStream.Read(buffer, 0, buffer.Length);
                objMemStream.Dispose();
                return buffer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Giải nén mảng byte truyền vào
        /// </summary>
        /// <param name="byteCompressed"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private byte[] DecompressByte(byte[] byteCompressed)
        {

            try
            {
                // Initialize memory stream with byte array.
                MemoryStream objMemStream = new MemoryStream(byteCompressed);
                // Initialize GZipStream object with memory stream.
                GZipStream objGZipStream = new GZipStream(objMemStream, CompressionMode.Decompress);
                // Define a byte array to store header part from compressed stream.
                byte[] sizeBytes = new byte[4];
                // Read the size of compressed stream.
                objMemStream.Position = objMemStream.Length - 5;
                objMemStream.Read(sizeBytes, 0, 4);
                int iOutputSize = BitConverter.ToInt32(sizeBytes, 0);
                // Posistion the to point at beginning of the memory stream to read 
                // compressed stream for decompression.
                objMemStream.Position = 0;
                byte[] decompressedBytes = new byte[iOutputSize];
                // Read the decompress bytes and write it into result byte array.
                objGZipStream.Read(decompressedBytes, 0, iOutputSize);
                objGZipStream.Dispose();
                objMemStream.Dispose();
                return decompressedBytes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Save content to file, nhưng ko giữ nội dụng của file trước đó
        /// </summary>
        /// <param name="FullPath"></param>
        /// <param name="Content"></param>
        public static void SaveFile(string FullPath, string Content)
        {
            try
            {
                System.IO.File.WriteAllText(FullPath, Content);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
