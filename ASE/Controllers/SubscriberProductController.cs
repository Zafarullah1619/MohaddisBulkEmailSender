using Amazon.EC2.Model;
using Amazon.SimpleEmail.Model;
using Org.Business.Methods;
using Org.Business.Objects;
using Org.Utils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;

namespace ASE.Controllers
{
    [Authorize]
    public class SubscriberProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {
            var lstProducts = SubscriberProductsBAL.lstSubscriberProducts();
            return View(lstProducts);
        }

        public ActionResult SAddEdit(long? ProductId)
        {
            ProductViewModal PVM = new ProductViewModal();
            if (ProductId.HasValue && ProductId.Value > 0)
            {
                var ProductDetail = ProductsBAL.ProductDetail(ProductId);
                var SubscriberProductEmailSettings = SubscriberProductsBAL.SubscriberProductEmailSettings(ProductId);
                List<string> listplaceholders = null;
                List<string> selelctedlistPlaceholders = null;
                var ContentQuery = SubscriberProductsBAL.ProductContentQuery(ProductId);

                if (ContentQuery != null && !string.IsNullOrEmpty(ContentQuery.InlineQuery))
                {
                    listplaceholders = SubscriberProductsBAL.GetColumnNamesAsPlaceHolders((long)ProductId, ContentQuery.InlineQuery);
                    selelctedlistPlaceholders = SubscriberProductEmailSettings?.Placeholders?.Split(',').ToList();
                }

                PVM.Product = ProductDetail;
                PVM.emailSettings = SubscriberProductEmailSettings ?? new EmailSettings();
                PVM.PlaceHolders = listplaceholders;
                PVM.SelectPlaceholders = selelctedlistPlaceholders;
            }
            else
            {
                PVM.Product = new Product();
                PVM.Product.IsSendEmail = false;
            }
            return View(PVM);
        }

