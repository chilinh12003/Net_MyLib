using System;
using System.Collections.Generic;
using System.Text;

namespace MyLib.MyUtility
{
    /// <summary>
    /// Lớp dành cho phân trang
    /// </summary>
    [Serializable()]
    public class MyPaging
    {
        /// <summary>
        /// Phân trang
        /// </summary>
        public enum PagingType
        {
            /// <summary>
            /// Nhảy về trước tới trang đầu tiên
            /// </summary>
            First = 1,
            /// <summary>
            /// Nhảy về trước 1 bước
            /// </summary>
            Previous = 2,
            /// <summary>
            /// Nhay về trước với số bước nhập vào
            /// </summary>
            SlidePrev = 3,
            /// <summary>
            /// Nháy tơi 1 vị trí nhất định nào đó
            /// </summary>
            Slide = 4,
            /// <summary>
            /// Nháy về sau 1 bước
            /// </summary>
            Next = 5,
            /// <summary>
            /// Nhảy về sau với số bước nhập vào
            /// </summary>
            SlideNext = 6,
            /// <summary>
            /// Nhảy về sau tới trang cuối cùng
            /// </summary>
            Last = 7

        }

        public int TotalRow = 0;
        public int TotalPage = 0;
        public int CurrentPageIndex = 0;
        public int PageSize = 5;

        public int BeginRow = 1;
        public int EndRow = 10;

        public int Page_1 = 1;
        public int Page_2 = 2;
        public int Page_3 = 3;

        public bool EnableFirst = true;
        public bool EnablePrevious = true;
        public bool EnableSlidePrev = true;
        public bool EnablePage_1 = true;
        public bool EnablePage_2 = true;
        public bool EnablePage_3 = true;
        public bool EnableSlideNext = true;
        public bool EnableNext = true;
        public bool EnableLast = true;

        public void SetToTalPage()
        {
            if (TotalRow % PageSize == 0)
                TotalPage = TotalRow / PageSize;
            else
                TotalPage = (int)(TotalRow/PageSize) + 1;

            if (CurrentPageIndex > TotalPage)
                CurrentPageIndex = TotalPage;

            if (CurrentPageIndex < 1)
                CurrentPageIndex = 1;

            if (CurrentPageIndex >= 3)
            {
                Page_3 = CurrentPageIndex;
                Page_2 = CurrentPageIndex - 1;
                Page_1 = CurrentPageIndex - 2;
            }
            else
            {
                Page_1 = 1;
                Page_2 = 2;
                Page_3 = 3;
            }
            BeginRow = (CurrentPageIndex - 1) * PageSize + 1;
            EndRow = (CurrentPageIndex - 1) * PageSize + PageSize;

        }

        public MyPaging()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public void CheckStatus()
        {
            EnableFirst = true;
            EnablePrevious = true;
            EnableSlidePrev = true;
            EnablePage_1 = true;
            EnablePage_2 = true;
            EnablePage_3 = true;
            EnableSlideNext = true;
            EnableNext = true;
            EnableLast = true;

            if (TotalPage <= 3)
            {
                EnableSlideNext = false;
                EnableSlidePrev = false;
            }

            if (CurrentPageIndex == 1)
            {
                EnableFirst = false;
                EnablePrevious = false;
                EnableSlidePrev = false;
            }

            if (CurrentPageIndex == TotalPage)
            {
                EnableSlideNext = false;
                EnableNext = false;
                EnableLast = false;
            }
            if (Page_1 == 1)
            {
                EnableSlidePrev = false;
            }

            if (Page_3 == TotalPage)
            {
                EnableSlideNext = false;
            }
            if (TotalPage < Page_3)
                EnablePage_3 = false;

            if (TotalPage < Page_2)
                EnablePage_2 = false;

            if (TotalPage <= 1)
            {
                EnableFirst = false;
                EnablePrevious = false;
                EnableSlidePrev = false;
                EnablePage_1 = false;
                EnablePage_2 = false;
                EnablePage_3 = false;
                EnableSlideNext = false;
                EnableNext = false;
                EnableLast = false;
            }
        }

