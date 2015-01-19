using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace MyLib.MyUtility
{
    /// <summary>
    /// Thư viện xử lý XML
    /// </summary>
    public class MyXML
    {
        /// <summary>
        /// Đọc dữ liệu XML từ URL
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        public static DataSet ReadXMLFromURL(string URL)
        {
            try
            {
                DataSet mDataSet = new DataSet();
                System.Xml.XmlTextReader mReader = new System.Xml.XmlTextReader(URL);

                //Đọc dữ liệu XML lên DataSet
                mDataSet.ReadXml(mReader);
                return mDataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy dữ liệu từ 1 file XML
        /// </summary>
        /// <param name="FileName">Đường dẫn tuyệt đối tới File XML</param>
        /// <returns></returns>
        public static DataSet GetXMLData(string FileName)
        {
            try
            {
                System.Xml.XmlTextReader mReader = new System.Xml.XmlTextReader(FileName);
                DataSet mDataSet = new DataSet();

                mDataSet.ReadXml(mReader);
                mReader.Close();
                return mDataSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Lấy dữ liệu từ 1 chuỗi XML truyền vào
        /// </summary>
        /// <param name="XMLContent"></param>
        /// <returns></returns>
        public static DataSet GetDataSetFromXMLString(string XMLContent)
        {
            try
            {
                System.IO.StringReader mReader = new System.IO.StringReader(XMLContent);
                DataSet mSet = new DataSet();
                mSet.ReadXml(mReader);
                return mSet;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy cấu trú dataset từ 1 chuỗi XMLSChema
        /// </summary>
        /// <param name="XMLSchema"></param>
        /// <returns></returns>
        public static DataSet GetDataSetFromXMLSchemaString(string XMLSchema)
        {
            try
            {
                System.IO.StringReader mReader = new System.IO.StringReader(XMLSchema);
                DataSet mSet = new DataSet();
                mSet.ReadXmlSchema(mReader);
                return mSet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Tạo 1 DataSet từ DataTable.
        /// </summary>
        /// <param name="mTable"></param>
        /// <returns></returns>
        public static DataSet CreateDataSet(DataTable mTable)
        {
            DataSet mSet = new DataSet("Parrent");
            mTable.TableName = "Child";
            mSet.Tables.Add(mTable);
            mSet.AcceptChanges();
            return mSet;

        }

        /// <summary>
        /// Lấy XML từ 1 datatable (Cấu trúc là XML là Parent --> Child
        /// </summary>
        /// <param name="mTable"></param>
        /// <returns></returns>
        public static string GetXML(DataTable mTable)
        {
            DataSet mSet = new DataSet("Parrent");
            mTable.TableName = "Child";
            mSet.Tables.Add(mTable.Copy());
            mSet.AcceptChanges();
            MyConvert.ConvertDateColumnToStringColumn(ref mSet);
            mSet.AcceptChanges();

            return mSet.GetXml();
        }
    }
}
