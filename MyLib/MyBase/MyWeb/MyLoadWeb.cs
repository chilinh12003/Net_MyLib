using System;
using System.Collections.Generic;
using System.Text;
using MyLib.MyUtility;
using System.Web;
using System.Data;
using System.Reflection;
namespace MyLib.MyBase.MyWeb
{
    /// <summary>
    /// Đây là lớp base dành cho các class load template trong MyLoad của từng wap,website
    /// </summary>
    public class MyLoadWeb
    {
       public MyLog mLog = new MyLog(typeof(MyLoadWeb));

        /// <summary>
        /// Trang hiện tại
        /// </summary>
       public MyASHX CurrentPage
       {
           get;
           set;
       }

        /// <summary>
        /// chứa danh sách các module được phép cache, và được khởi tạo khi application bắt đấu chạy
        /// <para>
        /// VD: "MyLoad.LoadStatic.MyBanner",true,5
        /// </para>
        /// </summary>
        public static System.Data.DataTable ModuleCacheList = new System.Data.DataTable();

        // Lấy đường dẫn file HTML
        // Tham số {0} sẽ được truyền vào sau.
        /// <summary>
        /// 
        /// </summary>
        public string TemplatePath;
        /// <summary>
        /// 
        /// </summary>
        public string TemplatePathRepeat;
        /// <summary>
        /// 
        /// </summary>
        public string TemplatePathRepeatChild;
     

        /// <summary>
        /// Cho phép Module này ẩn hay hiện (Disable = true thì sẽ trả về chuỗi HTML rỗng) nghĩa là không hiển thị gì cả
        /// </summary>
        public bool Disable = false;
      
        /// <summary>
        /// Key để lưu dữ liệu cate của HMTL, nhằm  mục đích không phải load lại nhiều lần
        /// </summary>
        public string CacheKey;

        /// <summary>
        /// Cho phép đối tượng này được phép catching HTML sau khi build ra hay không
        /// </summary>
        public bool AllowCache = false;

        /// <summary>
        /// Khởi tạo các giá trị đầu tiên
        /// </summary>
        public void Init()
        {
            CacheKey = this.GetType().Namespace + "." + this.GetType().Name;
            
        }
        /// <summary>
        /// 
        /// </summary>

        public MyLoadWeb()
        {
            this.CurrentPage = CurrentPage;
            Init();
        }
        public MyLoadWeb(MyASHX CurrentPage)
        {
            this.CurrentPage = CurrentPage;
            Init();
        }

        /// <summary>
        /// Thời gian cho phép catching trên class được tính bằng Giây
        /// </summary>
        public int CacheTime = 300;

        /// <summary>
        /// Lưu giữ một dữ liệu của đối tượng được tạo (lần kề trước). để phục vụ cho kiểm tra xem có được phép removeCache hay không
        /// </summary>
        public string PreviousKey
        {
            get
            {
                if (MyCurrent.CurrentPage != null && MyCurrent.CurrentPage.Session != null && MyCurrent.CurrentPage.Session[CacheKey] != null)
                {
                    return MyCurrent.CurrentPage.Session[CacheKey].ToString();
                }
                return string.Empty;
            }
            set
            {
                if (MyCurrent.CurrentPage != null && MyCurrent.CurrentPage.Session != null)
                {
                    MyCurrent.CurrentPage.Session[CacheKey] = value;
                }
            }
        }

        /// <summary>
        /// Hàm được chạy trước khi thực hiện trả về chuỗi HMTL
        /// </summary>
        protected virtual void Begin()
        {

        }

        /// <summary>
        /// Hàm chạy cuối cùng của đối tượng
        /// </summary>
        protected virtual void Finish()
        {

        }