        public void PagingSlide(PagingType mType, string ThamSo)
        {
            try
            {
                
                switch (mType)
                {
                    case PagingType.First:
                        #region MyRegion
                        CurrentPageIndex = 1;
                        Page_1 = 1;
                        Page_2 = 2;
                        Page_3 = 3;
                        break; 
                        #endregion
                    case PagingType.Previous:
                        #region MyRegion

                        //Không thay đổi nếu  CurrentPageIndex đang ở trang đầu tiên
                        if (CurrentPageIndex <= 1)
                            break;

                        CurrentPageIndex--;

                        if (CurrentPageIndex < Page_1)
                        {
                            Page_1--;
                            Page_2--;
                            Page_3--;
                        }
                       
                        #endregion
                        break;
                    case PagingType.SlidePrev:
                        #region MyRegion
                        if (Page_1 <= 1)
                            break;

                        if (Page_1 > 3)
                        {
                            CurrentPageIndex -= 3;
                            Page_1 -= 3;
                            Page_2 -= 3;
                            Page_3 -= 3;
                        }
                        else
                        {
                            CurrentPageIndex--;                            

                            Page_1 = 1;
                            Page_2 = 2;
                            Page_3 = 3;

                            CurrentPageIndex = CurrentPageIndex > Page_3 ? Page_3 : CurrentPageIndex;
                        }
                        
                        #endregion
                        break;
                    case PagingType.Slide:
                        #region MyRegion

                        if (!string.IsNullOrEmpty(ThamSo))
                        {
                            int Temp = int.Parse(ThamSo);
                            if (Temp >= 1 && Temp <= TotalPage)
                            {
                                CurrentPageIndex = Temp;
                                //Dịch chuyển page luôn
                                if (CurrentPageIndex == Page_1 && Page_1 > 1)
                                {
                                    Page_1--;
                                    Page_2--;
                                    Page_3--;
                                }
                                else if (CurrentPageIndex == Page_3 && Page_3 < TotalPage)
                                {
                                    Page_1++;
                                    Page_2++;
                                    Page_3++;
                                }
                            }
                        }
                        #endregion
                        break;
                    case PagingType.SlideNext:
                        #region MyRegion
                        if (TotalPage <= Page_3)
                            break;
                        
                        //Nếu số trang Bên Phải còn lại lớn hơn Step thì bước nhảy = Step
                        if ((TotalPage - Page_3) >= 3)
                        {
                            CurrentPageIndex += 3;

                            Page_1 += 3;
                            Page_2 += 3;
                            Page_3 += 3;
                        }
                        else //Nếu số trang Bên Phải còn lại NHỎ HƠN Step thì bước nhảy = Số Trang còn lại
                        {
                            CurrentPageIndex ++;

                            Page_3 = TotalPage;
                            Page_2 = TotalPage - 1;
                            Page_1 = TotalPage - 2;
                            CurrentPageIndex = CurrentPageIndex < Page_1 ? Page_1 : CurrentPageIndex;
                        }
                        
                        #endregion
                        break;
                    case PagingType.Next:
                        #region MyRegion
                        //Không thay đổi nếu  CurrentPageIndex đang ở trang cuối cùng
                        if (CurrentPageIndex >=  TotalPage)
                            break;

                        CurrentPageIndex++;

                        if (CurrentPageIndex > Page_3)
                        {
                            Page_1++;
                            Page_2++;
                            Page_3++;
                        }
                        #endregion
                        break;
                    case PagingType.Last:
                        #region MyRegion
                        CurrentPageIndex = TotalPage;
                        if (TotalPage > 3)
                        {
                            Page_1 = TotalPage - 2;
                            Page_2 = TotalPage - 1;
                            Page_3 = TotalPage;
                        }
                        else
                        {
                            Page_1 = 1;
                            Page_2 = 2;
                            Page_3 = 3;
                        }
                        #endregion
                        break;

                }
                BeginRow = (CurrentPageIndex - 1) * PageSize+1;
                EndRow = (CurrentPageIndex - 1) * PageSize + PageSize;
                CheckStatus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
