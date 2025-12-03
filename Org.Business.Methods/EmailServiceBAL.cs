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

namespace Org.Business.Methods
{
    public class EmailServiceBAL
    {
        public static void SendEmailUsingSMTP(string smtpServer, long smtpPort, bool? enableSSL, string smtpUser, string smtpPassword, string fromEmail, string toEmail, string subject, string body)
        {
            using (var mailMessage = new System.Net.Mail.MailMessage(fromEmail, toEmail))
            {
                mailMessage.Subject = !string.IsNullOrEmpty(subject) ? subject : "New Content Upload";
                mailMessage.Body = body;
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

        public static void SendEmailUsingAWS(string AwsAccessKeyId, string AwsSecretKey, string FromEmail, string toEmail, string subject, string body)
        {
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
                            Html = new Content(body)
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

    }
}
