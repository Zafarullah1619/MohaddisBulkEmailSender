using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using Org.Business.Methods;
using Org.Business.Objects;
using Org.Utils;
using S22.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ASE.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        // GET: Product
        public ActionResult Index()
        {

            var lstProducts = ProductsBAL.lstUpsellProducts();
            return View(lstProducts);
        }

        public ActionResult AddEdit(long? ProductId)
        {
            var ProductDetail = ProductsBAL.ProductDetail(ProductId);
            var ProductRules = ProductsBAL.ProductRules(ProductId);
            var ProductSMTP = ProductsBAL.ProductSMTP(ProductId);
            var ProductUpSells = ProductsBAL.ProductUpSells(ProductId);
            ProductViewModal PVM = new ProductViewModal();
            PVM.Product = ProductDetail;
            PVM.lstProductRules = ProductRules;
            PVM.SmtpConfiguration = ProductSMTP;
            PVM.lstProductUpsells = ProductUpSells;
            return View(PVM);
        }

        [HttpPost]
        public ActionResult UpdateProductInfo(long? ProductId, string ProductName, string DBServer, string DBUserName, string DBPassword, string DBServerType, string DataBaseName, string Upsells, string UpsellIDs)
        {
            var added = ProductsBAL.AddEditUpsProduct(ProductId, ProductName, DBServer, DBUserName, DBPassword, DBServerType, DataBaseName);
            if (added != null && added.Id > 0 && !string.IsNullOrWhiteSpace(Upsells))
            {
                byte[] data = Convert.FromBase64String(Upsells);
                string decodedString = Encoding.UTF8.GetString(data);
                var upsellAdded = ProductsBAL.AddEditProductUpsells(added.Id, decodedString, UpsellIDs);


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

        [HttpPost]
        public ActionResult SaveProductRules(List<ProductRulesViewModel> myModal)
        {

            if (myModal != null && myModal.Count > 0)
            {
                string combindedIDs = string.Join(",", myModal.Select(x => x.RuleId));
                foreach (var item in myModal)
                {
                    var added = ProductsBAL.AddEditProductRules(item, combindedIDs);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]

        public string RenderCreateRulePartial(long? ProductId, long? RuleNumber, bool FromScratch)
        {
            var ProductDetail = ProductsBAL.ProductDetail(ProductId);
            var ProductRules = ProductsBAL.ProductRules(ProductId);
            var lstProductUpsells = ProductsBAL.lstProductUpsells(ProductId);
            ViewBag.ProductRules = ProductRules;
            ViewBag.ProductUpsells = lstProductUpsells;
            ViewBag.RuleNumber = RuleNumber;

            ProductViewModal productViewModal = new ProductViewModal();
            productViewModal.lstProductRules = ProductRules;
            productViewModal.lstProductUpsells = lstProductUpsells;
            productViewModal.Product = ProductDetail;
            productViewModal.RuleNumber = RuleNumber;
            productViewModal.FromScratch = FromScratch;
            return RenderPartialToString("_CreateRules", productViewModal, ControllerContext);
        }

        [HttpPost]

        public string RenderExistingRulePartial(long? ProductId, long? RuleNumber)
        {
            var ProductDetail = ProductsBAL.ProductDetail(ProductId);
            var ProductRules = ProductsBAL.ProductRules(ProductId);
            var lstProductUpsells = ProductsBAL.lstProductUpsells(ProductId);
            ViewBag.ProductRules = ProductRules;
            ViewBag.ProductUpsells = lstProductUpsells;
            ViewBag.RuleNumber = RuleNumber;

            ProductViewModal productViewModal = new ProductViewModal();
            productViewModal.lstProductRules = ProductRules;
            productViewModal.lstProductUpsells = lstProductUpsells;
            productViewModal.Product = ProductDetail;
            productViewModal.RuleNumber = RuleNumber;
            return RenderPartialToString("_ExistingRules", productViewModal, ControllerContext);
        }

        /*
        public PartialViewResult RenderCreateRulePartial(long? ProductId, long? RuleNumber)
        {
            var ProductDetail = ProductsBAL.ProductDetail(ProductId);
            var ProductRules = ProductsBAL.ProductRules(ProductId);
            var lstProductUpsells = ProductsBAL.lstProductUpsells(ProductId);
            ViewBag.ProductRules = ProductRules;
            ViewBag.ProductUpsells = lstProductUpsells;
            ViewBag.RuleNumber = RuleNumber;
            return PartialView("~/Views/Shared/_CreateRules.cshtml");
        }
        */

        [HttpPost]
        public string ProductSMTPSettingsPartial(long? ProductId)
        {
            var ProductSMTP = ProductsBAL.ProductSMTP(ProductId);
            return RenderPartialToString("_ProductSMTPSettings", ProductSMTP, ControllerContext);
        }

        [HttpPost]
        public PartialViewResult UpsellConditionPartial(long? ProductId, long? RuleNumber, long? CondNumber)
        {
            var ProductDetail = ProductsBAL.ProductDetail(ProductId);
            var lstProductUpsells = ProductsBAL.lstProductUpsells(ProductId);
            ViewBag.ProductUpsells = lstProductUpsells;
            ViewBag.RuleNumber = RuleNumber;
            ViewBag.CondNumber = CondNumber;
            return PartialView("~/Views/Shared/_UpsellCondition.cshtml");
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

        }

        public ActionResult SendASEEmails(long ProductId)
        {
            var ProductRules = ProductsBAL.ProductRules(ProductId);
            var ProductDetail = ProductsBAL.ProductDetail(ProductId);
            var ProductSMTP = ProductsBAL.ProductSMTP(ProductId);
            var DebugMode = Convert.ToBoolean(ConfigurationManager.AppSettings["DebugMode"]);
            if (ProductRules != null && ProductRules.Where(x => x.IsActive == true).ToList().Count > 0)
            {
                foreach (var item in ProductRules.Where(x => x.IsActive == true))
                {
                    var rule = item.Rule;
                    string str = rule;//"Bought(HasGroupShots) AND NotBought(IsECoverAgency AND IsGSAgency AND IsECoverProAgency AND HasCashPallete AND IsSocialMember)";

                    var output = str.Split('(', ')');

                    string strOutPut = string.Empty;

                    int j = 0;

                    for (int i = 0; i < output.Length; i++)
                    {
                        if (j < output.Length)
                        {
                            if (output[j].ToString().ToUpper() == "BOUGHT")
                            {

                                strOutPut += output[j].ToString().ToUpper().Replace("BOUGHT", "(") + output[j + 1].ToString().Replace(" AND ", "=1 AND ").Replace(" OR ", "=1 OR ") + "=1)";
                            }
                            else
                            {
                                if (j + 1 < output.Length)
                                {
                                    strOutPut += output[j].ToString().ToUpper().Replace("NOTBOUGHT", "(") + output[j + 1].ToString().Replace(" AND ", "=0 AND ").Replace(" OR ", "=0 OR ") + "=0)";
                                }
                            }
                            j = j + 2;
                        }
                    }
                    Org.Utils.Logger.LogRelativeMessage(("strOutPut Rule : " + strOutPut), DebugMode);
                    List<ProductCustomer> lstCustomers = new List<ProductCustomer>();
                    if (ProductDetail.DBServerType == "MSSQL")
                    {
                        lstCustomers = PushNotificationsBAL.lstProductCustomers(strOutPut, ProductDetail.DBServer, ProductDetail.DataBaseName, ProductDetail.DBUserName, ProductDetail.DBPassword);
                        //if (lstCustomers.Count > 0)
                        //{
                        //    foreach (var cust in lstCustomers)
                        //    {
                        //        cust.Rule = strOutPut;
                        //    }
                        //}
                    }
                    else if (ProductDetail.DBServerType == "MySQL")
                    {
                        lstCustomers = PushNotificationsBAL.GetDataFromMySQL(strOutPut, ProductDetail.DBServer, ProductDetail.DataBaseName, ProductDetail.DBUserName, ProductDetail.DBPassword);
                        Logger.LogRelativeMessage(("MYSQL lstCustomers Count : " + lstCustomers.Count), DebugMode);
                    }
                    if (lstCustomers != null && lstCustomers.Count > 0)
                    {
                        var UserId = User.Identity.GetUserId();
                        var PushNotificationSettingsJson = ProductSMTP != null ? ProductSMTP.PushNotificationSettingsJson : "";
                        var FromEmail = ProductSMTP != null ? ProductSMTP.FromEmail : ConfigurationManager.AppSettings["FromEmail"];
                        var FromName = ProductSMTP != null ? ProductSMTP.FromName : ConfigurationManager.AppSettings["FromName"];
                        foreach (var cust in lstCustomers)
                        {
                            cust.Rule = strOutPut;
                            Notifications.NotificationsEmailHandler.AfterSaleEmailNotification(UserId, PushNotificationSettingsJson, ProductDetail.Id, FromEmail, FromName, item.Subject, item.EmailContent, cust.Email, strOutPut);
                        }
                    }
                }
            }
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RemoveProduct(long ProductId)
        {
            var removed = ProductsBAL.RemoveProduct(ProductId);
            return Json(removed, JsonRequestBehavior.AllowGet);
        }
    }
}