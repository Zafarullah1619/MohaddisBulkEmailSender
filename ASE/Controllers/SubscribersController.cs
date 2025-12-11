using ASE.Utility;
using Org.Business.Methods;
using Org.Business.Objects;
using Org.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ASE.Controllers
{
    public class SubscribersController : Controller
    {
        public ActionResult Add(ProductSubscribers productSubscribers)
        {
            return View(productSubscribers);
        }

        public ActionResult SendEmails()
        {
            try
            {
                var lstProducts = SubscriberProductsBAL.lstSubscriberProducts(true);
                Dictionary<string, EmailData> emailDataDict = new Dictionary<string, EmailData>();

                foreach (var product in lstProducts)
                {
                    var subscribers = SubscriberProductsBAL.lstSubscribers(product.Id);
                    if (subscribers.Count > 0)
                    {
                        var EmailSubBody = SubscriberProductsBAL.SubscriberProductEmailSettings(product.Id);
                        var ContentQuery = SubscriberProductsBAL.ProductContentQuery(product.Id);

                        var content = SubscriberProductsBAL.GetContentForProduct(product.Id, ContentQuery.InlineQuery);

                        if (content.Count > 0)
                        {
                            var cleanedContent = SubscriberProductsBAL.CleanContent(content);
                            string bodyTemplate = EmailSubBody.Body;

                            string productEmailBody = ViewRenderer.ReplacePlaceHolders(bodyTemplate, cleanedContent);

                            foreach (var sub in subscribers)
                            {
                                string email = sub.Email.Trim().ToLower();
                                string fromName = sub.Name;

                                if (!emailDataDict.ContainsKey(email))
                                {
                                    emailDataDict[email] = new EmailData
                                    {
                                        ToEmail = email,
                                        FromName = fromName
                                    };
                                }

                                emailDataDict[email].EmailBodies.Add(productEmailBody);
                            }
                        }
                    }
                }

                // SEND EMAILS
                // < add key = "AwsSecretKey" value = "9+4gErZZapBLxJYuf5hfcR1/ulApCCiLBpkmMSnX" />
                // < add key = "AwsAccessKeyId" value = "AKIARHQBNLSCPEL2OKKF" />

                var AwsSecretKey = StringHelper.Decrypt(ConfigurationManager.AppSettings["AwsSecretKey"], true);
                var AwsAccessKeyId = StringHelper.Decrypt(ConfigurationManager.AppSettings["AwsAccessKeyId"], true);
                var AwsFromEmail = ConfigurationManager.AppSettings["AwsFromEmail"];
                var AwsFromName = ConfigurationManager.AppSettings["AwsFromName"];
                string Subject = ConfigurationManager.AppSettings["Subject"];

                var SMTPFromEmail = ConfigurationManager.AppSettings["SMTPFromEmail"];
                var SmtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
                var SmtpUsername = ConfigurationManager.AppSettings["SmtpUsername"];
                var SmtpServer = ConfigurationManager.AppSettings["SmtpServer"];
                var SmtpPort = Convert.ToInt64(ConfigurationManager.AppSettings["SmtpPort"]);

                foreach (var entry in emailDataDict)
                {
                    var data = entry.Value;

                    string finalMergedBody = string.Join("<br/><hr/><br/>", data.EmailBodies);

                    try
                    {
                        EmailServiceBAL.SendEmailUsingAWS(AwsAccessKeyId, AwsSecretKey, AwsFromEmail, AwsFromName, data.ToEmail, Subject, finalMergedBody);

                        // EmailServiceBAL.SendEmailUsingSMTP(SmtpServer, SmtpPort, true, SmtpUsername, SmtpPassword, SMTPFromEmail, data.ToEmail, Subject, finalMergedBody);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogRelativeMessage("SendEmails ToEmail=>" + data.ToEmail + " Exception :: " + ex.Message, true);
                        continue;
                    }
                }

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger.LogRelativeMessage("SendEmails Exception :: " + ex.Message, true);
                throw ex;
            }
        }

    }
}















/* [HttpPost]
   public ActionResult Add(string Email, string Name, long ProductId)
   {
       Response.AppendHeader("Access-Control-Allow-Origin", "*");
       Response.AppendHeader("Access-Control-Allow-Methods", "POST");
       Response.AppendHeader("Access-Control-Allow-Headers", "Content-Type");

       var added = SubscribersBAL.AddSubscriber(Email, Name, ProductId);
       return Json(added, JsonRequestBehavior.AllowGet);
   }*/



/*public ActionResult GetProductContent()
{
    try
    {
        List<Product> products = ProductsBAL.lstSubscriberProducts();

        foreach (var product in products)
        {
            string content = SubscribersBAL.GetContentForProduct(product.Id);
            if (!string.IsNullOrEmpty(content))
            {
                List<ProductSubscribers> subscribers = ProductsBAL.lstSubscribers(product.Id);
                SendEmails(subscribers, content);
            }
        }
        var result = new { Success = true, Message = "Emails sent successfully." };
        return Json(result, JsonRequestBehavior.AllowGet);
    }
    catch (Exception ex)
    {
        // Handle exceptions as needed
        var result = new { Success = false, Message = "An error occurred while sending emails." };
        return Json(result, JsonRequestBehavior.AllowGet);
    }
}

private void SendEmails(List<ProductSubscribers> subscribers, string content)
{
    string smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
    int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
    string smtpUsername = ConfigurationManager.AppSettings["SmtpUsername"];
    string smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
    string fromEmail = ConfigurationManager.AppSettings["FromEmail"]; // Get "From" email address

    foreach (var subscriber in subscribers)
    {
        SendEmail(fromEmail, subscriber.Email, content, smtpServer, smtpPort, smtpUsername, smtpPassword);
    }
}

private void SendEmail(string fromEmail, string toEmail, string body, string smtpServer, int smtpPort, string smtpUsername, string smtpPassword)
{
    try
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(fromEmail);
        mail.To.Add(toEmail);
        mail.Subject = "New Content Uploaded";
        mail.Body = body;
        mail.IsBodyHtml = true; // Set to true if your email body contains HTML content

        SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
        smtpClient.EnableSsl = false; // Use SSL if required by your SMTP server

        smtpClient.Send(mail);
    }
    catch (Exception ex)
    {

        //Console.WriteLine("Error sending email: " + ex.Message);
    }
}*/
