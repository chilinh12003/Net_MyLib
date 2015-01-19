using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
namespace MyLib.MyUtility
{
    public class MyImage
    {
        /// <summary>
        /// Lấy full đường dẫn ảnh (đã bao gồm cả domain)
        /// </summary>
        /// <param name="mProductRow">Row của Product cần lầy</param>
        /// <param name="GetImageOrder">Thứ tự của Column cần lấy (ImagePath_1 --> GetImageOrder = 1</param>
        /// <returns></returns>
        public static string GetFullPathImage(DataRow mProductRow, int GetImageOrder)
        {
            try
            {
                string FullPath =MyConfig.DefaultImagePath;

                switch (GetImageOrder)
                {
                    case 1: //Lấy hình ảnh từ Column ImagePath_1
                        if (mProductRow["ImagePath_1"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_1"].ToString();
                        else if (mProductRow["ImagePath_2"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_2"].ToString();

                        break;
                    case 2: //Lấy hình ảnh từ Column ImagePath_2
                        
                        if (mProductRow["ImagePath_2"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_2"].ToString();
                        else if (mProductRow["ImagePath_1"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_1"].ToString();
                        break;
                    case 3: //Lấy hình ảnh từ Column ImagePath_3

                    default:
                        if (mProductRow["ImagePath_2"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_2"].ToString();
                        else if (mProductRow["ImagePath_1"] != DBNull.Value)
                            FullPath = mProductRow["ImagePath_1"].ToString();
                        break;
                }

                FullPath = MyConfig.ResourceLink + FullPath.Replace("~", "").Replace("..", "");
                return FullPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
