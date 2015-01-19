using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Globalization;
namespace MyLib.MyUtility
{
    public class MyConvert
    {
        public static DateTime StringToDateTime(string Content, string Format)
        {
            try
            {
                IFormatProvider mProvider  = new System.Globalization.CultureInfo("VI-vn").DateTimeFormat;

                return DateTime.ParseExact(Content, Format, mProvider);
            }
            catch
            {
                return DateTime.MinValue;
            }

        }

        /// <summary>
        /// Chuyển chuỗi chữ có dấu thành không dấu
        /// </summary>
        /// <param name="sString"></param>
        /// <returns></returns>
        public static string RemoveVietnameseString(string sString)
        {
            string sReturn;
            string sStandVN = "áàạảãâấầậẩẫăắằặẳẵÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴéèẹẻẽêếềệểễÉÈẸẺẼÊẾỀỆỂỄóòọỏõôốồộổỗơớờợởỡÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠúùụủũưứừựửữÚÙỤỦŨƯỨỪỰỬỮíìịỉĩÍÌỊỈĨđĐýỳỵỷỹÝỲỴỶỸ";
            string sStandEN = "aaaaaaaaaaaaaaaaaAAAAAAAAAAAAAAAAAeeeeeeeeeeeEEEEEEEEEEEoooooooooooooooooOOOOOOOOOOOOOOOOOuuuuuuuuuuuUUUUUUUUUUUiiiiiIIIIIdDyyyyyYYYYY";
            sReturn = sString;
            for (int i = 0; i < sStandEN.Length; i++)
            {
                sReturn = sReturn.Replace(sStandVN[i], sStandEN[i]);
            }
            return sReturn;
        }