        /// <summary>
        /// Trả về chuổi HTML cần Build của lớp này đồng thời trước khi trả thì cache lại
        /// </summary>
        /// <returns></returns>
        public string GetHTML()
        {
            Begin();

            if (Disable)
                return string.Empty;

            try
            {
                //Lấy thông tin về cache cho từng module.
                #region MyRegion
                if (ModuleCacheList != null && ModuleCacheList.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow mRow in ModuleCacheList.Rows)
                    {
                        if (mRow["ClassName"].ToString().Equals(this.GetType().FullName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            if (mRow["IsCache"] != DBNull.Value)
                            {
                                bool.TryParse(mRow["IsCache"].ToString(), out AllowCache);
                            }
                            if (mRow["CacheTime"] != DBNull.Value)
                                int.TryParse(mRow["CacheTime"].ToString(), out CacheTime);
                        }
                    }
                } 
                #endregion

                //nếu module cho phép cache thì tiến hành cache với key và tên namepace.Classname của module này
                if (AllowCache)
                {
                    #region MyRegion
                    if (MyCurrent.CurrentPage != null && MyCurrent.CurrentPage.Cache != null && MyCurrent.CurrentPage.Cache[CacheKey] != null)
                    {
                        try
                        {
                            return MyCurrent.CurrentPage.Cache[CacheKey].ToString();
                        }
                        catch (Exception ex)
                        {
                            mLog.Error(ex);
                            return BuildHTML();
                        }
                    }
                    else
                    {
                        string HTML = BuildHTML();
                        MyCurrent.CurrentPage.Cache.Add(CacheKey, HTML, null, DateTime.Now.AddSeconds(CacheTime), System.Web.Caching.Cache.NoSlidingExpiration, System.Web.Caching.CacheItemPriority.Normal, null);
                        return HTML;
                    } 
                    #endregion
                }
                else
                {
                    return BuildHTML();
                }
            }
            catch (Exception ex)
            {
                mLog.Error( ex);
                throw ex;
                
            }
            finally
            {
                Finish();
            }
        }

