using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using System.Collections;
using System.Globalization;
using System.ComponentModel;

namespace MyLib.MyUtility
{
    /// <summary>
    /// Chứa các hàm về xử lý text
    /// </summary>
    public class MyText
    {
        private static readonly string[] VietnameseSigns = new string[] { "aAeEoOuUiIdDyY", "ấầậẩẫắằặẳẵáàạảãâă", "ẤẦẬẨẪẮẰẶẲẴÁÀẠẢÃÂĂ", "ếềệểễéèẹẻẽê", "ẾỀỆỂỄÉÈẸẺẼÊ", "ốồộổỗớờợởỡóòọỏõôơ", "ỐỒỘỔỖỚỜỢỞỠÓÒỌỎÕÔƠ", "ứừựửữúùụủũư", "ỨỪỰỬỮÚÙỤỦŨƯ", "íìịỉĩ", "ÍÌỊỈĨ", "đ", "Đ", "ýỳỵỷỹ", "ÝỲỴỶỸ" };

        /// <summary>
        /// Chuyên tiếng việt có dấu thành không dấu
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        public static string RemoveSignVietnameseString(string strContent)
        {
            //Tiến hành thay thế , lọc bỏ dấu cho chuỗi

            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)

                    strContent = strContent.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return strContent;
        }

        /// <summary>
        /// Xóa  bỏ ký tự đặc biết, thay " " bằng "_"
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        public static string RemoveSpecialChar(string strContent)
        {
            string[] chars = new string[] { ",", "&", "?", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "-", "(", ")", ":", "|", "[", "]" };
           
            for (int i = 0; i < chars.Length; i++)
            {
                if (strContent.Contains(chars[i]))
                {
                    strContent = strContent.Replace(chars[i], "");
                }
            }
            strContent = strContent.Replace(" ", "_");
            return strContent;
        }

        public static string RemoveSpecialChar(string strContent, string ExceptionString)
        {
            string[] Default_Chars = new string[] { ",", "&", "?", ".", "/", "!", "@", "#", "$", "%", "^", "&", "*", "'", "\"", ";", "-", "(", ")", ":", "|", "[", "]" };

            for (int i = 0; i < Default_Chars.Length; i++)
            {
                if (ExceptionString.IndexOf(Default_Chars[i]) >= 0)
                {
                    continue;
                }
                if (strContent.Contains(Default_Chars[i]))
                {
                    strContent = strContent.Replace(Default_Chars[i], "");
                }
            }
            
            return strContent;
        }

        /// <summary>
        /// Xóa bó những kỹ tự đặc biệt khi sử dụng câu query trong mySqL (loại bỏ lỗi sql injection)
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        public static string ValidQueryMySQL(string strContent)
        {
            try
            {
                if (string.IsNullOrEmpty(strContent))
                {
                    return string.Empty;
                }
                string[] chars = new string[] { "--",";--",";","/*","*/","@@","@","'","#",
                                         " char "," nchar "," varchar "," nvarchar"," nvarchar(",
                                         " alter "," begin "," cast "," create "," cursor "," declare "," delete "," drop "," end "," exec "," execute ",
                                         " fetch "," insert "," kill "," open ",
                                         " select ", " sys "," sysobjects "," syscolumns ",
                                         " table "," table("," update " };

                for (int i = 0; i < chars.Length; i++)
                {
                    if (strContent.Contains(chars[i]))
                    {
                        strContent = strContent.Replace(chars[i], "");
                    }
                }
                
                return strContent;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Chuẩn hóa chuỗi tìm kiếm cho Fulltextsearch
        /// </summary>
        /// <param name="str_SearchContent"></param>
        /// <returns></returns>
        public static string ValidSearchContent(string str_SearchContent)
        {
            try
            {
                return str_SearchContent.Replace(" &", "&").Replace("& ", "&").Replace(" ", "+").Replace(", ", ",").Replace(" ,", ",").Replace(",", " or ");
            }
            catch
            {
                return str_SearchContent;
            }
        }

        /// <summary>
        /// Chuyển đổi HMTL sang dạng plain text
        /// </summary>
        /// <param name="HTMLContent"></param>
        /// <returns></returns>
        public static string ConverHTMLToPlainText(string HTMLContent)
        {
            try
            {
                string result;

                // Remove HTML Development formatting
                // Replace line breaks with space
                // because browsers inserts space
                result = HTMLContent.Replace("\r", " ");
                // Replace line breaks with space
                // because browsers inserts space
                result = result.Replace("\n", " ");
                // Remove step-formatting
                result = result.Replace("\t", string.Empty);
                // Remove repeating spaces because browsers ignore them
                result = System.Text.RegularExpressions.Regex.Replace(result,
                                                                      @"( )+", " ");

                // Remove the header (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*head([^>])*>", "<head>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*head( )*>)", "</head>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(<head>).*(</head>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all scripts (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*script([^>])*>", "<script>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*script( )*>)", "</script>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
                //         string.Empty,
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<script>).*(</script>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // remove all styles (prepare first by clearing attributes)
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*style([^>])*>", "<style>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"(<( )*(/)( )*style( )*>)", "</style>",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(<style>).*(</style>)", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert tabs in spaces of <td> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*td([^>])*>", "\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line breaks in places of <BR> and <LI> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*br( )*>", "\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*li( )*>", "\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // insert line paragraphs (double line breaks) in place
                // if <P>, <DIV> and <TR> tags
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*div([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*tr([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<( )*p([^>])*>", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // Remove remaining tags like <a>, links, images,
                // comments etc - anything that's enclosed inside < >
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"<[^>]*>", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // replace special characters:
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @" ", " ",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&bull;", " * ",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&lsaquo;", "<",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&rsaquo;", ">",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&trade;", "(tm)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&frasl;", "/",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&lt;", "<",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&gt;", ">",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&copy;", "(c)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&reg;", "(r)",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove all others. More can be added, see
                // http://hotwired.lycos.com/webmonkey/reference/special_characters/
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         @"&(.{2,6});", string.Empty,
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // for testing
                //System.Text.RegularExpressions.Regex.Replace(result,
                //       this.txtRegex.Text,string.Empty,
                //       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // make line breaking consistent
                result = result.Replace("\n", "\r");

                // Remove extra line breaks and tabs:
                // replace over 2 breaks with 2 and over 4 tabs with 4.
                // Prepare first to remove any whitespaces in between
                // the escaped characters and remove redundant tabs in between line breaks
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)( )+(\r)", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\t)( )+(\t)", "\t\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\t)( )+(\r)", "\t\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)( )+(\t)", "\r\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove redundant tabs
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)(\t)+(\r)", "\r\r",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Remove multiple tabs following a line break with just one tab
                result = System.Text.RegularExpressions.Regex.Replace(result,
                         "(\r)(\t)+", "\r\t",
                         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                // Initial replacement target string for line breaks
                string breaks = "\r\r\r";
                // Initial replacement target string for tabs
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }

                // That's it.
                return result.TrimEnd().TrimStart();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Xóa bỏ các ký tự đặc biệt.
        /// </summary>
        /// <param name="strContent"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(string strContent)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (char c in strContent)
                {
                    if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                    {
                        sb.Append(c);
                    }
                }
                return sb.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string RemoveSpecialCharacters(string strContent, string ExceptionString)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                foreach (char c in strContent)
                {
                    if ((ExceptionString.IndexOf(c) >= 0)||(c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
                    {
                        sb.Append(c);
                    }
                }
                return sb.ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}

