using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Org.Utils
{
    public static class Logger
    {
        // This takes very long time for execution, need to replace thi method.
        public static string GetImageName()
        {
            return "0_" + DateTime.Now.ToString("yyyyMMddHHmmssFFF"); // year month date hours minutes seconds mili second 
        }
        public static string GetImageName(int counter)
        {
            return "0_" + DateTime.Now.ToString("yyyyMMddHHmmssFFF") + counter.ToString(); // year month date hours minutes seconds mili second 
        }

        public static string GetTimeStamp()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssFFF");
        }


        public static string GetRandomString()
        {
            string randomstring = "";
            var guid = Guid.NewGuid();
            var url = Convert.ToBase64String(guid.ToByteArray()).Trim('+', '/', '=', '_', '-');
            randomstring = System.Text.RegularExpressions.Regex.Replace(url, "[^0-9a-zA-Z]+", "");
            return randomstring;
        }

        public static void LogMessage(string strMessage, string rootPath)
        {
            string day = DateTime.Now.Day.ToString();
            if (day.Length == 1)
            {
                day = "0" + day;
            }
            string month = DateTime.Now.Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string year = DateTime.Now.Year.ToString();
            string filename = "logs/" + year + month + day + ".txt";

            try
            {
                using (TextWriter output = System.IO.File.AppendText(System.Web.Hosting.HostingEnvironment.MapPath(rootPath + filename)))
                    output.WriteLine(string.Format("{0} {1}: {2}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), strMessage));
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }

        public static void LogRelativeMessage(string strMessage, bool debug, string relativePath = "~/logs/")
        {
            try
            {
                string mint = DateTime.Now.Minute.ToString();
                if (mint.Length == 1)
                {
                    mint = "0" + mint;
                }
                mint = "00";    //Same File for whole Hour
                string hour = DateTime.Now.Hour.ToString();
                if (hour.Length == 1)
                {
                    hour = "0" + hour;
                }
                string day = DateTime.Now.Day.ToString();
                if (day.Length == 1)
                {
                    day = "0" + day;
                }
                string month = DateTime.Now.Month.ToString();
                if (month.Length == 1)
                {
                    month = "0" + month;
                }
                string year = DateTime.Now.Year.ToString();
                string filename = relativePath + year + month + day + hour + mint + ".txt";

                using (TextWriter output = System.IO.File.AppendText(System.Web.Hosting.HostingEnvironment.MapPath(filename)))
                    output.WriteLine(string.Format("{0} {1}: {2}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), strMessage));
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
        }

        public static void LogMessage(string strMessage, string rootPath, long userProfileId)
        {
            string mint = DateTime.Now.Minute.ToString();
            if (mint.Length == 1)
            {
                mint = "0" + mint;
            }
            mint = "00";    //Same File for whole Hour
            string hour = DateTime.Now.Hour.ToString();
            if (hour.Length == 1)
            {
                hour = "0" + hour;
            }
            string day = DateTime.Now.Day.ToString();
            if (day.Length == 1)
            {
                day = "0" + day;
            }
            string month = DateTime.Now.Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string year = DateTime.Now.Year.ToString();
            string filename = "logs/" + year + month + day + hour + mint + ".txt";

            try
            {
                using (TextWriter output = System.IO.File.AppendText(rootPath + filename))
                    output.WriteLine(string.Format("{0} {1}:D <<{2}>> {3}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), userProfileId.ToString(), strMessage));
            }
            catch { }
        }

        public static void LogMessage(string strMessage, string rootPath, long userProfileId, long eCoverId)
        {
            string mint = DateTime.Now.Minute.ToString();
            if (mint.Length == 1)
            {
                mint = "0" + mint;
            }
            mint = "00";    //Same File for whole Hour
            string hour = DateTime.Now.Hour.ToString();
            if (hour.Length == 1)
            {
                hour = "0" + hour;
            }
            string day = DateTime.Now.Day.ToString();
            if (day.Length == 1)
            {
                day = "0" + day;
            }
            string month = DateTime.Now.Month.ToString();
            if (month.Length == 1)
            {
                month = "0" + month;
            }
            string year = DateTime.Now.Year.ToString();
            string filename = "logs/" + year + month + day + hour + mint + ".txt";

            try
            {
                using (TextWriter output = System.IO.File.AppendText(rootPath + filename))
                    output.WriteLine(string.Format("{0} {1}:D <<{2}>> <<{3}>> {4}", DateTime.Now.ToLongDateString(), DateTime.Now.ToLongTimeString(), userProfileId.ToString(), eCoverId.ToString(), strMessage));
            }
            catch { }
        }

        public static void LogAndReportCriticalMessage(string strMessage, string Body, string Subject, string FromEmail, string ToEmail, string SMTPUserName, string SMTPPassword, string SMTPHost, int SMTPPort, string relativePath = "~/logs/CriticalErrors/")
        {
            LogRelativeMessage(strMessage, true, relativePath);
            if (!string.IsNullOrWhiteSpace(Body) && !string.IsNullOrWhiteSpace(ToEmail) && !string.IsNullOrWhiteSpace(FromEmail))
            {
                SendCriticalEmail(Body, Subject, FromEmail, ToEmail, SMTPUserName, SMTPPassword, SMTPHost, SMTPPort);
            }
        }

        static void SendCriticalEmail(string Body, string Subject, string FromEmail, string ToEmail, string SMTPUserName, string SMTPPassword, string SMTPHost, int SMTPPort)
        {
            string mailBodyhtml = Body;
            var msg = new MailMessage(FromEmail, ToEmail, Subject, mailBodyhtml);
            msg.IsBodyHtml = true;
            var smtpClient = new SmtpClient(SMTPHost, SMTPPort);
            smtpClient.UseDefaultCredentials = true;
            smtpClient.Credentials = new NetworkCredential(SMTPUserName, SMTPPassword);
            smtpClient.EnableSsl = true;
            smtpClient.Send(msg);
            //Console.WriteLine("Email Sended Successfully");
        }

    }// ends class
}
