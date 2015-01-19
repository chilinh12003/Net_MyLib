using System;
using System.Data;
using System.ComponentModel;
using System.Reflection;

namespace MyLib.MyUtility
{
    /// <summary>
    /// Chứa các hàm xử lý Enum
    /// </summary>
    public class MyEnum
    {
        /// <summary>
        /// Lấy giá trị là chuổi của 1 Enum mà có phần mô tả
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string StringValueOf(System.Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            else
            {
                return value.ToString();
            }
        }

        /// <summary>
        /// Lấy giá trị của Enum
        /// </summary>
        /// <param name="value"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static object EnumValueOf(string value, Type enumType)
        {
            string[] names = Enum.GetNames(enumType);
            foreach (string name in names)
            {
                if (StringValueOf((Enum)Enum.Parse(enumType, name)).Equals(value))
                {
                    return Enum.Parse(enumType, name);
                }
            }

            throw new ArgumentException("The string is not a description or value of the specified enum.");
        }

        /// <summary>
        /// Tạo ra 1 datatable(ID,Text) từ kiều enum đưa vào
        /// </summary>
        /// <param name="enumType">typeof(1 enum nào đó)</param>
        /// <returns></returns>
        public static DataTable CrateDatasourceFromEnum(Type enumType)
        {
            try
            {

                DataTable tbl_Data = new DataTable();
                DataColumn col_ID = new DataColumn("ID");
                DataColumn col_Text = new DataColumn("Text");
                tbl_Data.Columns.Add(col_ID);
                tbl_Data.Columns.Add(col_Text);
                tbl_Data.AcceptChanges();
                string[] names = Enum.GetNames(enumType);
                foreach (string name in names)
                {
                    DataRow mRow = tbl_Data.NewRow();
                    mRow["Text"] = StringValueOf((Enum)Enum.Parse(enumType, name));
                    mRow["ID"] = (int)EnumValueOf(mRow["Text"].ToString(), enumType);
                    tbl_Data.Rows.Add(mRow);
                }
                tbl_Data.AcceptChanges();

                return tbl_Data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumType"></param>
        /// <param name="NotGetZeroItem">= true: không lấy giá trị là 0</param>
        /// <returns></returns>
        public static DataTable CrateDatasourceFromEnum(Type enumType, bool NotGetZeroItem)
        {
            try
            {

                DataTable tbl_Data = new DataTable();
                DataColumn col_ID = new DataColumn("ID");
                DataColumn col_Text = new DataColumn("Text");
                tbl_Data.Columns.Add(col_ID);
                tbl_Data.Columns.Add(col_Text);
                tbl_Data.AcceptChanges();
                string[] names = Enum.GetNames(enumType);
                foreach (string name in names)
                {
                    int ID = 0;
                    DataRow mRow = tbl_Data.NewRow();
                    mRow["Text"] = StringValueOf((Enum)Enum.Parse(enumType, name));
                    mRow["ID"] = ID=(int)EnumValueOf(mRow["Text"].ToString(), enumType);
                    if (NotGetZeroItem && ID == 0)
                        continue;
                    tbl_Data.Rows.Add(mRow);
                }
                tbl_Data.AcceptChanges();

                return tbl_Data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataTable CrateDatasourceFromEnum_IDIsString(Type enumType)
        {
            try
            {
                DataTable tbl_Data = new DataTable();
                DataColumn col_ID = new DataColumn("ID");
                DataColumn col_Text = new DataColumn("Text");
                tbl_Data.Columns.Add(col_ID);
                tbl_Data.Columns.Add(col_Text);
                tbl_Data.AcceptChanges();
                string[] names = Enum.GetNames(enumType);
                foreach (string name in names)
                {
                    DataRow mRow = tbl_Data.NewRow();
                    mRow["Text"] = StringValueOf((Enum)Enum.Parse(enumType, name));
                    mRow["ID"] = ((Enum)Enum.Parse(enumType, name)).ToString();
                    tbl_Data.Rows.Add(mRow);
                }
                tbl_Data.AcceptChanges();

                return tbl_Data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Khởi tạo dữ liệu cho cho Combobox Column (ID,Text)
        /// <para>VD: lấy dữ liệu về Giờ (từ 0-23h) cho combobix</para>
        /// </summary>
        /// <param name="Type">Lấy kiểu dữ liệu nào
        /// <para>Type = 1: Lấy dữ liệu cho Tháng</para>
        /// <para>Type = 2: Lấy dữ liệu cho Ngày của tháng hiện tại</para>
        /// <para>Type = 3: Lấy dữ liệu cho Giờ</para>
        /// <para>Type = 4: Lấy dữ liệu cho Phút</para>
        /// <para>Type = 5: Lấy dữ liệu cho Giây</para>
        /// </param>
        /// <returns></returns>
        public static DataTable GetDataFromTime(int Type,string Prefix,string Suffix)
        {
            try
            {
                DataTable mTable = new DataTable();
                DataColumn col_ID = new DataColumn("ID", typeof(int));
                DataColumn col_Text = new DataColumn("Text", typeof(string));
                mTable.Columns.Add(col_ID);
                mTable.Columns.Add(col_Text);

                int Begin = 0;
                int End = 0;
                
                switch (Type)
                {
                    case 1: //Tháng
                        Begin = 1;
                        End = 12;
                        
                        break;
                    case 2: //Ngày
                        Begin = 1;
                        End = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);                        
                        break;
                    case 3: //Giờ
                        Begin = 0;
                        End = 23;                       
                        break;
                    case 4: //Phút
                        Begin = 1;
                        End = 60;
                       
                        break;
                    case 5: //Giây
                        Begin = 1;
                        End = 60;
                        break;
                }

                for (int i = Begin; i <= End; i++)
                {
                    DataRow mRow = mTable.NewRow();
                    mRow["ID"] = i;
                    mRow["Text"] = Prefix + i.ToString() + Suffix;
                    mTable.Rows.Add(mRow);
                }
                mTable.AcceptChanges();
                return mTable;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