        [HttpPost]
        public ActionResult UpdateSubscriberProductInfo(long? ProductId, string ProductName, string DBServer, string DBUserName, string DBPassword, string DBServerType, string DataBaseName, bool IsSendEmail, string Subject, List<string> selectedPlaceholders, string Body)
        {


            var added = SubscriberProductsBAL.AddEditSubsProduct(ProductId, ProductName, DBServer, DBUserName, DBPassword, DBServerType, DataBaseName ,IsSendEmail);

            if (ProductId > 0)
            {
                Body = HttpUtility.UrlDecode(Body);
                string selectedPlaceholdersStr = string.Join(",", selectedPlaceholders);
                SubscriberProductsBAL.AddEditSubsProductEmailSetting(ProductId, Subject, Body, selectedPlaceholdersStr);
            }

            return Json(added.Id, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SMTPConfigurations(long? ProductId, string Title, string FromName, string FromEmail, string SmtpServer, long SmtpPort, string SmtpUser, string SmtpPassword, string SmtpDomain, string SmtpFooter, string SSLPort, long? PostMarkSignatureId, bool IsDefault = false, bool EnableSSL = false, long? Id = 0, bool IsAWSBulkEmail = false, string AwsServiceURL = "", long? AwsBulkEmailCount = 0, string AwsAccessKeyId = "", string AwsSecretKey = "")
        {
            var PostMarkToken = ConfigurationManager.AppSettings["PostmarkServerToken"];
            string PushNotificationSettingsJson = string.Empty;
            if (!Convert.ToBoolean(IsAWSBulkEmail))
            {
                PushNotificationSettingsJson = PushNotificationsBAL.ProviderSettings(Title, FromName, FromEmail, SmtpServer, SmtpPort, SmtpUser, SmtpPassword, SmtpDomain, SmtpFooter, IsDefault, EnableSSL);
            }
            else
            {
                SmtpUser = string.Empty;
                SmtpPort = 0;
                SmtpPassword = string.Empty;
            }

            var Added = PushNotificationsBAL.AddUserSmtpConfiguration(ProductId, Title, FromName, FromEmail, SmtpServer, SmtpPort, SmtpUser, SmtpPassword, SmtpDomain, SmtpFooter, IsDefault, PushNotificationSettingsJson, false, "0", Id, EnableSSL, "", IsAWSBulkEmail, AwsServiceURL, AwsBulkEmailCount, AwsAccessKeyId, AwsSecretKey);

            return Json(Added, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetInlineQuery(long? ProductId)
        {
            if (ProductId.HasValue)
            {
                string inlineQuery = SubscriberProductsBAL.ProductContentQuery(ProductId)?.InlineQuery; // Implement this method to retrieve the inline query
                return Json(new { InlineQuery = inlineQuery }, JsonRequestBehavior.AllowGet);
            }

            return Json(null, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UpdateInlineQuery(long? ProductId, string InlineQuery)
        {
            var added = SubscriberProductsBAL.AddEditInlineQuery(ProductId, InlineQuery);

            return Json(added.Id, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SendEmailManually(long? ProductId)
        {
            var ProductDetail = ProductsBAL.ProductDetail(ProductId);
            var ProductList = SubscriberProductsBAL.lstSubscriberProducts();
            var ProductSMTP = ProductsBAL.ProductSMTP(ProductId);

            EmailViewModel EVM = new EmailViewModel();
            EVM.Product = ProductDetail;
            EVM.ProductList = ProductList;
            if (ProductSMTP != null)
            {
                EVM.EmailSmtpConfiguration = ProductSMTP;
            }
            else
            {
                EVM.EmailSmtpConfiguration = new SmtpConfiguration();
            }

            return View(EVM);
        }

        [HttpPost]

        public ActionResult SendEmailManually(EmailViewModel model)
        {
            // Get the selected product IDs
            var selectedProductIds = model.SelectedProductIds;

            // Get the email subject and body
            var subject = model.EmailSubject;
            var body = model.Body;

            // Set SMTP configuration
            var isAWSBulkEmail = model.EmailSmtpConfiguration.IsAWSBulkEmail;
            var AwsAccessKeyId = model.EmailSmtpConfiguration.AwsAccessKeyId;
            var AwsSecretKey = model.EmailSmtpConfiguration.AwsSecretKey;

            var fromEmail = model.EmailSmtpConfiguration.FromEmail;

            // Set SMTP configuration
            var smtpUser = model.EmailSmtpConfiguration.SmtpUser;
            var smtpPassword = model.EmailSmtpConfiguration.SmtpPassword;
            var smtpServer = model.EmailSmtpConfiguration.SmtpServer;
            var smtpPort = model.EmailSmtpConfiguration.SmtpPort ?? 25;
            var enableSSL = model.EmailSmtpConfiguration.EnableSSL ?? true;

            try
            {
                foreach (var productId in selectedProductIds)
                {
                    if (productId > 0)
                    {
                        Product product = ProductsBAL.ProductDetail(productId);
                        List<ProductSubscribers> subscribers = SubscriberProductsBAL.lstSubscribers(productId);
                        foreach (var subscriber in subscribers)
                        {
                            if (isAWSBulkEmail == true)
                            {
                                // Send email using AWS configuration
                                EmailServiceBAL.SendEmailUsingAWS(AwsAccessKeyId, AwsSecretKey, fromEmail, subscriber.Email, subject, body);
                            }
                            else
                            {
                                // Send email using SMTP configuration
                                EmailServiceBAL.SendEmailUsingSMTP(smtpServer, smtpPort, enableSSL, smtpUser, smtpPassword, fromEmail, subscriber.Email, subject, body);
                            }

                        }
                        Logger.LogRelativeMessage($"Emails Sent successfully through manually for product {product.ProductName}, Total subscribers: {subscribers.Count}", true);
                    }
                    else
                    {
                        Logger.LogRelativeMessage($"No Product Found against this productid : {productId}", true);
                    }

                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                Logger.LogRelativeMessage("An error occurred while sending emails manually: " + ex.Message, true);
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public ActionResult RemoveSubsProduct(long ProductId)
        {
            var removed = ProductsBAL.RemoveProduct(ProductId);
            return Json(removed, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult SendEmailsAuto(long? productId = null)
        {
            try
            {
                List<Product> lstProducts = new List<Product>();

                if (productId.HasValue)
                {
                    Product product = ProductsBAL.ProductDetail(productId.Value);
                    lstProducts.Add(product);
                }
                else
                {
                    lstProducts = SubscriberProductsBAL.lstSubscriberProducts();
                }

                //List<Product> products = SubscriberProductsBAL.lstSubscriberProducts();
                foreach (var product in lstProducts)
                {
                    if (product != null)
                    {

                        var ProductSMTP = ProductsBAL.ProductSMTP(product.Id);
                        var EmailSubBody = SubscriberProductsBAL.SubscriberProductEmailSettings(product.Id);
                        var ContentQuery = SubscriberProductsBAL.ProductContentQuery(product.Id);


                        var content = SubscriberProductsBAL.GetContentForProduct(product.Id, ContentQuery.InlineQuery);
                        if (content.Count > 0)
                        {
                            var productcontent = SubscriberProductsBAL.CleanContent(content);

                            var Body = EmailSubBody.Body;
                            string emailBody = ViewRenderer.ReplacePlaceHolders(Body, productcontent);

                            List<ProductSubscribers> subscribers = SubscriberProductsBAL.lstSubscribers(product.Id);

                            foreach (var subscriber in subscribers)
                            {
                                var personalizedSubject = $"{EmailSubBody.Subject}";
                                personalizedSubject = personalizedSubject.Replace(Environment.NewLine, " ");

                                if (ProductSMTP.AwsAccessKeyId != "")
                                {
                                    // Send email using AWS configuration
                                    EmailServiceBAL.SendEmailUsingAWS(ProductSMTP.AwsAccessKeyId, ProductSMTP.AwsSecretKey, ProductSMTP.FromEmail, subscriber.Email, personalizedSubject, emailBody);
                                }
                                else
                                {
                                    // Send email using SMTP configuration
                                    EmailServiceBAL.SendEmailUsingSMTP(ProductSMTP.SmtpServer, (long)ProductSMTP.SmtpPort, ProductSMTP.EnableSSL, ProductSMTP.SmtpUser, ProductSMTP.SmtpPassword, ProductSMTP.FromEmail, subscriber.Email, personalizedSubject, emailBody);
                                }
                            }

                            Logger.LogRelativeMessage($"Emails sent successfully for product {product.ProductName} while using this Query {ContentQuery.InlineQuery}, Total subscribers: {subscribers.Count}", true);

                        }
                        else
                        {
                            Logger.LogRelativeMessage($"Content Not found for {product.ProductName} according to your entered Query {ContentQuery.InlineQuery}", true);
                        }
                    }
                    else
                    {
                        Logger.LogRelativeMessage($"Product not found for ID: {product.Id}", true); // Log the exception
                    }
                }

                var results = new { Success = true, Message = "Done to Send Emails" };
                return Json(results, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                Logger.LogRelativeMessage("An error occurred while sending emails: " + ex.Message, true); // Log the exception
                var result = new { Success = false, Message = "An error occurred while sending emails." };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

       
    }
}















/*      
        public ActionResult SendEmailsAuto(long? productId = null) //For Proper HTMLVIEWS
        {

            try
            {
                if (productId.HasValue)
                {
                    // Work for a specific product
                    Product product = ProductsBAL.ProductDetail(productId.Value);
                    var ProductSMTP = ProductsBAL.ProductSMTP(productId);
                    if (product != null)
                    {
                        ContentQuery cq = SubscriberProductsBAL.ProductContentQuery(productId);
                        string[] contentQueries = cq.InlineQuery.Split('~'); // Split the content queries by tilde (~)

                        foreach (var contentQuery in contentQueries)
                        {
                            var content = SubscriberProductsBAL.GetContentForProduct(product.Id, contentQuery);
                            if (content.Count > 0)
                            {
                                var modifiedContent = new List<Dictionary<string, object>>();

                                foreach (var contentRow in content)
                                {
                                    var modifiedRow = new Dictionary<string, object>();

                                    foreach (var columnName in contentRow.Keys)
                                    {
                                        var fieldValue = contentRow[columnName].ToString();

                                        // Decode HTML entities
                                        fieldValue = HttpUtility.HtmlDecode(fieldValue);

                                        // Remove all HTML tags and store as plain text
                                        fieldValue = Regex.Replace(fieldValue, "<[^>]+>", "");

                                        // Add the modified field to the modified row
                                        modifiedRow[columnName] = fieldValue;
                                    }

                                    // Add the modified row to the modified content
                                    modifiedContent.Add(modifiedRow);
                                }

                                ViewBag.Content = modifiedContent;

                                string productViewName = product.ProductName.Replace(" ", ""); // Remove spaces
                                string viewName = $"~/Views/EmailContentViews/{productViewName}.cshtml";

                                // Render the HTML view to a string
                                string emailBody = ViewRenderer.RenderViewToString(this, viewName, ViewBag.Content);

                                List<ProductSubscribers> subscribers = SubscriberProductsBAL.lstSubscribers(product.Id);

                                foreach (var subscriber in subscribers)
                                {
                                    var personalizedSubject = $"Hey {subscriber.Name},New content has been uploaded! Go and Check it out now";
                                    personalizedSubject = personalizedSubject.Replace(Environment.NewLine, " ");
                                    // Send email with the HTML table as the body
                                    if (ProductSMTP.AwsAccessKeyId!="")
                                    {
                                        // Send email using AWS configuration
                                        EmailServiceBAL.SendEmailUsingAWS(ProductSMTP.AwsAccessKeyId, ProductSMTP.AwsSecretKey, ProductSMTP.FromEmail, subscriber.Email, personalizedSubject, emailBody);
                                    }
                                    else
                                    {
                                        // Send email using SMTP configuration
                                        EmailServiceBAL.SendEmailUsingSMTP(ProductSMTP.SmtpServer, (long)ProductSMTP.SmtpPort, ProductSMTP.EnableSSL, ProductSMTP.SmtpUser, ProductSMTP.SmtpPassword, ProductSMTP.FromEmail, subscriber.Email, personalizedSubject, emailBody);
                                    }
                                }

                                Logger.LogRelativeMessage($"Emails sent successfully for product {product.ProductName} while using this Query {contentQuery}, Total subscribers: {subscribers.Count}", true);
                                var result = new { Success = true, Message = "Emails sent successfully." };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                Logger.LogRelativeMessage($"Content Not found according to your entered Query {contentQuery}", true);
                                var result = new { Success = false, Message = "Emails not sent successfully." };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                    else
                    {
                        Logger.LogRelativeMessage($"Product not found for ID: {productId}", true); // Log the exception
                        var result = new { Success = false, Message = "Product ID not found" };
                        return Json(result, JsonRequestBehavior.AllowGet);

                    }

                    return Json(JsonRequestBehavior.AllowGet);
                }
                else
                {
                    *//*// Work for all products
                    List<Product> products = SubscriberProductsBAL.lstSubscriberProducts();

                    foreach (var product in products)
                    {
                        ContentQuery cq = SubscriberProductsBAL.ProductContentQuery(product.Id);
                        string[] contentQueries = cq.InlineQuery.Split('~');
                        foreach (var contentQuery in contentQueries)
                        {
                            var content = SubscriberProductsBAL.GetContentForProduct(product.Id, contentQuery);
                            if (content.Count > 0)
                            {
                                var modifiedContent = new List<Dictionary<string, object>>();

                                foreach (var contentRow in content)
                                {
                                    var modifiedRow = new Dictionary<string, object>();

                                    foreach (var columnName in contentRow.Keys)
                                    {
                                        var fieldValue = contentRow[columnName].ToString();

                                        // Check if the field contains <p> tags
                                        if (fieldValue.Contains("<p>") && fieldValue.Contains("</p>"))
                                        {
                                            // Remove <p> tags and store as plain text
                                            fieldValue = Regex.Replace(fieldValue, "<.*?>", "");
                                        }

                                        // Add the modified field to the modified row
                                        modifiedRow[columnName] = fieldValue;
                                    }

                                    // Add the modified row to the modified content
                                    modifiedContent.Add(modifiedRow);
                                }

                                ViewBag.Content = modifiedContent;

                                // Render the HTML view to a string
                                string emailBody = ViewRenderer.RenderViewToString(this, "EmailContent", ViewBag.Content);

                                List<ProductSubscribers> subscribers = SubscriberProductsBAL.lstSubscribers(product.Id);

                                foreach (var subscriber in subscribers)
                                {
                                    var personalizedSubject = $"Hey {subscriber.Name},New content has been uploaded! Go and Check it out now";
                                    personalizedSubject = personalizedSubject.Replace(Environment.NewLine, " ");
                                    // Send email with the HTML table as the body
                                    EmailServiceBAL.SendEmailUsingSMTP(smtpServer, smtpPort, enablessl, smtpUsername, smtpPassword, fromEmail, subscriber.Email, personalizedSubject, emailBody);
                                }

                                Logger.LogRelativeMessage($"Emails sent successfully for product {product.ProductName} while using this Query {contentQuery}, Total subscribers: {subscribers.Count}", true);
                            }
                            else
                            {
                                Logger.LogRelativeMessage($"Content Not found according to your entered Query {contentQuery} and for Product {product.ProductName}", true);
                            }
                        }
                    }*//*

                    var result = new { Success = true, Message = "Emails sent successfully." };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                // Handle exceptions as needed
                Logger.LogRelativeMessage("An error occurred while sending emails: " + ex.Message, true); // Log the exception
                var result = new { Success = false, Message = "An error occurred while sending emails." };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }   */


/* private void SendEmails(List<ProductSubscribers> subscribers, string content)
        {
            string smtpServer = ConfigurationManager.AppSettings["SmtpServer"];
            int smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            string smtpUsername = ConfigurationManager.AppSettings["SmtpUsername"];
            string smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];
            string fromEmail = ConfigurationManager.AppSettings["SMTPFromEmail"]; // Get "From" email address

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
                smtpClient.EnableSsl = true; // Use SSL if required by your SMTP server

                smtpClient.Send(mail);
            }
            catch(Exception ex)
            {
                Logger.LogRelativeMessage("An error occurred while sending an email: " + ex.Message, true); // Log the exception
                // Handle exceptions or log errors
            }
        }*/








/*  [AllowAnonymous]
  public ActionResult SendEmailsAuto(long? productId = null)
  {

      try
      {
          if (productId.HasValue)
          {
              // Work for a specific product
              Product product = ProductsBAL.ProductDetail(productId.Value);
              if (product != null)
              {
                  ContentQuery cq = ProductsBAL.ProductContentQuery(productId);
                  string[] contentQueries = cq.InlineQuery.Split('~'); // Split the content queries by tilde (~)

                  foreach (var contentQuery in contentQueries)
                  {
                      List<Dictionary<string, object>> contentList = SubscribersBAL.GetContentForProduct(product.Id, contentQuery);

                      if (contentList.Count > 0)
                      {
                          foreach (var contentRow in contentList)
                          {
                              // Extract the 'Body' column from the contentRow dictionary
                              if (contentRow.TryGetValue("Body", out object content))
                              {
                                  List<ProductSubscribers> subscribers = ProductsBAL.lstSubscribers(product.Id);
                                  SendEmails(subscribers, content.ToString()); // Convert the content to string
                                  Logger.LogRelativeMessage($"Emails sent successfully for product {product.ProductName}, Total subscribers: {subscribers.Count}", true);
                              }
                          }
                      }
                  }
              }
              else
              {
                  Logger.LogRelativeMessage($"Product not found for ID: {productId}", true); // Log the exception
              }
          }
          else
          {
              // Work for all products
              List<Product> products = ProductsBAL.lstSubscriberProducts();

              foreach (var product in products)
              {
                  ContentQuery cq = ProductsBAL.ProductContentQuery(product.Id);
                  string[] contentQueries = cq.InlineQuery.Split('~');
                  foreach (var contentQuery in contentQueries)
                  {
                      List<Dictionary<string, object>> contentList = SubscribersBAL.GetContentForProduct(product.Id, contentQuery);

                      if (contentList.Count > 0)
                      {
                          foreach (var contentRow in contentList)
                          {
                              // Extract the 'Body' column from the contentRow dictionary
                              if (contentRow.TryGetValue("Body", out object content))
                              {
                                  List<ProductSubscribers> subscribers = ProductsBAL.lstSubscribers(product.Id);
                                  SendEmails(subscribers, content.ToString()); // Convert the content to string
                                  Logger.LogRelativeMessage($"Emails sent successfully for product {product.ProductName}, Total subscribers: {subscribers.Count}", true);
                              }
                          }
                      }
                  }
              }
          }

          var result = new { Success = true, Message = "Emails sent successfully." };
          return Json(result, JsonRequestBehavior.AllowGet);
      }
      catch (Exception ex)
      {
          // Handle exceptions as needed
          Logger.LogRelativeMessage("An error occurred while sending emails: " + ex.Message, true); // Log the exception
          var result = new { Success = false, Message = "An error occurred while sending emails." };
          return Json(result, JsonRequestBehavior.AllowGet);
      }
  }*/



/*  public ActionResult QueryAddEdit(long? ProductId)
        {
            var ProductContentQuery = ProductsBAL.ProductContentQuery(ProductId);
            ProductViewModal PVM = new ProductViewModal();
            PVM.ContentQuery = ProductContentQuery;
            return View(PVM);
        }*/





/*[HttpPost]
        public ActionResult SMTPConfigurations(long? ProductId, string Title, string FromName, string FromEmail, string SmtpServer, long SmtpPort, string SmtpUser, string SmtpPassword, string SmtpDomain, string SmtpFooter, string SSLPort, long? PostMarkSignatureId, bool IsDefault = false, bool EnableSSL = false, long? Id = 0, bool IsAWSBulkEmail = false, string AwsServiceURL = "", long? AwsBulkEmailCount = 0, string AwsAccessKeyId = "", string AwsSecretKey = "")
        {
            var PostMarkToken = ConfigurationManager.AppSettings["PostmarkServerToken"];
            string PushNotificationSettingsJson = string.Empty;
            if (!Convert.ToBoolean(IsAWSBulkEmail))
            {
                PushNotificationSettingsJson = PushNotificationsBAL.ProviderSettings(Title, FromName, FromEmail, SmtpServer, SmtpPort, SmtpUser, SmtpPassword, SmtpDomain, SmtpFooter, IsDefault, EnableSSL);
            }
            else
            {
                SmtpUser = string.Empty;
                SmtpPort = 0;
                SmtpPassword = string.Empty;
            }

            var Added = PushNotificationsBAL.AddUserSmtpConfiguration(ProductId, Title, FromName, FromEmail, SmtpServer, SmtpPort, SmtpUser, SmtpPassword, SmtpDomain, SmtpFooter, IsDefault, PushNotificationSettingsJson, false, "0", Id, EnableSSL, "", IsAWSBulkEmail, AwsServiceURL, AwsBulkEmailCount, AwsAccessKeyId, AwsSecretKey);

            return Json(Added, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string ProductSMTPSettingsPartial(long? ProductId)
        {
            var ProductSMTP = ProductsBAL.ProductSMTP(ProductId);
            return RenderPartialToString("_ProductSMTPSettings", ProductSMTP, ControllerContext);
        }

        public static string RenderPartialToString(string viewName, object model, ControllerContext controllerContext)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = controllerContext.RouteData.GetRequiredString("action");
            ViewDataDictionary ViewData = new ViewDataDictionary();
            TempDataDictionary TempData = new TempDataDictionary();
            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controllerContext, viewName);
                ViewContext viewContext = new ViewContext(controllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }

        }*/








/* public ActionResult SendEmailManually()
        {
            var products = ProductsBAL.lstProducts();
            var model = new EmailViewModel
            {
                ProductList = products, IsProductSelected = new List<bool>(new bool[products.Count]) // Initialize with false values
            };
            return View(model);

        }*/

/*    [HttpPost]
        public ActionResult SelectProducts(EmailViewModel model)
        {
            // Store the selected product IDs in sessions
            var selectedIds = model.ProductList
                              .Where((product, index) => model.IsProductSelected[index])
                              .Select(product => product.Id)
                              .ToArray();

            model.SelectedProductIds = selectedIds;
            //Session["EmailViewModel"] = model;

            // Return a JSON response indicating success
            return Json(new { success = true });
        }*/

/*[HttpPost]
        public ActionResult EmailSettings(EmailViewModel model)
        {
            bool isAWS = Request.Form["rdoAWS"] == "true";

            if (isAWS)
            {
                var smtpConfiguration = new SmtpConfiguration
                {
                    FromName = Request.Form["FromName"],
                    FromEmail = Request.Form["FromEmail"],
                    AwsServiceURL = Request.Form["AwsServiceURL"],
                    AwsBulkEmailCount = long.Parse(Request.Form["AwsBulkEmailCount"]),
                    AwsAccessKeyId = Request.Form["AwsAccessKeyId"],
                    AwsSecretKey = Request.Form["AwsSecretKey"]
                };

                model.SMTPConfiguration = smtpConfiguration;
            }
            else
            {
                var smtpConfiguration = new SmtpConfiguration
                {
                    // Populate properties for Custom SMTP configuration
                    Title = Request.Form["Title"],
                    FromName = Request.Form["FromName"],
                    FromEmail = Request.Form["FromEmail"],
                    SmtpServer = Request.Form["SmtpServer"],
                    SmtpPort = long.Parse(Request.Form["SmtpPort"]),
                    SmtpUser = Request.Form["SmtpUser"],
                    SmtpPassword = Request.Form["SmtpPassword"],
                    SmtpDomain = Request.Form["SmtpDomain"],
                    SmtpFooter = Request.Form["SmtpFooter"],
                    IsDefault = Request.Form["chkDefault"] == "on",
                    EnableSSL = Request.Form["EnableSSL"] == "true"
                };

                model.SMTPConfiguration = smtpConfiguration;
            }

            TempData["EmailViewModel"] = model;

            return RedirectToAction("AddSubjectAndBody");
        }*/

/*[HttpPost]
        public ActionResult AddSubjectAndBody(EmailViewModel model)
        {
            model.Subject = Request.Form["Subject"];
            model.Body = Request.Form["Body"];

            return View("SendEmailManually", model);
        }

        [HttpPost]
        public ActionResult SendEmail(EmailViewModel model)
        {
            long[] selectedProductIds = model.SelectedProductIds;
            SmtpConfiguration smtpConfiguration = model.SMTPConfiguration;
            string subject = model.Subject;
            string body = model.Body;

            return View("EmailSentConfirmation");
        }*/
/*[HttpPost]
public ActionResult SendEmailManually(EmailViewModel model)
{
    var selectedIds = model.ProductList
                       .Where((product, index) => model.IsProductSelected[index])
                       .Select(product => product.Id)
                       .ToArray();

    model.SelectedProductIds = selectedIds;

    var smtpConfiguration = new SmtpConfiguration
    {
        // Populate properties for Custom SMTP configuration
        Title = Request.Form["Title"],
        FromName = Request.Form["FromName"],
        FromEmail = Request.Form["FromEmail"],
        SmtpServer = Request.Form["SmtpServer"],
        SmtpPort = long.Parse(Request.Form["SmtpPort"]),
        SmtpUser = Request.Form["SmtpUser"],
        SmtpPassword = Request.Form["SmtpPassword"],
        SmtpDomain = Request.Form["SmtpDomain"],
        SmtpFooter = Request.Form["SmtpFooter"],
        IsDefault = Request.Form["chkDefault"] == "on",
        EnableSSL = Request.Form["EnableSSL"] == "true"
    };

    model.SMTPConfiguration = smtpConfiguration;

    model.Subject = Request.Form["Subject"];
    model.Body = Request.Form["Body"];

    return Json(model);
}*/