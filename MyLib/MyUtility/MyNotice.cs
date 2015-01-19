using System;
using System.Collections.Generic;
using System.Text;

namespace MyLib.MyUtility
{
    /// <summary>
    /// Lớp chứa các thông báo
    /// </summary>
    public class MyNotice
    {
        /// <summary>
        /// Các thông báo lỗi dành cho trang admin
        /// </summary>
        public struct AdminError
        {
            /// <summary>
            /// Lỗi tải dữ liêu
            /// </summary>
            public static string LoadDataError = "Có lỗi xảy ra trong quá trình tải dữ liệu!";
            public static string CheckExtensionError = "Có lỗi xảy ra trong quá trình kiểm tra định dạng file!";
            public static string CheckFileUploadSizeError = "Có lỗi xảy ra trong quá trình kiểm tra dung lượng file!";
            public static string DeleteFileError = "Có lỗi xảy ra trong quá trình xóa file trên server!";
            public static string UploadFileError = "Có lỗi xảy ra trong quá trình Upload file!";
            public static string CheckFileNameError = "Lỗi trong quá trình check fileName Empty!";
            public static string SaveDataError = "Có lỗi xảy ra trong quá trình lưu dữ liệu!";
            public static string DeleteDataError = "Có lỗi xảy ra trong quá trình xóa dữ liệu!";
            public static string EditDataError = "Có lỗi xảy ra trong quá trình chỉnh sửa dữ liệu!";
            public static string CheckPermissionError = "Có lỗi xảy ra trong quá trình Kiểm tra phân quyền!";
            public static string SeachError = "Có lỗi xảy ra trong quá trình tìm kiếm!";
            public static string ChangePasswordError = "Có lỗi xảy ra trong quá trình thay đổi mật khẩu!";
            public static string CopyDataError = "Có lỗi xảy ra trong quá trình sao chép dữ liệu!";
            public static string PagingError = "Có lỗi xảy ra trong quá trình phân trang!";
            public static string SendMessageError = "Có lỗi xảy ra trong quá trình gửi tin nhắn!";
            public static string ActiveError = "Có lỗi xảy ra trong quá trình kích hoạt!";
            public static string DeActiveError = "Có lỗi xảy ra trong quá trình hủy kích hoạt!";
            public static string LoginError = "Có lỗi xảy ra trong quá trình đăng nhập!";
            public static string UnknowError = "Có lỗi xảy ra!";
            public static string SortError = "Có lỗi xảy ra trong quá sắp xếp!";
        }
        /// <summary>
        /// Các thông báo thành công dành cho trang admin
        /// </summary>
        public struct AdminSuccess
        {
            public static string DeleteFileSuccess = "Xóa file thành công.";
            public static string SaveFileSuccess = "Lưu file thành công.";
            public static string SaveDataSuccess = "Lưu dữ liệu thành công.";
            public static string DeleteDataSuccess = "Xóa dữ liệu thành công.";
            public static string ChangePasswordSuccess = "Thay đổi mật khẩu thành công.";
            public static string CopyDataSuccess = "Sao chép dữ liệu thành công.";
            public static string SendMessageSuccess = "Gửi tin nhắn thành công.";
            public static string AddDataSuccess = "Thêm mới dữ liệu thành công";
        }

        /// <summary>
        /// Các thông báo không thành công dành cho trang admin
        /// </summary>
        public struct AdminUnSuccess
        {
            public static string DeleteDataUnSuccess = "Xóa dữ liệu KHÔNG thành công!";
            public static string SaveDataUnSuccess = "Lưu dữ liệu KHÔNG thành công!";
            public static string ChangePasswordUnSuccess = "Thay đổi mật khẩu KHÔNG thành công.";
            public static string CopyDataUnSuccess = "Sao chép dữ liệu không thành công.";
            public static string SendMessageUnSuccess = "Gửi tin nhắn KHÔNG thành công.";
            public static string AddDataUnSuccess = "Thêm mới dữ liệu KHÔNG thành công";
        }

        /// <summary>
        /// Các cảnh báo dành cho trang admin
        /// </summary>
        public struct AdminWarning
        {
            public static string FileNotExist = "File không tồn tại!";
            public static string AskBeforeDelete = "Bạn có chắc bạn muốn xóa mục này?";
            public static string AskBeforeDeleteAll = "Bạn có chắc bạn muốn xóa các mục đã chọn?";
            public static string DeleteFileNotAll = "Xóa không hết tất cả các file truyền vào!";           
        }

        public struct EndUserError
        {
            public static string LoadDataError = "Xin lỗi, hệ thống hiện tại đang quá tải.<br /> Xin quý khách vui lòng quay lại sau.";
        }
    }
}
