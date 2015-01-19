using System;
using System.Collections.Generic;
using System.Text;

namespace MyLib.MyUtility
{
    /// <summary>
    /// Lớp xử lý các thông tin về Partition ID
    /// </summary>
    public class MyPID
    {
        /// <summary>
        /// Lấy PID thông qua msisdn, khoảng của PID = [0,19];
        /// </summary>
        /// <param name="MSISDN"></param>
        /// <returns></returns>
        public static int GetPIDByPhoneNumber(string MSISDN, int MaxPID)
        {
            try
            {
                MyConfig.Telco mTelco = MyConfig.Telco.Nothing;
                if (!MyCheck.CheckPhoneNumber(ref MSISDN, ref mTelco, string.Empty))
                {
                    return 1;
                }
                string Number = "1";
                if (MSISDN.StartsWith("9"))
                {
                    Number = MSISDN.Substring(2, 2);
                }
                else if (MSISDN.StartsWith("1"))
                {
                    Number = MSISDN.Substring(3, 2);
                }

                int iNumber = 1;
                int.TryParse(Number, out iNumber);

                return iNumber % MaxPID + 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mDate"></param>
        /// <returns></returns>
        public static int GetPIDByDate(DateTime mDate)
        {
            try
            {
                return mDate.Month;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// Lấy 1 danh sách các PID theo ngày tháng
        /// </summary>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        public static List<int> GetPartionID(DateTime FromDate, DateTime ToDate)
        {
            try
            {
                List<int> PID = new List<int>();
                DateTime Temp = new DateTime(FromDate.Year, FromDate.Month, FromDate.Day);
                while (Temp <= ToDate)
                {
                    PID.Add(Temp.Month);
                    Temp.AddMonths(1);
                }
                return PID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
    
    }
}