        /// <summary>
        /// Xóa bảo cache cho một module
        /// </summary>
        /// <returns></returns>
        public bool RemoveCache()
        {
            try
            {
                if (MyCurrent.CurrentPage != null && MyCurrent.CurrentPage.Cache != null && MyCurrent.CurrentPage.Cache[CacheKey] != null)
                {

                    MyCurrent.CurrentPage.Cache.Remove(CacheKey);

                    if (MyCurrent.CurrentPage.Cache[CacheKey] != null)
                        return false;
                    else
                        return true;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Build lên chuỗi HTML của lớp
        /// </summary>
        /// <returns></returns>
        protected virtual string BuildHTML()
        {
            return string.Empty;
        }

        /// <summary>
        /// Đọc template từ file
        /// </summary>
        /// <param name="TemplatePath"></param>
        /// <returns></returns>
        public string LoadTemplate(string TemplatePath)
        {
            try
            {
                TemplatePath = MyFile.GetFullPathFile(TemplatePath);
                string strTemplate = MyFile.ReadFile(TemplatePath);

                //Replace Domain trước khi Format vì sẽ gấy ra lỗi.
                //VD: Replace {DNS} thanh http://xxx.vn
                strTemplate = strTemplate.Replace(MyConfig.DomainParameter, MyConfig.Domain);

                return strTemplate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///  Đọc template từ file
        /// </summary>
        /// <param name="TemplatePath"></param>
        /// <param name="IsUrl">True nếu là đường dẫn URL full</param>
        /// <returns></returns>
        public string LoadTemplate(string TemplatePath, bool IsUrl)
        {
            try
            {
                if (IsUrl)
                {
                    return MyFile.ReadContentFromURL(TemplatePath);
                }
                else
                {
                    TemplatePath = MyFile.GetFullPathFile(TemplatePath);
                    return MyFile.ReadFile(TemplatePath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load Template theo dữ liệu truyền vào là DataTable (load tất cả các Column)
        /// </summary>
        /// <param name="TemplatePath">Tên file Template</param>
        /// <param name="Data">DataTable dữ liệu</param>
        /// <returns></returns>
        public string LoadTemplate(string TemplatePath, DataTable Data)
        {
            try
            {
                TemplatePath = MyFile.GetFullPathFile(TemplatePath);
                if (!MyFile.CheckExistFile(ref TemplatePath))
                    throw new Exception("File Template không tồn tại.");

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                //Load Template vào 1 chuỗi
                string strTemplate = MyFile.ReadFile(TemplatePath);

                //Replace Domain trước khi Format vì sẽ gấy ra lỗi.
                //VD: Replace {DNS} thanh http://funzone.vn
                strTemplate = strTemplate.Replace(MyConfig.DomainParameter, MyConfig.Domain);

                //Tạo mảng lưu trữ các tham số của 1 Row trong Table
                System.Collections.ArrayList arr_ListPara = new System.Collections.ArrayList();

                foreach (DataRow mRow in Data.Rows)
                {
                    arr_ListPara = new System.Collections.ArrayList();

                    //Lấy dữ liệu từ tất cả các cột của dòng hiện tại
                    for (int i = 0; i < Data.Columns.Count; i++)
                    {
                        //Nếu column có định dạng là ngày tháng thì lấy kiểu định dạng là ngày tháng.
                        if (Data.Columns[i].DataType == typeof(DateTime))
                        {
                            arr_ListPara.Add(((DateTime)mRow[i]).ToString(MyConfig.ViewDateFormat));
                        }
                        else
                        {
                            arr_ListPara.Add(mRow[i].ToString());
                        }
                    }

                    //Ứng với mỗi dòng sẽ load 1 template
                    mBuilder.Append(string.Format(strTemplate, arr_ListPara.ToArray()));
                }
                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load Template với 1 tham số truyền vào
        /// </summary>
        /// <param name="TemplatePath"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public string LoadTemplateByString(string TemplatePath, string Value)
        {
            try
            {
                TemplatePath = MyFile.GetFullPathFile(TemplatePath);

                if (!MyFile.CheckExistFile(ref TemplatePath))
                    throw new Exception("File Template không tồn tại.");

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                //Load Template vào 1 chuỗi
                string strTemplate = MyFile.ReadFile(TemplatePath);

                //Replace Domain trước khi Format vì sẽ gấy ra lỗi.
                //VD: Replace {DNS} thanh http://funzone.vn
                strTemplate = strTemplate.Replace(MyConfig.DomainParameter, MyConfig.Domain);

                //Ứng với mỗi dòng sẽ load 1 template
                mBuilder.Append(string.Format(strTemplate, Value));

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Replace có dạng code như sau.
        /// <code>
        /// var someObj = new {pi = 3.14, date = DateTime.Now};
        /// string s = LoadTemplateByObject("{pi} first, {date} second", someObj);
        /// </code>
        /// </summary>
        /// <param name="TemplatePath"></param>
        /// <param name="mObj"></param>
        /// <returns></returns>
        public string LoadTemplateByObject(string TemplatePath, object mObj)
        {
            try
            {
                TemplatePath = MyFile.GetFullPathFile(TemplatePath);

                if (!MyFile.CheckExistFile(ref TemplatePath))
                    throw new Exception("File Template không tồn tại.");
                //Load Template vào 1 chuỗi
                string strTemplate = MyFile.ReadFile(TemplatePath);

                //Replace Domain trước khi Format vì sẽ gấy ra lỗi.
                //VD: Replace {DNS} thanh http://abc.vn
                strTemplate = strTemplate.Replace(MyConfig.DomainParameter, MyConfig.Domain);

                FieldInfo[] ListField = mObj.GetType().GetFields();

                foreach (FieldInfo mField in ListField)
                {
                    strTemplate = strTemplate.Replace("{" + mField.Name + "}", mField.GetValue(mObj).ToString());
                }

                return strTemplate;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Load template dựa các mảng các giá trị được truyền vào
        /// </summary>
        /// <param name="TemplatePath"></param>
        /// <param name="ArrayValue"></param>
        /// <returns></returns>
        public string LoadTemplateByArray(string TemplatePath, string[] ArrayValue)
        {
            try
            {
                if (ArrayValue.Length < 1)
                    return LoadTemplate(TemplatePath);

                TemplatePath = MyFile.GetFullPathFile(TemplatePath);

                if (!MyFile.CheckExistFile(ref TemplatePath))
                    throw new Exception("File Template không tồn tại.");

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                //Load Template vào 1 chuỗi
                string strTemplate = MyFile.ReadFile(TemplatePath);

                //Replace Domain trước khi Format vì sẽ gấy ra lỗi.
                //VD: Replace {DNS} thanh http://abc.vn
                strTemplate = strTemplate.Replace(MyConfig.DomainParameter, MyConfig.Domain);

                //Ứng với mỗi dòng sẽ load 1 template
                mBuilder.Append(string.Format(strTemplate, ArrayValue));

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load Template theo Index của Column
        /// </summary>
        /// <param name="TemplatePath">Tên file Template</param>
        /// <param name="Data">DataTable chứa dữ liệu</param>
        /// <param name="ColumnIndex">Index của Column cần lấy dữ liệu</param>
        /// <returns></returns>
        public string LoadTemplateByColumIndex(string TemplatePath, DataTable Data, int[] ColumnIndex)
        {
            try
            {
                if (ColumnIndex.Length < 1 || Data == null || Data.Columns.Count < ColumnIndex.Length)
                    throw new Exception("Số lượng item trong ColumnIndex truyền vào là không phù hợp");

                TemplatePath = MyFile.GetFullPathFile(TemplatePath);

                if (!MyFile.CheckExistFile(ref TemplatePath))
                    throw new Exception("File Template không tồn tại.");

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                //Load Template vào 1 chuỗi
                string strTemplate = MyFile.ReadFile(TemplatePath);

                //Replace Domain trước khi Format vì sẽ gấy ra lỗi.
                //VD: Replace {DNS} thanh http://funzone.vn
                strTemplate = strTemplate.Replace(MyConfig.DomainParameter, MyConfig.Domain);

                //Tạo mảng lưu trữ các tham số của 1 Row trong Table
                System.Collections.ArrayList arr_ListPara = new System.Collections.ArrayList();

                foreach (DataRow mRow in Data.Rows)
                {
                    arr_ListPara = new System.Collections.ArrayList();

                    //Lấy dữ liệu từ tất cả các cột của dòng hiện tại
                    for (int i = 0; i < ColumnIndex.Length; i++)
                    {
                        //Nếu column có định dạng là ngày tháng thì lấy kiểu định dạng là ngày tháng.
                        if (Data.Columns[ColumnIndex[i]].DataType == typeof(DateTime))
                        {
                            arr_ListPara.Add(((DateTime)mRow[ColumnIndex[i]]).ToString(MyConfig.ViewDateFormat));
                        }
                        else
                        {
                            arr_ListPara.Add(mRow[ColumnIndex[i]].ToString());
                        }
                    }

                    //Ứng với mỗi dòng sẽ load 1 template
                    mBuilder.Append(string.Format(strTemplate, arr_ListPara.ToArray()));

                }

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load Template theo Index của Column
        /// </summary>
        /// <param name="TemplatePath">Tên file Template</param>
        /// <param name="Data">DataTable chứa dữ liệu</param>
        /// <param name="ColumnIndex">Index của Column cần lấy dữ liệu</param>
        /// <param name="IsUrl">True nếu là đường dẫn URL full</param>
        /// <returns></returns>
        public string LoadTemplateByColumIndex(string TemplatePath, DataTable Data, int[] ColumnIndex, bool IsUrl)
        {
            try
            {
                if (ColumnIndex.Length < 1 || Data == null || Data.Columns.Count < ColumnIndex.Length)
                    throw new Exception("Số lượng item trong ColumnIndex truyền vào là không phù hợp");

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                //Load Template vào 1 chuỗi
                string strTemplate = string.Empty;
                if (IsUrl)
                {
                    strTemplate = MyFile.ReadContentFromURL(TemplatePath);
                }
                else
                {
                    TemplatePath = MyFile.GetFullPathFile(TemplatePath);

                    if (!MyFile.CheckExistFile(ref TemplatePath))
                        throw new Exception("File Template không tồn tại.");

                    strTemplate = MyFile.ReadFile(TemplatePath);
                }

                //Tạo mảng lưu trữ các tham số của 1 Row trong Table
                System.Collections.ArrayList arr_ListPara = new System.Collections.ArrayList();

                foreach (DataRow mRow in Data.Rows)
                {
                    arr_ListPara = new System.Collections.ArrayList();

                    //Lấy dữ liệu từ tất cả các cột của dòng hiện tại
                    for (int i = 0; i < ColumnIndex.Length; i++)
                    {
                        //Nếu column có định dạng là ngày tháng thì lấy kiểu định dạng là ngày tháng.
                        if (Data.Columns[ColumnIndex[i]].DataType == typeof(DateTime))
                        {
                            arr_ListPara.Add(((DateTime)mRow[ColumnIndex[i]]).ToString(MyConfig.ViewDateFormat));
                        }
                        else
                        {
                            arr_ListPara.Add(mRow[ColumnIndex[i]].ToString());
                        }
                    }

                    //Ứng với mỗi dòng sẽ load 1 template
                    mBuilder.Append(string.Format(strTemplate, arr_ListPara.ToArray()));

                }

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load Tempate theo tên Column
        /// </summary>
        /// <param name="TemplatePath">Tên file Template</param>
        /// <param name="Data"></param>
        /// <param name="ColumnName">Mảng các tên Column cần lấy dữ liệu</param>
        /// <returns></returns>
        public string LoadTemplateByColumnName(string TemplatePath, DataTable Data, string[] ColumnName)
        {
            try
            {
                if (ColumnName.Length < 1 || Data == null || Data.Columns.Count < ColumnName.Length)
                    throw new Exception("Số lượng item trong ColumnIndex truyền vào là không phù hợp");

                TemplatePath = MyFile.GetFullPathFile(TemplatePath);

                if (!MyFile.CheckExistFile(ref TemplatePath))
                    throw new Exception("File Template không tồn tại.");

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                //Load Template vào 1 chuỗi
                string strTemplate = MyFile.ReadFile(TemplatePath);

                //Replace Domain trước khi Format vì sẽ gấy ra lỗi.
                //VD: Replace {DNS} thanh http://funzone.vn
                strTemplate = strTemplate.Replace(MyConfig.DomainParameter, MyConfig.Domain);

                //Tạo mảng lưu trữ các tham số của 1 Row trong Table
                System.Collections.ArrayList arr_ListPara = new System.Collections.ArrayList();

                foreach (DataRow mRow in Data.Rows)
                {
                    arr_ListPara = new System.Collections.ArrayList();

                    //Lấy dữ liệu từ tất cả các cột của dòng hiện tại
                    for (int i = 0; i < ColumnName.Length; i++)
                    {

                        //if (ColumnName[i].ToLower() == "imagepath")
                        //{
                        //    arr_ListPara.Add(MyFile.GetFullResourceLink(mRow[ColumnName[i]].ToString()));
                        //    continue;
                        //}
                        // Kien DT -> Nếu total down là DBNull thì trả về 0
                        if (ColumnName[i].ToLower() == "totaldown")
                        {
                            if (mRow[ColumnName[i]].ToString() == "")
                            {
                                arr_ListPara.Add("0");
                                continue;
                            }
                        }
                        //Nếu Data không chứa Column này thì lấy chính tên Column thay giá trị
                        if (Data.Columns.Contains(ColumnName[i]))
                        {
                            //Nếu column có định dạng là ngày tháng thì lấy kiểu định dạng là ngày tháng.
                            if (Data.Columns[ColumnName[i]].DataType == typeof(DateTime))
                            {
                                arr_ListPara.Add(((DateTime)mRow[ColumnName[i]]).ToString(MyConfig.ViewDateFormat));
                            }
                            else
                            {
                                arr_ListPara.Add(mRow[ColumnName[i]].ToString());
                            }
                        }
                        else
                        {
                            arr_ListPara.Add(ColumnName[i]);
                        }
                    }

                    //Ứng với mỗi dòng sẽ load 1 template
                    mBuilder.Append(string.Format(strTemplate, arr_ListPara.ToArray()));
                }

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Load Tempate theo tên Column
        /// </summary>
        /// <param name="TemplatePath">Tên file Template</param>
        /// <param name="Data"></param>
        /// <param name="ColumnName">Mảng các tên Column cần lấy dữ liệu</param>
        /// <param name="IsUrl">True nếu là đường dẫn URL full</param>
        /// <returns></returns>
        public string LoadTemplateByColumnName(string TemplatePath, DataTable Data, string[] ColumnName, bool IsUrl)
        {
            try
            {
                if (ColumnName.Length < 1 || Data == null || Data.Columns.Count < ColumnName.Length)
                    throw new Exception("Số lượng item trong ColumnIndex truyền vào là không phù hợp");

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                //Load Template vào 1 chuỗi
                string strTemplate = string.Empty;
                if (IsUrl)
                {
                    strTemplate = MyFile.ReadContentFromURL(TemplatePath);
                }
                else
                {
                    TemplatePath = MyFile.GetFullPathFile(TemplatePath);

                    if (!MyFile.CheckExistFile(ref TemplatePath))
                        throw new Exception("File Template không tồn tại.");

                    strTemplate = MyFile.ReadFile(TemplatePath);
                }

                //Tạo mảng lưu trữ các tham số của 1 Row trong Table
                System.Collections.ArrayList arr_ListPara = new System.Collections.ArrayList();

                foreach (DataRow mRow in Data.Rows)
                {
                    arr_ListPara = new System.Collections.ArrayList();

                    //Lấy dữ liệu từ tất cả các cột của dòng hiện tại
                    for (int i = 0; i < ColumnName.Length; i++)
                    {

                        //if (ColumnName[i].ToLower() == "imagepath")
                        //{
                        //    arr_ListPara.Add(MyFile.GetFullResourceLink(mRow[ColumnName[i]].ToString()));
                        //    continue;
                        //}
                        // Kien DT -> Nếu total down là DBNull thì trả về 0
                        if (ColumnName[i].ToLower() == "totaldown")
                        {
                            if (mRow[ColumnName[i]].ToString() == "")
                            {
                                arr_ListPara.Add("0");
                                continue;
                            }
                        }
                        //Nếu Data không chứa Column này thì lấy chính tên Column thay giá trị
                        if (Data.Columns.Contains(ColumnName[i]))
                        {
                            //Nếu column có định dạng là ngày tháng thì lấy kiểu định dạng là ngày tháng.
                            if (Data.Columns[ColumnName[i]].DataType == typeof(DateTime))
                            {
                                arr_ListPara.Add(((DateTime)mRow[ColumnName[i]]).ToString(MyConfig.ViewDateFormat));
                            }
                            else
                            {
                                arr_ListPara.Add(mRow[ColumnName[i]].ToString());
                            }
                        }
                        else
                        {
                            arr_ListPara.Add(ColumnName[i]);
                        }
                    }

                    //Ứng với mỗi dòng sẽ load 1 template
                    mBuilder.Append(string.Format(strTemplate, arr_ListPara.ToArray()));
                }

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string LoadTemplateByColumnName(string TemplatePath, DataTable Data, string[] ColumnName, string[] ColumnPrefix)
        {
            try
            {
                if (ColumnName.Length < 1 || Data == null || Data.Columns.Count < ColumnName.Length)
                    throw new Exception("Số lượng item trong ColumnIndex truyền vào là không phù hợp");
                if (ColumnPrefix.Length != ColumnName.Length)
                    throw new Exception("Số lượng item trong ColumnIndex,ColumnPrefix truyền vào là không phù hợp");

                TemplatePath = MyFile.GetFullPathFile(TemplatePath);

                if (!MyFile.CheckExistFile(ref TemplatePath))
                    throw new Exception("File Template không tồn tại.");

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                //Load Template vào 1 chuỗi
                string strTemplate = MyFile.ReadFile(TemplatePath);

                //Replace Domain trước khi Format vì sẽ gấy ra lỗi.
                //VD: Replace {DNS} thanh http://funzone.vn
                strTemplate = strTemplate.Replace(MyConfig.DomainParameter, MyConfig.Domain);

                //Tạo mảng lưu trữ các tham số của 1 Row trong Table
                System.Collections.ArrayList arr_ListPara = new System.Collections.ArrayList();

                foreach (DataRow mRow in Data.Rows)
                {
                    arr_ListPara = new System.Collections.ArrayList();

                    //Lấy dữ liệu từ tất cả các cột của dòng hiện tại
                    for (int i = 0; i < ColumnName.Length; i++)
                    {
                        //if (ColumnName[i].ToLower() == "imagepath")
                        //{
                        //    arr_ListPara.Add(MyFile.GetFullResourceLink(mRow[ColumnName[i]].ToString()));
                        //    continue;
                        //}
                        // Kien DT -> Nếu total down là DBNull thì trả về 0
                        if (ColumnName[i].ToLower() == "totaldown")
                        {
                            if (mRow[ColumnName[i]].ToString() == "")
                            {
                                arr_ListPara.Add("0");
                                continue;
                            }
                        }
                        //Nếu Data không chứa Column này thì lấy chính tên Column thay giá trị
                        if (Data.Columns.Contains(ColumnName[i]))
                        {
                            //Nếu column có định dạng là ngày tháng thì lấy kiểu định dạng là ngày tháng.
                            if (Data.Columns[ColumnName[i]].DataType == typeof(DateTime))
                            {
                                arr_ListPara.Add(ColumnPrefix[i] + ((DateTime)mRow[ColumnName[i]]).ToString(MyConfig.ViewDateFormat));
                            }
                            else
                            {
                                arr_ListPara.Add(ColumnPrefix[i] + mRow[ColumnName[i]].ToString());
                            }

                        }
                        else
                        {
                            arr_ListPara.Add(ColumnPrefix[i] + ColumnName[i]);
                        }
                    }

                    //Ứng với mỗi dòng sẽ load 1 template
                    mBuilder.Append(string.Format(strTemplate, arr_ListPara.ToArray()));
                }

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string LoadTemplateByColumnName(string TemplatePath, DataTable Data, string[] ColumnName, string[] ColumnPrefix, bool IsUrl)
        {
            try
            {
                if (ColumnName.Length < 1 || Data == null || Data.Columns.Count < ColumnName.Length)
                    throw new Exception("Số lượng item trong ColumnIndex truyền vào là không phù hợp");
                if (ColumnPrefix.Length != ColumnName.Length)
                    throw new Exception("Số lượng item trong ColumnIndex,ColumnPrefix truyền vào là không phù hợp");

                StringBuilder mBuilder = new StringBuilder(string.Empty);

                //Load Template vào 1 chuỗi
                string strTemplate = string.Empty;
                if (IsUrl)
                {
                    strTemplate = MyFile.ReadContentFromURL(TemplatePath);
                }
                else
                {
                    TemplatePath = MyFile.GetFullPathFile(TemplatePath);

                    if (!MyFile.CheckExistFile(ref TemplatePath))
                        throw new Exception("File Template không tồn tại.");

                    strTemplate = MyFile.ReadFile(TemplatePath);
                }

                //Tạo mảng lưu trữ các tham số của 1 Row trong Table
                System.Collections.ArrayList arr_ListPara = new System.Collections.ArrayList();

                foreach (DataRow mRow in Data.Rows)
                {
                    arr_ListPara = new System.Collections.ArrayList();

                    //Lấy dữ liệu từ tất cả các cột của dòng hiện tại
                    for (int i = 0; i < ColumnName.Length; i++)
                    {
                        //if (ColumnName[i].ToLower() == "imagepath")
                        //{
                        //    arr_ListPara.Add(MyFile.GetFullResourceLink(mRow[ColumnName[i]].ToString()));
                        //    continue;
                        //}
                        // Kien DT -> Nếu total down là DBNull thì trả về 0
                        if (ColumnName[i].ToLower() == "totaldown")
                        {
                            if (mRow[ColumnName[i]].ToString() == "")
                            {
                                arr_ListPara.Add("0");
                                continue;
                            }
                        }
                        //Nếu Data không chứa Column này thì lấy chính tên Column thay giá trị
                        if (Data.Columns.Contains(ColumnName[i]))
                        {
                            //Nếu column có định dạng là ngày tháng thì lấy kiểu định dạng là ngày tháng.
                            if (Data.Columns[ColumnName[i]].DataType == typeof(DateTime))
                            {
                                arr_ListPara.Add(ColumnPrefix[i] + ((DateTime)mRow[ColumnName[i]]).ToString(MyConfig.ViewDateFormat));
                            }
                            else
                            {
                                arr_ListPara.Add(ColumnPrefix[i] + mRow[ColumnName[i]].ToString());
                            }

                        }
                        else
                        {
                            arr_ListPara.Add(ColumnPrefix[i] + ColumnName[i]);
                        }
                    }

                    //Ứng với mỗi dòng sẽ load 1 template
                    mBuilder.Append(string.Format(strTemplate, arr_ListPara.ToArray()));
                }

                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    
    }
}