        /// <summary>
        /// Chuyển kiểu bool sang bit
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static int ConvertBooleanToBit(bool o)
        {
            if (o)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// chuyển đối tượng kiểu bit sang kiểu bool
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static bool ConvertBitToBoolean(object o)
        {
            if (string.IsNullOrEmpty(o.ToString()))
            {
                return false;
            }
            else if (o.ToString().ToUpper()=="TRUE")
            {
                return true;
            }
            else{
                return false;
            }
        }

        /// <summary>
        /// Xóa bỏ các trường Datetime có trong Dataset và convert trường đó thành trường String trước khi xuất ra XML.
        /// Nhằm đảm bảo chính xác trước khi insert xuống database
        /// </summary>
        /// <param name="mSet">DataSet cần convert</param>
        /// <returns></returns>
        public static bool ConvertDateColumnToStringColumn(ref DataSet mSet)
        {
            try
            {
                if (mSet == null || mSet.Tables.Count < 1)
                    return true;

                List<string> mList_TableName = new List<string>();
                List<string> mList_ColumName = new List<string>();

                //lấy danh sách các tên table, tên cột là kiểu dữ liệu DateTime.
                foreach (DataTable mTable in mSet.Tables)
                {
                    foreach (DataColumn mCol in mTable.Columns)
                    {
                        if (mCol.DataType == typeof(DateTime))
                        {
                            mList_TableName.Add(mTable.TableName);
                            mList_ColumName.Add(mCol.ColumnName);
                        }
                    }
                }
                //Thêm các column mới vào table
                for (int i = 0; i < mList_TableName.Count; i++)
                {
                    mSet.Tables[mList_TableName[i]].Columns.Add(mList_ColumName[i] + "_Temp");
                }

                //Copy dữ liệu từ column thật sang column tạm
                for (int i = 0; i < mList_TableName.Count; i++)
                {
                    foreach (DataRow mRow in mSet.Tables[mList_TableName[i]].Rows)
                    {
                        if (mRow[mList_ColumName[i]] == DBNull.Value)
                        {
                            mRow[mList_ColumName[i] + "_Temp"] = DBNull.Value;
                        }
                        else
                        {
                            mRow[mList_ColumName[i] + "_Temp"] = ((DateTime)mRow[mList_ColumName[i]]).ToString(MyConfig.DateFormat_InsertToDB);
                        }
                    }
                }

                //Xóa các cột dữ liệu dích và đổi tên các cột dữ liệu ngồn
                for (int i = 0; i < mList_TableName.Count; i++)
                {
                    mSet.Tables[mList_TableName[i]].Columns.Remove(mList_ColumName[i]);
                    mSet.Tables[mList_TableName[i]].Columns[mList_ColumName[i] + "_Temp"].ColumnName = mList_ColumName[i];
                }
                mSet.AcceptChanges();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Chuyển dữ liệu của Datarow Thành dữ liệu Search, ứng với các trường đưa vào
        /// </summary>
        /// <param name="mRow"></param>
        /// <param name="ColumnName"></param>
        /// <returns></returns>
        public static string ConvertDataToSearchText(DataRow mRow, string[] ColumnName)
        {
            try
            {
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                for (int i = 0; i < ColumnName.Length; i++)
                {
                    if (mRow.Table.Columns[ColumnName[i]].DataType == typeof(string))
                    {
                        string Temp_vietnamese = mRow[ColumnName[i]].ToString();
                        string Temp_NoSign = MyText.RemoveSignVietnameseString(mRow[ColumnName[i]].ToString());
                        string Temp_NoSpace = Temp_NoSign.Replace(" ", "");

                        if (Temp_vietnamese.Length < 100 && Temp_vietnamese == Temp_NoSign && Temp_NoSign == Temp_NoSpace)
                        {
                            mBuilder.Append(ColumnName[i] + "=" + Temp_vietnamese + " , ");
                        }
                        else
                        {
                            mBuilder.Append(ColumnName[i] + "=" + Temp_vietnamese + " , ");
                            mBuilder.Append(ColumnName[i] + "=" + Temp_NoSign + " , ");
                            mBuilder.Append(ColumnName[i] + "=" + Temp_NoSpace + " , ");
                        }
                    }
                    else if (mRow.Table.Columns[ColumnName[i]].DataType == typeof(int) ||
                                mRow.Table.Columns[ColumnName[i]].DataType == typeof(float) ||
                            mRow.Table.Columns[ColumnName[i]].DataType == typeof(double))
                    {
                        mBuilder.Append(ColumnName[i] + "=" + mRow[ColumnName[i]].ToString() + " , ");
                    }
                }
                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static string ConvertDataToSearchText(int Type, DataRow mRow, string[] ColumnName)
        {
            try
            {
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                for (int i = 0; i < ColumnName.Length; i++)
                {
                    if (mRow.Table.Columns[ColumnName[i]].DataType == typeof(string))
                    {
                        string Temp_vietnamese = mRow[ColumnName[i]].ToString();
                        string Temp_NoSign = MyText.RemoveSignVietnameseString(mRow[ColumnName[i]].ToString());
                        string Temp_NoSpace = Temp_NoSign.Replace(" ", "");

                        if (Temp_vietnamese.Length < 100 && Temp_vietnamese == Temp_NoSign && Temp_NoSign == Temp_NoSpace)
                        {
                            mBuilder.Append(ColumnName[i] + "=" + Temp_vietnamese + " , ");
                        }
                        else
                        {
                            mBuilder.Append(ColumnName[i] + "=" + Temp_vietnamese + " , ");
                            mBuilder.Append(ColumnName[i] + "=" + Temp_NoSign + " , ");
                            mBuilder.Append(ColumnName[i] + "=" + Temp_NoSpace + " , ");
                        }
                    }
                    else if (mRow.Table.Columns[ColumnName[i]].DataType == typeof(int) ||
                                mRow.Table.Columns[ColumnName[i]].DataType == typeof(float) ||
                            mRow.Table.Columns[ColumnName[i]].DataType == typeof(double))
                    {
                        mBuilder.Append(ColumnName[i] + "=" + mRow[ColumnName[i]].ToString() + " , ");
                    }
                }
                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Chuyển 1 datarow thành 1 mảng chuổi
        /// </summary>
        /// <param name="mRow"></param>
        /// <returns></returns>
        public static string[] ConvertDataRowToArray(DataRow mRow)
        {
            try
            {
                string[] Arr = new string[mRow.Table.Columns.Count];
                int i = 0;
                foreach (DataColumn mCol in mRow.Table.Columns)
                {
                    if (mRow[mCol.ColumnName] == DBNull.Value)
                    {
                        Arr[i++] = string.Empty;
                        continue;
                    }
                    if (mCol.DataType == typeof(DateTime))
                    {
                        Arr[i++] = ((DateTime)mRow[mCol.ColumnName]).ToString(MyConfig.DateFormat_InsertToDB);
                    }
                    else
                    {
                        Arr[i++] = mRow[mCol.ColumnName].ToString();
                    }
                }

                return Arr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static string[] ConvertDataRowToArray(DataRow mRow, string[] ColumnName)
        {
            try
            {
                string[] Arr = new string[ColumnName.Length];
                int i = 0;
                for (int k = 0; k < ColumnName.Length; k++)
                {
                    if (mRow[ColumnName[k]] == DBNull.Value)
                    {
                        Arr[i++] = string.Empty;
                        continue;
                    }
                    if (mRow.Table.Columns[ColumnName[k]].DataType == typeof(DateTime))
                    {
                        Arr[i++] = ((DateTime)mRow[ColumnName[k]]).ToString(MyConfig.DateFormat_InsertToDB);
                    }
                    else
                    {
                        Arr[i++] = mRow[ColumnName[k]].ToString();
                    }
                }

                return Arr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Chuyển datarow sang dạng logfile
        /// </summary>
        /// <param name="mRow"></param>
        /// <returns></returns>
        public static string ConvertDataRowToStringLog(DataRow mRow)
        {
            try
            {
                StringBuilder mBuilder = new StringBuilder(string.Empty);
                int Count = 0;
                foreach(DataColumn mCol in mRow.Table.Columns)
                {
                    if (Count++ != 0)
                        mBuilder.Append(" || ");
                    mBuilder.Append(mCol.ColumnName + ":" + mRow[mCol.ColumnName].ToString());
                }
                return mBuilder.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Lấy ngày đầu tuần của ngày hiện tại dưa vào.
        /// </summary>
        /// <param name="Current"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfWeek(DateTime Current)
        {
            try
            {
                DayOfWeek CurrentDay = Current.DayOfWeek;
                switch (CurrentDay)
                {
                    case DayOfWeek.Monday:

                        break;
                    case DayOfWeek.Tuesday:
                        Current = Current.AddDays(-1);
                        break;
                    case DayOfWeek.Wednesday:
                        Current = Current.AddDays(-2);
                        break;
                    case DayOfWeek.Thursday:
                        Current = Current.AddDays(-3);
                        break;
                    case DayOfWeek.Friday:
                        Current = Current.AddDays(-4);
                        break;
                    case DayOfWeek.Saturday:
                        Current = Current.AddDays(-5);
                        break;
                    case DayOfWeek.Sunday:
                        Current = Current.AddDays(-6);
                        break;
                }

                return new DateTime(Current.Year, Current.Month, Current.Day);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy ngày đầu tiên của 1 tuần
        /// </summary>
        /// <param name="year"></param>
        /// <param name="weekNum"></param>
        /// <returns></returns>
        public static DateTime GetFirstDayOfWeek(int year, int weekNum)
        {

            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);           
        }

        /// <summary>
        /// lấy ngày cuối cùng của tuần trong năm
        /// </summary>
        /// <param name="year"></param>
        /// <param name="weekNum"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfWeek(int year, int weekNum)
        {

            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(3);
        }

        /// <summary>
        /// lấy giá trị của tuần trong năm
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int GetWeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        } 

        /// <summary>
        /// Chuyển 1 List Object sang 1 table
        /// </summary>
        /// <param name="ListObjects"></param>
        /// <returns></returns>
        public static DataTable ConvertObject2Table<T>(List<T> ListObjects)
        {
            try
            {
                if (ListObjects != null && ListObjects.Count > 0)
                {
                    Type mType = ListObjects[0].GetType();
                    DataTable mTable = new DataTable(mType.Name);
                   
                    foreach (PropertyInfo mPro in mType.GetProperties(BindingFlags.SetProperty))
                    {
                        //nếu là enum thì ko xử lý
                        if (mPro.GetType().IsEnum)
                            continue;

                        mTable.Columns.Add(new DataColumn(mPro.Name, mPro.PropertyType));

                    }
                    foreach (FieldInfo mField in mType.GetFields())
                    {
                        //nếu là enum thì ko xử lý
                        if (mField.GetType().IsEnum)
                            continue;

                        mTable.Columns.Add(new DataColumn(mField.Name,mField.FieldType));
                    }
                    foreach (var o in ListObjects)
                    {
                        DataRow mRow = mTable.NewRow();
                        foreach (DataColumn dc in mTable.Columns)
                        {
                            if(o.GetType().GetProperty(dc.ColumnName) != null)
                            mRow[dc.ColumnName] = o.GetType().GetProperty(dc.ColumnName).GetValue(o, null);

                            if (o.GetType().GetField(dc.ColumnName) != null)
                                mRow[dc.ColumnName] = o.GetType().GetField(dc.ColumnName).GetValue(o);

                        }
                        mTable.Rows.Add(mRow);
                    }
                    return mTable;
                }

                return new DataTable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Chuyển 1 datatbale thành 1 List Object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mTable"></param>
        /// <returns></returns>
        public static List<T> ConvertTable2Object<T>(DataTable mTable)
        {
            try
            {
                List<T> mList = new List<T>();
                Type mType = typeof(T);
                foreach (DataRow mRow in mTable.Rows)
                {
                    var mT = (T)Activator.CreateInstance(mType);

                    foreach (PropertyInfo mPro in mType.GetProperties(BindingFlags.SetProperty))
                    {
                        if (mTable.Columns.Contains(mPro.Name) && mRow[mPro.Name] != DBNull.Value)
                            mPro.SetValue(mT, mRow[mPro.Name], null);
                    }
                    foreach (FieldInfo mFiel in mType.GetFields())
                    {
                        if (mTable.Columns.Contains(mFiel.Name) && mRow[mFiel.Name] != DBNull.Value)
                            mFiel.SetValue(mT, mRow[mFiel.Name]);
                    }
                    mList.Add(mT);
                }
                return mList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
    }
}
