using Org.Business.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using System.Net.Http;
using System.Web.Mail;
using Org.Utils;
using Amazon.IdentityManagement.Model;
using Amazon.Runtime;
using Amazon;
using System.Web;
using System.Configuration;

namespace Org.Business.Methods
{
    public class EmailServiceBAL
    {
        public static void SendEmailUsingSMTP1(string smtpServer, long smtpPort, bool? enableSSL, string smtpUser, string smtpPassword, string fromEmail, string toEmail, string subject, string body, long? productId = null, long? subscriberId = null)
        {
            // Append unsubscribe footer to email body
            string emailBodyWithFooter = AppendUnsubscribeFooter(body, toEmail, productId, subscriberId);

            using (var mailMessage = new System.Net.Mail.MailMessage(fromEmail, toEmail))
            {
                mailMessage.Subject = !string.IsNullOrEmpty(subject) ? subject : "New Content Upload";
                mailMessage.Body = emailBodyWithFooter;
                mailMessage.IsBodyHtml = true; // Set to true if the email body is in HTML format

                using (var smtpClient = new SmtpClient(smtpServer, (int)smtpPort))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.EnableSsl = enableSSL ?? true;

                    if (!string.IsNullOrEmpty(smtpUser) && !string.IsNullOrEmpty(smtpPassword))
                    {
                        smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPassword);
                        
                        
                        try
                        {
                            smtpClient.Send(mailMessage);
                        }
                        catch (Exception ex)
                        {
                            // Handle the exception or log the error
                            Logger.LogRelativeMessage($"An error occurred while sending emails manually using SMTP Settings {smtpServer}, {smtpUser}: " + ex.Message, true);
                            throw new ApplicationException("Failed to send email.", ex);
                        }
                    }
                   
                }
            }
        }

        public static void SendEmailUsingAWS(string AwsAccessKeyId, string AwsSecretKey, string FromEmail, string FromName, string toEmail, string subject, string body, long? productId = null, long? subscriberId = null)
        {
            // Append unsubscribe footer to email body
            string emailBodyWithFooter = AppendUnsubscribeFooter(body, toEmail, productId, subscriberId);

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            //using (var client = new AmazonSimpleEmailServiceClient(AwsAccessKeyId, AwsSecretKey))
            AWSCredentials credentials = new BasicAWSCredentials(AwsAccessKeyId, AwsSecretKey);
            using (var client = AWSClientFactory.CreateAmazonSimpleEmailServiceClient(credentials, RegionEndpoint.USEast1))
            {
                var request = new SendEmailRequest
                {
                    Source = FromEmail,
                    Destination = new Destination
                    {
                        ToAddresses =
                     {
                         toEmail
                     }
                    },
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content(emailBodyWithFooter)
                        }
                    }
                };

                try
                {
                    client.SendEmail(request);
                }
                
                catch (Exception ex)
                {
                    // Handle the exception or log the error
                    Logger.LogRelativeMessage($"An error occurred while sending emails manually using AWS Settings {AwsAccessKeyId}, {AwsSecretKey}: " + ex.Message, true);
                    throw new ApplicationException("Failed to send email.", ex);
                }
                
            }
        }

        public static void SendEmailUsingSMTP(string smtpServer, long smtpPort, bool? enableSSL, string smtpUser, string smtpPassword, string fromEmail, string toEmail, string subject, string body, long? productId = null, long? subscriberId = null)
        {
            // Append unsubscribe footer to email body
            string emailBodyWithFooter = AppendUnsubscribeFooter(body, toEmail, productId, subscriberId);

            using (var mailMessage = new System.Net.Mail.MailMessage(fromEmail, toEmail))
            {
                mailMessage.Subject = !string.IsNullOrEmpty(subject) ? subject : "New Content Upload";
                mailMessage.Body = emailBodyWithFooter;
                mailMessage.IsBodyHtml = true; // Set to true if the email body is in HTML format

                using (var smtpClient = new SmtpClient(smtpServer, (int)smtpPort))
                {
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.EnableSsl = enableSSL ?? true;

                    if (!string.IsNullOrEmpty(smtpUser) && !string.IsNullOrEmpty(smtpPassword))
                    {
                        smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPassword);
                        
                        
                        try
                        {
                            smtpClient.Send(mailMessage);
                        }
                        catch (Exception ex)
                        {
                            // Handle the exception or log the error
                            Logger.LogRelativeMessage($"An error occurred while sending emails manually using SMTP Settings {smtpServer}, {smtpUser}: " + ex.Message, true);
                            throw new ApplicationException("Failed to send email.", ex);
                        }
                    }
                   
                }
            }
        }

        /// <summary>
        /// Generates unsubscribe footer with link in Urdu
        /// </summary>
        private static string AppendUnsubscribeFooter(string emailBody, string email, long? productId, long? subscriberId)
        {
            string unsubscribeLink = GenerateUnsubscribeLink(email, productId, subscriberId);
            
            // Urdu footer text
            string footerText = $@"
<div style=""margin-top: 20px; padding-top: 20px; border-top: 1px solid #e0e0e0; font-family: 'Jameel Noori Nastaleeq Regular','Jameel Noori Nastaleeq','Noto Nastaliq Urdu',serif; font-size: 12px; color: #666666; direction: rtl; text-align: right;"">
    <p style=""margin: 10px 0;"">آپ یہ ای میل ہماری نیوز لیٹر سروس کو سبسکرائب کرنے کی وجہ سے وصول کر رہے ہیں۔</p>
    <p style=""margin: 10px 0;"">اگر آپ مزید ای میلز موصول نہیں کرنا چاہتے تو آپ کسی بھی وقت <a href=""{unsubscribeLink}"" style=""color: #0066cc; text-decoration: underline;"">ان سبسکرائب</a> کر سکتے ہیں۔</p>
</div>";

            return emailBody + footerText;
        }

        /// <summary>
        /// Generates unsubscribe link with encoded email and product ID
        /// </summary>
        private static string GenerateUnsubscribeLink(string email, long? productId, long? subscriberId)
        {
            // Get base URL from configuration or use relative path
            string baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
            
            // If not in config, try to get from HttpContext
            if (string.IsNullOrEmpty(baseUrl) && HttpContext.Current != null && HttpContext.Current.Request != null)
            {
                baseUrl = HttpContext.Current.Request.Url?.GetLeftPart(UriPartial.Authority);
            }
            
            // If still empty, use relative path
            if (string.IsNullOrEmpty(baseUrl))
            {
                baseUrl = "";
            }
            
            // Encode email and create token
            string encodedEmail = HttpUtility.UrlEncode(email);
            string token = CreateUnsubscribeToken(email, productId, subscriberId);
            
            // If subscriber ID is available, use it for better security
            if (subscriberId.HasValue)
            {
                return $"{baseUrl}/SubscriberProduct/Unsubscribe?email={encodedEmail}&productId={productId}&subscriberId={subscriberId}&token={token}";
            }
            else if (productId.HasValue)
            {
                return $"{baseUrl}/SubscriberProduct/Unsubscribe?email={encodedEmail}&productId={productId}&token={token}";
            }
            else
            {
                return $"{baseUrl}/SubscriberProduct/Unsubscribe?email={encodedEmail}&token={token}";
            }
        }

        /// <summary>
        /// Creates a simple token for unsubscribe link verification
        /// </summary>
        private static string CreateUnsubscribeToken(string email, long? productId, long? subscriberId)
        {
            // Simple token based on email and product ID hash
            string tokenString = $"{email}|{productId}|{subscriberId}";
            byte[] tokenBytes = Encoding.UTF8.GetBytes(tokenString);
            // Simple hash-based token
            int hash = tokenString.GetHashCode();
            return Convert.ToBase64String(BitConverter.GetBytes(hash)).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        }

    }
}
