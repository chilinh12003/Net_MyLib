using System;
using System.Data;
using System.Web;
using System.Text;
using System.Net;
using System.Web.Mail;
using System.Net.Mail;

namespace MyLib.MyUtility
{
    /// <summary>
    /// Chứa các hàm dùng để gửi email
    /// </summary>
    public class MySendEmail
    {
        /// <summary>
        /// Gửi thông báo alert theo thông tin cấu hình trong config
        /// </summary>
        /// <param name="Content"></param>
        public static void SendEmailAlert(string Content)
        {
            if (MyConfig.SendEmailErrorByIComAccount)
            {
                SendEmail_ICom(MyConfig.AccountSendEmail, MyConfig.PasswordSendEmail, MyConfig.ListReceiveEmail, "ALERT_" + MyConfig.ApplicationName, Content);
            }
            else
            {
                SendEmail_Google(MyConfig.AccountSendEmail, MyConfig.PasswordSendEmail, MyConfig.ListReceiveEmail, "ALERT_" + MyConfig.ApplicationName, Content, System.Web.Mail.MailFormat.Text, string.Empty);
            }
        }

        public static void SendAlertEmail(string Subject, string Message)
        {
            if (MyConfig.SendEmailErrorByIComAccount)
            {
                SendEmail_ICom(MyConfig.AccountSendEmail, MyConfig.PasswordSendEmail, MyConfig.ListReceiveEmail, Subject, Message);
            }
            else
            {
                SendEmail_Google(MyConfig.AccountSendEmail, MyConfig.PasswordSendEmail, MyConfig.ListReceiveEmail, Subject, Message, System.Web.Mail.MailFormat.Text, string.Empty);
            }
        }

        public static bool SendEmail_Google(
            string pGmailEmail,
            string pGmailPassword,
            string pTo,
            string pSubject,
            string pBody,
            System.Web.Mail.MailFormat pFormat,
            string pAttachmentPath)
        {
            try
            {
                System.Web.Mail.MailMessage myMail = new System.Web.Mail.MailMessage();
                myMail.Fields.Add
                    ("http://schemas.microsoft.com/cdo/configuration/smtpserver",
                                  "smtp.gmail.com");
                myMail.Fields.Add
                    ("http://schemas.microsoft.com/cdo/configuration/smtpserverport",
                                  "465");
                myMail.Fields.Add
                    ("http://schemas.microsoft.com/cdo/configuration/sendusing",
                                  "2");

                myMail.Fields.Add
                ("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1");
                //Use 0 for anonymous
                myMail.Fields.Add
                ("http://schemas.microsoft.com/cdo/configuration/sendusername",
                    pGmailEmail);
                myMail.Fields.Add
                ("http://schemas.microsoft.com/cdo/configuration/sendpassword",
                     pGmailPassword);
                myMail.Fields.Add
                ("http://schemas.microsoft.com/cdo/configuration/smtpusessl",
                     "true");
                myMail.From = pGmailEmail;
                myMail.To = pTo;
                myMail.Subject = pSubject;
                myMail.BodyFormat = pFormat;
                myMail.Body = pBody;
                if (pAttachmentPath.Trim() != "")
                {
                    MailAttachment MyAttachment =
                            new MailAttachment(pAttachmentPath);
                    myMail.Attachments.Add(MyAttachment);
                    myMail.Priority = System.Web.Mail.MailPriority.High;
                }
                myMail.BodyEncoding = System.Text.Encoding.UTF8;
                System.Web.Mail.SmtpMail.SmtpServer = "smtp.gmail.com:465";
                System.Web.Mail.SmtpMail.Send(myMail);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static bool SendEmail_Google_New(
            string pGmailEmail,
            string pGmailPassword,
            string pTo,
            string pSubject,
            string pBody,
            string pAttachmentPath)
        {
            try
            {
                System.Net.Mail.MailMessage mMailMessage = new System.Net.Mail.MailMessage();
                string[] arr_to = pTo.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string item in arr_to)
                {
                    mMailMessage.To.Add(item);
                }
                mMailMessage.From = new MailAddress(pGmailEmail);
                mMailMessage.Subject = pSubject;
                mMailMessage.Body = pBody;
                mMailMessage.IsBodyHtml = true;

                if (pAttachmentPath.Trim() != "")
                {
                    mMailMessage.Attachments.Add(new Attachment(pAttachmentPath));
                }

                NetworkCredential cred = new NetworkCredential(pGmailEmail, pGmailPassword);

                SmtpClient mailClient = new SmtpClient("smtp.gmail.com", 587);
                mailClient.EnableSsl = true;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.UseDefaultCredentials = false;
                mailClient.Timeout = 20000;
                mailClient.Credentials = cred;
                mailClient.Send(mMailMessage);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        private static void SendEmail_ICom(
            string pGmailEmail,
            string pGmailPassword,
            string pTo,
            string pSubject,
            string pBody)
        {
            try
            {
                System.Net.Mail.MailMessage mail = new System.Net.Mail.MailMessage();
                System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient("smtp.gmail.com");

                mail.From = new System.Net.Mail.MailAddress(pGmailEmail);// + "@gmail.com");
                mail.To.Add(pTo);
                mail.Subject = pSubject;
                mail.Body = pBody;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(pGmailEmail, pGmailPassword);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
